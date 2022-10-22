using JanuszPOL.JanuszPOLBets.Data._DbContext;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
using Microsoft.EntityFrameworkCore;

namespace JanuszPOL.JanuszPOLBets.Repository.Games;

public interface IGamesRepository
{
    Task<GetGameResultDto[]> Get(GetGameDto dto);
    Task<GetGameResultDto[]> GetAll();
    void Add(AddGameDto gameDto);
    void Update(UpdateGameDto updateDto);

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
