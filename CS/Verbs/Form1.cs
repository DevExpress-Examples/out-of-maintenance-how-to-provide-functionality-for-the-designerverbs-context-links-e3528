using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Reflection;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;

namespace Verbs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            propertyGridControl1.SelectedObject = new BusinessObject();
            propertyVerbsControl1.PopulateVerbs();
        }
    }

    public class BusinessObject : ChartControl
    {
        // An arbitrary property to see in the PropertyGrid
        public Color Color { get; set; }

        // ** Item of interest ** Mark the method(s) we want to see in the PropertyGrid's command pane
        // We reuse the BrowsableAttribute here to flag this as a method to expose
        // We could create a new attribute class specifically for this purpose
        // But this seems quite appropriate
        // Interesting that this attribute was even designed to be applied to methods
        // I hope that doesn't imply a possible conflict in the future
        [Browsable(true)]
        public void TestMethod()
        {
            MessageBox.Show("TestMethod invoked: " + Color);
        }

        #region IComponent Members
        // IComponent required by PropertyGrid control to discover IMenuCommandService supporting DesignerVerbs

        public new event EventHandler Disposed;

        // ** Item of interest ** Return the site object that supports DesignerVerbs
        [Browsable(false)]
        public override ISite Site
        {
            // return our "site" which connects back to us to expose our tagged methods
            get
            {
                return new DesignerVerbSite(this);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IDisposable Members
        // IDisposable, part of IComponent support

        public new void Dispose()
        {
            // never called in this specific context with the PropertyGrid
            // but just reference the required Disposed event to avoid warnings
            if (Disposed != null)
                Disposed(this, EventArgs.Empty);
        }

        #endregion
    }

    public class DesignerVerbSite : IMenuCommandService, ISite
    {
        // our target object
        protected object _Component;

        public DesignerVerbSite(object component)
        {
            _Component = component;
        }

        #region IMenuCommandService Members
        // IMenuCommandService provides DesignerVerbs, seen as commands in the PropertyGrid control

        public void AddCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        public void AddVerb(DesignerVerb verb)
        {
            throw new NotImplementedException();
        }

        public MenuCommand FindCommand(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        public bool GlobalInvoke(CommandID commandID)
        {
            throw new NotImplementedException();
        }

        public void RemoveCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        public void RemoveVerb(DesignerVerb verb)
        {
            throw new NotImplementedException();
        }

        public void ShowContextMenu(CommandID menuID, int x, int y)
        {
            throw new NotImplementedException();
        }

        // ** Item of interest ** Return the DesignerVerbs collection
        public DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection Verbs = new DesignerVerbCollection();
                // Use reflection to enumerate all the public methods on the object
                MethodInfo[] mia = _Component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo mi in mia)
                {
                    // Ignore any methods without a [Browsable(true)] attribute
                    object[] attrs = mi.GetCustomAttributes(typeof(BrowsableAttribute), true);
                    if (attrs == null || attrs.Length == 0)
                        continue;
                    if (!((BrowsableAttribute)attrs[0]).Browsable)
                        continue;
                    // Add a DesignerVerb with our VerbEventHandler
                    // The method name will appear in the command pane
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                    Verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                }
                return Verbs;
            }
        }

        // ** Item of interest ** Handle invokaction of the DesignerVerbs
        private void VerbEventHandler(object sender, EventArgs e)
        {
            // The verb is the sender
            DesignerVerb verb = sender as DesignerVerb;
            // Enumerate the methods again to find the one named by the verb
            MethodInfo[] mia = _Component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo mi in mia)
            {
                object[] attrs = mi.GetCustomAttributes(typeof(BrowsableAttribute), true);
                if (attrs == null || attrs.Length == 0)
                    continue;
                if (!((BrowsableAttribute)attrs[0]).Browsable)
                    continue;
                if (verb.Text == mi.Name)
                {
                    // Invoke the method on our object (no parameters)
                    mi.Invoke(_Component, null);
                    return;
                }
            }
        }

        #endregion

        #region ISite Members
        // ISite required to represent this object directly to the PropertyGrid

        public IComponent Component
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // ** Item of interest ** Implement the Container property
        public IContainer Container
        {
            // Returning a null Container works fine in this context
            get
            {
                return null;
            }
        }

        // ** Item of interest ** Implement the DesignMode property
        public bool DesignMode
        {
            // While this *is* called, it doesn't seem to matter whether we return true or false
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IServiceProvider Members
        // IServiceProvider is the mechanism used by the PropertyGrid to discover our IMenuCommandService support

        // ** Item of interest ** Respond to requests for IMenuCommandService
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMenuCommandService))
                return this;
            return null;
        }

        #endregion
    }
}
