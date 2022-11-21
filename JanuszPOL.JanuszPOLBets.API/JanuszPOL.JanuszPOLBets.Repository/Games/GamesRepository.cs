using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using JanuszPOL.JanuszPOLBets.Data.Extensions;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JanuszPOL.JanuszPOLBets.Repository.Games;

public interface IGamesRepository
{
    Task<GetGameResultDto[]> Get(GetGameDto dto);
    Task<GetGameResultDto[]> GetAll();
    Task Add(AddGameDto gameDto);
    Task Update(UpdateGameDto updateDto);
    Task ClearResultForGame(long gameId);
    Task<SingleGameDto> GetGameById(long gameId);
    Task<SingleGameWithEventsDto> GetGameWithEventsById(int gameId, long accountId);
    Task<SimpleGameDto> GetSimpleGame(long gameId);
    Task<GameBetsDto> GetBetsForGame(long gameId);
}

public class GamesRepository : IGamesRepository
{
    private readonly DataContext _db;
    private readonly ILogger<GamesRepository> _logger;
    public GamesRepository(
        DataContext db,
        ILogger<GamesRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Update(UpdateGameDto updateDto)
    {
        var game = _db.Games.First(x => x.Id == updateDto.Id);

        game.Team1Score = updateDto.Team1Score;
        game.Team2Score = updateDto.Team2Score;

        if (updateDto.Team1Score > updateDto.Team2Score)
        {
            game.GameResultId = GameResult.Values.Team1;
        }
        else if (updateDto.Team1Score < updateDto.Team2Score)
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

    public async Task<SingleGameDto> GetGameById(long gameId)
    {
        var game = await _db.Games
            .Include(x => x.Team1)
            .Include(x => x.Team2)
            .FirstAsync(x => x.Id == gameId);

        return new SingleGameDto
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
            Started = DateTime.Now > game.GameDate
        };
    }

    public async Task<SingleGameWithEventsDto> GetGameWithEventsById(int gameId, long accountId)
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

        var gameDto = new SingleGameWithEventsDto
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

    public async Task<SimpleGameDto> GetSimpleGame(long gameId)
    {
        var game = await _db.Games
            .Where(x => x.Id == gameId)
            .Select(x => new SimpleGameDto
            {
                Id = x.Id,
                PhaseName = x.PhaseName,
                PhaseId = x.PhaseId,
                GameDate = x.GameDate,
                Team1Name = x.Team1.Name,
                Team1Score = x.Team1Score,
                Team2Name = x.Team2.Name,
                Team2Score = x.Team2Score
            })
            .FirstOrDefaultAsync();

        if (game == null)
        {
            _logger.LogError($"Can't find game with id: {gameId}");
            return null;
        }

        var allGameEvents = await _db.Event
            .Where(x => x.EventTypeId == EventType.RuleType.Boolean)
            .Select(x => new
            {
                EventId = x.Id,
                Name = x.Name,
                EventBets = x.Bets
                    .Where(y => !y.IsDeleted && y.GameId == gameId)
                    .Select(y => new { y.AccountId, y.Id, y.Result })
                    .ToList()
            })
            .ToListAsync();

        foreach (var gameEvent in allGameEvents)
        {
            var wrongEvents = gameEvent.EventBets.Where(x => gameEvent.EventBets.Where(y => y.Id != x.Id).Any(y => y.Result != x.Result)).ToList();

            if (wrongEvents.Any())
            {
                _logger.LogError($"There is a bet with different result for game: {game.Id}");
                foreach (var wrongItem in wrongEvents)
                {
                    _logger.LogError($"For event {gameEvent.EventId}, bet: {wrongItem.Id} for account {wrongItem.AccountId} has different result");
                }
            }
        }

        game.Events = allGameEvents
            .Select(x => new SimpleGameDto.GameEvent
            {
                EventId = x.EventId,
                Name = x.Name,
                Happened = x.EventBets.Any() ? x.EventBets.Any(y => y.Result == true) : null
            })
            .ToList();

        return game;
    }

    public async Task<GameBetsDto> GetBetsForGame(long gameId)
    {
        var game = await _db.Games
            .Where(x => x.Id == gameId)
            .Select(x => new
            {
                GameId = x.Id,
                Team1Name = x.Team1.Name,
                Team2Name = x.Team2.Name,
                Started = DateTime.Now > x.GameDate,
                x.GameDate,
                x.Team1Score,
                x.Team2Score,
                x.PhaseId,
                x.PhaseName,
                ExactScoreBets = x.EventBets.Where(y => y.Id == 8 && !y.IsDeleted).Select(y => new
                {
                    y.AccountId,
                    Team1Score = y.Value1,
                    Team2Score = y.Value2,
                    y.Result
                }).ToList(),
                ResultBets = x.EventBets.Where(y => y.Event.EventTypeId == EventType.RuleType.BaseBet).Select(y => new
                {
                    ResultBet = y.EventId,
                    y.AccountId,
                    y.Result
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (game == null)
        {
            return null;
        }

        if (!game.Started)
        {
            return new GameBetsDto();
        }

        var users = await _db.Accounts
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.UserName)
            .Select(x => new
            {
                x.UserName,
                x.Id
            })
            .ToListAsync();

        var dto = new GameBetsDto
        {
            GameId = game.GameId,
            GameDate = game.GameDate,
            Team1Name = game.Team1Name,
            Team2Name = game.Team2Name,
            Team1Score = game.Team1Score,
            Team2Score = game.Team2Score,
            PhaseId = game.PhaseId,
            PhaseName = game.PhaseName,
            Bets = new List<GameBetsDto.Bet>()
        };

        foreach (var user in users)
        {
            var result = game.ResultBets.Where(x => x.AccountId == user.Id).FirstOrDefault();
            var exactScore = game.ExactScoreBets.Where(x => x.AccountId == user.Id).FirstOrDefault();

            var bet = new GameBetsDto.Bet
            {
                Username = user.UserName,
                ResultBet = result != null ? result.ResultBet : null,
                ExactScoreTeam1 = exactScore != null ? exactScore.Team1Score : null,
                ExactScoreTeam2 = exactScore != null ? exactScore.Team2Score : null,
                ExactScoreResult = exactScore?.Result,
                ResultBetResult = result?.Result
            };

            dto.Bets.Add(bet);
        }

        return dto;
    }
}

public class GameBetsDto
{
    public long GameId { get; set; }
    public DateTime GameDate { get; set; }
    public string Team1Name { get; set; }
    public string Team2Name { get; set; }
    public int? Team1Score { get; set; }
    public int? Team2Score { get; set; }
    public Phase.Types PhaseId { get; set; }
    public string PhaseName { get; set; }
    public List<Bet> Bets { get; set; }

    public class Bet
    {
        public string Username { get; set; }
        public long? ExactScoreTeam1 { get; set; }
        public long? ExactScoreTeam2 { get; set; }
        public bool? ExactScoreResult { get; set; }
        public long? ResultBet { get; set; }
        public bool? ResultBetResult { get; set; }
    }
}

public class SimpleGameDto
{
    public long Id { get; set; }
    public DateTime GameDate { get; set; }
    public string PhaseName { get; set; }
    public Phase.Types PhaseId { get; set; }
    public string Team1Name { get; set; }
    public string Team2Name { get; set; }
    public int? Team1Score { get; set; }
    public int? Team2Score { get; set; }
    public List<GameEvent> Events { get; set; }

    public class GameEvent
    {
        public long EventId { get; set; }
        public string Name { get; set; }
        public bool? Happened { get; set; }
    }
}
