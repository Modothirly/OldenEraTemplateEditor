using System.ComponentModel;
using System.Globalization;

namespace OldenEraTemplateEditor.Common
{

public class UniversalObjectConverter<T> : ExpandableObjectConverter where T : class, new()
{
    // 1. 允许用户在 PropertyGrid 的文本框中直接输入文字
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string)) return true;
        return base.CanConvertFrom(context, sourceType);
    }

    // 2. 动态处理用户输入的指令 ("null" 或 "new")
    public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string input)
        {
            input = input.Trim().ToLower();

            // 如果输入为空或 "null"，将整个对象设为 null
            if (string.IsNullOrEmpty(input) || input == "null")
            {
                return null;
            }

            // 如果输入 "new"，利用反射动态创建该属性类型的实例
            if (input == "new")
            {
                // 动态调用 new T()
                return Activator.CreateInstance<T>(); 
            }
        }
        return base.ConvertFrom(context, culture, value);
    }

    // 3. 动态控制未展开状态下，格子里显示的提示文字
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            if (value == null)
            {
                // 动态获取当前类型的名称作为提示
                return $"(Null) 输入 new 创建";
            }
            
            // 对象不为 null 时显示的文字
            return $"清空该行可删除参数";
        }
        return base.ConvertTo(context, culture, value, destinationType)!;
    }
}

}