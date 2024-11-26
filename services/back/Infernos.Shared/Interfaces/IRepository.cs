using Infernos.Shared.Responses;
using System.Linq.Expressions;

namespace Infernos.Shared.Interfaces;

interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    Task<Response> CreateAsync(T entry);
    Task<Response> UpdateAsync(T entry);
    Task<Response> DeleteAsync(T entry);
}