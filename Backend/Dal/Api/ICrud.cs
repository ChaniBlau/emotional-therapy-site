using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Api;

public interface ICrud<T> where T : class
{
    Task<bool> CreateAsync(T entity);
    Task<T> ReadByIdAsync(string id);
    Task<IEnumerable<T>> ReadAllAsync();
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
}
