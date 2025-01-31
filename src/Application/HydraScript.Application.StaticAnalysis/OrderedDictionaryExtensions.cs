
namespace HydraScript.Application.StaticAnalysis
{
    internal static class OrderedDictionaryExtensions
    {
        public static TValue Extract<TKey, TValue>(
            this OrderedDictionary<TKey, TValue> dict, 
            TKey key) where TKey: notnull
        {
            var value = dict[key];
            dict.Remove(key);
            return value;
        }    
    }
}
