using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SerialLabs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public interface IRepository<TEntity>
        where TEntity : ITableEntity
    {
        Task SaveAsync(TEntity entity);
        Task<IList<TEntity>> GetCollectionAsync(int? takeCount = null);
        Task<TEntity> GetItemAsync(string partitionKey, string rowKey);
    }

    public abstract class BaseRepository<TModel, TEntity> : IRepository<TEntity>
         where TEntity : class, ITableEntity, new()
    {
        protected CloudTable Table { get; private set; }

        protected BaseRepository(string storageConnectionString, string tableName)
        {
            Guard.ArgumentNotNullOrWhiteSpace(storageConnectionString, "storageConnectionString");
            Guard.ArgumentNotNullOrWhiteSpace(storageConnectionString, "tableName");

            Table = GetTableReference(storageConnectionString, tableName);
        }

        public async Task SaveAsync(TEntity entity)
        {
            Guard.ArgumentNotNull(entity, "entity");

            Table.CreateIfNotExists();
            TableOperation operation = TableOperation.InsertOrMerge(entity);
            await Table.ExecuteAsync(operation);
        }
        public async Task<IList<TEntity>> GetCollectionAsync(int? takeCount = 100)
        {
            TableQuery<TEntity> query = new TableQuery<TEntity>().Take(takeCount);
            List<TEntity> result = new List<TEntity>();
            TableQuerySegment<TEntity> currentSegment = null;
            while (currentSegment == null || currentSegment.ContinuationToken != null)
            {
                currentSegment = await Table.ExecuteQuerySegmentedAsync<TEntity>(query, currentSegment != null ? currentSegment.ContinuationToken : null);
                result.AddRangeIfNotNull(currentSegment.Results);
            }
            return result;
        }
        public async Task<TEntity> GetItemAsync(string partitionKey, string rowKey)
        {
            TableResult result = await Table.ExecuteAsync(TableOperation.Retrieve<TEntity>(partitionKey, rowKey));
            return result.Result as TEntity;
        }


        protected static CloudTable GetTableReference(string storageConnectionString, string tableName)
        {
            Guard.ArgumentNotNullOrWhiteSpace(storageConnectionString, "storageConnectionString");
            Guard.ArgumentNotNullOrWhiteSpace(storageConnectionString, "tableName");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            return client.GetTableReference(tableName);
        }
    }
}