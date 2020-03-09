Option Explicit On
Option Strict On

Module MainModule

    'Show untracked files:
    'status -s -u

    Sub Main()

        Do

            Dim Command As String = String.Empty
            Console.WriteLine("p - Pull-Merge")
            Console.WriteLine("u - Untracked / modified files")
            Console.WriteLine("s - Status")
            Console.WriteLine("x - Exit")
            Console.WriteLine("Selection: ")
            Select Case Console.ReadKey.Key
                Case ConsoleKey.P
                    Command = "pull"
                Case ConsoleKey.U
                    Command = "status -s -u"
                Case ConsoleKey.S
                    Command = "status"
                Case ConsoleKey.X
                    Exit Do
            End Select
            Console.Clear()
            Console.WriteLine("---------------------------------------------------")

            Dim GitRoot As String = "C:\GIT"
            For Each Directory As String In System.IO.Directory.GetDirectories(GitRoot)
                Dim GitRepo As String = ">>" & Directory.Replace(GitRoot, String.Empty)
                If System.IO.Directory.Exists(System.IO.Path.Combine(Directory, ".git")) Then
                    Dim Answer As String = RunGitCommand(Directory, Command).TrimEnd(New Char() {Chr(10), Chr(13)})
                    Select Case Command
                        Case "pull"
                            If Answer <> "Already up to date." Then
                                Console.WriteLine(">> " & GitRepo)
                                Console.WriteLine(Answer)
                            End If
                        Case "status -s -u"
                            If Answer.Length > 0 Then
                                Console.WriteLine(">> " & GitRepo)
                                Console.WriteLine(Answer)
                            End If
                        Case Else
                            Console.WriteLine(">> " & GitRepo)
                    End Select
                End If
            Next Directory

            Console.WriteLine("=================================================")

        Loop Until 1 = 0

    End Sub

    Private Function RunGitCommand(ByVal Repo As String, ByVal Command As String) As String

        Dim stdout_str As String = String.Empty
        Dim gitInfo As New ProcessStartInfo
        With gitInfo
            .CreateNoWindow = False
            .UseShellExecute = False
            .RedirectStandardError = True
            .RedirectStandardOutput = True
            .FileName = "C:\Program Files\Git\bin\git.exe"
            .Arguments = Command
            .WorkingDirectory = Repo
        End With
        Dim gitProcess As New Process
        With gitProcess
            .StartInfo = gitInfo
            .Start() : .WaitForExit()
            Dim stderr_str As String = gitProcess.StandardError.ReadToEnd()
            stdout_str = gitProcess.StandardOutput.ReadToEnd()
            If stderr_str.Length > 0 Then Console.WriteLine(stderr_str)
            gitProcess.WaitForExit()
            gitProcess.Close()
        End With
        Return stdout_str

    End Function

End Module
