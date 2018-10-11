Imports SharpPcap
Public Class FrmMain
    Dim ServerIP As String = ""
    Dim device As ICaptureDevice
    Public device_selection As Integer
    Public device_list As String = ""
    Dim Client_Key1 As New Dictionary(Of Integer, Byte)()
    Dim Server_Key1 As New Dictionary(Of Integer, Byte)()
    Private Sub FrmMain_Closed() Handles Me.Closed
        End
    End Sub
    Private Sub Load_Settings()
        Dim Settings As String = IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "\settings.cfg")
        ServerIP = Split(Split(Settings, "IP=")(1), ";")(0)
    End Sub
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.MaximumSize = Me.Size
        Dim devices = CaptureDeviceList.Instance

        If devices.Count < 1 Then
            MsgBox("There is no device to capture.", vbCritical, "Error")
            End
        End If

        Dim i As Integer = 0
        For Each dev In devices
            If device_list <> "" Then
                device_list += vbNewLine
            End If
            device_list += String.Format("{0}) {1} {2}", i, dev.Name, dev.Description)
            i += 1
        Next

        device_selection = 0

    End Sub
    Private Sub PacketCapture_Open()
        Dim devices = CaptureDeviceList.Instance
        device = Devices(device_selection)
        AddHandler device.OnPacketArrival, New PacketArrivalEventHandler(AddressOf device_OnPacketArrival)
        Dim readTimeoutMilliseconds As Integer = 1000
        device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds)
        Dim filter As String = "ip and tcp"
        device.Filter = filter
        device.StartCapture()
    End Sub
    Private Sub PacketCapture_Stop()
        Try
            device.StopCapture()
        Catch
        End Try
        device.Close()
    End Sub

    Private Sub BtnWork_Click(sender As Object, e As EventArgs) Handles BtnWork.Click
        If BtnWork.Text = "START!" Then
            PacketCapture_Open()
            lblStatus.Text = "Now capturing..."
            lblStatus.ForeColor = Color.Red
            BtnWork.Text = "STOP"
            BtnWork.ForeColor = Color.Blue
        Else
            PacketCapture_Stop()
            lblStatus.Text = "Ready to capture."
            lblStatus.ForeColor = Color.Black
            BtnWork.Text = "START!"
            BtnWork.ForeColor = Color.Red
        End If
    End Sub
    Private Sub LstMain_Selected(sender As Object, e As EventArgs) Handles LstMain.ItemSelectionChanged
        If LstMain.SelectedItems.Count = 1 Then
            Dim LvItem As ListViewItem = LstMain.SelectedItems(0)
            TxtPacket_Info.Text = "Index: " & LvItem.SubItems(0).Text & vbNewLine &
                                  "Method: " & LvItem.SubItems(0).Tag & vbNewLine &
                                  "Time: " & LvItem.SubItems(1).Text & vbNewLine &
                                  "Port: " & LvItem.SubItems(2).Text & vbNewLine &
                                  "Opcode: " & LvItem.SubItems(3).Text & vbNewLine &
                                  "Size: " & LvItem.SubItems(4).Text
            TxtPacket_Data.Text = LvItem.SubItems(5).Text
        End If
    End Sub

    Private Sub device_OnPacketArrival(ByVal sender As Object, ByVal e As CaptureEventArgs)

        Dim time = e.Packet.Timeval.Date
        Dim len = e.Packet.Data.Length
        Dim packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data)
        Dim tcpPacket = CType(packet.Extract(GetType(PacketDotNet.TcpPacket)), PacketDotNet.TcpPacket)

        If tcpPacket Is Nothing Then
            Exit Sub
        End If

        Dim ipPacket = CType(tcpPacket.ParentPacket, PacketDotNet.IpPacket)
        Dim srcIp As Net.IPAddress = ipPacket.SourceAddress
        Dim dstIp As Net.IPAddress = ipPacket.DestinationAddress
        Dim srcPort As Integer = tcpPacket.SourcePort
        Dim dstPort As Integer = tcpPacket.DestinationPort
        Dim Method As String = ""
        Dim Port As Integer

        If tcpPacket.PayloadData.Length < 2 Then
            Exit Sub
        End If

        If dstIp.ToString() = ServerIP Then
            Method = "Send"
            Port = dstPort
        ElseIf srcIp.ToString() = ServerIP Then
            Method = "Recv"
            Port = srcPort
        Else
            Exit Sub
        End If

        Dim Captured() As Byte
        ReDim Captured(tcpPacket.PayloadData.Length - 1)

        For i = 0 To tcpPacket.PayloadData.Length - 1
            Captured(i) = tcpPacket.PayloadData(i)
        Next

        Dim Content_Decrypted_All() As DecryptResult
        If Method = "Send" Then

            Dim ClientKey As Byte = Packet_Getkey(Client_Key1, Port)
            Content_Decrypted_All = DecryptPacket_FromClient(Captured, Client_Key1(Port))

        Else 'If Method = "Recv" Then

            Dim ServerKey As Byte = Packet_Getkey(Server_Key1, Port)
            Content_Decrypted_All = DecryptPacket_FromServer(Captured, Server_Key1(Port))

        End If

        For i As Integer = 0 To UBound(Content_Decrypted_All)

            Dim Content_Decrypted As DecryptResult = Content_Decrypted_All(i)

            Dim Content_Debyte() As Byte = Content_Decrypted.ContentBytes
            Dim Header As PacketHeader = Content_Decrypted.Header

            Dim LstStr(5) As String
            LstStr(0) = LstMain.Items.Count
            LstStr(1) = String.Format("{0}:{1}:{2},{3}", time.Hour, time.Minute, time.Second, time.Millisecond)
            LstStr(2) = Port
            LstStr(3) = "0x" & Header.OpCode.ToString("X")
            LstStr(4) = "0x" & Header.PacketLength.ToString("X")
            LstStr(5) = BytesToHex(Content_Debyte)
            Dim LstStr_Item As New ListViewItem(LstStr)
            LstStr_Item.SubItems(0).Tag = Method
            If Method = "Recv" Then
                LstStr_Item.BackColor = Color.Silver
            End If

            If Content_Decrypted.Checksum = False Then
                LstStr_Item.BackColor = Color.LightCoral
            End If

            LstMain.Invoke(New MethodInvoker(Sub()
                                                 LstMain.Items.Add(LstStr_Item)
                                             End Sub))

        Next



    End Sub
    Private Function DecryptPacket_FromClient(ByRef Content() As Byte, ByRef Key1 As Byte) As DecryptResult()

        Dim DeContent() As DecryptResult = {}
        Dim LeftContent() As Byte = Content
        Dim NowContent() As Byte = {}
        Do Until (UBound(LeftContent) < 1)

            Dim Packet_SingleLen As UInt16 = Packet_GetLength(LeftContent, Key1)
            ReDim NowContent(Packet_SingleLen - 1)
            Buffer.BlockCopy(LeftContent, 0, NowContent, 0, Packet_SingleLen)
            Dim NewLeft() As Byte
            ReDim NewLeft(UBound(LeftContent) - NowContent.Length)
            If NewLeft.Length <> 0 Then
                Buffer.BlockCopy(LeftContent, NowContent.Length, NewLeft, 0, NewLeft.Length)
            End If
            LeftContent = NewLeft

            Array.Resize(DeContent, UBound(DeContent) + 1 + 1)
            DeContent(UBound(DeContent)) = Packet_Decrypt_FromClient(NowContent, Key1)

        Loop

        Return DeContent

    End Function
    Private Function DecryptPacket_FromServer(ByRef Content() As Byte, ByRef Key1 As Byte) As DecryptResult()

        Dim DeContent() As DecryptResult = {}
        Dim LeftContent() As Byte = Content
        Dim NowContent() As Byte = {}
        Do Until (UBound(LeftContent) < 1)

            If Packet_CheckHeader(LeftContent, 1) = True Then
                Key1 = 1
            End If

            Dim Packet_SingleLen As UInt16 = Packet_GetLength(LeftContent, Key1)
            ReDim NowContent(Packet_SingleLen - 1)
            Buffer.BlockCopy(LeftContent, 0, NowContent, 0, Packet_SingleLen)
            Dim NewLeft() As Byte
            ReDim NewLeft(UBound(LeftContent) - NowContent.Length)
            If NewLeft.Length <> 0 Then
                Buffer.BlockCopy(LeftContent, NowContent.Length, NewLeft, 0, NewLeft.Length)
            End If
            LeftContent = NewLeft

            Array.Resize(DeContent, UBound(DeContent) + 1 + 1)
            DeContent(UBound(DeContent)) = Packet_Decrypt_FromServer(NowContent, Key1)

        Loop

        Return DeContent

    End Function
    Private Sub Packet_Setkey(ByRef KeyDic As Dictionary(Of Integer, Byte), ByRef IntValue As Integer, ByRef KeyValue As Byte)
        If KeyDic.TryGetValue(IntValue, KeyValue) = False Then
            KeyDic.Add(IntValue, KeyValue)
        Else
            KeyDic(IntValue) = KeyValue
        End If
    End Sub
    Private Function Packet_Getkey(ByRef KeyDic As Dictionary(Of Integer, Byte), ByRef IntValue As Integer) As Byte
        Dim KeyValue As Byte
        If KeyDic.TryGetValue(IntValue, KeyValue) = False Then
            KeyDic.Add(IntValue, 1)
            KeyValue = 1
        End If
        Return KeyValue
    End Function

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click

        If LstMain.Items.Count = 0 Then
            MsgBox("Listview has no data.", vbCritical, "Error")
            Exit Sub
        End If

        Dim SaveText As String = ""
        Dim Parser As String = "#%@%#"
        For i As Integer = 0 To LstMain.Items.Count - 1
            If SaveText <> "" Then
                SaveText += vbNewLine
            End If

            SaveText = SaveText & LstMain.Items(i).SubItems(0).Tag & Parser &
                                  LstMain.Items(i).SubItems(1).Text & Parser &
                                  LstMain.Items(i).SubItems(2).Text & Parser &
                                  LstMain.Items(i).SubItems(3).Text & Parser &
                                  LstMain.Items(i).SubItems(4).Text & Parser &
                                  LstMain.Items(i).SubItems(5).Text
        Next
        Dim NowTime As String = Timestamp.ToString()
        Dim FileName As String = "packets-" & NowTime & ".tpk"
        IO.File.WriteAllText(Application.StartupPath & "/" & FileName, SaveText)

        MsgBox(FileName & " Saved.", vbInformation, "Saved")

    End Sub

    Private Sub BtnLoad_Click(sender As Object, e As EventArgs) Handles BtnLoad.Click

        If LstMain.Items.Count <> 0 Then
            Dim askRes As MsgBoxResult = MsgBox("Listview already has data." & vbNewLine & "If you continue, " &
                                                "you'll lose these data. Is it OK?", vbQuestion + vbYesNo, "Losing Data")
            If askRes <> vbYes Then
                Exit Sub
            End If
        End If

        Dim Open As OpenFileDialog = New OpenFileDialog()
        Open.Filter = "Trickster Packet Log(*.tpk)|*.tpk"
        Open.Title = "Select file"
        If Open.ShowDialog <> DialogResult.OK Then
            Exit Sub
        End If

        LstMain.Items.Clear()

        Dim FilePath As String = Open.FileName
        Dim ReadText As String = IO.File.ReadAllText(FilePath)
        Dim ReadText_Array() As String = Split(ReadText, vbNewLine)
        Dim Parser As String = "#%@%#"
        For i As Integer = 0 To UBound(ReadText_Array)
            Dim LineText() As String = Split(ReadText_Array(i), Parser)

            Dim LstStr(5) As String
            LstStr(0) = LstMain.Items.Count
            LstStr(1) = LineText(1)
            LstStr(2) = LineText(2)
            LstStr(3) = LineText(3)
            LstStr(4) = LineText(4)
            LstStr(5) = LineText(5)

            Dim LstStr_Item As New ListViewItem(LstStr)
            LstStr_Item.SubItems(0).Tag = LineText(0)
            If LineText(0) = "Recv" Then
                LstStr_Item.BackColor = Color.Silver
            End If
            LstMain.Items.Add(LstStr_Item)

        Next
    End Sub

    Private Sub BtnSetting_Click(sender As Object, e As EventArgs) Handles BtnSetting.Click
        FrmSetting.Show()
    End Sub

    Public Function Timestamp() As Long
        Dim timeSpan = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0))
        Return timeSpan.TotalSeconds
    End Function
End Class
