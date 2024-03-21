Imports System.IO
Imports System.Net.WebRequestMethods
Imports System.Threading
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds
Imports Meadow.Peripherals.Leds
Imports SQLite

Public Class MeadowApp
    'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
    Inherits App(Of F7FeatherV2)

    Dim InsertCount As Integer = 10
    Property Database As SQLiteConnection
    Property SensorValue As Double

    Public Overrides Function Run() As Task
        ' set an initial dummy sensor value
        SensorValue = 42.42

        Try
            ' configure the database And open a connection
            ConfigureDatabase()
            ' add some dummy sensor readings
            InsertDummyData()
            ' read out the data
            RetreiveData()

            ' update a record
            UpdateData()

            RetreiveData()

            ' retreive by primary key
            RetrieveByPrimaryKey()

            RetrieveViaLinqQuery()

            RetrieveViaTSqlQuery()

            DeleteARow()

        Catch ex As Exception
            Console.WriteLine($"Problem: {ex.Message}")
            If (ex.InnerException IsNot Nothing) Then
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}")
            End If
        End Try

        Return Task.CompletedTask
    End Function

    Sub ConfigureDatabase()
        ' by default, SQLite runs in `Serialized` mode, which is thread safe.
        ' if you need to change the threading mode, you can do it with the
        ' following API
        'SQLite3.Config(SQLite3.ConfigOption.SingleThread);

        ' database files should go in the `DataDirectory`
        Dim databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "MySqliteDatabase.db")
        ' make the connection
        Database = New SQLiteConnection(databasePath)
        ' add table(s)
        Database.CreateTable(Of SensorModel)()
        end Sub

    Sub InsertDummyData()
        For i As Integer = 1 To InsertCount
            Console.WriteLine($"Inserting row {i + 1}...")
            Dim sensorEntry = New SensorModel()
            With sensorEntry
                .Timestamp = DateTime.Now
                .Value = SensorValue
            End With
            Database.Insert(sensorEntry)
            Thread.Sleep(100)
            SensorValue += 1.23
        Next i
    End Sub

    Sub RetreiveData()
        Console.WriteLine("Reading back the data...")
        Dim rows = Database.Table(Of SensorModel)()
        For Each r In rows
            Console.WriteLine($"Reading was {r.Value} at {r.Timestamp.ToString("HH:mm:ss")}")
        Next
    End Sub

    Sub UpdateData()
        ' pull the first record out of the table
        Dim reading As SensorModel = Database.Table(Of SensorModel)().Take(1).First()

        Console.WriteLine($"Found a record, ID: {reading.ID}")

        ' change the value
        reading.Value = reading.Value * 2

        ' update the data
        Database.Update(reading)
    End Sub

    Sub RetrieveByPrimaryKey()
        Dim firstRow As SensorModel = Database.Table(Of SensorModel)().Take(1).First()
        Dim sensorReading1 = Database.Get(Of SensorModel)(firstRow.ID)
        Console.WriteLine($"Sensor Reading 1: {sensorReading1.Value}")
    End Sub

    Sub RetrieveViaSearchPredicate()
        Dim firstSensorReadingOver50 = Database.Get(Of SensorModel)(Function(reading) reading.Value > 50)
        Console.WriteLine($"found a sensor reading over 50; ID: {firstSensorReadingOver50.ID}, value: {firstSensorReadingOver50.Value}")
    End Sub

    Sub RetrieveViaLinqQuery()
        Console.WriteLine("RetrieveViaLinqQuery()")
        Dim readings = From rows In Database.Table(Of SensorModel)()
                       Where rows.Value > 50
                       Select rows
        Console.WriteLine($"Found {readings.Count()} readings over 50: ")
        For Each reading In readings
            Console.WriteLine($"ID: {reading.ID}, value: {reading.Value}")
        Next
    End Sub

    Sub RetrieveViaTSqlQuery()
        Console.WriteLine("RetrieveViaTSqlQuery()")
        Dim readings = Database.Query(Of SensorModel)("SELECT * FROM SensorReadings WHERE value > ?", 50)
        Console.WriteLine($"Found {readings.Count()} readings over 50: ")
        For Each reading In readings
            Console.WriteLine($"ID: {reading.ID}, value: {reading.Value}")
        Next
    End Sub

    Sub DeleteARow()
        ' pull the first record out of the table
        Dim reading As SensorModel = Database.Table(Of SensorModel)().Take(1).First()
        Console.WriteLine($"First record ID: {reading.ID}")
        Database.Delete(Of SensorModel)(reading.ID)
        Console.WriteLine($"Deleted the record")
        ' get the first record again
        reading = Database.Table(Of SensorModel)().Take(1).First()
        Console.WriteLine($"new first record ID: {reading.ID}")
    End Sub
End Class