Option Explicit On
Option Strict On

Module MainModule

    'Show untracked files:
    'status -s -u

    Sub Main()

        Do

            Dim Commands As New List(Of String)
            Console.WriteLine("Available commands:")
            Console.WriteLine("  p - Pull-Merge")
            Console.WriteLine("  s - Status")
            Console.WriteLine("  c - Commit & push")
            Console.WriteLine("  x - Exit")
            Console.Write("  Selection: ")
            Select Case Console.ReadKey.Key
                Case ConsoleKey.P
                    Commands.Add("pull")
                Case ConsoleKey.S
                    Commands.Add("status -s")
                Case ConsoleKey.C
                    Commands.Add("add *")
                    Commands.Add("commit -m " & InputBox("Commit message:", "Commit message", "Updates"))
                    Commands.Add("push")
                Case ConsoleKey.X
                    Exit Do
            End Select
            Console.Clear()

            Dim GitRoot As String = "C:\GIT"
            For Each Directory As String In System.IO.Directory.GetDirectories(GitRoot)
                Dim GitRepo As String = Directory.Replace(GitRoot, String.Empty)
                'Only enter directories that are GIT repos
                If System.IO.Directory.Exists(System.IO.Path.Combine(Directory, ".git")) Then
                    Console.WriteLine("   +++++ <" & GitRepo & "> +++++")
                    For Each GITCommand As String In Commands
                        Dim Answer As String = RunGitCommand(Directory, GITCommand).TrimEnd(New Char() {Chr(10), Chr(13)})
                        Select Case GITCommand
                            Case "pull"
                                If Answer <> "Already up to date." Then
                                    Console.WriteLine(">> " & GitRepo)
                                    Console.WriteLine(Answer)
                                End If
                            Case "status -s"
                                If Answer.Length > 0 Then
                                    Console.WriteLine(GitRepo & " -> " & Answer)
                                End If
                            Case "push"
                                Console.WriteLine(Answer)
                        End Select
                    Next GITCommand
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
