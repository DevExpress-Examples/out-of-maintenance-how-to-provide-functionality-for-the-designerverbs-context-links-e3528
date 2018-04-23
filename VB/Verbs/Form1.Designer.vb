Namespace Verbs
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.propertyGridControl1 = New DevExpress.XtraVerticalGrid.PropertyGridControl()
            Me.propertyVerbsControl1 = New Verbs.PropertyVerbsControl()
            CType(Me.propertyGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.propertyVerbsControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' propertyGridControl1
            ' 
            Me.propertyGridControl1.Dock = System.Windows.Forms.DockStyle.Top
            Me.propertyGridControl1.Location = New System.Drawing.Point(0, 0)
            Me.propertyGridControl1.Name = "propertyGridControl1"
            Me.propertyGridControl1.Size = New System.Drawing.Size(549, 365)
            Me.propertyGridControl1.TabIndex = 0
            ' 
            ' propertyVerbsControl1
            ' 
            Me.propertyVerbsControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.propertyVerbsControl1.Location = New System.Drawing.Point(0, 365)
            Me.propertyVerbsControl1.Name = "propertyVerbsControl1"
            Me.propertyVerbsControl1.PropertyGridControl = Me.propertyGridControl1
            Me.propertyVerbsControl1.Size = New System.Drawing.Size(549, 122)
            Me.propertyVerbsControl1.TabIndex = 1
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(549, 487)
            Me.Controls.Add(Me.propertyVerbsControl1)
            Me.Controls.Add(Me.propertyGridControl1)
            Me.Name = "Form1"
            Me.Text = "Form1"
            CType(Me.propertyGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.propertyVerbsControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        #End Region

        Private propertyGridControl1 As DevExpress.XtraVerticalGrid.PropertyGridControl
        Private propertyVerbsControl1 As PropertyVerbsControl
    End Class
End Namespace

