Option Explicit On
Option Strict On

Public Interface IGenerics
  Property Star As sGeneric
End Interface

Public Structure sGeneric

  Public Enum eCatType
    HD
  End Enum

  Public Structure sCat
    Public Value As String
    Public Type As eCatType
    Public Sub New(ByVal NewValue As String, ByVal NewType As eCatType)
      Me.Value = NewValue
      Me.Type = NewType
    End Sub
  End Structure

  Public Enum eMagnitudeType
    Visual
  End Enum

  Public Structure sMagnitude
    Public Value As Double
    Public Type As eMagnitudeType
    Public Sub New(ByVal NewValue As Double, ByVal NewType As eMagnitudeType)
      Me.Value = NewValue
      Me.Type = NewType
    End Sub
  End Structure

  '''<summary>Right ascension [degree].</summary>
  Public RightAscension As Double
  '''<summary>Declination [degree].</summary>
  Public Declination As Double
  '''<summary>Magnitude information.</summary>
  Public Magnitude As sMagnitude
  '''<summary>Catalog information.</summary>
  Public Cat As sCat

  '''<summary>Set the position of the object.</summary>
  Public Sub Invalidate()
    Me.RightAscension = Double.NaN
    Me.Declination = Double.NaN
  End Sub

  '''<summary>Set the position of the object.</summary>
  Public Sub New(ByVal RA As Double, ByVal DE As Double)
    Me.RightAscension = RA
    Me.Declination = DE
  End Sub

  '''<summary>Indicate that the object is empty.</summary>
  Public Function IsNothing() As Boolean
    If Double.IsNaN(RightAscension) = True Then Return True
    If Double.IsNaN(Declination) = True Then Return True
    Return False
  End Function

End Structure