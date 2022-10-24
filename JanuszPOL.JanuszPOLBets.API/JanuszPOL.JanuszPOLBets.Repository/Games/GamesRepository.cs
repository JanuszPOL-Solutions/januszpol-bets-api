using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Games;

public interface IGamesRepository
{
    Task<GetGameResultDto[]> Get(GetGameDto dto);
    Task<GetGameResultDto[]> GetAll();
    void Add(AddGameDto gameDto);
    void Update(UpdateGameDto updateDto);
    Task<SingleGameDto> GetGameById(int gameId, long accountId);

}
public class GamesRepository : IGamesRepository
{
    private readonly DataContext _db;

    public GamesRepository(DataContext db)
    {
        _db = db;
    }

    public void Update(UpdateGameDto updateDto)
    {
        var game = _db.Games.First(x => x.Id == updateDto.Id);
        game.Team1Score = updateDto.Team1Score;
        game.Team2Score = updateDto.Team2Score;
        game.Team1ScoreExtraTime = updateDto.Team1ScoreExtraTime;
        game.Team2ScoreExtraTime = updateDto.Team2ScoreExtraTime;
        game.Team1ScorePenalties = updateDto.Team1ScorePenalties;
        game.Team2ScorePenalties = updateDto.Team2ScorePenalties;
        game.GameResultId = updateDto.GameResultId;
        _db.SaveChanges();
        
    }
    public void Add(AddGameDto gameDto)
    {

        var game = new Game
        {
            Team1Id = gameDto.Team1Id,
            Team2Id = gameDto.Team2Id,
            GameDate = gameDto.GameDate,
            PhaseId = gameDto.PhaseId,
            PhaseName = gameDto.PhaseName,
        };

        _db.Games.Add(game);
        _db.SaveChanges();
        
    }

    public async Task<GetGameResultDto[]> Get(GetGameDto dto)
    {
        var games = await _db.Games
            .Where(x => string.IsNullOrEmpty(dto.NameContains) || x.Team1.Name.ToLower().Contains(dto.NameContains.ToLower()))
            .Where(x => string.IsNullOrEmpty(dto.NameStartsWith) || x.Team1.Name.ToLower().StartsWith(dto.NameStartsWith.ToLower()))
            .Select(x => new GetGameResultDto
            {
                Id = x.Id,
                Team1 = x.Team1.Name,
                Team2 = x.Team2.Name,
                Date =x.GameDate,
                PhaseName = x.PhaseName,
                Result = (int)x.GameResultId,
                Team1PenaltyScore = x.Team1ScorePenalties,
                Team2PenaltyScore = x.Team2ScorePenalties,
                Team1Score = x.Team1Score,
                Team2Score = x.Team2Score,
                Stage = (int)x.PhaseId,
                //TODO: add user context to get events count and result event
            })
            .Skip(dto.Skip)
            .Take(dto.Limit)
            .ToArrayAsync();

        return games;
    }

    public async Task<GetGameResultDto[]> GetAll()
    {
        return await _db.Games.Select(x => new GetGameResultDto
        {
            Id = x.Id,
            Team1= x.Team1.Name,
            Team2= x.Team2.Name
        }).ToArrayAsync();
    }

    public async Task<SingleGameDto> GetGameById(int gameId, long accountId)
    {
        var game = _db.Games.Include(x => x.Team1).Include(x => x.Team2).First(x => x.Id == gameId);

        var listSelectedEvents = _db.EventBet
            .Where(x => x.GameId == gameId)
            .Where(x => x.AccountId == accountId)
            .Select(x => new EventDto 
                { 
                    Id = x.Id,
                    BetCost = x.Event.BetCost,
                    EventTypeId = x.Event.EventTypeId,
                    GainedPoints = x.Result == true ? x.Event.WinValue : 0,
                    Name = x.Event.Name,
                    Team1Score = x.Value1,
                    Team2Score = x.Value2
                }).ToList();

        var resultEvent = listSelectedEvents.FirstOrDefault(x => x.EventTypeId == (int)EventType.RuleType.BaseBet);
        if (resultEvent != null) //tmp
        {
            resultEvent.MatchResult = resultEvent.Id;
        }

        var exactScoreEvent = listSelectedEvents.FirstOrDefault(x => x.EventTypeId == (int)EventType.RuleType.TwoExactValues);

        listSelectedEvents = listSelectedEvents
            .Where(x => x.EventTypeId != (int)EventType.RuleType.BaseBet)
            .Where(x => x.EventTypeId != (int)EventType.RuleType.TwoExactValues) //tmp
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
            ResultEvent = resultEvent,
            SelectedEvents = listSelectedEvents,
            Started = DateTime.Now > game.GameDate
        };

        return gameDto;
    }
}
