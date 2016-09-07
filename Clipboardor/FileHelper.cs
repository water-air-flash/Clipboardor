namespace Clipboardor
{using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;


    public static class FileHelper
    {
        public static Encoding ENCODING_UTF8 = Encoding.UTF8;
        public static Encoding ENCODING_GB2312 = Encoding.GetEncoding("gb2312");
        public const string REGULAR_EXPRESSIONS_IMAGE = "\\.(?:png|jpg|jpeg|tiff|gif|bmp)";
        public const string REGULAR_EXPRESSIONS_ACHIEVE = "\\.(?:7z|zip|rar)";
        public const string REGULAR_EXPRESSIONS_ADOBE = "\\.(?:psd|jpg|jpeg|tiff|fla|ai)";


        public static void MoveAllFilesToDirectory(this string fullname,string dst)
        {
            var fs = fullname.GetFiles(null, true);
            foreach (var item in fs)
            {
                try
                {
                    File.Move(item, dst.Combine(item.GetFileName()));
                }
                catch  
                {

                     
                }
            }
        }
        public static string GetApplicationData()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        }
        public static string GetLocalApplicationData()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        }
        public static string GetFolderPathOfFonts()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        }
        public static string GetFolderPathOfProgramFiles()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace(" (x86)","");
        }
        public static string GetFolderPathOfProgramFilesX86()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        }
     
        public static void ReorganizeFilesByByExtension(string fullName,bool isContainSubDir=false)
        {
            if (fullName.IsFile())
            {
                fullName.GetDirectoryName().Combine(fullName.GetExtension().TrimStart('.')).CreateDirectoryIfNoExist();
                File.Move(fullName, fullName.GetDirectoryName().Combine(fullName.GetExtension().TrimStart('.')).Combine(fullName.GetFileName()).GetUniqueFileName());
            }
            else if (fullName.IsDirectory())
            {
                var fs = fullName.GetFiles(null,isContainSubDir);
                var dir = fullName;

                foreach (var item in fs)
                {

                    
                    dir.Combine(item.GetExtension().TrimStart('.')).CreateDirectoryIfNoExist();
                    try
                    {
                        File.Move(item, dir.Combine(item.GetExtension().TrimStart('.')).Combine(item.GetFileName()).GetUniqueFileName());

                    }
                    catch  
                    {
                        
                      
                    }

                }
            }
        }
       
   
        //public static void ZipDirectoryWithPassword(string dir,string password)
        //{
        //    if (dir.IsDirectory())
        //    {
        //        SevenZipCompressor.SetLibraryPath("7z.dll".GetCommandLinePath());
        //        var sz = new SevenZipCompressor();


        //        sz.CompressDirectory(dir, dir + DateTime.Now.ToString("-yyyy-MM-dd") + ".7z", true, password);
        //    }
        //}
        //public static void ZipFilesWithPassword(IEnumerable<string> files, string password)
        //{
           
        //        SevenZipCompressor.SetLibraryPath("7z.dll".GetCommandLinePath());
        //    var dir = files.First().GetDirectoryName();
        //        var sz = new SevenZipCompressor();
        //    sz.CompressFilesEncrypted(dir + DateTime.Now.ToString("-yyyy-MM-dd") + ".7z", password, files.ToArray());
 
            
        //}
    }
}
