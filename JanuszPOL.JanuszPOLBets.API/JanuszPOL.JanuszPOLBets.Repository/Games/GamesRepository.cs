using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Games;

public interface IGamesRepository
{
    Task<GetGameResultDto[]> Get(GetGameDto dto);
    Task<GetGameResultDto[]> GetAll();

}
public class GamesRepository : IGamesRepository
{
    private readonly DataContext _db;

    public GamesRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<GetGameResultDto[]> Get(GetGameDto dto)
    {
        var games = await _db.Games
            .Where(x => string.IsNullOrEmpty(dto.NameContains) || x.Team1.Name.ToLower().Contains(dto.NameContains.ToLower()))
            .Where(x => string.IsNullOrEmpty(dto.NameStartsWith) || x.Team1.Name.ToLower().StartsWith(dto.NameStartsWith.ToLower()))
            .Select(x => new GetGameResultDto
            {
                GameId = x.Id,
                Team1Name = x.Team1.Name,
                Team2Name = x.Team2.Name,
                Winner = x.GameResultId.ToString(),
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
            GameId = x.Id,
            Team1Name = x.Team1.Name,
            Team2Name = x.Team2.Name,
            Winner = x.GameResultId.ToString(),
        }).ToArrayAsync();
    }
}
