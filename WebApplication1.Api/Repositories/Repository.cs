using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApplication1.API.Data;
using WebApplication1.API.Domain;

namespace WebApplication1.API.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private AppDbContext _context;

    public Repository()
    {
        _context = new AppDbContext();
    }
    
    public Repository( AppDbContext context )
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var allGames = _context.Set<T>().AsNoTracking();
        return await allGames.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        var query = _context.Set<T>().AsNoTracking();
        var entity = await query.FirstOrDefaultAsync(e => e.Id == id);
        
        return entity
               ?? throw new KeyNotFoundException($"Game with id {id} not found.");
    }
    
    public async Task<Studio> GetGameStudioAsync(Guid id)
    {
        var query = _context.Set<Game>().AsNoTracking();
        var game = await query.FirstOrDefaultAsync(e => e.Id == id);
        
        if (game == null)
            throw new KeyNotFoundException($"Game with id {id} not found.");
        var query2 = _context.Set<Studio>().AsNoTracking();
        var studio = await query2.FirstOrDefaultAsync(e => e.Id == game.StudioId);
        if (studio == null)
            throw new KeyNotFoundException($"Studio with id {game.StudioId} not found.");
        return studio;
    }
    
    public async Task<T> UpdateAsync(T entity)
    {
        entity.Update();
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

public async Task<T> AddAsync(T entity)
{
    if (entity == null)
        throw new ArgumentNullException(nameof(entity), $"{typeof(T).Name} não pode ser nulo.");

    if (entity is Game newGame)
    {
        var studio = await _context.Set<Studio>().FindAsync(newGame.StudioId)
                     ?? throw new KeyNotFoundException($"Studio com id {newGame.StudioId} não encontrado.");

        newGame.Studio = studio;
        studio.Games?.Add(newGame);
        await _context.Set<Game>().AddAsync(newGame);
        _context.Entry(studio).State = EntityState.Modified;
    }
    else
    {
        _context.Set<T>().Add(entity);
    }

    await _context.SaveChangesAsync();
    return entity;
}

    public async Task<bool> DeleteAsync(Guid id)
    {
        var game = await _context.Set<T>().FindAsync(id)
                   ?? throw new KeyNotFoundException($"Game with id {id} not found.");
        _context.Set<T>().Remove(game);
        await _context.SaveChangesAsync();
        return true;
    }

}