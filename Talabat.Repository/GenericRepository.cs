using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositores;
using Talabat.Core.Specification;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        #region WithOutSpecification
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))

            //    return (IReadOnlyList<T>)await _storeContext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            //else
                return await _storeContext.Set<T>().ToListAsync();

        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _storeContext.Set<T>().FindAsync(id);

            // return await _storeContext.Products.Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync();

        }
        #endregion

        #region WithSpecification
        public async Task<T> GetByEntityWithSpecification(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecification(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_storeContext.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpecification(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task AddAsync(T Item)
        =>await _storeContext.Set<T>().AddAsync(Item);

        public void Update(T Item)
       => _storeContext.Set<T>().Update(Item);

        public void Delete(T Item)
        => _storeContext.Set<T>().Update(Item);




        #endregion

    }
}
