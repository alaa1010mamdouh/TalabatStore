using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class BaseSpcification<T> : ISpecification<T> where T : BaseEntity
    {

        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get ; set ; }= new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public void SetOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;

        }

        public void SetOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        //Get All
        public BaseSpcification()
        {
           // Includes = new List<Expression<Func<T, object>>>();
           
        }
        //Get By Id
        public BaseSpcification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
           // Includes=new List<Expression<Func<T, object>>>();
        }
        public void ApplyPagination (int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPaginationEnabled = true;

        }
    }
}
