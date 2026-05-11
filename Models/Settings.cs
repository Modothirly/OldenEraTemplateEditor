using OldenEraTemplateEditor.Views;

namespace OldenEraTemplateEditor.Models
{
    public class Settings : IToolStripFileModel
    {
        public string dialogFilter => "JSON Files (*.json)|*.json";
        public void input(string json)
        {
            // TODO
            // throw new NotImplementedException();
        }

        public void output(string json)
        {
            // TODO
            // throw new NotImplementedException();
        }
    }
}