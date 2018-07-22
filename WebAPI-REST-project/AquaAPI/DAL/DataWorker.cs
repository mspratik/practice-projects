using System;

namespace AquaAPI.DAL
{
    public class DataWorker
    {
        private static Database _database = null;
        static DataWorker()
        {
            try
            {
                _database = DatabaseFactory.CreateDatabase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Database database
        {
            get { return _database; }
        }
    }
}