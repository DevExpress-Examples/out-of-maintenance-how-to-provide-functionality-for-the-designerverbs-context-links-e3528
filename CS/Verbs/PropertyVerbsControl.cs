using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraVerticalGrid;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using DevExpress.Utils.Drawing;

namespace Verbs {
    public class PropertyVerbsControl : PanelControl {
        PropertyGridControl fPropertyGridControl;
        public PropertyGridControl PropertyGridControl {
            get {
                return fPropertyGridControl;
            }
            set {
                if (fPropertyGridControl == value)
                    return;
                fPropertyGridControl = value;
                PopulateVerbs();
            }
        }

        DesignerVerbCollection GetVerbs() {
            if (DesignMode)
                return null;
            if (PropertyGridControl == null)
                return null;
            Component component = PropertyGridControl.SelectedObject as Component;
            if (component == null)
                return null;
            ISite site = component.Site;
            IMenuCommandService service = (IMenuCommandService)site.GetService(typeof(IMenuCommandService));
            if (service == null)
                return null;
            return service.Verbs;
        }

        public void PopulateVerbs() {
            DesignerVerbCollection verbs = GetVerbs();
            if (verbs == null)
                return;
            UnsubscribeEvents();
            HyperLinkEdit oldEditor = null;
            HyperLinkEdit editor = null;
            Controls.Clear();

            foreach (DesignerVerb verb in verbs) {
                oldEditor = editor;
                editor = new HyperLinkEdit();
                editor.Properties.Appearance.BackColor = Color.Transparent;
                editor.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                editor.CustomDisplayText += editor_CustomDisplayText;
                editor.OpenLink += editor_OpenLink;
                Controls.Add(editor);
                GraphicsInfo.Default.AddGraphics(null);
                SizeF calcTextSize = editor.Properties.Appearance.CalcTextSize(GraphicsInfo.Default.Cache, verb.Text, Width);
                GraphicsInfo.Default.ReleaseGraphics();
                editor.Width = calcTextSize.ToSize().Width + 10;
                if (oldEditor != null) {
                    if (editor.Width + oldEditor.Bounds.Right > Width)
                        editor.Location = new Point(0, oldEditor.Bottom);
                    else
                        editor.Location = new Point(oldEditor.Bounds.Right, oldEditor.Bounds.Y);

                }
                editor.EditValue = verb;
            }
        }

        void UnsubscribeEvents() {
            foreach (HyperLinkEdit editor in Controls) {
                editor.CustomDisplayText -= editor_CustomDisplayText;
                editor.OpenLink -= editor_OpenLink;
            }
        }

        void editor_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e) {
            (e.EditValue as DesignerVerb).Invoke();
        }

        void editor_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e) {
            e.DisplayText = (e.Value as DesignerVerb).Text + ",";
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            PopulateVerbs();
        }
    }
}
