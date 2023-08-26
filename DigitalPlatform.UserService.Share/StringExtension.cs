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
        public static string GeneratePassword(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated;
        }
    }
}
