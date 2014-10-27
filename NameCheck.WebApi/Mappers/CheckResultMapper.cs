
using Newtonsoft.Json;
using SerialLabs;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class CheckResultMapper : IMapper<CheckResultModel, CheckResultEntity>
    {
        public CheckResultModel ToModel(CheckResultEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            Dictionary<string, bool> extensions = JsonConvert.DeserializeObject<Dictionary<string, bool>>(entity.Extensions);

            return new CheckResultModel
            {
                Name = entity.Name,
                DateUtc = entity.DateUtc,
                Twitter = entity.Twitter,
                Extensions = extensions
            };
        }

        public CheckResultEntity ToEntity(CheckResultModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckResultEntity
            {
                Name = model.Name,
                DateUtc = model.DateUtc,
                Twitter = model.Result != null ? model.Result.Twitter : false,
                Extensions = JsonConvert.SerializeObject(model.Extensions)
            };
        }


        public IList<CheckResultModel> ToModel(IList<CheckResultEntity> entityCollection)
        {
            if (entityCollection == null)
            {
                return null;
            }
            List<CheckResultModel> result = new List<CheckResultModel>();
            foreach (var item in entityCollection)
            {
                result.AddIfNotNull(ToModel(item));
            }
            return result;
        }

        public IList<CheckResultEntity> ToEntity(IList<CheckResultModel> modelCollection)
        {
            if (modelCollection == null)
            {
                return null;
            }
            List<CheckResultEntity> result = new List<CheckResultEntity>();
            foreach (var item in modelCollection)
            {
                result.AddIfNotNull(ToEntity(item));
            }
            return result;
        }
    }
}