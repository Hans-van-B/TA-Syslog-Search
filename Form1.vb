Public Class Form1

    '---- Initialize application ----------------------------------------------
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        xtrace_init()
        xtrace_subs("Form1_Load")
        xtrace_header()
        WriteInfo("Log file = " & LogFile)
        xtrace("Initializing")
        ReadDefaults()
        Read_Command_Line_Arg()
        Me.Text = AppName & " V" & AppVer
        SetSyslog()

        xtrace_sube("Form1_Load")
    End Sub
    Sub SetSyslog()
        xtrace_subs("SetSyslog")
        TextBoxSysLog.Text = SysLog
        xtrace_sube("SetSyslog")
    End Sub

    Private Sub TextBoxSysLog_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSysLog.TextChanged
        xtrace_subs("TextBoxSysLog_TextChanged")
        SysLog = TextBoxSysLog.Text

        If My.Computer.FileSystem.FileExists(SysLog) Then
            TextBoxSysLog.ForeColor = Color.DarkGreen
            xtrace_i("Exists")
        Else
            TextBoxSysLog.ForeColor = Color.DarkRed
            xtrace_i("Does not Exist")
        End If
        xtrace_sube("TextBoxSysLog_TextChanged")
    End Sub

    '==== Main Menu ===========================================================

    Private Sub ToolStripFileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripFileOpen.Click
        xtrace_subs("ToolStripFileOpen_Click")
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            SysLog = OpenFileDialog1.FileName
            SetSyslog()
        End If
        xtrace_sube("ToolStripFileOpen_Click")
    End Sub

    Private Sub ToolStripMenuSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuSave.Click
        xtrace_subs("ToolStripMenuSave_Click")
        If SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim Output As String = SaveFileDialog1.FileName
            My.Computer.FileSystem.WriteAllText(Output, TextBoxInfo.Text, False)
        End If
        xtrace_sube("ToolStripMenuSave_Click")
    End Sub

    Private Sub ToolStripMenuReset_Click(sender As Object, e As EventArgs) Handles ToolStripMenuReset.Click
        TextBoxInfo.Text = ""
        TextBox1.Text = "0"
        TextBox2.Text = "0"
        TextBox3.Text = "0"
        TextBox4.Text = "0"
        TextBox5.Text = "0"
        TextBox6.Text = "0"
        TextBox7.Text = "0"
        TextBox9.Text = "0"
        TextBox10.Text = "0"
    End Sub

    '---- File, Exit
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        xtrace_subs("Menu, File, Exit")
        exit_program()
    End Sub

    '---- Show Settings -------------------------------------------------------
    Private Sub ShowSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowSettingsToolStripMenuItem.Click
        xtrace_subs("Menu, Settings, Show settings")
        If ShowSettingsToolStripMenuItem.Checked Then
            TextBoxInfo.Visible = True
            TextBoxInfo.Dock = DockStyle.Fill
            TextBoxInfo.BringToFront()
        Else
            TextBoxInfo.Visible = False
        End If
    End Sub

    '---- Show Log ------------------------------------------------------------
    Private Sub ShowLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowLogToolStripMenuItem.Click
        xtrace_subs("Menu, Settings, Show log file")
        Process.Start(LogFile)
    End Sub

    '---- Show help
    Private Sub HelpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem1.Click
        xtrace_subs("Menu, Help, Help")
        ShowHelp()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        xtrace_subs("Menu, Help, About")
        ShowHelpAbout()
    End Sub

    Private Sub SupportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupportToolStripMenuItem.Click
        xtrace_subs("Menu, Help, Support")
        ShowSupport()
        xtrace_sube("Help, Support")
    End Sub

    Sub SetStatus(Msg As String)
        ToolStripStatusLabel1.Text = Msg
        Do_Events()
    End Sub
    Sub SetStatus(LinNo As Integer)
        ToolStripStatusLabel1.Text = "Read line " & LinNo.ToString
        Do_Events()
    End Sub
    Private Sub ButtonStartSearch_Click(sender As Object, e As EventArgs) Handles ButtonStartSearch.Click
        xtrace_subs("ButtonStartSearch_Click")
        SetStatus("Initialize")
        ToolStripMenuCancel.Checked = False

        '---- Set/Fill Gui
        Dim SString As String
        Dim Seq As Integer = 0
        Dim LabelRow(RowMax) As Object
        Dim TextBoxRow(RowMax) As Object
        Dim HitCount(RowMax) As Integer
        LabelRow(1) = Label1
        LabelRow(2) = Label2
        LabelRow(3) = Label3
        LabelRow(4) = Label4
        LabelRow(5) = Label5
        LabelRow(6) = Label6
        LabelRow(7) = Label7
        LabelRow(8) = Label8
        LabelRow(9) = Label9
        LabelRow(10) = Label10

        TextBoxRow(1) = TextBox1
        TextBoxRow(2) = TextBox2
        TextBoxRow(3) = TextBox3
        TextBoxRow(4) = TextBox4
        TextBoxRow(5) = TextBox5
        TextBoxRow(6) = TextBox6
        TextBoxRow(7) = TextBox7
        TextBoxRow(8) = TextBox8
        TextBoxRow(9) = TextBox9
        TextBoxRow(10) = TextBox10

        For Each SString In SearchString
            Seq += 1
            xtrace_i("Assign " & Seq.ToString & " : " & SString)
            If Seq > RowMax Then
                xtrace_warn("RowMax " & RowMax.ToString & " exceeded")
                Exit For
            End If
            If SString Is Nothing Then
                xtrace_i("End of list reached")
                Exit For
            End If

            LabelRow(Seq).Text = SString
            LabelRow(Seq).Visible = True
            HitCount(Seq) = 0
            TextBoxRow(Seq).Text = "0"
            TextBoxRow(Seq).Visible = True
        Next

        '---- Start Search ----
        Dim SList(10) As Object
        For Nr = 1 To RowMax
            SList(Nr) = New List(Of String)
        Next

        Dim ReadFile
        ReadFile = My.Computer.FileSystem.OpenTextFileReader(SysLog)
        Dim Line As String
        Dim LinNo As Integer
        Dim LineMatches As Boolean
        Dim SStr
        While Not ReadFile.EndOfStream
            LinNo += 1
            SetStatus(LinNo)
            Line = ReadFile.ReadLine()
            Seq = 0
            For Each SString In SearchString
                Seq += 1
                LineMatches = True
                For Each SStr In SString.Split(";") ' Search multiple strings in a line
                    If InStr(Line, SStr) = 0 Then
                        LineMatches = False
                    End If
                Next

                If LineMatches Then
                    HitCount(Seq) += 1
                    TextBoxRow(Seq).Text = HitCount(Seq).ToString
                    SList(Seq).Add(LinNo.ToString & "|" & Line)
                    Continue While
                End If
            Next

            If ToolStripMenuCancel.Checked Then Exit While
        End While
        ReadFile.Close

        '---- List Result
        SetStatus("Listing search results...")

        Seq = 0
        TextBoxInfo.AppendText(vbCrLf)
        For Each SString In SearchString
            Seq += 1
            Line = Strings.Left(SString & " ........................................................", 60)
            TextBoxInfo.AppendText(Line & " : " & HitCount(Seq).ToString & vbCrLf)
        Next

        Seq = 0
        For Each SString In SearchString
            Seq += 1
            TextBoxInfo.AppendText(vbCrLf)
            TextBoxInfo.AppendText(SString & vbCrLf)

            For Each Line In SList(Seq)
                TextBoxInfo.AppendText(Line & vbCrLf)
            Next
        Next

        SetStatus("Search finished!")

        xtrace_sube("ButtonStartSearch_Click")
    End Sub
End Class
