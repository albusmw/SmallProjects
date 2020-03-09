Option Explicit On
Option Strict On

Module MainModule

    'Show untracked files:
    'status -s -u

    Sub Main()

        Dim GitRoot As String = "C:\GIT"
        For Each Directory As String In System.IO.Directory.GetDirectories(GitRoot)
            If System.IO.Directory.Exists(System.IO.Path.Combine(Directory, ".git")) Then
                Console.WriteLine(">>" & Directory.Replace(GitRoot, String.Empty))
                RunGitCommand(Directory)
            End If
        Next Directory

        Console.WriteLine("OK")
        Console.ReadKey()

    End Sub

    Private Sub RunGitCommand(ByVal Repo As String)

        Dim gitInfo As New ProcessStartInfo
        With gitInfo
            .CreateNoWindow = False
            .UseShellExecute = False
            .RedirectStandardError = True
            .RedirectStandardOutput = True
            .FileName = "C:\Program Files\Git\bin\git.exe"
            .Arguments = "status -s -u"
            .WorkingDirectory = Repo
        End With
        Dim gitProcess As New Process
        With gitProcess
            .StartInfo = gitInfo
            .Start() : .WaitForExit()
            Dim stderr_str As String = gitProcess.StandardError.ReadToEnd()
            Dim stdout_str As String = gitProcess.StandardOutput.ReadToEnd()
            Console.WriteLine(stdout_str)
            gitProcess.WaitForExit()
            gitProcess.Close()
        End With

    End Sub

End Module
