using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Repository.Games;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.Repository.Teams;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;

namespace JanuszPOL.JanuszPOLBets.Services.Games;

public interface IGamesService
{
    Task<ServiceResult<IList<GetGamesResult>>> Get(GetGamesInput input);
    Task<ServiceResult<IList<GetGamesResult>>> GetAll();
    bool AddGame(AddGameInput gameInput);
    void UpdateGame(UpdateGameInput updateInput);
   

}

public class GamesService : IGamesService
{
    private readonly IGamesRepository _gamesRepository;
    private readonly ITeamsRepository _teamsRepository;

    public GamesService(IGamesRepository gamesRepository, ITeamsRepository teamsRepository)
    {
        _gamesRepository = gamesRepository;
        _teamsRepository = teamsRepository;
    }

    public void UpdateGame(UpdateGameInput updateInput)
    {
        var updatedGame = new UpdateGameDto 
        { 
            Id = updateInput.Id,
            Team1Score = updateInput.Team1Score,
            Team2Score = updateInput.Team2Score,
            Team1ScoreExtraTime = updateInput.Team1ScoreExtraTime,
            Team2ScoreExtraTime = updateInput.Team2ScoreExtraTime,
            Team1ScorePenalties = updateInput.Team1ScorePenalties,
            Team2ScorePenalties = updateInput.Team2ScorePenalties,
            GameResultId = updateInput.GameResultId
        };
        _gamesRepository.Update(updatedGame);
    }
    public bool AddGame(AddGameInput gameInput)
    {
        if (!_teamsRepository.Exists(gameInput.Team1Id) || !_teamsRepository.Exists(gameInput.Team2Id) || gameInput.Team1Id == gameInput.Team2Id)
        {
            return false;
        }
        else
        {
            _gamesRepository.Add(new AddGameDto { Team1Id = gameInput.Team1Id, Team2Id = gameInput.Team2Id, GameDate = gameInput.GameDate });
            return true;
        }
    }

    public async Task<ServiceResult<IList<GetGamesResult>>> Get(GetGamesInput input)
    {
        if (input == null)
        {
            return ServiceResult<IList<GetGamesResult>>.WithErrors("Input can't be null");
        }

        if (input.Skip < 0)
        {
            return ServiceResult<IList<GetGamesResult>>.WithErrors("Skip value must be at least 0");
        }

        if (input.Limit < 1)
        {
            return ServiceResult<IList<GetGamesResult>>.WithErrors("Limit value must be at least 1");
        }

        var games = await _gamesRepository.Get(new GetGameDto
        {
            NameContains = input.NameContains,
            Limit = input.Limit,
            Skip = input.Skip,
            NameStartsWith = input.NameStartsWith
        });

        var result = ServiceResult<IList<GetGamesResult>>.WithSuccess(
            games.Select(x => new GetGamesResult
            {
                GameId = x.GameId,
                Team1Name = x.Team1Name,
                Team2Name = x.Team2Name,
                Winner = x.Winner
            }).ToList()
        );

        return result;
    }

    public async Task<ServiceResult<IList<GetGamesResult>>> GetAll()
    {
        var games = await _gamesRepository.GetAll();

        var result = ServiceResult<IList<GetGamesResult>>.WithSuccess(
            games.Select(x => new GetGamesResult
            {
                GameId = x.GameId,
                Team1Name = x.Team1Name,
                Team2Name = x.Team2Name,
                Winner = x.Winner
            }).ToList()
        );

        return result;
    }

    
}
