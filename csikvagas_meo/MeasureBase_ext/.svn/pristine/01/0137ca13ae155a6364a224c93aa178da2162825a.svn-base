using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace e77.MeasureBase
{
    /// <summary>
    /// FlattenDictionaryHierarchy extension method can flatten this Value too (and IDictionaly by default)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFlattenable<T>
    {
        KeyValuePair<string, T>[] Flatten(string prefix_in);
    }

    public static class HierarchyDatabaseHelper
    {
        public static char LEVEL_SEPARATOR_CHAR = '/';

        static public string CutFirstLevel(string str_in)
        {
            int pos = str_in.IndexOf(LEVEL_SEPARATOR_CHAR);
            if (pos == -1)
                throw new ArgumentException(string.Format(
                    "There is not level separator ('{0}' at '{1}')",
                    LEVEL_SEPARATOR_CHAR, str_in));

            return str_in.Substring(pos, str_in.Length - pos);
        }

        static public string CutLastLevel(string str_in)
        {
            int pos = str_in.LastIndexOf(LEVEL_SEPARATOR_CHAR);
            if (pos == -1)
                throw new ArgumentException(string.Format(
                    "There is not level separator ('{0}' at '{1}')",
                    LEVEL_SEPARATOR_CHAR, str_in));

            return str_in.Substring(0, pos);
        }
        

        static public List<string> GetAllChildId<T>(IDictionary<string, T> dictionary_in, string parent_in)
        {
            List<string> res = new List<string>();

            foreach (string key in dictionary_in.Keys)
                if (key.StartsWith(parent_in))
                    res.Add(key);

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent_in"can be null or string.Empty</param>
        /// <param name="child_in"></param>
        /// <returns>parent_in + LEVEL_SEPARATOR_CHAR + child_in</returns>
        private static string GetChildID(string parent_in, string child_in)
        {
            if (parent_in == null || parent_in == string.Empty)
                return child_in;
            else
                return string.Format("{0}{1}{2}", parent_in, LEVEL_SEPARATOR_CHAR, child_in);
        }

        static  public KeyValuePair<string, T>[]
            FlattenDictionaryHierarchy<T>(this IDictionary dictionary_in, bool ignoreOtherLeafType_in)
        {
            return dictionary_in.FlattenDictionaryHierarchy<T>(string.Empty, ignoreOtherLeafType_in);
        }

        /// <summary>
        /// Recursive!
        /// Exceptions: ArgumentException if leaf Value cannot cast to T and ignoreOtherLeafType_in == false;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary_in"></param>
        /// <param name="prefix_in">flattened parent key</param>
        /// <param name="ignoreOtherLeafType_in">In case of leaf Value cannot cast to T: ignore or ArgumentException</param>
        /// <returns></returns>
        static public KeyValuePair<string, T>[] FlattenDictionaryHierarchy<T>
                (this IDictionary dictionary_in, string prefix_in, bool ignoreOtherLeafType_in)
        {
            List<KeyValuePair<string, T>> res = new List<KeyValuePair<string, T>>();

            foreach (System.Collections.DictionaryEntry pair in dictionary_in)
            {
                if (pair.Value is IDictionary)
                {
                    res.AddRange(FlattenDictionaryHierarchy<T>( //Recursion!
                        (IDictionary)pair.Value,
                            GetChildID(prefix_in, pair.Key.ToString()), ignoreOtherLeafType_in));
                }
                else if (pair.Value is IFlattenable<T>)
                {
                    string format = prefix_in == null || prefix_in == string.Empty ?
                        "{2}{1}" : "{0}{1}{2}{1}";

                    res.AddRange((pair.Value as IFlattenable<T>).Flatten(string.Format(format,
                        prefix_in, LEVEL_SEPARATOR_CHAR, pair.Key.ToString())));
                }
                else
                {
                    if (pair.Value is T)
                        res.Add(new KeyValuePair<string, T>(
                            GetChildID(prefix_in, pair.Key.ToString()), ((T)pair.Value)));
                    else if (!ignoreOtherLeafType_in)
                        throw new ArgumentException(
                            string.Format("Leaf item '{0}' (type: '{1}') cannot to be casted to '{2}'. (Prefix= {3}, Key= {4})",
                                    pair.Value, pair.Value.GetType(), typeof(T), prefix_in, pair.Key));
                }
            }

            return res.ToArray();
        }
    }
    

}
