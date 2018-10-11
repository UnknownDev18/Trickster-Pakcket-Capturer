<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSetting
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
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

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblAdapter = New System.Windows.Forms.Label()
        Me.LstAdapter = New System.Windows.Forms.ComboBox()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblAdapter
        '
        Me.lblAdapter.AutoSize = True
        Me.lblAdapter.Location = New System.Drawing.Point(12, 18)
        Me.lblAdapter.Name = "lblAdapter"
        Me.lblAdapter.Size = New System.Drawing.Size(91, 14)
        Me.lblAdapter.TabIndex = 0
        Me.lblAdapter.Text = "Select adapter"
        '
        'LstAdapter
        '
        Me.LstAdapter.FormattingEnabled = True
        Me.LstAdapter.Location = New System.Drawing.Point(15, 40)
        Me.LstAdapter.Name = "LstAdapter"
        Me.LstAdapter.Size = New System.Drawing.Size(493, 22)
        Me.LstAdapter.TabIndex = 1
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(440, 118)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(68, 27)
        Me.BtnClose.TabIndex = 2
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'FrmSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(520, 166)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.LstAdapter)
        Me.Controls.Add(Me.lblAdapter)
        Me.Font = New System.Drawing.Font("D2Coding", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmSetting"
        Me.Text = "Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblAdapter As Label
    Friend WithEvents LstAdapter As ComboBox
    Friend WithEvents BtnClose As Button
End Class
