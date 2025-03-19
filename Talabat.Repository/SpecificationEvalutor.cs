using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        // function to built Qurey

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,ISpecification <T> spec)
        {
            var query = inputQuery;// _storeContext.Set<T>()
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);// _soreContext.Where() 

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
          

                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
