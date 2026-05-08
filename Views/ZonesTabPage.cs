using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldenEraTemplateEditor.Views
{
    public class ZonesTabPage : TabPage
    {
        public ZonesTabPage()
        {
            Text = "Zones";

            var label = new Label()
            {
                Text = "Zones",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(label);
        }
    }
}