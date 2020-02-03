Option Explicit On
Option Strict On

Public Class MainForm

    Dim MyPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location

    Dim Downloader As New AstroCalc.NET.Common.cDownloader
    Dim EvaIDs As New Dictionary(Of String, String)              'Dictionary of EVA ID's and station names
    Dim AllTrains As New List(Of String)                         'Lik to all trains

    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click

        'https://christoph-stoeber.de/bahnprojekt/dokumentation/bahndetafeldoku.php

        'Mit infos:
        'https://reiseauskunft.bahn.de/bin/bhftafel.exe/dn?ld=37235&protocol=https:&rt=1&

        'delayedJourney=on                                  Zeigt nur verspätete oder ausfallende Züge an. 

        Dim Language As String = "d"
        Dim Darstellung As String = "n"                             'n: Standard / l: nur Text OHNE ECHTZEIT
        Dim QueryDate As String = Format(Now, "dd.MM.yy")           ' "12.01.20"
        Dim QueryTime As String = Format(Now.AddHours(-1), "hh:mm")      '"12:00"
        Dim Station As String = "8004132"                   'Stachus
        Dim maxJourneys As String = "50"                    'Anzahl angezeigter Züge
        Dim BoardType As String = "dep"                     'arr/dep

        Dim StartURL As String = "https://reiseauskunft.bahn.de/bin/bhftafel.exe/" & Language & Darstellung & "?ld=37235&country=DEU&protocol=https:&rt=1&input=" & Station & "&boardType=" & BoardType & "&time=" & QueryTime & "&productsFilter=11111&&&date=" & QueryDate & "&selectDate=&maxJourneys=" & maxJourneys & "&start=yes"

        'Initial scan for start station
        Navigate(StartURL)

        Dim LinkCount As Integer = 0
        Dim NewTrainLinks As Integer = 0
        AddLog(wbMain.Document.Links.Count.ValRegIndep & " links")
        For Each Element As HtmlElement In wbMain.Document.Links
            'Get link and filter
            Dim TrainInfoLink As String = Element.GetAttribute("href")
            If TrainInfoLink.StartsWith("https://reiseauskunft.bahn.de/bin/traininfo.exe") Then
                LinkCount += 1
                'Get the unique link for this train for the current day
                '1.) Remove ld
                Dim ldPos As Integer = TrainInfoLink.IndexOf("ld=")
                If ldPos > 0 Then
                    TrainInfoLink = TrainInfoLink.Substring(0, ldPos) & TrainInfoLink.Substring(TrainInfoLink.IndexOf("&", ldPos) + 1)
                End If
                '2.) Remove time
                Dim timePos As Integer = TrainInfoLink.IndexOf("time=")
                If timePos > 0 Then
                    TrainInfoLink = TrainInfoLink.Substring(0, timePos) & TrainInfoLink.Substring(TrainInfoLink.IndexOf("&", timePos) + 1)
                End If
                '2.) Remove station
                Dim stationPos As Integer = TrainInfoLink.IndexOf("station_evaId=")
                If stationPos > 0 Then
                    TrainInfoLink = TrainInfoLink.Substring(0, stationPos) & TrainInfoLink.Substring(TrainInfoLink.IndexOf("&", stationPos) + 1)
                End If
                If AllTrains.Contains(TrainInfoLink) = False Then
                    NewTrainLinks += 1
                    AllTrains.Add(TrainInfoLink)
                End If
            End If
        Next Element
        AddLog("  > " & LinkCount.ValRegIndep & " trains found")
        AddLog("  > " & NewTrainLinks.ValRegIndep & " new trains found")

        'Get all trains present
        CheckAllTrains(AllTrains)
        AddLog("=====================================================================")

    End Sub

    Private Sub CheckAllTrains(ByVal TrainList As List(Of String))
        Dim NewEVAIDCount As Integer = 0
        For Each Train As String In TrainList

            Dim TrainDepOnTime As Integer = 0
            Dim TrainDepDelayedOnTime As Integer = 0
            Dim TrainDepDelayed As Integer = 0
            Navigate(Train)

            'Check all links
            For Each Element As HtmlElement In wbMain.Document.Links
                'Get link and filter
                Dim InnerLink As String = Element.GetAttribute("href")
                If InnerLink.StartsWith("https://reiseauskunft.bahn.de/bin/bhftafel.exe") Then
                    Dim BahnhofName As String = Element.InnerText.Trim              'Text as displayed
                    Dim EvaID As String = InnerLink.PartAfter("input=")
                    EvaID = EvaID.Substring(EvaID.IndexOf("%") + 3)
                    EvaID = EvaID.Substring(0, EvaID.IndexOf("&"))
                    If EvaIDs.ContainsKey(EvaID) = False Then
                        EvaIDs.Add(EvaID, BahnhofName)
                        NewEVAIDCount += 1
                    End If
                End If
            Next Element

            'Check all departure elements
            Dim AllElements As HtmlElementCollection = wbMain.Document.GetElementsByTagName("td")
            For Each Elements As HtmlElement In AllElements
                If IsNothing(Elements.InnerText) = False Then
                    If Elements.InnerText.Contains("ab ") Then
                        If Elements.InnerText = Elements.InnerHtml Then
                            'Both are the same -> no additional information ...
                            TrainDepOnTime += 1
                        Else
                            If Elements.InnerHtml.Contains("class=delayOnTime") Then
                                TrainDepDelayedOnTime += 1
                            Else
                                If Elements.InnerHtml.Contains("class=delay") Then
                                    TrainDepDelayed += 1
                                End If
                            End If
                        End If
                    End If
                End If
            Next Elements
            Dim TotalDeps As Integer = TrainDepOnTime + TrainDepDelayed + TrainDepDelayedOnTime

            If TrainDepDelayed + TrainDepDelayedOnTime > 0 Then
                AddLog("  > " & Train)
                AddLog("  > " & TotalDeps.ValRegIndep & " departures, " & TrainDepDelayedOnTime.ValRegIndep & " delayed on time, " & TrainDepDelayed.ValRegIndep & " delayed")
            End If
        Next Train
        If NewEVAIDCount > 0 Then AddLog("  > " & NewEVAIDCount.ValRegIndep & " new EVA ID's found")
    End Sub

    Private Sub Navigate(ByVal URL As String)
        wbMain.ScriptErrorsSuppressed = True
        wbMain.Navigate(URL)
        Do
            System.Windows.Forms.Application.DoEvents()
        Loop Until wbMain.IsBusy = False
        'MsgBox("OK")
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text = "SBahnMonitor (" & MyPath & ")"
    End Sub

    Private Sub AddLog(ByVal Text As String)
        tbLog.Text &= Text & System.Environment.NewLine
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub StoreAllEVAIDsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StoreAllEVAIDsToolStripMenuItem.Click
        'Store all stations
        Dim AllStations As New List(Of String)
        For Each EvaID As String In EvaIDs.Keys
            AllStations.Add(EvaID.Trim & ";" & EvaIDs(EvaID))
        Next EvaID
        System.IO.File.WriteAllLines("AllStations.csv", AllStations.ToArray, System.Text.Encoding.Unicode)
    End Sub

    Private Sub CheckAllTrainsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckAllTrainsToolStripMenuItem.Click
        CheckAllTrains(AllTrains)
    End Sub

    Private Sub tsbBack_Click(sender As Object, e As EventArgs) Handles tsbBack.Click
        wbMain.GoBack()
    End Sub

    Private Sub tsbForward_Click(sender As Object, e As EventArgs) Handles tsbForward.Click
        wbMain.GoForward()
    End Sub

End Class
