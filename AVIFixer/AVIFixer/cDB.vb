Option Explicit On
Option Strict On

Public Class cDB

    Public ChunkMoves As New Dictionary(Of Integer, Integer)

    Public Property FFMPEGExe As String = "C:\bin\ffmpeg.exe"
    Public Property OverwriteFiles As Boolean = True

End Class
