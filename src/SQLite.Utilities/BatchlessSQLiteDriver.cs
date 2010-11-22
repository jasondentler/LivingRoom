using NHibernate.Driver;

namespace SQLite.Utilities
{
    public class BatchlessSQLiteDriver : SQLite20Driver 
    {

        public override bool SupportsMultipleQueries
        {
            get
            {
                return false;
            }
        }

    }
}
