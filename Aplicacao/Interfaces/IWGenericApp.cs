using System.Linq.Expressions;

namespace Aplicacao.Interfaces
{
    public interface IWGenericApp<T> where T : class
    {
        Task Add(T obj);
        Task Update(T obj);
        Task Delete(T obj);
        Task<List<T?>> GetListAsync();
        Task<T?> GetEntidadeByExpressionAsync(Expression<Func<T, bool>> exT, bool AsNoTracking = true);
    }
}
