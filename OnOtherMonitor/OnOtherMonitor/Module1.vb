Option Explicit On
Option Strict On

Module Module1

    <Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowPos")>
    Public Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, x As Int32, y As Int32, cx As Int32, cy As Int32, wFlags As Int32) As IntPtr
    End Function

    Public Const SWP_NOMOVE As Short = &H2
    Public Const SWP_NOSIZE As Short = 1
    Public Const SWP_NOZORDER As Short = &H4
    Public Const SWP_SHOWWINDOW As Short = &H40
    ReadOnly HWND_BOTTOM As IntPtr = New IntPtr(1)

    Sub Main()

        Dim NewProcess As Process = Process.Start("explorer.exe")
        NewProcess.WaitForInputIdle()
        Dim handle As IntPtr = NewProcess.Handle
        If handle <> IntPtr.Zero Then
            SetWindowPos(handle, HWND_BOTTOM, 500, 400, 0, 0, SWP_NOZORDER Or SWP_NOSIZE Or SWP_SHOWWINDOW)
        End If

    End Sub

End Module
