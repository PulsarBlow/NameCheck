
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
            return new CheckResultModel
            {
                Name = entity.Name,
                DateUtc = entity.DateUtc,
                Result = new AvailabilityResult
                {
                    DomainCom = entity.IsDomainComAvailable,
                    Twitter = entity.IsTwitterAvailable,
                    Facebook = entity.IsFacebookAvailable
                }
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
                IsDomainComAvailable = model.Result != null ? model.Result.DomainCom : false,
                IsTwitterAvailable = model.Result != null ? model.Result.Twitter : false,
                IsFacebookAvailable = model.Result != null ? model.Result.Facebook : false
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