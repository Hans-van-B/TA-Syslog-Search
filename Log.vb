Module Log
    Dim SubLevel As Integer = 0
    Dim LTrace As Integer = 2
    Dim G_TL_Sub As Integer = 2  ' Trace level for Subroutines
    Public LogFile As String = Temp & "\" & AppName & ".log"
    Dim LogStatus As Integer = 0

    Sub xtrace_init()
        InitTemp()

        ' Show the command-line string at the top of the log file
        My.Computer.FileSystem.WriteAllText(LogFile, Environment.CommandLine & vbNewLine, False)
        FlushLogCashe()
        My.Computer.FileSystem.WriteAllText(LogFile, "xtrace_init" & vbNewLine, True)
        LogStatus = 1
    End Sub

    Sub xtrace_header()
        xtrace_subs("xtrace_header")
        xtrace_i(AppName & " V" & AppVer)
        xtrace_TimeStamp()

        xtrace_i("AppRoot = " & AppRoot)
        xtrace_i("Log level to logfile = " & LTrace.ToString, 2)
        xtrace_sube("xtrace_header")
        LogStatus = 2
    End Sub

    '---- xtrace ----
    Sub xtrace_root(Msg As String, TV As Integer)
        Dim Nr As Int16
        Dim Tab As String = ""
        If LogStatus < 1 Then
            WriteLogCache(Msg)
            Exit Sub
        End If

        ' If subroutines are Not logged then tabbing is also disabeled
        If LTrace >= G_TL_Sub Then
            ' If exiting main or sublevel maint error,
            ' this if makes it more clear
            If SubLevel >= 0 Then Tab = "|"

            For Nr = 1 To SubLevel
                Tab = Tab + "  |"
            Next
        End If

        If TV <= LTrace Then
            My.Computer.FileSystem.WriteAllText(LogFile, Tab & Msg & vbNewLine, True)
        End If
    End Sub
    Sub xtrace(Msg As String)
        xtrace_root(" " & Msg, 2)
    End Sub

    Sub xtrace(Msg As String, TV As Integer)
        xtrace_root(" " & Msg, TV)
    End Sub

    '---- xtrace_i ----
    Sub xtrace_i(Msg As String)
        xtrace(" * " & Msg)
    End Sub

    Sub xtrace_i(Msg As String, TV As Integer)
        xtrace(" * " & Msg, TV)
    End Sub

    '--- xtrace TimeStamp ----
    Sub xtrace_TimeStamp()
        xtrace_i("Timestamp = " & DateTime.Now)
    End Sub

    '--- xtrace Sub ----
    Sub xtrace_subs(Msg As String)
        xtrace_root("->" & Msg & " (" & (SubLevel + 1).ToString & ")", G_TL_Sub)
        SubLevel = SubLevel + 1
    End Sub

    Sub xtrace_sube(Msg As String)
        SubLevel = SubLevel - 1
        xtrace_root("<-" & Msg & " (" & (SubLevel + 1).ToString & ")", G_TL_Sub)
    End Sub

    Sub xtrace_err(Msg As String)
        xtrace("ERROR: " & Msg)
    End Sub

    Sub xtrace_Warn(Msg As String)
        xtrace("Warning: " & Msg)
    End Sub

    Public Sub WriteInfo(Msg)
        Form1.TextBoxInfo.AppendText(Msg & vbNewLine)
        xtrace(Msg)
    End Sub

    '=================================================================

    Dim LogCache As New List(Of String)
    Sub WriteLogCache(Msg)
        LogCache.Add(Msg)
    End Sub

    Sub FlushLogCashe()
        Dim Msg As String
        For Each Msg In LogCache
            My.Computer.FileSystem.WriteAllText(LogFile, Msg & vbNewLine, True)
        Next
    End Sub

End Module
