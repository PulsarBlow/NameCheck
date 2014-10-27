
using System.Collections.Generic;
namespace NameCheck.WebApi
{
    public interface IMapper<TModel, TEntity>
    {
        TModel ToModel(TEntity entity);
        IList<TModel> ToModel(IList<TEntity> entityCollection);
        TEntity ToEntity(TModel model);
        IList<TEntity> ToEntity(IList<TModel> modelCollection);
    }
}