using System.Data.Common;

namespace Utils.Net.Interfaces
{
    /// <summary>
    /// Provides functionality for work with common data providers.
    /// </summary>
    public interface IDBConnectionManager
    {
        /// <summary>
        /// Gets a value indicating whether the connection is open.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a read command and generating an error.
        /// Default value is 30 seconds.
        /// </summary>
        int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a write command and generating an error.
        /// Default value is 60 seconds.
        /// </summary>
        int WriteTimeout { get; set; }


        /// <summary>
        /// Establishes new connection.
        /// </summary>
        void Connect();

        /// <summary>
        /// Terminates current connection.
        /// </summary>
        void Disconnect();


        /// <summary>
        /// Executes a sql query with the given statement.
        /// </summary>
        /// <param name="sqlStatement">Query statement.</param>
        /// <returns>DataReader object.</returns>
        DbDataReader ExecuteRead(string sqlStatement);

        /// <summary>
        /// Executes non query with the given statement.
        /// </summary>
        /// <param name="sqlStatement">Non query statement.</param>
        void ExecuteWrite(string sqlStatement);

        /// <summary>
        /// Execute parameterized non query with given parameterized command.
        /// </summary>
        /// <param name="parameterizedCommand">DbCommand to be executed.</param>
        void ExecuteParameterizedWrite(DbCommand parameterizedCommand);
    }
}
