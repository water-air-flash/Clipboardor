
namespace Clipboardor
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
 
    public static class StringExtension
    {

        public static IEnumerable<string> LinesStrict(this string content)
        {

            return content.Split('\n').Where(i => !string.IsNullOrWhiteSpace(i)).OrderBy(i => i).Distinct().Select(i => i.Trim());

        }

        public static string NewLine(this string content)
        {

            return content + "\r\n";
        }
        public static List<string> Substract(this IEnumerable<string> source, IEnumerable<string> to)
        {

            var sls = source.ToList();
            var tls = to.ToList();
            foreach (var item in tls)
            {
                if (sls.Contains(item))
                    sls.Remove(item);
            }
            return sls;
        }

        public static string Between(this string text, string first, string last, bool isFirstMatchForEnd = false, bool includeFirstAndLast = false)
        {
            int start = text.IndexOf(first);
            if (start < 0) return null;
            if (!includeFirstAndLast) start += first.Length;
            text = text.Substring(start);

            int end = isFirstMatchForEnd ? text.IndexOf(last) : text.LastIndexOf(last);
            if (end < 0) return null;
            if (includeFirstAndLast) end += last.Length;
            return text.Remove(end);
        }



        //public static string CombineURL(this string url, string url2)
        //{
        //    return URLHelpers.CombineURL(url, url2);
        //}


        public static bool Contains(this string str, string value, StringComparison comparisonType)
        {
            return str.IndexOf(value, comparisonType) >= 0;
        }



        public static IEnumerable<Tuple<string, string>> ForEachBetween(this string text, string front, string back)
        {
            int f = 0;
            int b = 0;
            while (text.Length > f
                   && 0 <= (f = text.IndexOf(front, f))
                   && 0 <= (b = text.IndexOf(back, f + front.Length)))
            {
                string result = text.Substring(f, (b + back.Length) - f);
                yield return new Tuple<string, string>(result, result.Substring(front.Length, (result.Length - back.Length) - front.Length));
                f += front.Length;
            }
        }



        public static int FromBase(this string text, int radix, string digits)
        {
            if (string.IsNullOrEmpty(digits))
            {
                throw new ArgumentNullException("digits", string.Format("Digits must contain character value representations"));
            }

            radix = Math.Abs(radix);
            if (radix > digits.Length || radix < 2)
            {
                throw new ArgumentOutOfRangeException("radix", radix, string.Format("Radix has to be > 2 and < {0}", digits.Length));
            }

            // Convert to Base 10
            int value = 0;
            if (!string.IsNullOrEmpty(text))
            {
                for (int i = text.Length - 1; i >= 0; --i)
                {
                    int temp = digits.IndexOf(text[i]) * (int)Math.Pow(radix, text.Length - (i + 1));
                    if (0 > temp)
                    {
                        throw new IndexOutOfRangeException("Text contains characters not found in digits.");
                    }
                    value += temp;
                }
            }
            return value;
        }



        public static byte[] HexToBytes(this string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }


        public static bool IsNotNullOrWhiteSpace(this string content)
        {
            return !string.IsNullOrWhiteSpace(content);
        }



        public static bool IsNumber(this string text)
        {
            foreach (char c in text)
            {
                if (!char.IsNumber(c)) return false;
            }

            return true;
        }


        public static bool IsRegexMatch(this string content, string pattern, bool isIgnoreCase = true)
        {
            if (isIgnoreCase)
                return Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase);

            else
                return Regex.IsMatch(content, pattern);

        }



        public static bool IsValidUrl(this string url)
        {
            return Uri.IsWellFormedUriString(url.Trim(), UriKind.Absolute);
        }
        public static bool IsWhiteSpaceOrEmpty(this string content)
        {


            return string.IsNullOrWhiteSpace(content);
        }



        public static string Left(this string str, int length)
        {
            if (length < 1) return string.Empty;
            if (length < str.Length) return str.Substring(0, length);
            return str;
        }



        public static string[] Lines(this string text)
        {
            return text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }



        public static string ParseQuoteString(this string str)
        {
            str = str.Trim();

            int firstQuote = str.IndexOf('"');

            if (firstQuote >= 0)
            {
                str = str.Substring(firstQuote + 1);

                int secondQuote = str.IndexOf('"');

                if (secondQuote >= 0)
                {
                    str = str.Remove(secondQuote);
                }
            }

            return str;
        }



        public static string RemoveLeft(this string str, int length)
        {
            if (length < 1) return string.Empty;
            if (length < str.Length) return str.Remove(0, length);
            return str;
        }



        public static string RemoveRight(this string str, int length)
        {
            if (length < 1) return string.Empty;
            if (length < str.Length) return str.Remove(str.Length - length);
            return str;
        }



        public static string RemoveWhiteSpaces(this string str)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in str)
            {
                if (!Char.IsWhiteSpace(c)) result.Append(c);
            }

            return result.ToString();
        }



        public static string Repeat(this string str, int count)
        {
            if (!string.IsNullOrEmpty(str) && count > 0)
            {
                StringBuilder sb = new StringBuilder(str.Length * count);

                for (int i = 0; i < count; i++)
                {
                    sb.Append(str);
                }

                return sb.ToString();
            }

            return null;
        }



        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                return str;
            }

            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }



        public static string ReplaceAll(this string text, string search, Func<string> replace)
        {
            while (true)
            {
                int location = text.IndexOf(search);

                if (location < 0) break;

                text = text.Remove(location, search.Length).Insert(location, replace());
            }

            return text;
        }



        public static bool ReplaceFirst(this string text, string search, string replace, out string result)
        {
            int location = text.IndexOf(search);

            if (location < 0)
            {
                result = text;
                return false;
            }

            result = text.Remove(location, search.Length).Insert(location, replace);
            return true;
        }



        public static string ReplaceWith(this string str, string search, string replace,
            int occurrence = 0, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            if (!string.IsNullOrEmpty(search))
            {
                int count = 0, location;

                while (occurrence == 0 || occurrence > count)
                {
                    location = str.IndexOf(search, comparison);
                    if (location < 0) break;
                    count++;
                    str = str.Remove(location, search.Length).Insert(location, replace);
                }
            }

            return str;
        }



        public static string Reverse(this string str)
        {
            char[] chars = str.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }



        public static string Right(this string str, int length)
        {
            if (length < 1) return string.Empty;
            if (length < str.Length) return str.Substring(str.Length - length);
            return str;
        }

        public static IEnumerable<string> SplitByRegex(this string content, string pattern)
        {
            return Regex.Split(content, pattern);
        }
 


        public static int ToInteger(this string content)
        {
            var match = Regex.Match(content, "[0-9]+").Value;
            if (match.IsNotNullOrWhiteSpace())
            {
                return int.Parse(match);
            }
            else
            {
                return -1;
            }
        }

        public static int ToIntegerFormRight(this string content)
        {
            var match = Regex.Match(content, "[0-9]+", RegexOptions.RightToLeft).Value;
            if (match.IsNotNullOrWhiteSpace())
            {
                return int.Parse(match);
            }
            else
            {
                return -1;
            }
        }

        public static IEnumerable<string> ToLinesWithRegex(this string content, string split = "\n", bool isNoContainsEmptyLines = true)
        {
            if (isNoContainsEmptyLines)
                return Regex.Split(content, split).Where(i => i.IsNotNullOrWhiteSpace()).Select(i => i.Trim());
            else
                return Regex.Split(content, split).Select(i => i.Trim());
        }


        public static string ToStringBy(this IEnumerable<string> ls, string concat = "\r\n")
        {
            var r = "";
            foreach (var item in ls)
            {
                r += item + concat;
            }
            return r;
        }



        public static string Truncate(this string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > maxLength)
            {
                return str.Substring(0, maxLength);
            }

            return str;
        }



        public static string Truncate(this string str, int maxLength, string endings, bool truncateFromRight = true)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > maxLength)
            {
                int length = maxLength - endings.Length;

                if (length > 0)
                {
                    if (truncateFromRight)
                    {
                        str = str.Left(length) + endings;
                    }
                    else
                    {
                        str = endings + str.Right(length);
                    }
                }
            }

            return str;
        }

    }
}