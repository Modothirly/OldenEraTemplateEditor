using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OldenEraTemplateEditor.Views
{
    public class GlobalTabPage : TabPage
    {
        public GlobalTabPage()
        {
            Text = "Global";

            var label = new Label()
            {
                Text = "Global",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(label);
        }
    }
}