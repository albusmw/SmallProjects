Option Explicit On
Option Strict On

Public Class PyEphem

    'Call PyEphem (taken from http://rhodesmill.org/pyephem/)

    Private Shared PythonPath As String = String.Empty

    Public Shared Sub ConvertEpoche(ByVal Original As sGeneric, ByRef Target As sGeneric, ByVal Epo_original As Integer, ByVal Epo_targer As Integer)
        Dim PyCode As New List(Of String)
        Dim Original_ra As String = Str(Original.RightAscension).Trim
        Dim Original_dec As String = Str(Original.Declination).Trim
        PyCode.Add("import ephem")
        PyCode.Add("pos_orig = ephem.Equatorial('" & Original_ra & "', '" & Original_dec & "', epoch='" & Epo_original.ToString.Trim & "')")
        PyCode.Add("print pos_orig.ra, pos_orig.dec")
        PyCode.Add("result = ephem.Equatorial(pos_orig, epoch='" & Epo_targer.ToString.Trim & "')")
        PyCode.Add("print result.ra, result.dec")
        Dim ErrorLog As String = String.Empty                                                           'error code
        Dim Result As String() = Split(ExecutePython(PyCode, ErrorLog), System.Environment.NewLine)     'run
        Dim TargetOut As String() = Split(Result(1), " ")
        Target.RightAscension = GetPyRightAscension(TargetOut(0))
        Target.Declination = GetPyDeclination(TargetOut(1))
    End Sub

    Public Shared Function GetJulianDate(ByVal UTC As DateTime) As Double
        Dim PyCode As New List(Of String)
        PyCode.Add("import ephem")                                              'import ephem data
        PyCode.Add("print ephem.julian_date('" & Util.PyEphemDate(UTC) & "')")  'call julian date function
        Dim ErrorLog As String = String.Empty                                   'error code
        Dim Result As String = ExecutePython(PyCode, ErrorLog)                  'run
        Return Val(Result.Trim)
    End Function

    Public Shared Function CheckIfInstalled() As String
        Return CheckIfInstalled(String.Empty)
    End Function

    Public Shared Function CheckIfInstalled(ByVal ForcePythonPath As String) As String

        'Load python path if not present
        If String.IsNullOrEmpty(ForcePythonPath) = False Then
            PythonPath = ForcePythonPath
        Else
            If String.IsNullOrEmpty(PythonPath) = True Then
                PythonPath = GetPythonPath()
            End If
        End If
        If String.IsNullOrEmpty(PythonPath) = True Then Return "Python is NOT installed!"

        'If python is installed, check if ephem is installed
        Dim PyCode As New List(Of String)
        PyCode.Add("try:")                                      'try to
        PyCode.Add("  import ephem")                            'import ephem data
        PyCode.Add("  print('Installed')")                       'print OK
        PyCode.Add("except ImportError, e:")                    'catch
        PyCode.Add("  print('NOT Installed')")                   'print NOT OK
        Dim ErrorLog As String = String.Empty                   'error code
        Dim Result As String = ExecutePython(PyCode, ErrorLog)  'RUN

        If Result.Trim = "Installed" Then
            'Everything is OK
            Return String.Empty
        Else
            Return "PyEphem is NOT installed!"
        End If

    End Function

    Public Shared Function GetSolarParams() As Dictionary(Of String, String)

        Dim PyCode As New List(Of String)
        Dim ErrorLog As String
        Dim ReturnArg As String()

        'Basic calculation for solar parameters
        PyCode.Clear()
        LoadLocation(PyCode)
        PyCode.Add("sun = ephem.Sun()")
        PyCode.Add("sun.compute(MyLocation)")
        PyCode.Add("print(MyLocation.previous_rising(sun))")     'rising - previous
        PyCode.Add("print(MyLocation.next_rising(sun))")         'rising - next
        PyCode.Add("print(MyLocation.previous_setting(sun))")    'setting - previous
        PyCode.Add("print(MyLocation.next_setting(sun))")        'setting - next
        PyCode.Add("print(sun.alt, sun.az)")                     'solar position
        ErrorLog = String.Empty
        ReturnArg = Split(ExecutePython(PyCode, ErrorLog), System.Environment.NewLine)

        'Get raising and setting times
        Dim PrevRising As DateTime = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(0)))
        Dim NextRising As DateTime = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(1)))
        Dim PrevSetting As DateTime = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(2)))
        Dim NextSetting As DateTime = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(3)))

        Dim RetVal As New Dictionary(Of String, String)

        'Check if it is day or night now
        Dim ItIsDay As Boolean
        If PrevRising.Day = Now.Day And NextSetting.Day = Now.Day Then ItIsDay = True Else ItIsDay = False

        'Next, compute twilight ...
        'http://rhodesmill.org/pyephem/rise-set.html#computing-twilight
        Dim TwilightOK As Boolean = False
        Dim AstroDawnBegin As DateTime
        Dim AstroDawnEnd As DateTime

        If ItIsDay Then
            'It is day now
            PyCode.Clear()
            LoadLocation(PyCode)
            PyCode.Add("MyLocation.horizon = '-6'")                                                         'civil twilight
            PyCode.Add("print MyLocation.next_setting(ephem.Sun(), use_center=True)")
            PyCode.Add("print MyLocation.next_rising(ephem.Sun(), use_center=True)")
            PyCode.Add("MyLocation.horizon = '-12'")                                                        'nautical twilight 
            PyCode.Add("print MyLocation.next_setting(ephem.Sun(), use_center=True)")
            PyCode.Add("print MyLocation.next_rising(ephem.Sun(), use_center=True)")
            PyCode.Add("MyLocation.horizon = '-18'")                                                        'astronomical twilight 
            PyCode.Add("print MyLocation.next_setting(ephem.Sun(), use_center=True)")
            PyCode.Add("print MyLocation.next_rising(ephem.Sun(), use_center=True)")
            ErrorLog = String.Empty
            ReturnArg = Split(ExecutePython(PyCode, ErrorLog), System.Environment.NewLine)
            If ReturnArg.Length >= 6 Then
                AstroDawnBegin = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(4)))
                AstroDawnEnd = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(5)))
                TwilightOK = True
            End If
        Else
            'It is night now
            PyCode.Clear()
            LoadLocation(PyCode)
            PyCode.Add("MyLocation.horizon = '-6'")                                                         'civil twilight
            PyCode.Add("print MyLocation.previous_setting(ephem.Sun(), use_center=True)")
            PyCode.Add("print MyLocation.next_rising(ephem.Sun(), use_center=True)")
            PyCode.Add("MyLocation.horizon = '-12'")                                                        'nautical twilight
            PyCode.Add("print MyLocation.previous_setting(ephem.Sun(), use_center=True)")
            PyCode.Add("print MyLocation.next_rising(ephem.Sun(), use_center=True)")
            PyCode.Add("MyLocation.horizon = '-18'")                                                        'astronomical twilight
            PyCode.Add("print MyLocation.previous_setting(ephem.Sun(), use_center=True)")
            PyCode.Add("print MyLocation.next_rising(ephem.Sun(), use_center=True)")
            ErrorLog = String.Empty
            ReturnArg = Split(ExecutePython(PyCode, ErrorLog), System.Environment.NewLine)
            If ReturnArg.Length >= 6 Then
                AstroDawnBegin = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(4)))
                AstroDawnEnd = TimeCalc.UTCToLocal(Util.PyEphemDate(ReturnArg(5)))
                TwilightOK = True
            End If
        End If

        If ItIsDay Then
            'It is day now
            RetVal.Add("Sun-set time", Util.MyTimeFormat(NextSetting))
            RetVal.Add("<b>Day</b>, duration", Util.FormatTimeSpan(NextSetting - PrevRising))
            RetVal.Add("Time to sun set", Util.FormatTimeSpan(NextSetting - Now))
            RetVal.Add("Time to astro twilight", Util.FormatTimeSpan(AstroDawnBegin - Now))
            RetVal.Add("Duration next night", Util.FormatTimeSpan(NextRising - NextSetting))
            RetVal.Add("Duration between astro twilights", Util.FormatTimeSpan(AstroDawnEnd - AstroDawnBegin))
        Else
            'It is night now
            RetVal.Add("<b>Night</b>, duration", Util.FormatTimeSpan(NextRising - PrevSetting))
            RetVal.Add("Time to sun raise", Util.FormatTimeSpan(NextRising - Now))
            RetVal.Add("Time to astro twilight", Util.FormatTimeSpan(AstroDawnEnd - Now))
            RetVal.Add("Sun-raise time", Util.MyTimeFormat(NextRising))
        End If

        Return RetVal

    End Function

    Private Shared Sub LoadLocation(ByRef PyCode As List(Of String))
        PyCode.Add("import ephem")                                                                      'import ephem data
        PyCode.Add("MyLocation = ephem.Observer()")                                                     'set an observer position
        PyCode.Add("MyLocation.lat = '" & Globals.Latitude.ToString.Trim.Replace(",", ".") & "'")       'set latitude
        PyCode.Add("MyLocation.lon = '" & Globals.Longitude.ToString.Trim.Replace(",", ".") & "'")      'set longitude
        PyCode.Add("MyLocation.elevation = " & Globals.Elevation.ToString.Trim.Replace(",", "."))       'set elevation
        PyCode.Add("MyLocation.date = '" & Util.PyEphemDate(Now.ToUniversalTime) & "'")                 'set date and time
    End Sub

    Private Shared Function ExecutePython(ByVal Commands As List(Of String), ByRef ErrorLog As String) As String

        Dim InputFile As String = System.IO.Path.GetTempPath & "\Command.py"

        System.IO.File.WriteAllLines(InputFile, Commands.ToArray)
        Dim Command As String = Chr(34) & InputFile & Chr(34) '& " > " & Chr(34) & OutputFile & Chr(34)

        Dim PythonScript As New System.Diagnostics.Process
        PythonScript.StartInfo.FileName = PythonPath
        PythonScript.StartInfo.Arguments = Command

        PythonScript.StartInfo.UseShellExecute = False
        'PythonScript.StartInfo.RedirectStandardInput = True
        PythonScript.StartInfo.RedirectStandardOutput = True
        PythonScript.StartInfo.RedirectStandardError = True
        PythonScript.StartInfo.CreateNoWindow = True

        PythonScript.Start()

        Dim PythonOutput As System.IO.StreamReader = PythonScript.StandardOutput
        Dim PythonError As System.IO.StreamReader = PythonScript.StandardError

        Try

      Do
        System.Threading.Thread.Sleep(1)
        System.Windows.Forms.Application.DoEvents()
      Loop Until PythonScript.HasExited = True

            System.IO.File.Delete(InputFile)

            ErrorLog = PythonError.ReadToEnd

            Return PythonOutput.ReadToEnd

        Catch ex As Exception

            Debug.Print(ex.Message)
            Return String.Empty

        End Try

    End Function

    Private Shared Function GetPyRightAscension(ByVal Result As String) As Double
        Dim Values As String() = Split(Result.Trim, ":")
        Return Val(Values(0)) + (Val(Values(1)) / 60) + (Val(Values(2)) / 3600)
    End Function

    Private Shared Function GetPyDeclination(ByVal Result As String) As Double
        Dim Values As String() = Split(Result.Trim, ":")
        Return Val(Values(0)) + (Val(Values(1)) / 60) + (Val(Values(2)) / 3600)
    End Function

    '''<summary>Get the path to the python executable.</summary>
    '''<returns>Path to Python.exe.</returns>
    Private Shared Function GetPythonPath() As String

        Dim RegPath As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths"
        Dim Key As String = "Python.exe"

        Dim RegAccess As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegPath).OpenSubKey(Key)
        If IsNothing(RegAccess) Then Return Nothing

        Dim XX As String() = RegAccess.GetSubKeyNames
        Dim ReadValue As String = CStr(RegAccess.GetValue(RegAccess.GetValueNames()(0)))

        If String.IsNullOrEmpty(ReadValue) = False Then
            If System.IO.File.Exists(ReadValue) = True Then
                Return ReadValue
            End If
        End If

        Return Nothing

    End Function

End Class
