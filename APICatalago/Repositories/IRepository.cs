using System.Linq.Expressions;

namespace APICatalago.Repositories
{
    public interface IRepository<T>
    {
        // cuidado para nao violar o principio ISP (todas as interfaces que herdarem dessa generica devem suar esses metodos)
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);

    }
}
