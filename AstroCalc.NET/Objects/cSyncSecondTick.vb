Option Explicit On
Option Strict On

'''<summary>Timer that raises ticks in 1 second delta sync with the system time (ms are tried to be 0).</summary>
Public Class cSyncSecondTick : Inherits Timer

    Private NextExpectedTime As DateTime
    Private IsInit As Boolean = False

    Public Shadows Event Tick(ByVal sender As Object, ByVal e As System.EventArgs)

    '''<summary>Interval is fixed to 1000.</summary>
    Public Overloads Property Interval() As Integer
        Get
            Return 1000
        End Get
        Set(ByVal value As Integer)
            MyBase.Interval = 1000
        End Set
    End Property

    Public Sub New()
        MyBase.Interval = 1000
    End Sub

    Private Sub cSyncTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Tick
        Try
            Dim ThisTime As DateTime = Now
            If Not IsInit Then
                'First call - calc initial delta and to not tick now
                Dim DeltaMilliSeconds As Integer = 1000 - ThisTime.Millisecond
                NextExpectedTime = ThisTime.AddMilliseconds(DeltaMilliSeconds)                        'next tick expected when the ms as 1000 -> next full second
                If DeltaMilliSeconds > 0 Then
                    MyBase.Interval = DeltaMilliSeconds                                                   'wait this time
                    IsInit = True
                Else
                    ReInit()
                End If
            Else
                '2nd and further calls
                Dim DeltaMilliSeconds As Integer = CInt((Now - NextExpectedTime).TotalMilliseconds)   'error (positiv if call was too early)
                NextExpectedTime = NextExpectedTime.AddSeconds(1)                                     'next expected call is 1 second further
                If DeltaMilliSeconds < 1000 Then
                    MyBase.Interval = 1000 - DeltaMilliSeconds                                            'adjust interval length
                    RaiseEvent Tick(Me, System.EventArgs.Empty)                                           'raise the tick
                Else
                    ReInit()
                End If
            End If
        Catch ex As Exception
            'Re-init
            ReInit()
        End Try
    End Sub

    Private Sub ReInit()
        IsInit = False
        MyBase.Interval = 1000
    End Sub

End Class