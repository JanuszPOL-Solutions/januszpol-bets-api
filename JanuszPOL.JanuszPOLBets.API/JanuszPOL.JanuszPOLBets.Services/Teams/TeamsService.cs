using JanuszPOL.JanuszPOLBets.Repository.Teams;
using JanuszPOL.JanuszPOLBets.Repository.Teams.Dto;
using JanuszPOL.JanuszPOLBets.Services.Common;
using JanuszPOL.JanuszPOLBets.Services.Teams.ServiceModels;

namespace JanuszPOL.JanuszPOLBets.Services.Teams
{
    public interface ITeamsService
    {
        Task<ServiceResult<IList<GetTeamsResult>>> Get(GetTeamsInput input);
    }

    public class TeamsService : ITeamsService
    {
        private readonly ITeamsRepository _teamsRepository;

        public TeamsService(ITeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        public async Task<ServiceResult<IList<GetTeamsResult>>> Get(GetTeamsInput input)
        {
            if (input == null)
            {
                //TODO: maybe it shoulkd throw exception and catch it in middleware instead
                return ServiceResult<IList<GetTeamsResult>>.WithErrors("Input can't be null");
            }

            if (input.Skip < 0)
            {
                return ServiceResult<IList<GetTeamsResult>>.WithErrors("Skip value must be at least 0");
            }

            if (input.Limit < 1)
            {
                return ServiceResult<IList<GetTeamsResult>>.WithErrors("Skip value must be at least 1");
            }

            var teams = await _teamsRepository.Get(new GetTeamDto
            {
                NameContains = input.NameContains,
                Limit = input.Limit,
                Skip = input.Skip,
                NameStartsWith = input.NameStartsWith
            });

            var result = ServiceResult<IList<GetTeamsResult>>.WithSuccess(
                teams.Select(x => new GetTeamsResult
                {
                    TeamId = x.TeamId,
                    Name = x.Name,
                    FlagUrl = x.FlagUrl
                }).ToList()
            );

            return result;
        }
    }
}
