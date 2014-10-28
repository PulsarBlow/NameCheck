

namespace NameCheck.WebApi
{
    public class NameCheckRepository : BaseRepository<NameCheckModel, NameCheckEntity>
    {

        public NameCheckRepository(string storageConnectionString)
            : base(storageConnectionString, "NameChecks")
        { }

    }
}