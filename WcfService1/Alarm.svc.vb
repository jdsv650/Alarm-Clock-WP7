Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.Text


Public Class Alarm
    Implements IAlarm

    Public Function ReturnAlarmList(ByVal uname As String) As List(Of AlarmDetail) Implements IAlarm.ReturnAlarmList
        Dim context As AlarmDataClassesDataContext = New AlarmDataClassesDataContext()
        Dim ucontext As AlarmUsersDataClassesDataContext = New AlarmUsersDataClassesDataContext()


        Dim resNum = From u In ucontext.UserAlarms
                     Where (u.UserName = uname)
                     Select u.AlarmNumber

        Dim alarml As List(Of AlarmDetail) = New List(Of AlarmDetail)

        For Each num In resNum
            Dim count As Integer = num
            Dim alarmRes = From a In context.AlarmDetails Where a.AlarmNumber = count Select a

            For Each a In alarmRes
                Dim alarm As AlarmDetail = New AlarmDetail

                alarm.AlarmDateTime = a.AlarmDateTime
                alarm.AlarmNumber = a.AlarmNumber

                If IsDBNull(a.AlarmText) Then
                    alarm.AlarmText = "  "
                Else
                    alarm.AlarmText = a.AlarmText 'alarm text
                End If

                alarm.AlarmText = a.AlarmText
                alarm.AudibleAlarm = a.AudibleAlarm
                alarm.Blinking = a.Blinking
                alarm.Color = a.Color
                alarm.email1 = a.email1
                alarm.email2 = a.email2
                alarm.email3 = a.email3
                alarm.Flashlight = a.Flashlight
                alarm.Friday = a.Friday
                alarm.Monday = a.Monday
                alarm.Saturday = a.Saturday
                alarm.Snooze = a.Snooze
                alarm.SnoozeInterval = a.SnoozeInterval
                alarm.SoundNumber = a.SoundNumber
                alarm.Sunday = a.Sunday
                alarm.Thursday = a.Thursday
                alarm.Tuesday = a.Tuesday
                alarm.Vibrate = a.Vibrate
                alarm.VisualAlarm = a.VisualAlarm
                alarm.VolumeLevel = a.VolumeLevel
                alarm.Wednesday = a.Wednesday

                alarml.Add(alarm)
            Next a
        Next

        ' Join u In ucontext.UserAlarms On u.AlarmNumber Equals a.AlarmNumber
        ' Where (u.UserName = uname)
        'Linq crashes here -- ok to set {.Color = "RED" }  Not {.Color = a.Color} ?
        ' Select New AlarmDetail With {.AlarmDateTime = a.AlarmDateTime, .AlarmNumber = a.AlarmNumber, .AlarmText = a.AlarmText,
        '              .AudibleAlarm = a.AudibleAlarm, .Blinking = a.Blinking, .Color = a.Color, .email1 = a.email1,
        '              .email2 = a.email2, .email3 = a.email3, .Flashlight = a.Flashlight, .Friday = a.Friday,
        '              .Monday = a.Monday, .Saturday = a.Saturday, .Snooze = a.Snooze, .SnoozeInterval = a.SnoozeInterval,
        '              .SoundNumber = a.SoundNumber, .Sunday = a.Sunday, .Thursday = a.Thursday, .Tuesday = a.Tuesday,
        '             .Vibrate = a.Vibrate, .VisualAlarm = a.VisualAlarm, .VolumeLevel = a.VolumeLevel, .Wednesday = a.Wednesday}
        '  Return alarmList.ToList

        Return alarml
    End Function

    Public Function UpdateAlarm(ByVal alarm As AlarmDetail) As Integer Implements IAlarm.UpdateAlarm
        Dim context As AlarmDataClassesDataContext = New AlarmDataClassesDataContext()
        Dim updatedAlarm As AlarmDetail = New AlarmDetail()

        Try
            ' Query the database for the row - catch if alarm doesn't exist
            Dim alrm =
                (From al In context.AlarmDetails
                Where al.AlarmNumber = alarm.AlarmNumber
                Select al).Single

            ' MsgBox(alrm.AlarmNumber)

            alrm.AlarmDateTime = alarm.AlarmDateTime
            alrm.AlarmText = alarm.AlarmText
            alrm.AudibleAlarm = alarm.AudibleAlarm
            alrm.Blinking = alarm.Blinking
            alrm.Color = alarm.Color
            alrm.email1 = alarm.email1
            alrm.email2 = alarm.email2
            alrm.email3 = alarm.email3
            alrm.Flashlight = alarm.Flashlight
            alrm.Friday = alarm.Friday
            alrm.Monday = alarm.Monday
            alrm.Saturday = alarm.Saturday
            alrm.Snooze = alarm.Snooze
            alrm.SnoozeInterval = alarm.SnoozeInterval
            alrm.SoundNumber = alarm.SoundNumber
            alrm.Sunday = alarm.Sunday
            alrm.Thursday = alarm.Thursday
            alrm.Tuesday = alarm.Tuesday
            alrm.Vibrate = alarm.Vibrate
            alrm.VisualAlarm = alarm.VisualAlarm
            alrm.VolumeLevel = alarm.VolumeLevel
            alrm.Wednesday = alarm.Wednesday

            context.SubmitChanges()

        Catch ex As Exception
            '  MsgBox("Update Error")
            Return (-1)
        End Try

        Return 1
    End Function

    Function RemoveAlarm(ByVal num As Integer) As Integer Implements IAlarm.RemoveAlarm
        Dim context As AlarmDataClassesDataContext = New AlarmDataClassesDataContext()
        Dim ucontext As AlarmUsersDataClassesDataContext = New AlarmUsersDataClassesDataContext()

        Dim usera =
            (From ua In ucontext.UserAlarms
            Where ua.AlarmNumber = num
            Select ua).Single

        ucontext.UserAlarms.DeleteOnSubmit(usera)
        Try
            ucontext.SubmitChanges()
        Catch ex As Exception
            MsgBox("Delete Error on UserAlarms")
            Return (-1)
        End Try

        Dim alrm =
              (From al In context.AlarmDetails
              Where al.AlarmNumber = num
              Select al).Single

        context.AlarmDetails.DeleteOnSubmit(alrm)
        Try
            context.SubmitChanges()
        Catch ex As Exception
            MsgBox("Delete Error on AlarmDetails")
            Return (-1)
        End Try

        Return 1
    End Function

    Function AddAlarm(ByVal alarm As AlarmDetail, ByVal usern As String) As Integer Implements IAlarm.AddAlarm
        Dim context As AlarmDataClassesDataContext = New AlarmDataClassesDataContext()
        Dim ucontext As AlarmUsersDataClassesDataContext = New AlarmUsersDataClassesDataContext()

        'ins alarmdetails first
        context.AlarmDetails.InsertOnSubmit(alarm)
        Try
            context.SubmitChanges()
        Catch ex As Exception
            MsgBox("Insert Error on AlarmDetails")
            Return (-1)
        End Try

        Dim i = From a In context.AlarmDetails
                Order By (a.AlarmNumber)
                Descending Select a
        '  MsgBox(i.First.AlarmNumber.ToString)

        'ins useralarms
        Dim ua As UserAlarm = New UserAlarm
        ' get alarm num just inserted
        ua.AlarmNumber = i.First.AlarmNumber
        ua.UserName = usern

        ucontext.UserAlarms.InsertOnSubmit(ua)
        Try
            ucontext.SubmitChanges()
        Catch ex As Exception
            '  MsgBox("Insert Error on UserAlarms")
            Return (-1)
        End Try

        Return 1
    End Function

    Private Function GetConnectionString() As String

        Return CType(System.Configuration.ConfigurationManager.ConnectionStrings("AlarmClockConnectionString").ConnectionString, String)
        ' "AlarmClockConnectionString" is the name of the Connection String set up in Web.Config 
        ' TeamBConnectionString for CSD
    End Function

End Class
