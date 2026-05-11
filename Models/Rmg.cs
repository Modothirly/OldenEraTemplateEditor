using System.Text.Json;
using OldenEraTemplateEditor.Common;
using OldenEraTemplateEditor.Views;
using OldenEraTemplateEditor.Views.LayoutEngine;

namespace OldenEraTemplateEditor.Models
{
    public class Rmg : IToolStripFileModel
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public RmgTemplate rmgTemplate = new();
        public List<VariantModel> variantList = new();

        public string dialogFilter => "JSON Files (*.rmg.json)|*.rmg.json";

        public void input(string path)
        {
            var posFilepath = path.Replace(".rmg.json", ".rmg.pos.json");
            var json = File.ReadAllText(path);

            var options = new JsonSerializerOptions();
            options.Converters.Add(
                new SingleOrArrayConverterFactory());
            var s = JsonSerializer.Deserialize<RmgTemplate>(json, options);
            if (s is null) throw new InvalidDataException("File is empty or invalid.");

            VariantModelList? variantModelList = null;
            if (File.Exists(posFilepath))
            {
                var posJson = File.ReadAllText(posFilepath);
                variantModelList = JsonSerializer.Deserialize<VariantModelList>(posJson, options);

            }

            rmgTemplate = s;
            if (rmgTemplate.Variants == null)
            {
                rmgTemplate.Variants = new();
            }
            if (variantModelList is null)
            {

                variantList.Clear();
               var forceDirectedLayout = new ForceDirectedLayout();
                for (int i = 0; i < rmgTemplate.Variants?.Count; i++)
                {
                    var variant = rmgTemplate.Variants?[i];
                    if (variant == null) return;

                    VariantModel variantModel = new();
                    variantModel.RebuildCanvasData(variant);
                    variantList.Add(variantModel);
                    forceDirectedLayout.AutoLayout(variantModel);
                }
            }
            else
            {
                variantList = variantModelList.VariantList;
            }
        }

        public void output(string path)
        {
            var json = JsonSerializer.Serialize(rmgTemplate, JsonOptions);
            File.WriteAllText(path, json);

            var posFilepath = path.Replace(".rmg.json", ".rmg.pos.json");

            VariantModelList variantModelList = new();
            variantModelList.VariantList = this.variantList;
            var posJson = JsonSerializer.Serialize(variantModelList, JsonOptions);
            File.WriteAllText(posFilepath, posJson);

        }

    }
}