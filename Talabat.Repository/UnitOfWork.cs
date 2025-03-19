using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositores;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext DbContext)
        {
            _repositories=new Hashtable();
            _dbContext = DbContext;
        }
        public async Task<int> CompleteAsync()
         => await  _dbContext.SaveChangesAsync();
    

        public async ValueTask DisposeAsync()
         =>await _dbContext.DisposeAsync();
        

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Type=typeof(TEntity).Name;
            if (!_repositories.ContainsKey(Type))
            {
                var Repository=new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(Type, Repository);
                
            }
            return _repositories[Type] as IGenericRepository<TEntity>;

        }
    }
}
