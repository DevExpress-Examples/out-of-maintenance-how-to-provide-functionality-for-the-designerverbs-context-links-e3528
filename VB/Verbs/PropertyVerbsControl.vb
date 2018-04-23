Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.XtraEditors
Imports DevExpress.XtraVerticalGrid
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing
Imports DevExpress.Utils.Drawing

Namespace Verbs
    Public Class PropertyVerbsControl
        Inherits PanelControl

        Private fPropertyGridControl As PropertyGridControl
        Public Property PropertyGridControl() As PropertyGridControl
            Get
                Return fPropertyGridControl
            End Get
            Set(ByVal value As PropertyGridControl)
                If fPropertyGridControl Is value Then
                    Return
                End If
                fPropertyGridControl = value
                PopulateVerbs()
            End Set
        End Property

        Private Function GetVerbs() As DesignerVerbCollection
            If DesignMode Then
                Return Nothing
            End If
            If PropertyGridControl Is Nothing Then
                Return Nothing
            End If
            Dim component As Component = TryCast(PropertyGridControl.SelectedObject, Component)
            If component Is Nothing Then
                Return Nothing
            End If
            Dim site As ISite = component.Site
            Dim service As IMenuCommandService = DirectCast(site.GetService(GetType(IMenuCommandService)), IMenuCommandService)
            If service Is Nothing Then
                Return Nothing
            End If
            Return service.Verbs
        End Function

        Public Sub PopulateVerbs()
            Dim verbs As DesignerVerbCollection = GetVerbs()
            If verbs Is Nothing Then
                Return
            End If
            UnsubscribeEvents()
            Dim oldEditor As HyperLinkEdit = Nothing
            Dim editor As HyperLinkEdit = Nothing
            Controls.Clear()

            For Each verb As DesignerVerb In verbs
                oldEditor = editor
                editor = New HyperLinkEdit()
                editor.Properties.Appearance.BackColor = Color.Transparent
                editor.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
                AddHandler editor.CustomDisplayText, AddressOf editor_CustomDisplayText
                AddHandler editor.OpenLink, AddressOf editor_OpenLink
                Controls.Add(editor)
                GraphicsInfo.Default.AddGraphics(Nothing)
                Dim calcTextSize As SizeF = editor.Properties.Appearance.CalcTextSize(GraphicsInfo.Default.Cache, verb.Text, Width)
                GraphicsInfo.Default.ReleaseGraphics()
                editor.Width = calcTextSize.ToSize().Width + 10
                If oldEditor IsNot Nothing Then
                    If editor.Width + oldEditor.Bounds.Right > Width Then
                        editor.Location = New Point(0, oldEditor.Bottom)
                    Else
                        editor.Location = New Point(oldEditor.Bounds.Right, oldEditor.Bounds.Y)
                    End If

                End If
                editor.EditValue = verb
            Next verb
        End Sub

        Private Sub UnsubscribeEvents()
            For Each editor As HyperLinkEdit In Controls
                RemoveHandler editor.CustomDisplayText, AddressOf editor_CustomDisplayText
                RemoveHandler editor.OpenLink, AddressOf editor_OpenLink
            Next editor
        End Sub

        Private Sub editor_OpenLink(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs)
            TryCast(e.EditValue, DesignerVerb).Invoke()
        End Sub

        Private Sub editor_CustomDisplayText(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs)
            e.DisplayText = (TryCast(e.Value, DesignerVerb)).Text & ","
        End Sub

        Protected Overrides Sub OnResize(ByVal e As EventArgs)
            MyBase.OnResize(e)
            PopulateVerbs()
        End Sub
    End Class
End Namespace
