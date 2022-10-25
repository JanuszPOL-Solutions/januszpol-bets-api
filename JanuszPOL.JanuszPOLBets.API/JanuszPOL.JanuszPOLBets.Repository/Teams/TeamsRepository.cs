using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Repository.Teams.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Teams;

public interface ITeamsRepository
{
    Task<GetTeamResultDto[]> Get(GetTeamDto dto);
    bool Exists(long id);
}

public class TeamsRepository : ITeamsRepository
{
    private readonly DataContext _db;

    public TeamsRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<GetTeamResultDto[]> Get(GetTeamDto dto)
    {
        var teams = await _db.Teams
            .Select(x => new GetTeamResultDto
            {
                TeamId = x.Id,
                Name = x.Name,
                FlagUrl = x.FlagUrl
            })
            .ToArrayAsync();

        return teams;
    }

    public bool Exists(long id)
    {
        return _db.Teams.Any(x => x.Id == id);
    }
}
