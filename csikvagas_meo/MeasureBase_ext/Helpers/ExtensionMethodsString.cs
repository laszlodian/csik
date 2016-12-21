using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Globalization;

namespace e77.MeasureBase
{
    public class StringParseException : MeasureBaseException
    {
        public StringParseException(string message) :
            base(message)
        { ;}
    }

    public static class ExtensionMethodsString
    {
        /// <summary>
        /// Cuts a string into 2 side, based on a separator. Whitespaces has been trimmed. Empty results are not accepted.
        /// 
        /// Exceptions: StringParseException
        /// </summary>
        /// <param name="input_in"></param>
        /// <param name="separator_in"></param>
        /// <param name="left_out"></param>
        /// <param name="right_out"></param>
        public static void Cut2Strings(this string input_in, char separator_in, out string left_out, out string right_out)
        {
            Cut2Strings(input_in, separator_in, out left_out, out right_out, true, true, true, true, false);
        }

        /// <summary>
        /// Cuts a string into 2 side, based on a separator. Whitespaces has been trimmed.
        /// 
        /// Exceptions: StringParseException
        /// 
        /// </summary>
        /// <param name="input_in"></param>
        /// <param name="separator_in"></param>
        /// <param name="left_out"></param>
        /// <param name="right_out"></param>
        /// <param name="emptyLeftAccepted"></param>
        /// <param name="emptyRightAccepted"></param>
        /// <param name="trimInput_in"></param>
        /// <param name="trimOutputs_in"></param>
        /// <param name="moreSeparatorAccepted_in">true 'A:B:C' -> 'A', 'B:C', false: StringParseException</param>
        public static void Cut2Strings(this string input_in, char separator_in, out string left_out, out string right_out,
                    bool emptyLeftAccepted, bool emptyRightAccepted, bool trimInput_in, bool trimOutputs_in, bool moreSeparatorAccepted_in)
        {
            left_out = string.Empty;
            right_out = string.Empty;

            if (trimInput_in)
                input_in = input_in.Trim();

            int sepPos = input_in.IndexOf(separator_in);
            if (sepPos == -1)
                throw new StringParseException(string.Format("'{0}' ban  nincs '{1}'",
                    input_in, separator_in));

            if (!moreSeparatorAccepted_in
                && input_in.LastIndexOf(separator_in) != sepPos)
            {
                throw new StringParseException(string.Format("'{0}' ban  több '{1}' van.",
                    input_in, separator_in));
            }
            
            if (sepPos > 0)
                left_out = input_in.Substring(0, sepPos);

            left_out = left_out.Substring(0, sepPos);
            right_out = input_in.Substring(sepPos + 1, input_in.Length - sepPos - 1);

            if (trimOutputs_in)
            {
                if (left_out != string.Empty)
                    left_out = left_out.Trim();

                if (right_out != string.Empty)
                    right_out = right_out.Trim();
            }

            if (!emptyLeftAccepted && left_out == string.Empty)
            {
                throw new StringParseException(string.Format("'{0}'-ban a elválasztó ('{1}') elött nincs adat.",
                    input_in, separator_in));
            }

            if (!emptyRightAccepted && right_out == string.Empty)
            {
                throw new StringParseException(string.Format("'{0}'-ban a elválasztó ('{1}') után nincs adat.",
                    input_in, separator_in));
            }
        }

        /// <summary>
        /// Get trimed right substring after a fixed left part (without whitespaces at the end). Example:
        /// string str = GetValue("  S/N:ua3123 ", "S/N:", 4); // => "ua3123", exception if result smaller than 4 characters
        /// 
        /// Exceptions: StringParseException
        /// </summary>
        /// <param name="str_in">string to parse</param>
        /// <param name="leftPart_in">Left side. If not match exception will thrown.</param>
        /// <param name="minimal_lenght">Exception thrown if right side does not reach this size. Use -1 for turn off this check.</param>
        /// <returns></returns>
        public static string GetValue(this string str_in, string leftPart_in, int minimal_lenght)
        {
            str_in = str_in.Trim();
            if (!str_in.StartsWith(leftPart_in))
                throw new StringParseException(
                    string.Format("'{0}' nem '{1}'-al kezdődik.",
                    str_in, leftPart_in));

            if (str_in.Length <= leftPart_in.Length ||
                (minimal_lenght != -1 && str_in.Length - leftPart_in.Length < minimal_lenght))
                throw new StringParseException(
                    string.Format("a '{0}' nem elég hosszú. Kulcs: '{1}', Minimal érték hossz: '{2}'",
                    str_in, leftPart_in, minimal_lenght));

            if (str_in.Length < leftPart_in.Length)
                return string.Empty;

            return str_in.Substring(leftPart_in.Length, str_in.Length - leftPart_in.Length).Trim();
        }

        /// <summary>
        /// "HelloWorld" -> "hello_world" 
        /// </summary>
        /// <param name="obj_in"></param>
        /// <returns></returns>
        public static string FromCamelCase(this string obj_in)
        {
            if(obj_in == string.Empty)
                return string.Empty;

            StringBuilder res = new StringBuilder(obj_in.Length);
            //first char:
            res.Append(char.ToLowerInvariant(obj_in[0]));

            for (int i = 1; i < obj_in.Length; i++)
            {
                if ('A' <= obj_in[i] && obj_in[i] <= 'Z')
                {
                    res.Append('_');
                }

                res.Append(char.ToLowerInvariant(obj_in[i]));
            }
            return res.ToString();
        }


        /// <summary>
        ///  "hello_world" -> "HelloWorld"
        /// </summary>
        /// <param name="obj_in"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string obj_in)
        {
            if(obj_in == string.Empty)
                return string.Empty;

            StringBuilder res = new StringBuilder(obj_in.Length);
            //first char:
            if( 'a' <= obj_in[0] && obj_in[0] <= 'z')
                res.Append((char)( obj_in[0] - 'a' + 'A' ));//to Hi char
            else
                res.Append( obj_in[0] );

            for(int i = 1; i < obj_in.Length; i++)
                if (obj_in[i] == '_'  
                    && ('a' <= obj_in[i+1] && obj_in[i+1] <= 'z'))
                {
                    res.Append((char)(obj_in[++i] - 'a' + 'A'));//to Hi char
                }
                else
                    res.Append(obj_in[i]);

            return res.ToString();
        }

        public static string[] ToLower(this string[] list_in)
        {
            List<string> res = new List<string>(list_in.Length);
            foreach (string s in list_in)
                res.Add(s.ToLowerInvariant());
            return res.ToArray();
        }

        /// <summary>
        /// Transforms the first character of the string to uppercase character
        /// </summary>
        /// <param name="s">String that has a arbitrary first character</param>
        /// <returns>String that has an uppercase first character</returns>
        /// <example>"helloworld" -> "Helloworld"</example>
        public static String ToFirstCharUpper(this String s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(s.Substring(0, 1).ToUpper());
            sb.Append(s.Substring(1));
            return sb.ToString();
        }

        /// <summary>
        /// Creates coma separated list of items.
        /// Example:
        ///  int[] array = new int[] { 3, 4, 5 };
        /// string res = array.ItemsToString(", ");
        /// res = "3, 4, 5"
        /// </summary>
        /// <param name="input">any enumerable collection</param>
        /// <param name="separator">separator betveen items</param>
        /// <returns></returns>
        public static string ItemsToString(this System.Collections.IEnumerable input)
        {
            return ItemsToString(input, ", ", string.Empty);
        }

        /// <summary>
        /// Creates separated list of items.
        /// Example:
        ///  int[] array = new int[] { 3, 4, 5 };
        /// string res = array.ItemsToString(", ");
        /// res = "3, 4, 5"
        /// </summary>
        /// <param name="input">any enumerable collection</param>
        /// <param name="separator">separator betveen items</param>
        /// <returns></returns>
        public static string ItemsToString(this System.Collections.IEnumerable input, string separator)
        {
            return ItemsToString(input, separator, string.Empty);
        }

        /// <summary>
        /// Creates separated list of items.
        /// Example:
        ///  int[] array = new int[] { 3, 4, 5 };
        /// string res = array.ItemsToString(", ", "+");
        /// res = "+3, +4, +5"
        /// </summary>
        /// <param name="input">any enumerable collection</param>
        /// <param name="separator">separator betveen items</param>
        /// <param name="prefix">prefix for items</param>
        /// <returns></returns>
        public static string ItemsToString(this System.Collections.IEnumerable input, string separator, string prefix)
        {
            StringBuilder strResult = new StringBuilder();

            if(input != null)
                foreach (object o in input)
                {
                    if (strResult.Length > 0)
                        strResult.Append(separator);

                    strResult.Append(prefix);
                    strResult.Append(o.ToString());
                }

            return strResult.ToString();
        }

        /// <summary>
        /// Adds postfix_in after the name of the file before the extension
        /// </summary>
        /// <param name="inputFile_in"></param>
        /// <param name="postfix_in"></param>
        /// <returns>{inputFile_in.Name}{postfix_in}.{inputFile_in.Extension}</returns>
        public static string AddFileNamePostfix(this string inputFile_in, string postfix_in)
        {
            int pointPos = inputFile_in.LastIndexOf('.');
            int backSlashPos = inputFile_in.LastIndexOf('\\');
            if(pointPos != -1 && pointPos > backSlashPos)
            {   //add postfix before extension
                return string.Format("{0}{1}{2}",
                    inputFile_in.Substring(0, pointPos),
                    postfix_in, inputFile_in.Substring(pointPos, inputFile_in.Length - pointPos));
            }
            else
                return string.Format("{0}{1}", inputFile_in, postfix_in);//no extension: add to end
        }

        /// <summary>
        /// "Alma 234".TryGetParamInt("Alma", out value); //-> value == 234
        /// </summary>
        /// <param name="this_in"></param>
        /// <param name="param_prefix_in"></param>
        /// <param name="result_out"></param>
        /// <returns></returns>
        public static bool TryGetParamInt(this string this_in, string param_prefix_in, out int result_out)
        {
            return this_in.TryGetParamInt(param_prefix_in, out result_out, StringComparison.Ordinal);
        }

        public static bool TryGetParamInt(this string this_in, string param_prefix_in, out int result_out, StringComparison compare_in)
        {
            return this_in.TryGetParamInt(param_prefix_in, out result_out, compare_in, NumberStyles.None);
        }

        public static bool TryGetParamInt(this string this_in, string param_prefix_in, out int result_out,
            StringComparison compare_in, NumberStyles numberStyles_in)
        {
            result_out = 0;
            string tmp;
            return (TryGetParam(this_in, param_prefix_in, out tmp, compare_in)
                && int.TryParse(tmp, numberStyles_in, CultureInfo.InvariantCulture, out result_out));
        }

        public static bool TryGetParam(this string this_in, string param_prefix_in, out string result_out, StringComparison compare_in)
        {
            result_out = string.Empty;

            if (this_in.StartsWith(param_prefix_in))
            {
                result_out = this_in.Substring(param_prefix_in.Length,
                        this_in.Length - param_prefix_in.Length).Trim();
                return true;
            }

            return false;
        }

        /// <summary>
        /// e.g.: 
        /// "Table: somethig string ".TryGetParam("Table:", out str); // -> str = "somethig string"; 
        /// </summary>
        /// <param name="this_in"></param>
        /// <param name="param_prefix_in"></param>
        /// <param name="result_out">trimmed string if this string begins with param_prefix_in. 
        /// Else null (string.Empty can be valid too)</param>
        /// <returns>true if param_prefix_in match, and result_out has been filled.</returns>
        public static bool TryGetParam(this string this_in, string param_prefix_in, out string result_out)
        {
            return this_in.TryGetParam(param_prefix_in, out result_out, StringComparison.Ordinal);
        }

        /// <summary>
        /// Key must be exist, or NotFoundException will be send
        /// </summary>
        /// <param name="items_in"></param>
        /// <param name="key_in"></param>
        /// <returns></returns>
        public static string GetItemValue(this string[] items_in, string key_in)
        {
            string res;
            string pair = items_in.FirstOrDefault(item => item.StartsWith(key_in));
            if (pair != null)
            {
                if (pair.TryGetParam(key_in, out res))
                    return res;
                else
                    throw new MeasureInternalException("pair: {0}; key: {1}", pair.ToString(), key_in);
            }
            else
                throw new NotFoundException("'{0}' not found at '{1}'",
                    key_in, items_in.ItemsToString());
        }

        public static int GetNumberOf(this string str_in, char ch_in)
        {
            int res = 0;
            foreach (char ch in str_in)
                if (ch == ch_in)
                    res++;
            return res;
        }

        /// <summary>
        /// "1alma" => _1alma
        /// "alma" => alma
        /// </summary>
        /// <param name="str_in"></param>
        /// <returns></returns>
        public static string SqlColumnName_DigitPrefix(this string str_in)
        {
            if (char.IsDigit(str_in[0]))
                return '_' + str_in;
            else
                return str_in;
        }

        #region Wildcard ('?', '*') support 
        
        private static Regex metaRegex = new Regex("[\\+\\{\\\\\\[\\|\\(\\)\\.\\^\\$]");
        private static Regex questRegex = new Regex("\\?");
        private static Regex starRegex = new Regex("\\*");
        private static Regex commaRegex = new Regex(",");
        //protected static Regex slashRegex = new Regex("(?=/)");
        //protected static Regex backslashRegex = new Regex("(?=[\\\\:])");

        /// <summary>
        /// NOT WORKING CORRECTLY 
        /// </summary>
        /// <param name="pattern_in"></param>
        /// <param name="caseInsensitive_in"></param>
        /// <returns></returns>
        public static Regex RegexFromWildcard(string pattern_in, bool caseInsensitive_in)
        {
            RegexOptions options = RegexOptions.None;

            // match right-to-left (for speed) if the pattern starts with a *

            if (pattern_in.Length > 0 && pattern_in[0] == '*')
                options = RegexOptions.RightToLeft | RegexOptions.Singleline;
            else
                options = RegexOptions.Singleline;

            // case insensitivity

            if (caseInsensitive_in)
                options |= RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

            // Remove regex metacharacters

            pattern_in = metaRegex.Replace(pattern_in, "\\$0");

            // Replace wildcard metacharacters with regex codes

            pattern_in = questRegex.Replace(pattern_in, ".");
            pattern_in = starRegex.Replace(pattern_in, ".*");
            pattern_in = commaRegex.Replace(pattern_in, "\\z|\\A");

            // anchor the pattern at beginning and end, and return the regex

            return new Regex("\\A" + pattern_in + "\\z", options);
        }
     
        /// <summary>
        /// NOT WORKING CORRECTLY 
        /// Note: Calling this function multiple times is slower than string.IsMatchWildCard(Regex), because it creates Regex object all times
        /// </summary>
        /// <param name="this_in"></param>
        /// <param name="pattern_in"></param>
        /// <param name="caseInsensitive_in"></param>
        /// <returns></returns>
        public static bool IsMatchWildCard(this string this_in, string pattern_in, bool caseInsensitive_in)
        {
            Regex regex = RegexFromWildcard(pattern_in, caseInsensitive_in);
            return regex.IsMatch(this_in);
        }

        /// <summary>
        /// NOT WORKING CORRECTLY 
        /// Example:
        /// Regex regex = ExtensionMethodsString.RegexFromWildcard("*any?12", false);
        /// foreach (string str in stringCollection)
        ///    if (str.IsMatchWildCard( regex ))
        /// </summary>
        /// <param name="this_in"></param>
        /// <param name="regex_in">created by RegexFromWildcard() function</param>
        /// <returns></returns>
        public static bool IsMatchWildCard(this string this_in, Regex regex_in)
        {
            return regex_in.IsMatch(this_in);
        }

        #endregion
    } //ExtensionMethods

}
