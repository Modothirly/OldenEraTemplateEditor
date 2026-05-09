using OldenEraTemplateEditor.Models;

namespace OldenEraTemplateEditor.Views
{
    public class GlobalTabPage : EditorTabPage
    {
        public GlobalTabPage(Rmg rmg, Settings settings) : base(rmg, settings)
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

        public override void RefreshData()
        {
            // TODO
        }
    }
}