using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        //Signature for where
      public Expression<Func<T,bool>> Criteria { get; set; }
        //Signature for include

        public List<Expression<Func<T, object>>> Includes { get; set; }
        //Signature for order by
        public Expression<Func<T, object>> OrderBy { get; set; }
        //Signature for order by descending
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        //TaKe
        public int Take { get; set; }
        //Skip
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

    }
}
