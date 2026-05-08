using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Views;

namespace OldenEraTemplateEditor.Models
{
    public class Settings : IToolStripFileModel
    {
        void IToolStripFileModel.input(string json)
        {
            throw new NotImplementedException();
        }

        string IToolStripFileModel.output()
        {
            throw new NotImplementedException();
        }
    }
}