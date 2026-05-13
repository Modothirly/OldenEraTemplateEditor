using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using OldenEraTemplateEditor.Common;

namespace OldenEraTemplateEditor.Views.Dialog
{
    public class ConnectionFormDialog : FormDialog<ConnectionFormDto>
    {
        protected override string title => "Connection";
        public ConnectionFormDialog(ConnectionFormDto obj) : base(obj)
        {
        }

    }

    public class ConnectionFormDto
    {
        [Category("1.from/to")]
        [ReadOnly(true)]
        public string From { get; set; }
        [Category("1.from/to")]
        [ReadOnly(true)]
        public string To { get; set; }
        [TypeConverter(typeof(ConnectionTypeSelectConverter))]
        public string ConnectionType { get; set; }
        public int GuardValue { get; set; }
    }
}