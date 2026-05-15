using OldenEraTemplateEditor.Models;

namespace OldenEraTemplateEditor.Views
{
    abstract public class EditorTabPage(Rmg rmg) : TabPage
    {
        public readonly Rmg rmg = rmg;
        public abstract void RefreshData();

    }
}