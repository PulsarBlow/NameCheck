using SerialLabs;
using SerialLabs.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public class CheckResultDataService : DataService<CheckResultModel, DescendingSortedGuid, CheckResultEntity>
    {
        protected BalancedPartitionKeyResolver PartitionKeyResolver = new BalancedPartitionKeyResolver();

        public CheckResultDataService(IRepository<CheckResultEntity> repository, IMapper<CheckResultModel, CheckResultEntity> mapper)
            : base(repository, mapper)
        { }

        public override async Task SaveAsync(CheckResultModel model)
        {
            Guard.ArgumentNotNull(model, "model");
            if (model.Id == null)
            {
                model.Id = DescendingSortedGuid.NewSortedGuid();
            }
            if (model.DateUtc == DateTime.MinValue)
            {
                model.DateUtc = DateTime.UtcNow;
            }
            CheckResultEntity entity = Mapper.ToEntity(model);
            entity.PartitionKey = PartitionKeyResolver.Resolve(model.Id.ToString());
            entity.RowKey = model.Id.ToString();
            await Repository.SaveAsync(entity);
        }

        public override async Task<IList<CheckResultModel>> GetCollectionAsync(int? takeCount = null)
        {
            var entities = await Repository.GetCollectionAsync(takeCount);
            return Mapper.ToModel(entities);

        }

        public override async Task<CheckResultModel> GetItemAsync(DescendingSortedGuid id)
        {
            var entity = await Repository.GetItemAsync(PartitionKeyResolver.Resolve(id.ToString()), id.ToString());
            return Mapper.ToModel(entity);
        }
    }
}