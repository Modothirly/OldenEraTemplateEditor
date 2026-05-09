using OldenEraTemplateEditor.Models;

namespace OldenEraTemplateEditor.Views
{
    abstract public class EditorTabPage(Rmg rmg, Settings settings) : TabPage
    {
        public readonly Rmg rmg = rmg;
        public readonly Settings settings = settings;

        public abstract void RefreshData();

    }
}