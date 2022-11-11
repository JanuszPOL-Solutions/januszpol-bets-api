using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using JanuszPOL.JanuszPOLBets.Data.Extensions;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Games;

public interface IGamesRepository
{
    Task<GetGameResultDto[]> Get(GetGameDto dto);
    Task<GetGameResultDto[]> GetAll();
    Task Add(AddGameDto gameDto);
    Task Update(UpdateGameDto updateDto);
    Task ClearResultForGame(long gameId);
    Task<SingleGameDto> GetGameById(int gameId, long accountId);
}

public class GamesRepository : IGamesRepository
{
    private readonly DataContext _db;

    public GamesRepository(DataContext db)
    {
        _db = db;
    }

    public async Task Update(UpdateGameDto updateDto)
    {
        var game = _db.Games.First(x => x.Id == updateDto.Id);

        game.Team1Score = updateDto.Team1Score;
        game.Team2Score = updateDto.Team2Score;

        if(updateDto.Team1Score > updateDto.Team2Score)
        {
            game.GameResultId = GameResult.Values.Team1;
        }
        else if(updateDto.Team1Score < updateDto.Team2Score)
        {
            game.GameResultId = GameResult.Values.Team2;
        }
        else
        {
            game.GameResultId = GameResult.Values.Draw;
        }

        await _db.SaveChangesAsync();
    }

    public async Task Add(AddGameDto gameDto)
    {
        var game = new Game
        {
            Team1Id = gameDto.Team1Id,
            Team2Id = gameDto.Team2Id,
            GameDate = gameDto.GameDate,
            PhaseId = gameDto.PhaseId,
            PhaseName = gameDto.PhaseName,
        };

        await _db.Games.AddAsync(game);
        await _db.SaveChangesAsync();
    }

    public async Task<GetGameResultDto[]> Get(GetGameDto dto)
    {
        var games = await _db.Games
            .Where(x => dto.Phase == null || x.PhaseId == dto.Phase.Value)
            .Where(x => dto.TeamIds == null || dto.TeamIds.Contains(x.Team1Id) || dto.TeamIds.Contains(x.Team2Id))
            .Where(x => dto.PhaseNames == null || dto.PhaseNames.Contains(x.PhaseName))
            .OrderBy(x => x.GameDate)
            .Where(x => dto.Beted == null ||
                (dto.Beted == GetGameDto.BetState.NotBeted && //not beted
                    !x.EventBets
                        .Where(y => y.AccountId == dto.AccountId)
                        .Where(y => !y.IsDeleted)
                        .Where(y => y.Event.EventTypeId == EventType.RuleType.BaseBet)
                        .Any()) ||
                (dto.Beted == GetGameDto.BetState.Beted && //beted
                    x.EventBets
                            .Where(y => y.AccountId == dto.AccountId)
                            .Where(y => !y.IsDeleted)
                            .Where(y => y.Event.EventTypeId == EventType.RuleType.BaseBet)
                            .Any()
                ) ||
                (dto.Beted == GetGameDto.BetState.ToBet &&
                    !x.EventBets
                                .Where(y => y.AccountId == dto.AccountId)
                                .Where(y => !y.IsDeleted)
                                .Where(y => y.Event.EventTypeId == EventType.RuleType.BaseBet)
                                .Any() && x.GameDate > DateTime.Now
                )
            )
            .Select(x => new GetGameResultDto
            {
                Id = x.Id,
                Team1 = x.Team1.Name,
                Team2 = x.Team2.Name,
                GameDate = x.GameDate,
                PhaseName = x.PhaseName,
                Result = (int?)x.GameResultId,
                Team1PenaltyScore = x.Team1ScorePenalties,
                Team2PenaltyScore = x.Team2ScorePenalties,
                Team1Score = x.Team1Score,
                Team2Score = x.Team2Score,
                Phase = (int)x.PhaseId,
                ResultEventBet = x.EventBets
                    .Where(y => y.AccountId == dto.AccountId && !y.IsDeleted && y.Event.EventTypeId == EventType.RuleType.BaseBet)
                    .Select(x => x.EventId)
                    .FirstOrDefault(),
                EventsBetedCount = x.EventBets
                    .Count(y => y.AccountId == dto.AccountId && !y.IsDeleted && y.Event.EventTypeId != EventType.RuleType.BaseBet)
            })
            .ToArrayAsync();

        return games;
    }

    public async Task<GetGameResultDto[]> GetAll()
    {
        return await _db.Games.Select(x => new GetGameResultDto
        {
            Id = x.Id,
            Team1 = x.Team1.Name,
            Team2 = x.Team2.Name
        }).ToArrayAsync();
    }

    public async Task<SingleGameDto> GetGameById(int gameId, long accountId)
    {
        var game = await _db.Games
            .Include(x => x.Team1)
            .Include(x => x.Team2)
            .FirstAsync(x => x.Id == gameId);

        var listSelectedEvents = await _db.EventBet
            .Where(x => !x.IsDeleted)
            .Where(x => x.GameId == gameId)
            .Where(x => x.AccountId == accountId)
            .Select(x => new GameEventBetDto
            {
                Id = x.Id,
                EventId = x.EventId,
                BetCost = x.Event.BetCost,
                EventTypeId = x.Event.EventTypeId,
                GainedPoints = x.Result == true ? x.Event.WinValue : 0,
                Name = x.Event.Name,
                Team1Score = x.Value1,
                Team2Score = x.Value2
            }).ToListAsync();

        var resultEventBet = listSelectedEvents.FirstOrDefault(x => x.EventTypeId == EventType.RuleType.BaseBet);
        if (resultEventBet != null) //tmp
        {
            resultEventBet.MatchResult = resultEventBet.EventId;
        }

        var exactScoreEvent = listSelectedEvents.FirstOrDefault(x => x.EventTypeId == EventType.RuleType.TwoExactValues);

        listSelectedEvents = listSelectedEvents
            .Where(x => x.EventTypeId != EventType.RuleType.BaseBet)
            .Where(x => x.EventTypeId != EventType.RuleType.TwoExactValues) //tmp
            .ToList();

        var gameDto = new SingleGameDto
        {
            Id = game.Id,
            Team1 = game.Team1.Name,
            Team2 = game.Team2.Name,
            GameDate = game.GameDate,
            Team1Score = game.Team1Score,
            Team2Score = game.Team2Score,
            Team1ScoreExtraTime = game.Team1ScoreExtraTime,
            Team2ScoreExtraTime = game.Team2ScoreExtraTime,
            Team1ScorePenalties = game.Team1ScorePenalties,
            Team2ScorePenalties = game.Team2ScorePenalties,
            PhaseName = game.PhaseName,
            PhaseId = game.PhaseId,
            GameResultId = game.GameResultId,
            ExactScoreEvent = exactScoreEvent,
            ResultEvent = resultEventBet,
            SelectedEvents = listSelectedEvents,
            Started = DateTime.Now > game.GameDate
        };

        return gameDto;
    }

    public async Task ClearResultForGame(long gameId)
    {
        var game = _db.Games.First(x => x.Id == gameId);
        
            game.Team1Score = null;
            game.Team2Score = null;
            game.Team1ScoreExtraTime = null;
            game.Team2ScoreExtraTime = null;
            game.Team1ScorePenalties = null;
            game.Team2ScorePenalties = null;
            game.GameResultId = null;

        await _db.SaveChangesAsync();
        
    }
}
