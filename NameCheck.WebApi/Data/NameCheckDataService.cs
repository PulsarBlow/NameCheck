using SerialLabs;
using SerialLabs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public class NameCheckDataService : DataService<NameCheckModel, DescendingSortedGuid, NameCheckEntity>
    {
        protected BalancedPartitionKeyResolver PartitionKeyResolver = new BalancedPartitionKeyResolver();

        public NameCheckDataService(IRepository<NameCheckEntity> repository, IMapper<NameCheckModel, NameCheckEntity> mapper)
            : base(repository, mapper)
        { }

        public override async Task SaveAsync(NameCheckModel model)
        {
            Guard.ArgumentNotNull(model, "model");
            if (model.Id == DescendingSortedGuid.Empty)
            {
                model.Id = DescendingSortedGuid.NewSortedGuid();
            }
            if (model.DateUtc == DateTime.MinValue)
            {
                model.DateUtc = DateTime.UtcNow;
            }

            NameCheckEntity entity = Mapper.ToEntity(model);
            entity.PartitionKey = PartitionKeyResolver.Resolve(model.Id.ToString());
            entity.RowKey = model.Id.ToString();
            await Repository.SaveAsync(entity);
        }

        public override async Task<IList<NameCheckModel>> GetCollectionAsync(int? takeCount = null)
        {
            var entities = await Repository.GetCollectionAsync(takeCount);
            var models = Mapper.ToModel(entities);
            if (models != null)
            {
                models = models.OrderBy(x => x.Id.ToString()).ToList();
            }
            return models;
        }

        public override async Task<NameCheckModel> GetItemAsync(DescendingSortedGuid id)
        {
            var entity = await Repository.GetItemAsync(PartitionKeyResolver.Resolve(id.ToString()), id.ToString());
            return Mapper.ToModel(entity);
        }
    }
}