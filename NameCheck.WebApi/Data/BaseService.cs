using Microsoft.WindowsAzure.Storage.Table;
using SuperMassive;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public interface IDataService<TModel, TId>
    {
        Task SaveAsync(TModel model);
        Task<IList<TModel>> GetCollectionAsync(int? takeCount = null);
        Task<TModel> GetItemAsync(TId id);
    }

    public abstract class DataService<TModel, TModelId, TEntity> : IDataService<TModel, TModelId>
        where TEntity : class, ITableEntity, new()
    {
        protected IRepository<TEntity> Repository;
        protected IMapper<TModel, TEntity> Mapper;

        protected DataService(IRepository<TEntity> repository, IMapper<TModel, TEntity> mapper)
        {
            Guard.ArgumentNotNull(repository, "repository");
            Repository = repository;
            Guard.ArgumentNotNull(mapper, "mapper");
            Mapper = mapper;
        }

        public abstract Task SaveAsync(TModel model);


        public abstract Task<IList<TModel>> GetCollectionAsync(int? takeCount = null);

        public abstract Task<TModel> GetItemAsync(TModelId id);
    }
}