using System.ComponentModel;
using OldenEraTemplateEditor.Common;
using OldenEraTemplateEditor.Models;

namespace OldenEraTemplateEditor.Views.Dialog
{
    public class ContentCountLimitGroupDialog : FormDialog<ContentCountLimitGroupDto>
    {
        protected override string title => "ContentCountLimit";
        public ContentCountLimitGroupDialog(ContentCountLimitGroupDto dto) : base(dto) { }
    }

    public class ContentCountLimitItemDialog : FormDialog<ContentCountLimitItemDto>
    {
        protected override string title => "ContentSidLimit";
        public ContentCountLimitItemDialog(ContentCountLimitItemDto dto) : base(dto) { }
    }

    public class ContentCountLimitGroupDto
    {
        public string name { get; set; } = "new_limit";
    }

    public class ContentCountLimitItemDto
    {
        [TypeConverter(typeof(SidSelectConverter))]
        public string sid { get; set; } = "";
        public int maxCount { get; set; } = 1;
    }

    public class ZoneLayoutNameDialog : FormDialog<ZoneLayoutNameDto>
    {
        protected override string title => "ZoneLayout";
        public ZoneLayoutNameDialog(ZoneLayoutNameDto dto) : base(dto) { }
    }

    public class ZoneLayoutNameDto
    {
        public string name { get; set; } = "new_layout";
    }
}
