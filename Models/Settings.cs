using OldenEraTemplateEditor.Views;

namespace OldenEraTemplateEditor.Models
{
    public class Settings : IToolStripFileModel
    {
        public string dialogFilter => "JSON Files (*.json)|*.json";
        public void input(string json)
        {
            throw new NotImplementedException();
        }

        public void output(string json)
        {
            throw new NotImplementedException();
        }
    }
}