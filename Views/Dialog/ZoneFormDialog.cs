using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Common;

namespace OldenEraTemplateEditor.Views.Dialog
{
    public class ZoneFormDialog : FormDialog<ZoneFormDto>
    {

        protected override string title => "zone";
        public ZoneFormDialog(ZoneFormDto dto) : base(dto)
        {
        }
    }

    public class ZoneFormDto
    {
        [Category("1.name")]
        public string name { get; set; }
        [TypeConverter(typeof(PlayerSelectConverter))]
        [Category("2.city/spawn")]
        public string spawn { get; set; }
        [Category("2.city/spawn")]
        public int cityAmount { get; set; }
        public double? GuardMultiplier { get; set; }
        public int? GuardedContentValue { get; set; }
        public int? UnguardedContentValue { get; set; }
        public int? ResourcesValue { get; set; }
    }
}