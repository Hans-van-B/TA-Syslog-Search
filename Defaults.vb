Module Defaults
    Sub ReadDefaults()
        Dim ReadFile

        If My.Computer.FileSystem.FileExists(IniFile1) Then
            WriteInfo("Read Defaults " & IniFile1)
            ReadFile = My.Computer.FileSystem.OpenTextFileReader(IniFile1)

        ElseIf My.Computer.FileSystem.FileExists(IniFile2) Then
            WriteInfo("Read Defaults " & IniFile2)
            ReadFile = My.Computer.FileSystem.OpenTextFileReader(IniFile2)

        Else
            WriteInfo("Failed to read:")
            WriteInfo("   " & IniFile1)
            WriteInfo("Or " & IniFile2)
            CreateIniFile()
            Exit Sub
        End If

        Dim Line As String
        Dim Group As String = ""
        Dim P1, P2 As Integer
        Dim DName, DVal As String

        While Not ReadFile.EndOfStream
            Try
                Line = ReadFile.ReadLine()

                '---- Remark lines ----
                If Left(Line, 1) = "#" Then
                    Continue While
                End If

                '---- Read Group ----------
                P1 = InStr(Line, "[")
                P2 = InStr(Line, "]")

                If P1 = 1 And P2 > 2 Then
                    Group = Mid(Line, 2, P2 - 2)
                    xtrace("Group = " & Group)
                    Continue While
                End If

                '---- Pick Lists ----
                ' Reset group (Optional)
                If Len(Line) < 2 Then
                    Group = ""
                    Continue While
                End If

                If Group = "COMBOBOX01" Then
                    'Form1.COMBOBOX01.Items.Add(Line)
                End If

                '---- Read Defaults
                P1 = InStr(Line, "=")
                DName = Left(Line, P1 - 1)
                DVal = Mid(Line, P1 + 1)
                xtrace("Default " & DName & "=" & DVal)

                If Group = "INIT" Then

                    If DName = "WindowState" Then
                        If UCase(DVal) = "MIN" Then
                            xtrace("Minimize form")
                            Form1.WindowState = FormWindowState.Minimized
                        End If
                    End If

                    If DName = "SysLog" Then
                        SysLog = DVal
                        xtrace_i("Set SysLog = " & SysLog)
                    End If
                End If

                If Group = "SEARCH" Then
                    If DName.ToUpper = "STRING" Then
                        xtrace_i("Add: """ & DVal & """")
                        SearchString.Add(DVal)
                    End If
                End If

            Catch ex As Exception

            End Try
        End While
        ReadFile.Dispose()
    End Sub

    Function StringToBoolean(Val As String) As Boolean
        Val = UCase(Val).Trim

        If (Val = "TRUE") _
        Or (Val = "1") _
        Or (Val = "Y") Then
            Return True
        Else
            Return False
        End If
    End Function

    '---- Create ini file -----------------------------------------------------
    Sub CreateIniFile()
        My.Computer.FileSystem.WriteAllText(IniFile1, "[INIT]" & vbNewLine, False)
    End Sub
End Module
