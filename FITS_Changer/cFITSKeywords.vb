Option Explicit On
Option Strict On

Public Class FITSKeywords

  Public Shared [CCD_TEMP] As String() = ({"CCD-TEMP", "[°C] Actual measured sensor temperature at the start of exposure"})
  Public Shared [COORFLAG] As String() = ({"COORFLAG", "['error'/'missing'/'uncertain'] Quality flag of the recorded coordinates (right ascension and declination)"})

  Public Shared [DATE_OBS] As String() = ({"DATE-OBS", "[YYYY-MM-DDThh:mm:ss] UT date and time of the start of the observation"})
  Public Shared [DATEORIG] As String() = ({"DATEORIG", "Original recorded date of the observation (evening date)"})
  Public Shared [DEC_ORIG] As String() = ({"DEC-ORIG", "Original recorded declination of the telescope pointing (plate center)"})

  Public Shared [EXPTIME] As String() = ({"EXPTIME", "[s] Exposure time of the first exposure"})

  Public Shared [FILTER] As String() = ({"FILTER", "Filter type"})
  Public Shared [FOV1] As String() = ({"FOV1", "[deg] Field of view along axis 1"})
  Public Shared [FOV2] As String() = ({"FOV2", "[deg] Field of view along axis 2"})

  Public Shared [OBJECT] As String() = ({"OBJECT", "Character string giving a name for the object observed"})
  Public Shared [OBSERVAT] As String() = ({"OBSERVAT", "Observatory name"})
  Public Shared [OBSERVER] As String() = ({"OBSERVER", "Character string identifying who acquired the data associated with the header"})

  Public Shared [RA_ORIG] As String() = ({"RA-ORIG", "Original recorded right ascension of the telescope pointing (plate center)"})

  Public Shared [SITEELEV] As String() = ({"SITEELEV", "[m] Elevation of the imaging site in meters over sea level"})
  Public Shared [SITELAT] As String() = ({"SITELAT", "Latitude of the observing site, in decimal degrees"})
  Public Shared [SITELONG] As String() = ({"SITELONG", "East longitude of the observing site, in decimal degrees"})

  Public Shared [TELAPER] As String() = ({"TELAPER", "[m] Clear aperture of the telescope"})
  Public Shared [TELESCOP] As String() = ({"TELESCOP", "Character string identifying the telescope used to acquire the data associated with the header"})
  Public Shared [TELFOC] As String() = ({"TELFOC", "[m] Focal length of the telescope"})
  Public Shared [TELSCALE] As String() = ({"TELSCALE", "[arcsec/mm] Plate scale of the telescope"})
  Public Shared [TEMPERAT] As String() = ({"TEMPERAT", "[°C] Air temperature"})

End Class