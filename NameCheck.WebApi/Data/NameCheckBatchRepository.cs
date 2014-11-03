

namespace NameCheck.WebApi
{
    public class NameCheckBatchRepository : BaseRepository<NameCheckBatchModel, NameCheckBatchEntity>
    {

        public NameCheckBatchRepository(string storageConnectionString)
            : base(storageConnectionString, "NameCheckBatches")
        { }

    }
}