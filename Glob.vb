Module Glob
    ' Template Windows Forms
    Public AppName As String = "TA-Syslog-Search"
    Public AppVer As String = "0.02"

    Public AppRoot As String = Application.StartupPath
    Public CD As String = My.Computer.FileSystem.CurrentDirectory

    ' Log File
    Public Temp As String = Environment.GetEnvironmentVariable("TEMP")
    Public G_TL_FR As Integer = 2   ' Trace level File Reads
    Public ErrorCount As Integer = 0
    Public WarningCount As Integer = 0
    Public ExitProgram As Boolean = False
    Public StartSeq As String = ""

    ' Defaults
    Public IniFile1 As String = AppRoot & "\" & AppName & ".ini"
    Public IniFile2 As String = AppRoot & "\Data\" & AppName & ".ini"
    Public Verbose As Boolean = False

    Public SysLog As String = ""
    Public RowMax As Integer = 10
    Public SearchString As New List(Of String)

    Public EndDelay As Integer = 2
    Public EndPause As Boolean = False
    Public ShowGui As Boolean = False

    Sub InitTemp()
        xtrace(" - InitTemp")
        Temp = Temp & "\TAIS"
        If Not My.Computer.FileSystem.DirectoryExists(Temp) Then
            My.Computer.FileSystem.CreateDirectory(Temp)
        End If

        ' Influence the log file name with a command-line parameter
        For Each argument As String In My.Application.CommandLineArgs
            If Left(argument, 6) = "--seq=" Then
                StartSeq = Mid(argument, 7)
                xtrace(" - StartSeq = " & StartSeq)
                Exit For          ' Only process the first occurence
            End If
        Next

        If StartSeq = "" Then
            LogFile = Temp & "\" & AppName & ".log"
        Else
            LogFile = Temp & "\" & AppName & "_" & StartSeq & ".log"
        End If

        xtrace(" - LogFile = " & LogFile)
    End Sub
End Module
