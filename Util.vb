Module Util
    '---- Wait ----------------------------------------------------------------
    Public Sub wait(ByVal interval As Integer)
        xtrace_i("Wait " & interval.ToString)
        interval = interval * 1000

        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            ' Allows UI to remain responsive
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub

    '---- Exit Program --------------------------------------------------------
    Sub exit_program()
        xtrace(" ")
        If (ErrorCount > 0) Or (WarningCount > 0) Then
            xtrace("Found " & ErrorCount.ToString & " Errors", 2)
            xtrace("Found " & WarningCount.ToString & " Warnings", 2)
        Else
            xtrace("Exit Ok", 1)
        End If

        xtrace("")
        xtrace("================")
        xtrace("  Exit Program  ")
        xtrace("================")

        ExitProgram = True
        xtrace_TimeStamp()
        Application.Exit()
    End Sub

    Sub Do_Events()
        Application.DoEvents()
    End Sub
End Module
