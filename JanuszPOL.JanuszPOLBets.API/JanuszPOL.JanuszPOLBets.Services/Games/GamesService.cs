﻿using JanuszPOL.JanuszPOLBets.Repository.Games;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.Repository.Teams;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;

namespace JanuszPOL.JanuszPOLBets.Services.Games;

public interface IGamesService
{
    Task<ServiceResult<IList<GetGameResultDto>>> Get(GetGamesInput input);
    Task<ServiceResult<IList<GetGamesResult>>> GetAll();
    Task<bool> AddGame(AddGameInput gameInput);
    Task<ServiceResult<SingleGameDto>> GetGame(int gameId);
    Task<ServiceResult<SingleGameWithEventsDto>> GetGameWithEvents(int gameId, long accountId);
    Task<ServiceResult<SimpleGameDto>> GetSimpleGame(long gameId);
    Task<ServiceResult<GameBetsDto>> GetGameBets(long gameId);
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

    public async Task<ServiceResult<SingleGameDto>> GetGame(int gameId)
    {
        return ServiceResult<SingleGameDto>.WithSuccess(await _gamesRepository.GetGameById(gameId));
    }

    public async Task<ServiceResult<SingleGameWithEventsDto>> GetGameWithEvents(int gameId, long accountId)
    {
        var game = await _gamesRepository.GetGameWithEventsById(gameId, accountId);

        var result = ServiceResult<SingleGameWithEventsDto>.WithSuccess(game);

        return result;
    }

    
    public async Task<bool> AddGame(AddGameInput gameInput)
    {
        if (!_teamsRepository.Exists(gameInput.Team1Id) || !_teamsRepository.Exists(gameInput.Team2Id) || gameInput.Team1Id == gameInput.Team2Id)
        {
            return false;
        }
        else
        {
            await _gamesRepository.Add(new AddGameDto
            {
                Team1Id = gameInput.Team1Id,
                Team2Id = gameInput.Team2Id,
                GameDate = gameInput.GameDate,
                PhaseId = gameInput.PhaseId,
                PhaseName = gameInput.PhaseName
            });
            return true;
        }
    }

    public async Task<ServiceResult<IList<GetGameResultDto>>> Get(GetGamesInput input)
    {
        if (input == null)
        {
            return ServiceResult<IList<GetGameResultDto>>.WithErrors("Input can't be null");
        }

        var games = await _gamesRepository.Get(new GetGameDto
        {
            AccountId = input.AccountId,
            Phase = input.Phase,
            Beted = input.Beted,
            PhaseNames = input.PhaseNames,
            TeamIds = input.TeamIds
        });

        return ServiceResult<IList<GetGameResultDto>>.WithSuccess(games.ToList());
    }

    public async Task<ServiceResult<IList<GetGamesResult>>> GetAll()
    {
        var games = await _gamesRepository.GetAll();

        var result = ServiceResult<IList<GetGamesResult>>.WithSuccess(
            games.Select(x => new GetGamesResult
            {
                Id = x.Id,
                Team1 = x.Team1,
                Team2 = x.Team2
            }).ToList()
        );

        return result;
    }

    public async Task<ServiceResult<SimpleGameDto>> GetSimpleGame(long gameId)
    {
        var game = await _gamesRepository.GetSimpleGame(gameId);

        if (game == null)
        {
            return ServiceResult<SimpleGameDto>.WithErrors("Nie istnieje taki mecz");
        }

        return ServiceResult<SimpleGameDto>.WithSuccess(game);
    }

    public async Task<ServiceResult<GameBetsDto>> GetGameBets(long gameId)
    {
        var dto = await _gamesRepository.GetBetsForGame(gameId);

        if (dto == null)
        {
            return ServiceResult<GameBetsDto>.WithErrors("Nie istnieje taki mecz");
        }

        if (dto.GameId == 0)
        {
            return ServiceResult<GameBetsDto>.WithErrors("Mecz jeszcze się nie rozpoczął");
        }

        return ServiceResult<GameBetsDto>.WithSuccess(dto);
    }
}
