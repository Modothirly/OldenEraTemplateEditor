using System.Text.Json;
using OldenEraTemplateEditor.Common;
using OldenEraTemplateEditor.Services;
using OldenEraTemplateEditor.Views;
using OldenEraTemplateEditor.Views.Dialog;
using OldenEraTemplateEditor.Views.LayoutEngine;
using OldenEraTemplateEditor.Views.PanelSupport;

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

        public void neww()
        {
            rmgTemplate = new();
            variantList = new();
        }

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
                    forceDirectedLayout.AutoLayout(variantModel, variant.Connections);
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
            var imageFilepath = path.Replace(".rmg.json", ".png");
            if (rmgTemplate.Variants.Count > 0)
            {
                ImageSupport.outputImage(rmgTemplate.Variants[0], variantList[0], imageFilepath);
            }

        }

        public int AddVariant()
        {
            return ZoneService.AddVariant(this);
        }
        public void DeleteVariant(int variantIndex)
        {
            ZoneService.DeleteVariant(this, variantIndex);
        }

        public void AddZone(ZoneFormDto ZoneFormDto, int variantIndex, int x, int y)
        {
            ZoneService.AddZone(this, ZoneFormDto, variantIndex, x, y);
        }
        public void AddConnection(ConnectionFormDto ConnectionFormDto, int variantIndex)
        {
            ZoneService.AddConnection(this, ConnectionFormDto, variantIndex);
        }
        public void DeleteZone(string ZoneName, int variantIndex)
        {
            ZoneService.DeleteZone(this, ZoneName, variantIndex);
        }
        public void DeleteConnection(string ConnectionName, int variantIndex)
        {
            ZoneService.DeleteConnection(this, ConnectionName, variantIndex);
        }

        public void AddMandatoryContentGroup(MandatoryContentDto dto)
        {
            ContentService.AddMandatoryContentGroup(this, dto);
        }
        public void AddMandatoryContentItem(MandatoryContentDto dto)
        {
            ContentService.AddMandatoryContentItem(this, dto);
        }
        public bool DeleteMandatoryContentGroup(MandatoryContentGroup group)
        {
            return ContentService.DeleteMandatoryContentGroup(this, group);
        }
        public void DeleteMandatoryContentItem(MandatoryContentGroup group, ContentItem item)
        {
            ContentService.DeleteMandatoryContentItem(group, item);
        }

        public void AddContentCountLimit(ContentCountLimitGroupDto dto)
        {
            ContentService.AddContentCountLimit(this, dto);
        }
        public void AddContentCountLimitItem(ContentCountLimit limit, ContentCountLimitItemDto dto)
        {
            ContentService.AddContentCountLimitItem(limit, dto);
        }
        public bool DeleteContentCountLimit(ContentCountLimit limit)
        {
            return ContentService.DeleteContentCountLimit(this, limit);
        }
        public void DeleteContentCountLimitItem(ContentCountLimit limit, ContentSidLimit item)
        {
            ContentService.DeleteContentCountLimitItem(limit, item);
        }

        public void AddZoneLayout(string name)
        {
            ContentService.AddZoneLayout(this, name);
        }
        public bool DeleteZoneLayout(ZoneLayout layout)
        {
            return ContentService.DeleteZoneLayout(this, layout);
        }

        public void ApplyGlobalDto(GlobalDto dto)
        {
            RmgService.ApplyGlobalDto(this, dto);
        }
    }
}