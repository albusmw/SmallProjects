Option Explicit On
Option Strict On

'Classes to enable WCF interoperation

Public Class IDB_Properties
    Public Const ResponseFormat As ServiceModel.Web.WebMessageFormat = ServiceModel.Web.WebMessageFormat.Xml
    Public Const BodyStyle As ServiceModel.Web.WebMessageBodyStyle = ServiceModel.Web.WebMessageBodyStyle.Wrapped
End Class

'''<summary>Web service interfaces.</summary>
<ServiceModel.ServiceContract>
Public Interface IDB

    '''<summary>Get a list of all available parameters.</summary>
    <ServiceModel.Web.WebGet(UriTemplate:="GetParameterList", ResponseFormat:=IDB_Properties.ResponseFormat, BodyStyle:=IDB_Properties.BodyStyle)>
    <ServiceModel.OperationContract()>
    Function GetParameterList() As Collections.Generic.List(Of String)

    '''<summary>Get value of a certain parameter.</summary>
    <ServiceModel.Web.WebGet(UriTemplate:="GetParameter/{Key}", ResponseFormat:=IDB_Properties.ResponseFormat, BodyStyle:=IDB_Properties.BodyStyle)>
    <ServiceModel.OperationContract()>
    Function GetParameter(Key As String) As Object

    '''<summary>Set value of a certain parameter.</summary>
    <ServiceModel.Web.WebGet(UriTemplate:="SetParameter/{Key}={Value}", ResponseFormat:=IDB_Properties.ResponseFormat, BodyStyle:=IDB_Properties.BodyStyle)>
    <ServiceModel.OperationContract()>
    Function SetParameter(Key As String, Value As String) As Object

    '''<summary>Run an exposure.</summary>
    <ServiceModel.Web.WebGet(UriTemplate:="RunExposure", ResponseFormat:=IDB_Properties.ResponseFormat, BodyStyle:=IDB_Properties.BodyStyle)>
    <ServiceModel.OperationContract()>
    Function RunExposure() As Object

End Interface

'''<summary>Database holding relevant information.</summary>
Public Class cDB_ServiceContract : Implements IDB

    '''<summary>DB objec to get information from.</summary>
    Private Shared DB As cDB

    '''<summary>Raise event if a value was changed e.g. from the WCF.</summary>
    Public Shared Event ValueChanged()

    '''<summary>Run an exposure.</summary>
    Public Shared Event StartExposure()

    Public Sub New()

    End Sub

    '''<summary>Configure the DB to use.</summary>
    Public Sub New(ByRef DBToUse As cDB)
        DB = DBToUse
    End Sub

    '''<summary>Get a list of all available parameters.</summary>
    Public Function GetParameterList() As Collections.Generic.List(Of String) Implements IDB.GetParameterList
        Dim RetVal As New Collections.Generic.List(Of String)
        For Each SingleProperty As Reflection.PropertyInfo In DB.GetType.GetProperties()
            RetVal.Add(SingleProperty.Name)
        Next SingleProperty
        Return RetVal
    End Function

    '''<summary>Get value of a certain parameter.</summary>
    Public Function GetParameter(Key As String) As Object Implements IDB.GetParameter
        For Each SingleProperty As Reflection.PropertyInfo In DB.GetType.GetProperties()
            If Key.ToUpper = SingleProperty.Name.ToUpper Then
                Return SingleProperty.GetValue(DB, Nothing)
            End If
        Next SingleProperty
        Return Nothing
    End Function

    '''<summary>Set value of a certain parameter.</summary>
    Public Function SetParameter(Key As String, Value As String) As Object Implements IDB.SetParameter
        For Each SingleProperty As Reflection.PropertyInfo In DB.GetType.GetProperties()
            If Key.ToUpper = SingleProperty.Name.ToUpper Then
                Select Case SingleProperty.PropertyType.Name.ToUpper
                    Case "Double".ToUpper
                        Dim NewProp As Double
                        If Double.TryParse(Value, NewProp) = True Then SingleProperty.SetValue(DB, NewProp)
                End Select
                RaiseEvent ValueChanged()
                Return SingleProperty.GetValue(DB, Nothing)
            End If
        Next SingleProperty
        Return Nothing
    End Function

    Public Function RunExposure() As Object Implements IDB.RunExposure
        RaiseEvent StartExposure()
    End Function

End Class
