using JanuszPOL.JanuszPOLBets.Repository.Games;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Services.Games
{

    public interface IGamesService
    {
        Task<ServiceResult<IList<GetGamesResult>>> Get(GetGamesInput input);
        Task<ServiceResult<IList<GetGamesResult>>> GetAll();

    }

    public class GamesService : IGamesService
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesService(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
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
}
