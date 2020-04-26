Option Explicit On
Option Strict On

Public Class Globals

    Public Shared MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

    Public Shared Property Latitude() As Double
        Get
            Return MyLatitude
        End Get
        Set(value As Double)
            MyLatitude = value
        End Set
    End Property
    Private Shared MyLatitude As Double = 48 + (8 / 60) + (13.94 / 3600)

    Public Shared Property Longitude() As Double
        Get
            Return MyLongitude
        End Get
        Set(value As Double)
            MyLongitude = value
        End Set
    End Property
    Private Shared MyLongitude As Double = 11 + (34 / 60) + (31.98 / 3600)

    Public Shared Property Elevation() As Double
        Get
            Return MyElevation
        End Get
        Set(value As Double)
            MyElevation = value
        End Set
    End Property
    Private Shared MyElevation As Double = 515

End Class
