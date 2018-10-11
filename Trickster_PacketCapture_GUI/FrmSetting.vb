
Public Class FrmSetting
    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub FrmSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.MaximumSize = Me.Size
        ' ## Load Adapters
        Dim StrAdapters() As String = Split(FrmMain.device_list, vbNewLine)
        For i As Integer = 0 To UBound(StrAdapters)
            LstAdapter.Items.Add(StrAdapters(i))
        Next
        LstAdapter.SelectedIndex = FrmMain.device_selection

    End Sub

    Private Sub LstAdapter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstAdapter.SelectedIndexChanged
        FrmMain.device_selection = LstAdapter.SelectedIndex
    End Sub
End Class