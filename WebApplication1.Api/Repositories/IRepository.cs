using WebApplication1.API.Domain;

namespace WebApplication1.API.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> GetByIdAsync(Guid id);
    public Task<T> UpdateAsync(T entity);
    public Task<T> AddAsync(T entity);
    public Task<bool> DeleteAsync(Guid id);
    public Task<Studio> GetGameStudioAsync(Guid id);
}