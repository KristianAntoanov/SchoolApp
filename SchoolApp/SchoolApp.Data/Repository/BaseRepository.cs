using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Repository.Contracts;

namespace SchoolApp.Data.Repository
{
    public class BaseRepository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<T> GetDbSet<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public void Add<T>(T item) where T : class
        {
            GetDbSet<T>().Add(item);
            _dbContext.SaveChanges();
        }

        public async Task AddAsync<T>(T item) where T : class
        {
            await GetDbSet<T>().AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public void AddRange<T>(ICollection<T> items) where T : class
        {
            GetDbSet<T>().AddRange(items);
            _dbContext.SaveChanges();
        }

        public async Task AddRangeAsync<T>(ICollection<T> items) where T : class
        {
            await GetDbSet<T>().AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
        }

        public bool Delete<T>(int id) where T : class
        {
            T? entity = GetById<T>(id);

            if (entity == null)
            {
                return false;
            }

            GetDbSet<T>().Remove(entity);
            _dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            T? entity = await GetByIdAsync<T>(id);

            if (entity == null)
            {
                return false;
            }

            GetDbSet<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public bool DeleteByGuidId<T>(Guid id) where T : class
        {
            T? entity = GetByGuidId<T>(id);

            if (entity == null)
            {
                return false;
            }

            GetDbSet<T>().Remove(entity);
            _dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteByGuidIdAsync<T>(Guid id) where T : class
        {
            T? entity = await GetByGuidIdAsync<T>(id);

            if (entity == null)
            {
                return false;
            }

            GetDbSet<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            GetDbSet<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            GetDbSet<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return GetDbSet<T>().ToArray();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await GetDbSet<T>().ToArrayAsync();
        }

        public IQueryable<T> GetAllAttached<T>() where T : class
        {
            return GetDbSet<T>().AsQueryable();
        }

        public T? GetById<T>(int id) where T : class
        {
            return GetDbSet<T>().Find(id);
        }

        public async Task<T?> GetByIdAsync<T>(int id) where T : class
        {
            return await GetDbSet<T>().FindAsync(id);
        }

        public T? GetByGuidId<T>(Guid id) where T : class
        {
            return GetDbSet<T>().Find(id);
        }

        public async Task<T?> GetByGuidIdAsync<T>(Guid id) where T : class
        {
            return await GetDbSet<T>().FindAsync(id);
        }

        public T? FirstOrDefault<T>(Func<T, bool> predicate) where T : class
        {
            return GetDbSet<T>().FirstOrDefault(predicate);
        }

        public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await GetDbSet<T>().FirstOrDefaultAsync(predicate);
        }

        public bool Update<T>(T item) where T : class
        {
            try
            {
                GetDbSet<T>().Attach(item);
                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync<T>(T item) where T : class
        {
            try
            {
                GetDbSet<T>().Attach(item);
                _dbContext.Entry(item).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}