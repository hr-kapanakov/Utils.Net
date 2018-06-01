using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Managers.Tests
{
    [TestClass]
    public class DBConnectionManagerTests
    {
        #region Constants

        private const string TestDbFileName = "TestDatabase.sqlite";

        #endregion

        #region Members
      
        private readonly string testDbPath = 
            Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), TestDbFileName);

        private DBConnectionManager testManager;

        private KeyValuePair<string, int> testStoredRecord = new KeyValuePair<string, int>("Stored", 100);
        private KeyValuePair<string, int> testNewRecord = new KeyValuePair<string, int>("New", 50);
        private KeyValuePair<string, int> testParameterizedRecord = new KeyValuePair<string, int>("Parameterized", 10);

        #endregion

        [TestInitialize]
        public void SetUp()
        {
            CreateTestDatabase();
            testManager = new DBConnectionManager("Test");
        }

        [TestCleanup]
        public void CleanUp()
        {
            testManager.Disconnect();

            while (File.Exists(testDbPath))
            {
                try
                {
                    System.Threading.Thread.Sleep(200);
                    File.Delete(testDbPath);
                }
                catch (IOException)
                {
                }
            }
        }

        [TestMethod]
        public void DBConnectionManagerTest()
        {
            Assert.IsNotNull(testManager);
        }
        
        [TestMethod]
        public void ConnectTest()
        {
            if (testManager.IsConnected)
            {
                testManager.Disconnect();
            }

            testManager.Connect();
            Assert.IsTrue(testManager.IsConnected);
        }

        [TestMethod]
        public void DisconnectTest()
        {
            if (!testManager.IsConnected)
            {
                testManager.Connect();
            }

            testManager.Disconnect();
            Assert.IsFalse(testManager.IsConnected);
        }

        [TestMethod]
        public void ExecuteReadTest()
        {
            string sqlQuery = $"Select Name, Score From TestTable Where Name = '{testStoredRecord.Key}' And Score = {testStoredRecord.Value}";
            var reader = testManager.ExecuteRead(sqlQuery);
            Assert.IsTrue(reader.HasRows);
            reader.Close();
        }

        [TestMethod]
        public void ExecuteWriteTest()
        {
            string sqlQuery = $"Insert Into TestTable (Name, Score) Values ('{testNewRecord.Key}', {testNewRecord.Value})";
            testManager.ExecuteWrite(sqlQuery);

            sqlQuery = $"Select Name, Score From TestTable Where Name = '{testNewRecord.Key}' And Score = {testNewRecord.Value}";
            var reader = testManager.ExecuteRead(sqlQuery);
            Assert.IsTrue(reader.HasRows);
            reader.Close();
        }

        [TestMethod]
        public void ExecuteParameterizedWriteTest()
        {
            var insertQuery = $"INSERT INTO TestTable (Name, Score) VALUES ('{testParameterizedRecord.Key}', @Score);";
            var parameterizedCommand = new SQLiteCommand(insertQuery);
            parameterizedCommand.Parameters.Add(string.Format("@{0}", "Score"), System.Data.DbType.Int32, 20).Value = 10;
            testManager.ExecuteParameterizedWrite(parameterizedCommand);

            string sqlQuery = $"Select Name, Score From TestTable Where Name = '{testParameterizedRecord.Key}' And Score = {testParameterizedRecord.Value}";
            var reader = testManager.ExecuteRead(sqlQuery);
            Assert.IsTrue(reader.HasRows);
            reader.Close();
        }

        // [TestMethod]
        public void CreateTestDatabase()
        {
            SQLiteConnection.CreateFile(testDbPath);

            using (var tmpConn = new SQLiteConnection())
            {
                tmpConn.ConnectionString = $"Data Source={TestDbFileName};Version=3;";
                tmpConn.Open();

                string sqlCreateTableQuery = "CREATE TABLE TestTable (" +
                                             "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                             "Name VARCHAR(45) NOT NULL DEFAULT ''," +
                                             "Score INT NULL)";

                var createTableCommand = new SQLiteCommand(sqlCreateTableQuery, tmpConn);
                createTableCommand.ExecuteNonQuery();
                createTableCommand.Dispose();

                var sqlInsertQuery = $"Insert Into TestTable (Name, Score) Values ('{testStoredRecord.Key}', {testStoredRecord.Value})";

                var insertRecordCommand = new SQLiteCommand(sqlInsertQuery, tmpConn);
                insertRecordCommand.ExecuteNonQuery();
                insertRecordCommand.Dispose();
            }
        }
    }
}
