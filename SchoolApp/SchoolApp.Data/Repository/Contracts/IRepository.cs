using System.Linq.Expressions;

namespace SchoolApp.Data.Repository.Contracts;

public interface IRepository
{
    void Add<T>(T item) where T : class;
    Task AddAsync<T>(T item) where T : class;

    void AddRange<T>(ICollection<T> items) where T : class;
    Task AddRangeAsync<T>(ICollection<T> items) where T : class;

    bool Delete<T>(int id) where T : class;
    Task<bool> DeleteAsync<T>(int id) where T : class;

    bool DeleteByGuidId<T>(Guid id) where T : class;
    Task<bool> DeleteByGuidIdAsync<T>(Guid id) where T : class;

    void DeleteRange<T>(IEnumerable<T> entities) where T : class;
    Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class;

    IEnumerable<T> GetAll<T>() where T : class;
    Task<IEnumerable<T>> GetAllAsync<T>() where T : class;

    IQueryable<T> GetAllAttached<T>() where T : class;

    T? GetById<T>(int id) where T : class;
    Task<T?> GetByIdAsync<T>(int id) where T : class;

    T? GetByGuidId<T>(Guid id) where T : class;
    Task<T?> GetByGuidIdAsync<T>(Guid id) where T : class;

    T? FirstOrDefault<T>(Func<T, bool> predicate) where T : class;
    Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

    bool Update<T>(T item) where T : class;
    Task<bool> UpdateAsync<T>(T item) where T : class;

    //TODO to think about making save methods
}