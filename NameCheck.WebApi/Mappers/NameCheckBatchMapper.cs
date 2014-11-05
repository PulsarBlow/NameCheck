using Newtonsoft.Json;
using SuperMassive;
using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class NameCheckBatchMapper : IMapper<NameCheckBatchModel, NameCheckBatchEntity>
    {
        public NameCheckBatchModel ToModel(NameCheckBatchEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            List<NameCheckModel> nameChecks = JsonConvert.DeserializeObject<List<NameCheckModel>>(entity.NameChecksJson);

            return new NameCheckBatchModel
            {
                Id = DescendingSortedGuid.Parse(entity.RowKey),
                Value = entity.Value,
                UserIp = entity.UserIp,
                DateUtc = entity.DateUtc,
                EndpointType = (EndpointType)Enum.Parse(typeof(EndpointType), entity.EndpointType),
                NameChecks = nameChecks
            };
        }

        public NameCheckBatchEntity ToEntity(NameCheckBatchModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new NameCheckBatchEntity
            {
                Value = model.Value,
                UserIp = model.UserIp,
                DateUtc = model.DateUtc,
                EndpointType = model.EndpointType.ToString(),
                NameChecksJson = JsonConvert.SerializeObject(model.NameChecks),
            };
        }

        public IList<NameCheckBatchModel> ToModel(IList<NameCheckBatchEntity> entityCollection)
        {
            if (entityCollection == null)
            {
                return null;
            }
            List<NameCheckBatchModel> result = new List<NameCheckBatchModel>();
            foreach (var item in entityCollection)
            {
                result.AddIfNotNull(ToModel(item));
            }
            return result;
        }

        public IList<NameCheckBatchEntity> ToEntity(IList<NameCheckBatchModel> modelCollection)
        {
            if (modelCollection == null)
            {
                return null;
            }
            List<NameCheckBatchEntity> result = new List<NameCheckBatchEntity>();
            foreach (var item in modelCollection)
            {
                result.AddIfNotNull(ToEntity(item));
            }
            return result;
        }
    }
}