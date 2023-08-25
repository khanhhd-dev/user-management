using Newtonsoft.Json;
using System.Collections;
using System.Reflection;

namespace DigitalPlatform.UserService.Share
{
    public static class StringExtension
    {
        public static bool IsValid(this string text)
        {
            return !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }

        public static string ToJson(this object obj)
        {
            if (obj == null) return string.Empty;
            return JsonConvert.SerializeObject(obj);
        }

        public static void Trim<T>(T instance)
        {
            try
            {
                if (instance == null)
                    return;

                if (instance is IEnumerable)
                {
                    foreach (var listItem in instance as IEnumerable)
                        Trim(listItem);
                    return;
                }

                var props = instance.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    // Ignore non-string properties
                    //.Where(prop => prop.PropertyType == typeof(string))
                    // Ignore indexers
                    .Where(prop => prop.GetIndexParameters().Length == 0)
                    // Must be both readable and writable
                    .Where(prop => prop.CanWrite && prop.CanRead);

                foreach (PropertyInfo prop in props)
                {
                    var nodeType = prop.PropertyType;
                    if (nodeType == typeof(string))
                    {
                        string currentValue = (string)prop.GetValue(instance, null);
                        if (currentValue != null)
                        {
                            prop.SetValue(instance, currentValue.Trim(), null);
                        }
                    }
                    else if (nodeType != typeof(object) && Type.GetTypeCode(nodeType) == TypeCode.Object)
                    {
                        Trim(prop.GetValue(instance, null));
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

    }
}
