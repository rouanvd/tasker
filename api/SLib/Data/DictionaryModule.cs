using System.Collections.Generic;
using System.Linq;

namespace SLib.Data
{
    /// <summary>
    ///   Module that provides common methods for working with Dictionary values.
    /// </summary>
    public static class DictionaryModule
    {
        public static bool ContainsKeyCI<U>(this IDictionary<string,U> dict, string key)
        {
            if (dict == null || string.IsNullOrWhiteSpace( key ))
                return false;

            IEnumerable<string> existingKeys = dict.Keys.Select(k => k?.ToUpper()).ToList();
            key = key.ToUpper();

            bool containsKey = existingKeys.Contains( key );
            return containsKey;
        }


        public static IDictionary<string,U> ToDictionaryWithUpperCaseKeys<U>(this IDictionary<string,U> dict)
        {
            if (dict == null)
                return null;

            Dictionary<string, U> upperCaseKeyDict = dict.ToDictionary( kvp => kvp.Key?.ToUpper(), kvp => kvp.Value );
            return upperCaseKeyDict;
        }
    }
}
