using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Repository.Teams.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Teams;

public interface ITeamsRepository
{
    Task<GetTeamResultDto[]> Get(GetTeamDto dto);
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
            .Where(x => string.IsNullOrEmpty(dto.NameContains) || x.Name.ToLower().Contains(dto.NameContains.ToLower()))
            .Where(x => string.IsNullOrEmpty(dto.NameStartsWith) || x.Name.ToLower().StartsWith(dto.NameStartsWith.ToLower()))
            .Select(x => new GetTeamResultDto
            {
                TeamId = x.Id,
                Name = x.Name,
                FlagUrl = x.FlagUrl
            })
            .Skip(dto.Skip)
            .Take(dto.Limit)
            .ToArrayAsync();

        return teams;
    }
}
