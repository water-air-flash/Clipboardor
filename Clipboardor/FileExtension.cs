
namespace Clipboardor
{using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Text;


    public static class FileExtension
    {
        public static void MoveFileSystemEntries(this string fullName, string dst)
        {
            if (fullName != dst)
            {
                if (Path.GetPathRoot(fullName) == Path.GetPathRoot(dst))
                {

                    if (fullName.IsDirectory())
                    {

                        Directory.Move(fullName, dst);
                    }
                    else if (fullName.IsFile())
                    {

                        File.Move(fullName, dst);
                    }
                }
                else {


                    if (fullName.IsDirectory())
                    {

                       // Directory.(fullName, dst);
                    }
                    else if (fullName.IsFile())
                    {

                        File.Copy(fullName, dst);
                        File.Delete(fullName);
                    }
                }
            }
        }
        public static string GetParent(this string fullName)
        {

            return Path.GetDirectoryName(fullName);
        }

        public static void DeleteSafe(this string fullname)
        {

            try
            {
                if (fullname.IsDirectory())
                {

                    Directory.Delete(fullname, true);
                }
                else if (fullname.IsFile())
                {

                    File.Delete(fullname);
                }
            }
            catch (Exception e)
            {
                "debug.txt".GetCommandLinePath().AppendToFile(e.Message.NewLine());
            }
        }
        public static void ChangeParentDirectory(this string fullname, string dir)
        {

            File.Move(fullname, dir.Combine(fullname.GetFileName()));
        }
        public static void ChangeFullName(this string fullname, string where)
        {

            File.Move(fullname, where);
        }
        public static string AddShortDateTimeToFileName(this string fullName)
        {
            return Path.Combine(fullName.GetDirectoryName(), string.Format("{0}{1}{2}", fullName.GetFileNameWithoutExtension(), DateTime.Now.ToString("-yyyy-MM-dd"), fullName.GetExtension()));
        }
        public static string AddStringToFileName(this string fullName, string add)
        {
            return Path.Combine(fullName.GetDirectoryName(), string.Format("{0}{1}{2}", fullName.GetFileNameWithoutExtension(), add, fullName.GetExtension()));
        }
        public static void AppendToFile(this string fullName, string content, bool isUTF8 = true)
        {
            if (isUTF8)

                File.AppendAllText(fullName, "\r\n" + content, Encoding.UTF8);
            else
                File.AppendAllText(fullName, "\r\n" + content, Encoding.GetEncoding("gb2312"));

        }


        public static void ChangeFileName(this string fullName, string replace)
        {

            if (fullName.IsDirectory())
            {
                Directory.Move(fullName, fullName.GetDirectoryName().Combine(replace.GetValidFileName().Trim()));
            }
            else if (fullName.IsFile())
            {
                var to = fullName.GetDirectoryName().Combine(replace.GetValidFileName().Trim() + fullName.GetExtension());
                if (fullName != to)
                {
                    File.Move(fullName, to.GetUniqueFileName());
                }
            }
        }


        public static string Combine(this string fullName, string concat)
        {
            return Path.Combine(fullName, concat);
        }

        public static void CopyFilesByType(this string fullName, string pattern)
        {
            if (fullName.IsNotNullOrWhiteSpace() && pattern.IsNotNullOrWhiteSpace())
            {
                var dstdir = fullName + " - Copyed";
                dstdir.CreateDirectoryIfNoExist();

                var dir = Directory.GetDirectories(fullName, "*", SearchOption.AllDirectories).OrderBy(i => i.Length);
                var dstdires = dir.Select(i => i.Replace(fullName, dstdir));

                foreach (var item in dstdires)
                {
                    item.CreateDirectoryIfNoExist();
                }
                var files = fullName.GetFiles(pattern, true);
                foreach (var item in files)
                {
                    File.Copy(item, item.Replace(fullName, dstdir));
                }

            }
        }


        public static void CreateDirectoryIfNoExist(this string fullName)
        {
            if (!Directory.Exists(fullName))
            {
                Directory.CreateDirectory(fullName);


            }
        }


        public static void Delete(this string fullName)
        {
            if (fullName.IsFile())
            {
                File.Delete(fullName);
            }
            else
            {
                Directory.Delete(fullName, true);
                if (fullName.IsDirectory())
                {
                    Directory.Delete(fullName);
                }
            }
        }

        public static void DeleteEmptyDiretories(this string fullname)
        {
            var dir = Directory.GetDirectories(fullname);
            foreach (var item in dir)
            {
                try
                {
                    Directory.Delete(item);
                }
                catch
                {
                }
            }
        }


        public static string GetCommandLinePath(this string fileName)
        {
            return Path.Combine(Environment.GetCommandLineArgs()[0].GetDirectoryName(), fileName.GetValidFileName());
        }


        public static string GetDesktopPath(this string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName.GetValidFileName());
        }


        public static string GetDirectoryName(this string fileName)
        {
            return Path.GetDirectoryName(fileName);

        }


        public static string GetExtension(this string fileName)
        {
            return Path.GetExtension(fileName);

        }


        public static string GetFileName(this string fileName)
        {
            return Path.GetFileName(fileName);

        }


        public static string GetFileNameWithoutExtension(this string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);

        }


        public static IEnumerable<string> GetFiles(this string fullName, string fileTypePattern = null, bool isContainsSubDirectories = false)
        {
            if (!Directory.Exists(fullName))
            {
                return null;
            }
            if (isContainsSubDirectories)
            {
                if (fileTypePattern == null)
                {
                    return Directory.GetFiles(fullName, "*", SearchOption.AllDirectories);

                }
                else
                {
                    return Directory.GetFiles(fullName, "*", SearchOption.AllDirectories).Where(i => i.GetExtension().IsRegexMatch(fileTypePattern));

                }
            }
            else
            {
                if (fileTypePattern == null)
                {
                    return Directory.GetFiles(fullName);

                }
                else
                {
                    return Directory.GetFiles(fullName).Where(i => i.GetExtension().IsRegexMatch(fileTypePattern));
                }
            }

        }


        public static IEnumerable<string> GetFileSystemEntries(this string fullName)
        {
            return Directory.GetFileSystemEntries(fullName).OrderBy(i => i.IsFile());
        }




        public static string GetParentDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }


        public static string GetUniqueFileName(this string fullName)
        {
            int i = 1;
            while (File.Exists(fullName))
            {
                fullName = Path.Combine(fullName.GetDirectoryName(), string.Format("{0}-{1:000}{2}", System.Text.RegularExpressions.Regex.Replace(fullName.GetFileNameWithoutExtension(), "\\-[0-9]+$", ""), i, fullName.GetExtension()));
                i++;
            }
            return fullName;
        }



        public static string GetValidFileName(this string fileName, char replacer = '-')
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();
            var filename = System.Text.RegularExpressions.Regex.Replace(fileName.Replace(": ", "-").Replace("--", ""), "\\s{2,}", " ").Replace("&#39;", "'");
            foreach (var item in invalidCharacters)
            {
                filename = filename.Replace(item, replacer);
            }
            return filename.TrimEnd(replacer);
        }


        public static bool IsDirectory(this string fullName)
        {
            if (fullName.IsNotNullOrWhiteSpace())
            {
                return Directory.Exists(fullName);
            }
            else
            {
                return false;
            }
        }


        public static bool IsFile(this string fullName)
        {
            if (fullName.IsNotNullOrWhiteSpace())
            {
                return File.Exists(fullName);
            }
            else
            {
                return false;
            }
        }


      

        private static bool IsValidFile(string filePath, Type enumType)
        {
            string ext = filePath.GetExtension();

            if (!string.IsNullOrEmpty(ext))
            {
                return Enum.GetNames(enumType).Any(x => ext.Equals(x, StringComparison.InvariantCultureIgnoreCase));
            }

            return false;
        }


        public static IEnumerable<string> ReadFileToLines(this string fullName, bool isUseUTF8 = true)
        {
            if (isUseUTF8)
                return File.ReadAllLines(fullName, FileHelper.ENCODING_UTF8);
            else
                return File.ReadAllLines(fullName, FileHelper.ENCODING_GB2312);

        }

        public static string ReadFileToText(this string fullName, bool isUseUTF8 = true)
        {
            if (isUseUTF8)
                return File.ReadAllText(fullName, FileHelper.ENCODING_UTF8);
            else
                return File.ReadAllText(fullName, FileHelper.ENCODING_GB2312);

        }


        public static void Run(this string fullName)
        {
            if (fullName.IsDirectory() || fullName.IsFile())
                Process.Start(fullName);
        }


        public static void WriteContentToFile(this string fullName, string content, bool isUseUTF8 = true)
        {
            if (isUseUTF8)
                File.WriteAllText(fullName, content, FileHelper.ENCODING_UTF8);
            else
                File.WriteAllText(fullName, content, FileHelper.ENCODING_GB2312);


        }


    }
}
