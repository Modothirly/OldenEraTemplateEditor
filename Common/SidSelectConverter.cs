using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace OldenEraTemplateEditor.Common
{
    public class SidSelectConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        => true;

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            => true; // 只能选不能输入

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            string[] values = Constant.Sids;
            return new StandardValuesCollection(values);
        }
    }
}