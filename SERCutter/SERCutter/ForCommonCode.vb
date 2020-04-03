Option Explicit On
Option Strict On

Public Class ForCommonCode

    Public Shared Function SlidingAVG(ByRef Data() As Long, ByVal WindowLength As Integer, ByRef PeakPos As Integer) As Long()

        Dim RetVal(Data.GetUpperBound(0)) As Long
        Dim Sum As Long = 0
        Dim Peak As Long = Long.MinValue : PeakPos = Integer.MinValue
        Dim MinIdx As Integer = -1
        'Init for first value
        For Idx As Integer = 0 To WindowLength - 1
            Sum += Data(Idx)
            RetVal(Idx) = Sum
        Next Idx


        For Idx As Integer = WindowLength To Data.GetUpperBound(0)
            MinIdx += 1                                     'Calculate Idx to subtract
            Sum += Data(Idx)                                'Add new value
            Sum -= Data(MinIdx)                             'Subtract value left of window
            RetVal(Idx) = Sum                               'Remember return value
            If Sum > Peak Then                              'If a new peak was found
                Peak = Sum                                  'Store peak value
                PeakPos = Idx                               'Store peak index
            End If
        Next Idx

        Return RetVal                                       'Return complete array

    End Function

End Class
