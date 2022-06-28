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
            //TODO: add validation
            var teams = await _teamsRepository.Get(new GetTeamDto());
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
