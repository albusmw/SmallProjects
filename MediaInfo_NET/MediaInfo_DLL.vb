Partial Public Class MediaInfo

    Private Const DLLName As String = "MediaInfo.dll"

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Sub MediaInfo_Close(ByVal Handle As IntPtr)
    End Sub

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_Count_Get(ByVal Handle As IntPtr, ByVal StreamKind As UIntPtr, ByVal StreamNumber As IntPtr) As UIntPtr
    End Function

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Sub MediaInfo_Delete(ByVal Handle As IntPtr)
    End Sub

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_Get(ByVal Handle As IntPtr, ByVal StreamKind As UIntPtr, ByVal StreamNumber As UIntPtr, <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.VBByRefStr)> ByRef Parameter As String, ByVal KindOfInfo As UIntPtr, ByVal KindOfSearch As UIntPtr) As IntPtr
    End Function

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_GetI(ByVal Handle As IntPtr, ByVal StreamKind As UIntPtr, ByVal StreamNumber As UIntPtr, ByVal Parameter As UIntPtr, ByVal KindOfInfo As UIntPtr) As IntPtr
    End Function

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_Inform(ByVal Handle As IntPtr, ByVal Reserved As UIntPtr) As IntPtr
    End Function

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_New() As IntPtr
    End Function

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_Open(ByVal Handle As IntPtr, <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.VBByRefStr)> ByRef FileName As String) As UIntPtr
    End Function

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_Option(ByVal Handle As IntPtr, <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.VBByRefStr)> ByRef Option_ As String, <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.VBByRefStr)> ByRef Value As String) As IntPtr
    End Function

    <Runtime.InteropServices.DllImport(DLLName, CharSet:=Runtime.InteropServices.CharSet.Unicode, SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function MediaInfo_State_Get(ByVal Handle As IntPtr) As UIntPtr
    End Function

End Class
