using Notification.Email.Interfaces;
using System.Reflection;

namespace Notification.Email.Services
{
    public class PlaceholderReplacer : IPlaceholderReplacer
    {
        public string Replace(string input, object obj)
        {
            if (obj == null)
                return input;
            foreach (var property in obj.GetType().GetProperties())
                input = ReplacePlaceholderWithPropertyValue(input, obj, property);
            return input;
        }

        private static string ReplacePlaceholderWithPropertyValue(string input, object obj, PropertyInfo property)
        {
            var propertyValue = property.GetValue(obj, null);
            var propertyValueString = propertyValue?.ToString();
            return input.Replace($"[[{property.Name}]]", propertyValueString);
        }
    }
}
