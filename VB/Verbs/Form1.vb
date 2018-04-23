Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.ComponentModel.Design
Imports System.Reflection
Imports DevExpress.XtraCharts
Imports DevExpress.XtraEditors

Namespace Verbs
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            propertyGridControl1.SelectedObject = New BusinessObject()
            propertyVerbsControl1.PopulateVerbs()
        End Sub
    End Class

    Public Class BusinessObject
        Inherits ChartControl

        ' An arbitrary property to see in the PropertyGrid
        Public Property Color() As Color

        ' ** Item of interest ** Mark the method(s) we want to see in the PropertyGrid's command pane
        ' We reuse the BrowsableAttribute here to flag this as a method to expose
        ' We could create a new attribute class specifically for this purpose
        ' But this seems quite appropriate
        ' Interesting that this attribute was even designed to be applied to methods
        ' I hope that doesn't imply a possible conflict in the future
        <Browsable(True)> _
        Public Sub TestMethod()
            MessageBox.Show("TestMethod invoked: " & Color.ToString())
        End Sub

        #Region "IComponent Members"
        ' IComponent required by PropertyGrid control to discover IMenuCommandService supporting DesignerVerbs

        Public Shadows Event Disposed As EventHandler

        ' ** Item of interest ** Return the site object that supports DesignerVerbs
        <Browsable(False)> _
        Public Overrides Property Site() As ISite
            ' return our "site" which connects back to us to expose our tagged methods
            Get
                Return New DesignerVerbSite(Me)
            End Get
            Set(ByVal value As ISite)
                Throw New NotImplementedException()
            End Set
        End Property

        #End Region

        #Region "IDisposable Members"
        ' IDisposable, part of IComponent support

        Public Shadows Sub Dispose()
            ' never called in this specific context with the PropertyGrid
            ' but just reference the required Disposed event to avoid warnings
            RaiseEvent Disposed(Me, EventArgs.Empty)
        End Sub

        #End Region
    End Class

    Public Class DesignerVerbSite
        Implements IMenuCommandService, ISite

        ' our target object
        Protected _Component As Object

        Public Sub New(ByVal component As Object)
            _Component = component
        End Sub

        #Region "IMenuCommandService Members"
        ' IMenuCommandService provides DesignerVerbs, seen as commands in the PropertyGrid control

        Public Sub AddCommand(ByVal command As MenuCommand) Implements IMenuCommandService.AddCommand
            Throw New NotImplementedException()
        End Sub

        Public Sub AddVerb(ByVal verb As DesignerVerb) Implements IMenuCommandService.AddVerb
            Throw New NotImplementedException()
        End Sub

        Public Function FindCommand(ByVal commandID As CommandID) As MenuCommand Implements IMenuCommandService.FindCommand
            Throw New NotImplementedException()
        End Function

        Public Function GlobalInvoke(ByVal commandID As CommandID) As Boolean Implements IMenuCommandService.GlobalInvoke
            Throw New NotImplementedException()
        End Function

        Public Sub RemoveCommand(ByVal command As MenuCommand) Implements IMenuCommandService.RemoveCommand
            Throw New NotImplementedException()
        End Sub

        Public Sub RemoveVerb(ByVal verb As DesignerVerb) Implements IMenuCommandService.RemoveVerb
            Throw New NotImplementedException()
        End Sub

        Public Sub ShowContextMenu(ByVal menuID As CommandID, ByVal x As Integer, ByVal y As Integer) Implements IMenuCommandService.ShowContextMenu
            Throw New NotImplementedException()
        End Sub

        ' ** Item of interest ** Return the DesignerVerbs collection
        Public ReadOnly Property Verbs() As DesignerVerbCollection Implements IMenuCommandService.Verbs
            Get

                Dim Verbs_Renamed As New DesignerVerbCollection()
                ' Use reflection to enumerate all the public methods on the object
                Dim mia() As MethodInfo = _Component.GetType().GetMethods(BindingFlags.Public Or BindingFlags.Instance)
                For Each mi As MethodInfo In mia
                    ' Ignore any methods without a [Browsable(true)] attribute
                    Dim attrs() As Object = mi.GetCustomAttributes(GetType(BrowsableAttribute), True)
                    If attrs Is Nothing OrElse attrs.Length = 0 Then
                        Continue For
                    End If
                    If Not DirectCast(attrs(0), BrowsableAttribute).Browsable Then
                        Continue For
                    End If
                    ' Add a DesignerVerb with our VerbEventHandler
                    ' The method name will appear in the command pane
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                    Verbs_Renamed.Add(New DesignerVerb(mi.Name, AddressOf VerbEventHandler))
                Next mi
                Return Verbs_Renamed
            End Get
        End Property

        ' ** Item of interest ** Handle invokaction of the DesignerVerbs
        Private Sub VerbEventHandler(ByVal sender As Object, ByVal e As EventArgs)
            ' The verb is the sender
            Dim verb As DesignerVerb = TryCast(sender, DesignerVerb)
            ' Enumerate the methods again to find the one named by the verb
            Dim mia() As MethodInfo = _Component.GetType().GetMethods(BindingFlags.Public Or BindingFlags.Instance)
            For Each mi As MethodInfo In mia
                Dim attrs() As Object = mi.GetCustomAttributes(GetType(BrowsableAttribute), True)
                If attrs Is Nothing OrElse attrs.Length = 0 Then
                    Continue For
                End If
                If Not DirectCast(attrs(0), BrowsableAttribute).Browsable Then
                    Continue For
                End If
                If verb.Text = mi.Name Then
                    ' Invoke the method on our object (no parameters)
                    mi.Invoke(_Component, Nothing)
                    Return
                End If
            Next mi
        End Sub

        #End Region

        #Region "ISite Members"
        ' ISite required to represent this object directly to the PropertyGrid

        Public ReadOnly Property Component() As IComponent Implements ISite.Component
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        ' ** Item of interest ** Implement the Container property
        Public ReadOnly Property Container() As IContainer Implements ISite.Container
            ' Returning a null Container works fine in this context
            Get
                Return Nothing
            End Get
        End Property

        ' ** Item of interest ** Implement the DesignMode property
        Public ReadOnly Property DesignMode() As Boolean Implements ISite.DesignMode
            ' While this *is* called, it doesn't seem to matter whether we return true or false
            Get
                Return True
            End Get
        End Property

        Public Property Name() As String Implements ISite.Name
            Get
                Throw New NotImplementedException()
            End Get
            Set(ByVal value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        #End Region

        #Region "IServiceProvider Members"
        ' IServiceProvider is the mechanism used by the PropertyGrid to discover our IMenuCommandService support

        ' ** Item of interest ** Respond to requests for IMenuCommandService
        Public Function GetService(ByVal serviceType As Type) As Object Implements System.IServiceProvider.GetService
            If serviceType Is GetType(IMenuCommandService) Then
                Return Me
            End If
            Return Nothing
        End Function

        #End Region
    End Class
End Namespace
