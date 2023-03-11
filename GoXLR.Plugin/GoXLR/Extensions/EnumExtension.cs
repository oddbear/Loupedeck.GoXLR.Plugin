using System;
using System.ComponentModel;
using System.Reflection;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var description = fieldInfo.GetCustomAttribute<DescriptionAttribute>(false);
            return description?.Description ?? enumValue.ToString();
        }
    }
}