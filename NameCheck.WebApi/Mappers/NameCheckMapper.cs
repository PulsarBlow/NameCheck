using Newtonsoft.Json;
using SerialLabs;
using SerialLabs.Data;
using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class NameCheckMapper : IMapper<NameCheckModel, NameCheckEntity>
    {
        public NameCheckModel ToModel(NameCheckEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            Dictionary<string, bool> socialNetworks = JsonConvert.DeserializeObject<Dictionary<string, bool>>(entity.SocialNetworksJson);
            Dictionary<string, bool> domains = JsonConvert.DeserializeObject<Dictionary<string, bool>>(entity.DomainsJson);

            return new NameCheckModel
            {
                Id = DescendingSortedGuid.Parse(entity.RowKey),
                Name = entity.Name,
                Query = entity.Query,
                DateUtc = entity.DateUtc,
                EndpointType = (EndpointType)Enum.Parse(typeof(EndpointType), entity.EndpointType),
                SocialNetworks = socialNetworks,
                Domains = domains
            };
        }

        public NameCheckEntity ToEntity(NameCheckModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new NameCheckEntity
            {
                Name = model.Name,
                Query = model.Query,
                DateUtc = model.DateUtc,
                EndpointType = model.EndpointType.ToString(),
                SocialNetworksJson = JsonConvert.SerializeObject(model.SocialNetworks),
                DomainsJson = JsonConvert.SerializeObject(model.Domains)
            };
        }

        public IList<NameCheckModel> ToModel(IList<NameCheckEntity> entityCollection)
        {
            if (entityCollection == null)
            {
                return null;
            }
            List<NameCheckModel> result = new List<NameCheckModel>();
            foreach (var item in entityCollection)
            {
                result.AddIfNotNull(ToModel(item));
            }
            return result;
        }

        public IList<NameCheckEntity> ToEntity(IList<NameCheckModel> modelCollection)
        {
            if (modelCollection == null)
            {
                return null;
            }
            List<NameCheckEntity> result = new List<NameCheckEntity>();
            foreach (var item in modelCollection)
            {
                result.AddIfNotNull(ToEntity(item));
            }
            return result;
        }
    }
}