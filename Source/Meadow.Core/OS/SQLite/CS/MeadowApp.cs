using Meadow;
using Meadow.Devices;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SQLite_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        int InsertCount = 10;

        SQLiteConnection Database { get; set; }
        
        public double SensorValue { get; set; }

        public override Task Run()
        {
            // set an initial dummy sensor value
            SensorValue = 42.42;

            try
            {
                // configure the database and open a connection
                ConfigureDatabase();
                // add some dummy sensor readings
                InsertDummyData();
                // read out the data
                RetreiveData();

                // update a record
                UpdateData();

                RetreiveData();

                // retreive by primary key
                RetrieveByPrimaryKey();

                RetrieveViaLinqQuery();

                RetrieveViaTSqlQuery();

                DeleteARow();

            }
            catch (Exception ex)
            {
                Resolver.Log.Info($"Problem: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Resolver.Log.Info($"Inner exception: {ex.InnerException.Message}");
                }
            }

            return Task.CompletedTask;
        }

        void ConfigureDatabase()
        {
            // by default, SQLite runs in `Serialized` mode, which is thread safe.
            // if you need to change the threading mode, you can do it with the
            // following API
            //SQLite3.Config(SQLite3.ConfigOption.SingleThread);

            // database files should go in the `DataDirectory`
            var databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "MySqliteDatabase.db");
            // make the connection
            Database = new SQLiteConnection(databasePath);
            // add table(s)
            Database.CreateTable<SensorModel>();
        }

        void InsertDummyData()
        {
            for (int i = 0; i < InsertCount; i++)
            {
                Resolver.Log.Info($"Inserting row {i + 1}...");
                Database.Insert(new SensorModel { Timestamp = DateTime.Now, Value = SensorValue });
                Thread.Sleep(100);
                SensorValue += 1.23;
            }
        }

        void RetreiveData()
        {
            Resolver.Log.Info("Reading back the data...");
            var rows = Database.Table<SensorModel>();
            foreach (var r in rows)
            {
                Resolver.Log.Info($"Reading was {r.Value} at {r.Timestamp.ToString("HH:mm:ss")}");
            }
        }

        void UpdateData()
        {
            // pull the first record out of the table
            SensorModel reading = Database.Table<SensorModel>().Take(1).First();

            Resolver.Log.Info($"Found a record, ID: {reading.ID}");

            // change the value
            reading.Value = reading.Value * 2;

            // update the data
            Database.Update(reading);
        }

        void RetrieveByPrimaryKey()
        {
            SensorModel firstRow = Database.Table<SensorModel>().Take(1).First();
            var sensorReading1 = Database.Get<SensorModel>(firstRow.ID);
            Resolver.Log.Info($"Sensor Reading 1: {sensorReading1.Value}");
        }

        void RetrieveViaSearchPredicate()
        {
            var firstSensorReadingOver50 = Database.Get<SensorModel>(reading => reading.Value > 50);
            Resolver.Log.Info($"found a sensor reading over 50; ID: {firstSensorReadingOver50.ID}, value: {firstSensorReadingOver50.Value}");
        }

        void RetrieveViaLinqQuery()
        {
            Resolver.Log.Info("RetrieveViaLinqQuery()");
            var readings = from rows in Database.Table<SensorModel>()
                           where rows.Value > 50
                           select rows;
            Resolver.Log.Info($"Found {readings.Count()} readings over 50: ");
            foreach (var reading in readings)
            {
                Resolver.Log.Info($"ID: {reading.ID}, value: {reading.Value}");
            }
        }

        void RetrieveViaTSqlQuery()
        {
            Resolver.Log.Info("RetrieveViaTSqlQuery()");
            var readings = Database.Query<SensorModel>("SELECT * FROM SensorReadings WHERE value > ?", 50);
            Resolver.Log.Info($"Found {readings.Count()} readings over 50: ");
            foreach (var reading in readings)
            {
                Resolver.Log.Info($"ID: {reading.ID}, value: {reading.Value}");
            }
        }

        void DeleteARow()
        {
            // pull the first record out of the table
            SensorModel reading = Database.Table<SensorModel>().Take(1).First();
            Resolver.Log.Info($"First record ID: {reading.ID}");
            Database.Delete<SensorModel>(reading.ID);
            Resolver.Log.Info($"Deleted the record");
            // get the first record again
            reading = Database.Table<SensorModel>().Take(1).First();
            Resolver.Log.Info($"new first record ID: {reading.ID}");
        }
    }
}
