Imports Meadow
Imports Meadow.Devices

Public Class MeadowApp
    'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
    Inherits App(Of F7FeatherV2)

    Public Overrides Function Initialize() As Task
        Resolver.Log.Info($"Initializing...")

        Dim fileLogger = New FileLogger()

        ' output the log contents just for display.  Do it before adding the logger so we don't recurse
        Dim lineNumber = 1
        Dim contents = fileLogger.GetLogContents()
        If contents.Length > 0 Then
            Resolver.Log.Info($"Log contents{Environment.NewLine}------------")

            For Each line In contents
                Resolver.Log.Info($"{lineNumber:000}> {line}")
                lineNumber = lineNumber + 1
            Next
            Resolver.Log.Info($"------------")
        Else
            Resolver.Log.Info($"Log is empty")
        End If

        ' an our own logger to the system logger
        Resolver.Log.AddProvider(fileLogger)

        Return Task.CompletedTask
    End Function

    Public Overrides Function Run() As Task
        Resolver.Log.Info("Run... (VB.NET)")

        Resolver.Log.Info("This will not end up in the file")

        ' prefix a random number just so we can see differences
        Dim r As New Random()
        Resolver.Log.Warn($"But this will [{r.Next(0, 1000)}]")

        Return Task.CompletedTask
    End Function

End Class