Imports System.IO
Imports Meadow
Imports Meadow.Logging

Public Class FileLogger
    Implements ILogProvider

    Private Property LogFilePath As String

    Public Sub New()
        LogFilePath = Path.Combine(MeadowOS.FileSystem.DocumentsDirectory, "meadow.log")

        If Not File.Exists(LogFilePath) Then
            File.Create(LogFilePath).Close()
        End If
    End Sub

    Public Sub Log(level As LogLevel, message As String) Implements ILogProvider.Log

        Select Case level
            Case LogLevel.Warning
            Case LogLevel.Error
                LogToFile(message)
        End Select
    End Sub

    Private Sub LogToFile(message As String)
        If message.EndsWith(Environment.NewLine) Then
            File.AppendAllText(LogFilePath, message)
        Else
            File.AppendAllText(LogFilePath, message + Environment.NewLine)
        End If
    End Sub

    Public Function GetLogContents() As String()
        If Not File.Exists(LogFilePath) Then
            Return New String() {}
        End If

        Return File.ReadLines(LogFilePath).ToArray()
    End Function

    Public Sub TruncateLog()
        File.Create(LogFilePath).Close()
    End Sub

End Class
