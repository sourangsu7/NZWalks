namespace NZWalks.API.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(Guid id, T entity);
        Task<T> Delete(Guid id);
    }
}
