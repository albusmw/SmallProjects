Option Explicit On
Option Strict On

Partial Public Class cSERFormat

    Public Class cSerFormatWriter

        Dim FileIO As System.IO.FileStream
        Dim BinaryOUT As System.IO.BinaryWriter

        Public Property Header As New cSERHeader

        Public Function InitForWrite(ByVal NewSERFile As String) As Boolean

            If IsNothing(Header) = True Then Return False

            FileIO = New System.IO.FileStream(NewSERFile, IO.FileMode.Create, IO.FileAccess.Write)
            BinaryOUT = New System.IO.BinaryWriter(FileIO)

            BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes("LUCAM-RECORDER"))                  'Header
            BinaryOUT.Write(BitConverter.GetBytes(CType(4660, Int32)))                              'LuID
            BinaryOUT.Write(BitConverter.GetBytes(CType(0, Int32)))                                 'ColorID
            BinaryOUT.Write(BitConverter.GetBytes(CType(0, Int32)))                                 'LittleEndian
            BinaryOUT.Write(BitConverter.GetBytes(CType(Header.FrameWidth, Int32)))                 'ImageWidth
            BinaryOUT.Write(BitConverter.GetBytes(CType(Header.FrameHeight, Int32)))                'ImageHeight
            BinaryOUT.Write(BitConverter.GetBytes(CType(Header.PixelDepthPerPlane, Int32)))         'PixelDepthPerPlane
            BinaryOUT.Write(BitConverter.GetBytes(CType(Header.FrameCount, Int32)))                 'FrameCount
            BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes(Header.Observer.PadRight(40)))      'Observer
            BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes(Header.Instrument.PadRight(40)))    'Instrument
            BinaryOUT.Write(System.Text.Encoding.ASCII.GetBytes(Header.Telescope.PadRight(40)))     'Telescope
            BinaryOUT.Write(BitConverter.GetBytes(CType(Header.DateTimeLocalRaw, Int64)))           'FrameCount
            BinaryOUT.Write(BitConverter.GetBytes(CType(Header.DateTimeUTCRaw, Int64)))             'FrameCount

            Return True

        End Function

        Public Sub AppendFrame(ByRef Frame(,) As UInt16)
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

End Class

