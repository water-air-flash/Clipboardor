using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using System.IO;
using System.IO.Compression;
using CryptSharp;
using System.Net;

namespace Clipboardor
{
    public partial class ConvertColor : Form
    {

        public static void RGB2CMYK(int red, int green, int blue, out double c, out double m, out double y, out double k)
        {
            c = (double)(255 - red) / 255;
            m = (double)(255 - green) / 255;
            y = (double)(255 - blue) / 255;

            k = (double)Math.Min(c, Math.Min(m, y));
            if (k == 1.0)
            {
                c = m = y = 0;
            }
            else
            {
                c = (c - k) / (1 - k);
                m = (m - k) / (1 - k);
                y = (y - k) / (1 - k);
            }
        }
        public static string CMYK2RGB(double c, double m, double y, double k)
        {
            var r = Convert.ToInt32((1.0 - c) * (1.0 - k) * 255.0);
            var g = Convert.ToInt32((1.0 - m) * (1.0 - k) * 255.0);
            var b = Convert.ToInt32((1.0 - y) * (1.0 - k) * 255.0);

            return String.Format("#{0:x2}{1:x2}{2:x2}", r, g, b);
        }
        public static string CMYK2RGB(double[] cmy)
        {
            var c = cmy[0];
            var m = cmy[1];
            var y = cmy[2];
            var k = cmy[3];

            var r = Convert.ToInt32((1.0 - c) * (1.0 - k) * 255.0);
            var g = Convert.ToInt32((1.0 - m) * (1.0 - k) * 255.0);
            var b = Convert.ToInt32((1.0 - y) * (1.0 - k) * 255.0);

            return String.Format("#{0:x2}{1:x2}{2:x2}", r, g, b);
        }
        public ConvertColor()
        {
            InitializeComponent();

        }

        #region
        private void button21_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();
        }



        private void button1_Click(object sender, EventArgs e)
        {




        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        #endregion







        public static string GetHTMLByHttpRequest(string url, string proxy, int timeout = 5000)
        {
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(url);
            if (string.IsNullOrWhiteSpace(proxy))
            {
                wr.Proxy = null;
            }
            else
            {
                wr.Proxy = new WebProxy(proxy);
            }
            wr.Timeout = timeout;
            HttpWebResponse wrp = (HttpWebResponse)wr.GetResponse();
            var charset = wrp.CharacterSet;
            string c = string.Empty;
            Stream st = wrp.GetResponseStream();
            if (wrp.ContentEncoding.ToLower().Contains("gzip"))
            {
                st = new System.IO.Compression.GZipStream(st, System.IO.Compression.CompressionMode.Decompress);
            }
            using (var sr = new StreamReader(st
                , Encoding.GetEncoding(charset)))
            {
                c = sr.ReadToEnd();
            }
            return c;
        }





        private void directory_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]));

        }



        private void handle_ToLowerCase(object sender, EventArgs e)
        {

        }

        private void handle_WindowsPathToPosixPath(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            textBox1.Text = FileNameHelper.WindowsToPosixPath(textBox1.Text.Trim());
            textBox1.SelectAll();
            textBox1.Copy();
        }


        private void 单行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = Regex.Replace(textBox1.Text, "[\r\n]+", "");
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText += "{0}";
        }

        private void 移除空行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            var ls = textBox1.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var maker = new StringBuilder();
            foreach (var item in ls)
            {
                if (item.IsNotNullOrWhiteSpace())
                {
                    maker.Append(item).Append('\n');
                }
            }
            textBox1.Text = maker.ToString();
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void 单行ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Regex.Replace(textBox1.Text, "[\r\n]+", "");
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void 文件夹单行ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var dlg = new OpenFileDialog();
            dlg.Filter = "*.*|*.*";
            var lse = new string[] { ".rar" };
            var unbom = new UTF8Encoding(false);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var dir = Path.GetDirectoryName(dlg.FileName);

                var lsf = Directory.GetFiles(dir).Where(i => !lse.Contains(Path.GetExtension(i).ToLower()));
                foreach (var item in lsf)
                {
                    var ls = item.ReadFileToText().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var maker = new StringBuilder();
                    foreach (var i in ls)
                    {
                        if (i.IsNotNullOrWhiteSpace())
                        {
                            maker.Append(i).Append('\n');
                        }
                    }

                    File.WriteAllText(item, maker.ToString().Trim(new char[] { '\uFEFF', '\u200B' }), unbom);
                }
            }




        }

        private void base64GIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.IsNotNullOrWhiteSpace()) return;
            // var fileName = "001.gif".GetDesktopPath();

            byte[] fileBytes = Convert.FromBase64String(textBox1.Text);

            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                using (var fs = new FileStream("001.gif".GetDesktopPath(), FileMode.OpenOrCreate))
                {
                    Image streamImage = Image.FromStream(ms);
                    streamImage.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
                }

            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            textBox1.Text = textBox1.Text.Replace("\"", "\\\"");
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void 移除多余空格HTMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            var ls = textBox1.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var maker = new StringBuilder();
            foreach (var item in ls)
            {
                if (item.IsNotNullOrWhiteSpace())
                {
                    maker.Append(item.Trim());
                }
            }
            textBox1.Text = Regex.Replace(maker.ToString(), "\\s{2,}", " ");
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void hMTL模板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            var ls = Regex.Split(StringHelper.RemoveHTMComments(textBox1.Text), "@@@");

            var maker = new StringBuilder();
            var ca = StringHelper.GenerateAlphabetArray().ToArray();

            for (int i = 0; i < ls.Length; i++)
            {

                maker.Append('$').Append(ca[i]).Append('=').Append('"').Append(StringHelper.CompressHTM(ls[i])).Append('"').Append(';').Append('\n');
            }

            textBox1.Text = maker.ToString();
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void hMTL模板数组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            var ls = Regex.Split(StringHelper.RemoveHTMComments(textBox1.Text), "@@@");

            var maker = new StringBuilder();
            var ca = StringHelper.GenerateAlphabetArray().ToArray();
            maker.Append('[');

            for (int i = 0; i < ls.Length; i++)
            {

                maker.Append('"').Append(StringHelper.CompressHTM(ls[i])).Append('"').Append(',').Append('\n');
            }
            maker.Append(']');
            textBox1.Text = maker.ToString();
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            var ls = Regex.Split(StringHelper.RemoveHTMComments(textBox1.Text), "@@@");

            var maker = new StringBuilder();
            var ca = StringHelper.GenerateAlphabetArray().ToArray();
            maker.Append('{');

            for (int i = 0; i < ls.Length; i++)
            {

                maker.Append('"').Append(ca[i]).Append('"').Append(':').Append('"').Append(StringHelper.CompressHTM(ls[i])).Append('"').Append(',').Append('\n');
            }
            maker.Append('}');

            textBox1.Text = maker.ToString();
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void ConvertColor_Load(object sender, EventArgs e)
        {
            cssBox.Text = Properties.Settings.Default.LastedSCSS;

            comboBox2.Items.AddRange(StringHelper.GenerateAlphabetArray().Select(i => i.ToString()).ToArray());
        }

        #region Pattern
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            textBox3.SelectedText = "$$$";

        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.IsNotNullOrWhiteSpace() && comboBox2.Text.IsNotNullOrWhiteSpace())
            {
                var ls = StringHelper.GenerateAlphabetArray(comboBox2.Text.ToCharArray().First()).ToArray();


                var maker = new StringBuilder();

                for (int i = 0; i < ls.Length; i++)
                {
                    maker.Append(textBox3.Text.Replace("$$$", ls[i].ToString()));
                }

                textBox3.Text = maker.ToString();
                textBox3.SelectAll();
                textBox3.Copy();
            }
        }
        #endregion

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();

            var ls = Regex.Split(StringHelper.RemoveHTMComments(textBox1.Text), "@@@");

            var maker = new StringBuilder();
            var ca = StringHelper.GenerateAlphabetArray().ToArray();
            maker.Append('(');

            for (int i = 0; i < ls.Length; i++)
            {

                maker.Append('"').Append(StringHelper.CompressHTM(ls[i])).Append('"').Append(',').Append('\n')
                   .Append(ca[i]).Append(',').Append('\n');
            }
            maker.Append(')');
            textBox1.Text = maker.ToString();
            textBox1.SelectAll();
            textBox1.Copy();
        }

     

        #region SCSS

        private void scssButton_Click(object sender, EventArgs e)
        {
            var inputFile = cssBox.Text;

            if (!inputFile.IsFile())
            {
                var dlg = new OpenFileDialog();
                dlg.Filter = "*.*|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    inputFile = dlg.FileName;
                    cssBox.Text = inputFile;

                    Properties.Settings.Default.LastedSCSS = inputFile;
                    Properties.Settings.Default.Save();
                }

            }
            var content = inputFile.ReadFileToText();
            var outFile = Path.ChangeExtension(cssBox.Text, ".css");

            outFile.WriteContentToFile(CSSHelper.SCSSToCSS(content, new string[] { inputFile.GetDirectoryName() }));
        }


        #endregion

        private void translationButton_ButtonClick(object sender, EventArgs e)
        {
            if (!textBox1.Text.IsNotNullOrWhiteSpace())
            {
                textBox1.SelectAll();
                textBox1.Paste();
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text)) return;
            var query = textBox1.Text.Split('\n').First().Trim().ToLower();


            var c = GetHTMLByHttpRequest("http://fanyi.youdao.com/openapi.do?keyfrom=asdsadasd&key=1271366606&type=data&doctype=json&version=1.1&q=" + query, null);

            Newtonsoft.Json.Linq.JObject rss = Newtonsoft.Json.Linq.JObject.Parse(c);
            string cs = query + "\r\n";
            try
            {
                cs += "/" + rss["basic"].Value<string>("phonetic") + "/" + "\r\n";
            }
            catch { }
            try
            {
                var postTitles =
   from p in rss["translation"]
   select (string)p;
                cs += postTitles.First() + "\r\n";
            }
            catch { }

            try
            {
                var basic =
 from p in rss["basic"]["explains"]
 select (string)p;
                cs += basic.ToStringBy();
            }
            catch { }

            try
            {
                var sss = rss["web"];
                foreach (var item in sss)
                {
                    try
                    {
                        Newtonsoft.Json.Linq.JArray iii = (Newtonsoft.Json.Linq.JArray)item.ToList().First().First();
                        cs += item.Value<string>("key") + "\r\n";

                        foreach (Newtonsoft.Json.Linq.JValue i1 in iii)
                        {
                            cs += i1.Value + " ";
                        }
                        cs += "\r\n";
                    }
                    catch { }
                }
            }
            catch { }

            textBox1.Text = cs;

        }

        #region MyRegion
        private void capitalButton_ButtonClick(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();
            textBox1.Text = textBox1.Text.Trim().ToUpper();
            textBox1.SelectAll();
            textBox1.Copy();
        }
        private void capitastartButton_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();



            textBox1.Text = textBox1.Text.Split(' ').Select((i) =>
            {
                if (i.Length > 1)
                {
                    i = i.Substring(0, 1).ToUpper() + i.Substring(1).ToLower();
                }
                else
                {

                    i = i.ToUpper();
                }
                return i;
            }).ToStringBy(" ");
            textBox1.SelectAll();
            textBox1.Copy();
        }
        private void 小写ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Paste();


            textBox1.Text = StringHelper.ToLowerCase(textBox1.Text.Trim());
            textBox1.SelectAll();
            textBox1.Copy();
        }

        #endregion
        #region App
        private void 目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]));
        }
        private void appButtton_ButtonClick(object sender, EventArgs e)
        {
            if (Clipboard.GetText().IsFile())
                System.Diagnostics.Process.Start("chrome", Clipboard.GetText());
        }
        #endregion
        #region Color
        private void hEXRGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var s = Regex.Match(textBox1.Text, "[0-9]+");
            var c = "";
            if (s.Success)
            {

                c = string.Format("{0}", int.Parse(s.Value).ToString("X2") + int.Parse(s.Value).ToString("X2") + int.Parse(s.Value).ToString("X2"));
                textBox1.Text = c;
                textBox1.SelectAll();
                textBox1.Copy();

            }

        }

        #endregion

        #region Compress

        private void javaScriptCompressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var content = Clipboard.GetText();
            var min = new Microsoft.Ajax.Utilities.Minifier();

            content = min.MinifyJavaScript(content);

            Clipboard.SetText(content);

        }

        private void cSSCompressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var content = Clipboard.GetText();
            var min = new Microsoft.Ajax.Utilities.Minifier();

            content = min.MinifyStyleSheet(content);

            Clipboard.SetText(content);
        }


        #endregion

        #region 代码
        private void 代码段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var content = Clipboard.GetText();
            var maker = new StringBuilder();


            content = StringHelper.EscapeDoubleQuotation(content);

            Clipboard.SetText(content);
        }

        private void codeButton_Click(object sender, EventArgs e)
        {
            var content = Clipboard.GetText();
            content = StringHelper.EscapeSlash(content);

            content = StringHelper.EscapeNewLine(content);

            content = StringHelper.RemoveUnNeedWhiteSpace(content);

            content = StringHelper.EscapeDoubleQuotation(content);

            Clipboard.SetText(content);
        }
        #endregion

        private void handleTrim(object sender, EventArgs e)
        {
            var content = Clipboard.GetText();
            Clipboard.SetText(content.Trim());

        }
        #region ZIP
        private void 解压ZIPToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var dlg = new OpenFileDialog();
            dlg.Filter = "*.zip|*.zip";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in dlg.FileNames)
                {
                   ZipFile.ExtractToDirectory(item,item.Substring(0,item.Length-4));
                   
                }
            }
        }
        #endregion

        private void 压缩目录加密ToolStripMenuItem_Click(object sender, EventArgs e)
        {
                
            var dlg = new OpenFileDialog();
            dlg.Filter = "*.*|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ZipHelper.CompressDirectoryEncrypted(Path.GetDirectoryName(dlg.FileName));
            }
        }

        private void snippetsButton_Click(object sender, EventArgs e)
        {
            var dir = @"C:\Users\Administrator\Documents\Visual Studio 2012\Code Snippets\Visual C#\My Code Snippets";
            var pattern = ("".GetCommandLinePath()+"\\pattern\\csharp.txt").ReadFileToText();

            var content = Clipboard.GetText().Trim();

            var arg = content.Split('\n').First().Trim().Substring(2).Trim().ToLower();

            content = string.Format(pattern, arg, content);

            File.WriteAllText(dir.Combine(arg.GetValidFileName()+".snippet"), content, Encoding.UTF8);

            
          

        }
     
    







    }
}

