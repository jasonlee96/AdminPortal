using System.Data;
using System.Reflection;
using System.Runtime.Serialization;

namespace CommonService
{
    public static class HelperMethodExtension
    {
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
        public static string Underscore(this string value)
            => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));

        public static string GetEnumName<T>(this T val)
        {
            if (!typeof(T).IsEnum)
                throw new Exception("Object is not an enumerated type!");
            return Enum.GetName(typeof(T), val);
        }

        /// <summary>
        /// This method is to retrieve EnumMemberAttribute's Value if any. Otherwise it will execute GetEnumName instead.
        /// </summary>
        public static string GetEnumValue<T>(this T val) where T : Enum
        {
            if (!typeof(T).IsEnum)
                throw new Exception("Object is not an enumerated type!");

            var value = typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == val.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;

            return value ?? Enum.GetName(typeof(T), val);
        }

        public static T ToEnum<T>(this string val)
        {
            if (!typeof(T).IsEnum)
                throw new Exception("T is not an enumerated type!");

            return (T)Enum.Parse(typeof(T), val, true);
        }

        
    }
}
