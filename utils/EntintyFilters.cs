using System.Text.RegularExpressions;

namespace restaurant_api.Utils
{
    public static class EntintyFilters
    {
        public static bool HasSearchString(string name, string? searchString)
        {
            if (searchString == null) return true;
            var regex = new Regex($"{searchString}", RegexOptions.IgnoreCase);
            return regex.IsMatch(name);
        }
    }
}
