Option Explicit On
Option Strict On

Public Class cSerFormatWriter

    Dim FileIO As System.IO.FileStream
    Dim BinaryOUT As System.IO.BinaryWriter

    Public Property ImageWidth As Int32 = 0
    Public Property ImageHeight As Int32 = 0
    Public Property PixelDepthPerPlane As Integer = 0
    Public Property FrameCount As Integer = 0
    Public Property Observer As String = String.Empty
    Public Property Instrument As String = String.Empty
    Public Property Telescope As String = String.Empty
    Public Property DateTimeLocalRaw As Int64 = 0
    Public Property DateTimeUTCRaw As Int64 = 0

    Public Sub InitForWrite(ByVal NewSERFile As String)

        FileIO = New System.IO.FileStream(NewSERFile, IO.FileMode.Create, IO.FileAccess.Write)
        BinaryOUT = New System.IO.BinaryWriter(FileIO)

        BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes("LUCAM-RECORDER"))              'Header
        BinaryOUT.Write(BitConverter.GetBytes(CType(4660, Int32)))                          'LuID
        BinaryOUT.Write(BitConverter.GetBytes(CType(8, Int32)))                             'ColorID
        BinaryOUT.Write(BitConverter.GetBytes(CType(0, Int32)))                             'LittleEndian
        BinaryOUT.Write(BitConverter.GetBytes(CType(ImageWidth, Int32)))                    'ImageWidth
        BinaryOUT.Write(BitConverter.GetBytes(CType(ImageHeight, Int32)))                   'ImageHeight
        BinaryOUT.Write(BitConverter.GetBytes(CType(PixelDepthPerPlane, Int32)))            'PixelDepthPerPlane
        BinaryOUT.Write(BitConverter.GetBytes(CType(FrameCount, Int32)))                    'FrameCount
        BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes(Observer.PadRight(40)))         'Observer
        BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes(Instrument.PadRight(40)))       'Instrument
        BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes(Telescope.PadRight(40)))        'Telescope
        BinaryOUT.Write(BitConverter.GetBytes(CType(DateTimeLocalRaw, Int64)))              'FrameCount
        BinaryOUT.Write(BitConverter.GetBytes(CType(DateTimeUTCRaw, Int64)))                'FrameCount

    End Sub

    Public Sub AppendFrame(ByRef Frame(,) As Short)
        For Idx1 As Integer = 0 To Frame.GetUpperBound(0)
            For Idx2 As Integer = 0 To Frame.GetUpperBound(1)
                BinaryOUT.Write(BitConverter.GetBytes(Frame(Idx1, Idx2)))
            Next Idx2
        Next Idx1
    End Sub

    Public Sub CloseSerFile()
        BinaryOUT.Flush()
        FileIO.Flush()
        BinaryOUT.Close()
        FileIO.Close()
    End Sub

End Class
