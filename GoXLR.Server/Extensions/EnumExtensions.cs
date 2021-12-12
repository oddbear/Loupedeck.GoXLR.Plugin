using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GoXLR.Server.Extensions
{
    using System.Collections.Generic;

    public static class EnumExtensions
    {
        public static TEnum EnumParse<TEnum>(string value)
            where TEnum : struct, Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static IEnumerable<TEnum> EnumGetValues<TEnum>()
            where TEnum : struct, Enum
        {
            return (IEnumerable<TEnum>)Enum.GetValues(typeof(TEnum));
        }

        public static bool TryParseEnumFromDescription<TEnum>(string description, out TEnum enumValue)
            where TEnum : struct, Enum
        {
            try
            {
                var values = EnumGetValues<TEnum>();
                foreach (var value in values)
                {
                    if (value.GetEnumDescription() != description)
                        continue;

                    enumValue = value;
                    return true;
                }

                enumValue = default;
                return false;
            }
            catch
            {
                enumValue = default;
                return false;
            }
        }

        public static string[] GetAllEnumDescription<TEnum>()
            where TEnum : struct, Enum
        {
            return EnumGetValues<TEnum>()
                .Select(value => value.GetEnumDescription())
                .ToArray();
        }

        public static string GetEnumDescription<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            var enumType = typeof(TEnum);

            var fieldName = value.ToString();
            var fieldInfo = enumType.GetField(fieldName);

            var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>(false);

            return attribute?.Description
                   ?? throw new InvalidOperationException($"Enum '{enumType.Name}' with value '{value}' is missing the 'DescriptionAttribute'");
        }
    }
}
