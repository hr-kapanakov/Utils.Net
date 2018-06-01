using System.Configuration;
using System.Data;
using System.Data.Common;
using Utils.Net.Interfaces;

namespace Utils.Net.Managers
{
    /// <summary>
    /// Provides functionality for work with common data providers.
    /// </summary>
    public class DBConnectionManager : IDBConnectionManager
    {
        #region Members

        private readonly object locker = new object();

        private ConnectionStringSettings connectionStringSettings;
        private DbProviderFactory factory;
        private DbConnection connection;

        #endregion

        /// <summary>
        /// Gets a value indicating whether the connection is open.
        /// </summary>
        public bool IsConnected => connection != null && connection.State == ConnectionState.Open;

        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a read command and generating an error.
        /// Default value is 30 seconds.
        /// </summary>
        public int ReadTimeout { get; set; } = 30;

        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a write command and generating an error.
        /// Default value is 60 seconds.
        /// </summary>
        public int WriteTimeout { get; set; } = 60;


        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnectionManager" /> class.
        /// </summary>
        /// <param name="configurationConnStringName">Name of the connection string in app or web config file.</param>
        public DBConnectionManager(string configurationConnStringName)
        {
            // Here is an example of the connectionString in app or web config file 
            // <? xml version = "1.0" ?>
            // <configuration>
            //     <appSettings>
            //         < !--all your application settings -->
            //     </appSettings>
            //     <connectionStrings>
            //         <add name = "MySQLDataBase" providerName = "MySql.Data.MySqlClient"
            //             connectionString = "User ID=root;Password=myPassword;Host=localhost;Port=3306;
            //             Database = myDataBase; Direct = true; Protocol = TCP; Compress = false; Pooling = true;
            //             Min Pool Size = 0; Max Pool Size = 100; Connection Lifetime = 0; "/>
            //     </connectionStrings >
            // </configuration >

            connectionStringSettings = ConfigurationManager.ConnectionStrings[configurationConnStringName];
            factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
        }


        /// <summary>
        /// Establishes new connection.
        /// </summary>
        public void Connect()
        {
            if (IsConnected)
            {
                return;
            }

            connection = factory.CreateConnection();
            connection.ConnectionString = connectionStringSettings.ConnectionString;
            connection.Open();
        }

        /// <summary>
        /// Terminates current connection.
        /// </summary>
        public void Disconnect()
        {
            if (connection == null)
            {
                return;
            }

            connection.Close();
            connection = null;
        }


        /// <summary>
        /// Executes a sql query with the given statement.
        /// </summary>
        /// <param name="sqlStatement">Query statement.</param>
        /// <returns>DataReader object.</returns>
        public DbDataReader ExecuteRead(string sqlStatement)
        {
            Connect();

            DbDataReader reader;
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sqlStatement;
                cmd.CommandTimeout = ReadTimeout;

                reader = cmd.ExecuteReader(CommandBehavior.Default);
            }
            
            return reader;
        }

        /// <summary>
        /// Executes non query with the given statement.
        /// </summary>
        /// <param name="sqlStatement">Non query statement.</param>
        public void ExecuteWrite(string sqlStatement)
        {
            Connect();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStatement;
                cmd.CommandTimeout = WriteTimeout;

                lock (locker)
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Execute parameterized non query with given parameterized command.
        /// </summary>
        /// <param name="parameterizedCommand">DbCommand to be executed.</param>
        public void ExecuteParameterizedWrite(DbCommand parameterizedCommand)
        {
            Connect();

            using (parameterizedCommand)
            {
                parameterizedCommand.Connection = connection;
                parameterizedCommand.CommandType = CommandType.Text;
                parameterizedCommand.CommandTimeout = WriteTimeout;

                lock (locker)
                {
                    parameterizedCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
