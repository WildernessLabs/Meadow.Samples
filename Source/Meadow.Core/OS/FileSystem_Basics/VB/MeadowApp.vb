Imports System.IO
Imports System.Net.WebRequestMethods
Imports System.Threading
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds
Imports Meadow.Peripherals.Leds

Public Class MeadowApp
    'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
    Inherits App(Of F7FeatherV2)

    Public Overrides Function Run() As Task
        Console.WriteLine("Meadow File System Tests")
        ' list out the named directories available at MeadowOS.FileSystem.[x]
        EnumerateNamedDirectories()

        ' get the size of the `app.exe` file.
        FileStatus(Path.Combine(MeadowOS.FileSystem.UserFileSystemRoot, "App.exe"))

        ' create a `hello.txt` file in the `/Temp` directory
        CreateFile(MeadowOS.FileSystem.TempDirectory, "hello.txt")

        ' check on that file.
        FileStatus(Path.Combine(MeadowOS.FileSystem.TempDirectory, "hello.txt"))

        'Tree("/", true);
        ' write out a tree of all files in the user file system root
        Tree(MeadowOS.FileSystem.UserFileSystemRoot, True)

        Console.WriteLine("Testing complete")

        Return Task.CompletedTask
    End Function

    Public Sub EnumerateNamedDirectories()
        Console.WriteLine("The following named directories are available:")
        Console.WriteLine($"{vbTab} MeadowOS.FileSystem.UserFileSystemRoot: {MeadowOS.FileSystem.UserFileSystemRoot}")
        Console.WriteLine($"{vbTab} MeadowOS.FileSystem.CacheDirectory: {MeadowOS.FileSystem.CacheDirectory}")
        Console.WriteLine($"{vbTab} MeadowOS.FileSystem.DataDirectory: {MeadowOS.FileSystem.DataDirectory}")
        Console.WriteLine($"{vbTab} MeadowOS.FileSystem.DocumentsDirectory: {MeadowOS.FileSystem.DocumentsDirectory}")
        Console.WriteLine($"{vbTab} MeadowOS.FileSystem.TempDirectory: {MeadowOS.FileSystem.TempDirectory}")
    End Sub

    Public Sub CreateFile(path As String, filename As String)
        Console.WriteLine($"Creating '{path}/{filename}'...")

        If (Not Directory.Exists(path)) Then
            Console.WriteLine("Directory doesn't exist, creating.")
            Directory.CreateDirectory(path)
        End If

        Try
            Using fs = IO.File.CreateText(IO.Path.Combine(path, filename))
                fs.WriteLine("Hello Meadow File!")
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub FileStatus(path As String)
        Console.Write($"FileStatus() File: {IO.Path.GetFileName(path)} ")
        Try
            Using stream = IO.File.Open(path, FileMode.Open, FileAccess.Read)
                Console.WriteLine($"Size: {stream.Length,-8}")
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub Tree(root As String, Optional showSize As Boolean = False)
        Dim fileCount = 0
        Dim folderCount = 0

        ShowFolder(root, folderCount, fileCount, showSize, 0)
        Console.WriteLine(String.Empty)
        Console.WriteLine($"{folderCount} directories")
        Console.WriteLine($"{fileCount} files")
    End Sub

    Sub ShowFolder(folder As String, depth As Integer, folderCount As Integer, fileCount As Integer, showSize As Boolean, Optional last As Boolean = False)
        Dim files() As String = Nothing

        Try
            files = Directory.GetFiles(folder)
            Console.WriteLine($"{GetPrefix(depth, last AndAlso files.Length = 0)}{Path.GetFileName(folder)}")
        Catch
            Console.WriteLine($"{GetPrefix(depth, last)}{Path.GetFileName(folder)}")
            Console.WriteLine($"{GetPrefix(depth + 1, last)}<cannot list files>")
        End Try
        If (files IsNot Nothing) Then
            For Each file In files
                Dim prefix = GetPrefix(depth + 1, last)
                If (showSize) Then
                    Dim fi As FileInfo = Nothing
                    Try
                        fi = New FileInfo(file)
                        prefix += $"[{fi.Length,8}]  "
                    Catch
                        prefix += $"[   error]  "
                    End Try
                End If
                Console.WriteLine($"{prefix}{Path.GetFileName(file)}")
                fileCount += 1
            Next
        End If

        Dim dirs As String() = Nothing
        Try
            dirs = Directory.GetDirectories(folder)
        Catch
            If (files Is Nothing AndAlso files.Length = 0) Then
                Console.WriteLine($"{GetPrefix(depth + 1, last)}<cannot list sub-directories>")
            Else
                Console.WriteLine($"{GetPrefix(depth + 1)}<cannot list sub-directories>")
            End If
        End Try
        If (dirs IsNot Nothing) Then
            For i As Integer = 1 To dirs.Length - 1
                Console.WriteLine(i)
                ShowFolder(dirs(i), depth + 1, folderCount, fileCount, showSize, i = dirs.Length - 1)
                folderCount += 1
            Next i
        End If
    End Sub
    Function GetPrefix(d As Integer, Optional isLast As Boolean = False) As String
        Dim p = String.Empty

        For i As Integer = 0 To d - 1
            If (i = d - 1) Then
                p += "+--"
            ElseIf (isLast AndAlso i = d - 2) Then
                p += "   "
            Else
                p += "|  "
            End If
        Next i

        Return p
    End Function
End Class
