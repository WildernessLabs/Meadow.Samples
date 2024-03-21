Imports SQLite

<Table("SensorReadings")>
Public Class SensorModel
    <PrimaryKey, AutoIncrement>
    Public Property ID As Integer
    Public Property Timestamp As DateTime
    Public Property Value As Double
End Class
