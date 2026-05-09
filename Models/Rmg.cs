using System.Text.Json;
using OldenEraTemplateEditor.Common;
using OldenEraTemplateEditor.Views;

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

        public void input(string path)
        {
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions();
            options.Converters.Add(
                new SingleOrArrayConverterFactory());
            var s = JsonSerializer.Deserialize<RmgTemplate>(json, options);
            if (s is null) throw new InvalidDataException("File is empty or invalid.");
            rmgTemplate = s;
            if (rmgTemplate.Variants == null)
            {
                rmgTemplate.Variants = new();
            }
            variantList.Clear();
            for (int i = 0; i < rmgTemplate.Variants?.Count; i++)
            {
                var variant = rmgTemplate.Variants?[i];
                if (variant == null) return;

                VariantModel variantModel = new();
                variantModel.RebuildCanvasData(variant);
                variantList.Add(variantModel);

            }
        }

        public void output(string path)
        {
            var json = JsonSerializer.Serialize(rmgTemplate, JsonOptions);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// 从 Unfrozen 模型重建画布数据（ZoneNode / ZoneConnection）
        /// </summary>
        public void RebuildCanvasData(int i)
        {


        }
    }
}