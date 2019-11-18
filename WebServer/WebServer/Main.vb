Option Explicit On
Option Strict On

Public Class Main

    Public Shared Sub Main()
        Dim ws As WebServer = WebServer.getWebServer
        ws.VirtualRoot() = "C:/webserver_root/"
        ws.StartWebServer()
    End Sub

End Class
