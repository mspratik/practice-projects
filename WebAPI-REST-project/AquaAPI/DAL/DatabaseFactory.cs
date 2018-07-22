using System;
using System.Configuration;
using System.Reflection;

namespace AquaAPI.DAL
{
    public sealed class DatabaseFactory
    {
        public static DatabaseFactorySectionHandler sectionHandler = (DatabaseFactorySectionHandler)ConfigurationManager.GetSection("databaseFactoryConfiguration");
        private DatabaseFactory() { }
        public static Database CreateDatabase()
        {
            // Verify a DatabaseFactoryConfiguration line exists in the web.config.
            if (sectionHandler.Name.Length == 0)
            {
                throw new Exception("Database name not defined in DatabaseFactoryConfiguration section of web.config.");
            }

            try
            {
                // Find the class
                Type database = Type.GetType(sectionHandler.Name);
                // Get it's constructor
                ConstructorInfo constructor = database.GetConstructor(new Type[] { });
                // Invoke it's constructor, which returns an instance.
                Database createdObject = (Database)constructor.Invoke(null);
                // Initialize the connection string property for the database.
                createdObject.connectionString = sectionHandler.ConnectionString;
                // Pass back the instance as a Database
                return createdObject;
            }
            catch (Exception ex)
            {
                throw new Exception("Error instantiating database " + sectionHandler.Name + ". " + ex.Message);
            }
        }
    }
}