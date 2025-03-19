using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Core.Repositores
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        #region WithOutSpecification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region WithSpecification
        Task<IReadOnlyList<T>> GetAllWithSpecification(ISpecification<T> spec);

        Task<T> GetByEntityWithSpecification(ISpecification<T> spec);
        Task<int> GetCountWithSpecification(ISpecification<T> spec);

        Task AddAsync(T Item);
        void Update(T Item);
        void Delete(T Item);

        #endregion
    }
}
