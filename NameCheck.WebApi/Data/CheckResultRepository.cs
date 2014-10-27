

namespace NameCheck.WebApi
{
    public class CheckResultRepository : BaseRepository<CheckResultModel, CheckResultEntity>
    {

        public CheckResultRepository(string storageConnectionString)
            : base(storageConnectionString, "CheckResults")
        { }

    }
}