<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.LstMain = New System.Windows.Forms.ListView()
        Me.Count = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Time = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Port = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Opcode = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PacketSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Data = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TxtPacket_Data = New System.Windows.Forms.TextBox()
        Me.TxtPacket_Info = New System.Windows.Forms.TextBox()
        Me.BtnWork = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.BtnSetting = New System.Windows.Forms.PictureBox()
        Me.BtnLoad = New System.Windows.Forms.PictureBox()
        Me.BtnSave = New System.Windows.Forms.PictureBox()
        CType(Me.BtnSetting, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnSave, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LstMain
        '
        Me.LstMain.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Count, Me.Time, Me.Port, Me.Opcode, Me.PacketSize, Me.Data})
        Me.LstMain.FullRowSelect = True
        Me.LstMain.Location = New System.Drawing.Point(10, 45)
        Me.LstMain.Name = "LstMain"
        Me.LstMain.Size = New System.Drawing.Size(521, 310)
        Me.LstMain.TabIndex = 0
        Me.LstMain.UseCompatibleStateImageBehavior = False
        Me.LstMain.View = System.Windows.Forms.View.Details
        '
        'Count
        '
        Me.Count.Text = "Index"
        Me.Count.Width = 50
        '
        'Time
        '
        Me.Time.Text = "Time"
        Me.Time.Width = 90
        '
        'Port
        '
        Me.Port.Text = "Port"
        '
        'Opcode
        '
        Me.Opcode.Text = "Opcode"
        Me.Opcode.Width = 70
        '
        'PacketSize
        '
        Me.PacketSize.Text = "Size"
        Me.PacketSize.Width = 70
        '
        'Data
        '
        Me.Data.Text = "Data"
        Me.Data.Width = 155
        '
        'TxtPacket_Data
        '
        Me.TxtPacket_Data.Location = New System.Drawing.Point(536, 148)
        Me.TxtPacket_Data.Multiline = True
        Me.TxtPacket_Data.Name = "TxtPacket_Data"
        Me.TxtPacket_Data.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxtPacket_Data.Size = New System.Drawing.Size(230, 207)
        Me.TxtPacket_Data.TabIndex = 1
        '
        'TxtPacket_Info
        '
        Me.TxtPacket_Info.Location = New System.Drawing.Point(536, 45)
        Me.TxtPacket_Info.Multiline = True
        Me.TxtPacket_Info.Name = "TxtPacket_Info"
        Me.TxtPacket_Info.Size = New System.Drawing.Size(230, 95)
        Me.TxtPacket_Info.TabIndex = 2
        '
        'BtnWork
        '
        Me.BtnWork.Font = New System.Drawing.Font("D2Coding", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.BtnWork.ForeColor = System.Drawing.Color.Red
        Me.BtnWork.Location = New System.Drawing.Point(10, 10)
        Me.BtnWork.Name = "BtnWork"
        Me.BtnWork.Size = New System.Drawing.Size(89, 28)
        Me.BtnWork.TabIndex = 3
        Me.BtnWork.Text = "START!"
        Me.BtnWork.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Location = New System.Drawing.Point(105, 18)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(426, 20)
        Me.lblStatus.TabIndex = 4
        Me.lblStatus.Text = "Ready to capture."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BtnSetting
        '
        Me.BtnSetting.BackgroundImage = Global.Trickster_PacketCapture_GUI.My.Resources.Resources.gear
        Me.BtnSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnSetting.Location = New System.Drawing.Point(679, 10)
        Me.BtnSetting.Name = "BtnSetting"
        Me.BtnSetting.Size = New System.Drawing.Size(25, 25)
        Me.BtnSetting.TabIndex = 9
        Me.BtnSetting.TabStop = False
        '
        'BtnLoad
        '
        Me.BtnLoad.BackgroundImage = Global.Trickster_PacketCapture_GUI.My.Resources.Resources.load
        Me.BtnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnLoad.Location = New System.Drawing.Point(710, 10)
        Me.BtnLoad.Name = "BtnLoad"
        Me.BtnLoad.Size = New System.Drawing.Size(25, 25)
        Me.BtnLoad.TabIndex = 8
        Me.BtnLoad.TabStop = False
        '
        'BtnSave
        '
        Me.BtnSave.BackgroundImage = CType(resources.GetObject("BtnSave.BackgroundImage"), System.Drawing.Image)
        Me.BtnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnSave.Location = New System.Drawing.Point(741, 10)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(25, 25)
        Me.BtnSave.TabIndex = 7
        Me.BtnSave.TabStop = False
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(782, 377)
        Me.Controls.Add(Me.BtnSetting)
        Me.Controls.Add(Me.BtnLoad)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.BtnWork)
        Me.Controls.Add(Me.TxtPacket_Info)
        Me.Controls.Add(Me.TxtPacket_Data)
        Me.Controls.Add(Me.LstMain)
        Me.Font = New System.Drawing.Font("D2Coding", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FrmMain"
        Me.Text = "Trickster PacketCapture Tool - GUI"
        CType(Me.BtnSetting, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnSave, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LstMain As ListView
    Friend WithEvents Time As ColumnHeader
    Friend WithEvents Port As ColumnHeader
    Friend WithEvents Opcode As ColumnHeader
    Friend WithEvents PacketSize As ColumnHeader
    Friend WithEvents Data As ColumnHeader
    Friend WithEvents TxtPacket_Data As TextBox
    Friend WithEvents TxtPacket_Info As TextBox
    Friend WithEvents BtnWork As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents Count As ColumnHeader
    Friend WithEvents BtnSave As PictureBox
    Friend WithEvents BtnLoad As PictureBox
    Friend WithEvents BtnSetting As PictureBox
End Class
