using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using LibSassHost;
using LibSassHost.Helpers;
using Ionic.Zip;
using System.IO;

namespace Clipboardor
{
    #region String

    public static class StringHelper
    {
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        public static string RemoveHTMComments(string content)
        {
            return Regex.Replace(content, "<!--(?!\\[).*?(?!<\\])-->", "");
        }
        public static string EscapeNewLine(string content)
        {
            return Regex.Replace(content,"[\r\n]+", "\\n");
        }
        public static string EscapeSlash(string content)
        {
            return content.Replace("\\", "\\\\");
        }
        public static string RemoveUnNeedWhiteSpace(string content)
        {
            return Regex.Replace(content, "\\s{2,}", " ");
        }
        public static string EscapeDoubleQuotation(string content)
        {
            return content.Replace("\"", "\\\"");
        }
        public static string CompressHTM(string content)
        {
            var ls = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var maker = new StringBuilder();
            foreach (var item in ls)
            {
                if (item.IsNotNullOrWhiteSpace())
                {
                    maker.Append(EscapeDoubleQuotation(item.Trim()));
                }
            }
            return Regex.Replace(maker.ToString(), "\\s{2,}", " ");
        }
        public static string ToLowerCase(string content)
        {
            if (content == null) return null;

            return content.ToLower();
        }
        public static IEnumerable<char> GenerateAlphabetArray()
        {
            return Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i);
        }
        public static IEnumerable<char> GenerateAlphabetArray(char endWhere)
        {
            return Enumerable.Range('a', endWhere - 'a' + 1).Select(i => (Char)i);
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
    #endregion

    #region File

    public static class FileNameHelper
    {
        public static string WindowsToPosixPath(string content)
        {
            if (content == null) return null;

            return content.Replace("\\", "/");
        }
    }
    #endregion

    #region CSS
    public static class CSSHelper
    {
        public static string SCSSToCSS(string inputContent,string[] paths)
        {
            using (var compiler = new SassCompiler())
            {
                try
                {
                    var options = new CompilationOptions { SourceMap = false,OutputStyle=OutputStyle.Compressed,IncludePaths=paths};
                    CompilationResult result = compiler.Compile(inputContent, null,null,
                        options);
                    return result.CompiledContent;
                   
                }
                catch (SassСompilationException e)
                {
                    return e.Message;
                }
            }



        }
    }
    #endregion

    #region ZIP

    public static class ZipHelper
    {
        public static void CompressDirectoryEncrypted(string directory,string password="psycho240802+euphoria")
        {
            if (Directory.Exists(directory))
            {
                using (var zip=new ZipFile())
                {
                    zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    zip.Password = password;
                    zip.AddDirectory(directory);
                    zip.Save(directory +" ___ "+ StringHelper.GetDateTime()+".zip");
                    
                }
            }
        }
    }
    #endregion
}
