namespace Clipboardor
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
 
  public static class ConfigureExtension
  {
      public static void LoadConfigurationListBox(this ListBox listbox, string fileName)
      {
          if (!File.Exists(fileName)) return;
          var lines = File.ReadAllLines(fileName, Encoding.UTF8).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.Trim()).OrderBy(i => i).Distinct();
          listbox.Items.Clear();
          listbox.Items.AddRange(lines.ToArray());
      }
      public static void WriteConfigurationListBox(this ListBox listbox, string fileName, string content)
      {

          var ls = new List<string>();
          foreach (var item in listbox.Items)
          {
              ls.Add(item.ToString());
          }
          ls.Add(content.Trim());
          ls = ls.Distinct().OrderBy(i => i).ToList();

          var c = "";
          foreach (var item in ls)
          {
              c += item + "\r\n";
          }
          File.WriteAllText(fileName, c, Encoding.UTF8);
      }
      public static void LoadConfigureComboBox(this ComboBox combobox, string fileName, bool isOrdered = true)
        {
            if (fileName.GetCommandLinePath().IsFile())
            {
                combobox.Items.Clear();
                if (isOrdered)
                    combobox.Items.AddRange(fileName.GetCommandLinePath().ReadFileToLines().Where(i => i.IsNotNullOrWhiteSpace()).OrderBy(i => i).ToArray());
                else
                    combobox.Items.AddRange(fileName.GetCommandLinePath().ReadFileToLines().Where(i => i.IsNotNullOrWhiteSpace()).ToArray());

            }
        }
      public static void WriteConfigureComboBox(this ComboBox combobox, string fileName)
        {
            if (string.IsNullOrWhiteSpace(combobox.Text))
                return;
            if (fileName.GetCommandLinePath().IsFile())
            {
                var ls = fileName.GetCommandLinePath().ReadFileToLines().ToList();
                ls.Add(combobox.Text);
                fileName.GetCommandLinePath().WriteContentToFile(ls.Distinct().ToStringBy());

            }
            else
            {
                fileName.GetCommandLinePath().WriteContentToFile(combobox.Text);

            }
        }
      public static void LoadConfigure(this ComboBox combobox, string fileName, bool isOrdered = true)
        {
            if (fileName.GetCommandLinePath().IsFile())
            {
                combobox.Items.Clear();
                if(isOrdered)
                combobox.Items.AddRange(fileName.GetCommandLinePath().ReadFileToLines().Where(i => i.IsNotNullOrWhiteSpace()).OrderBy(i=>i).ToArray());
                else
                    combobox.Items.AddRange(fileName.GetCommandLinePath().ReadFileToLines().Where(i => i.IsNotNullOrWhiteSpace()).ToArray());

            }
        }
      public static void WriteConfigure(this ComboBox combobox, string fileName)
        {
            if (string.IsNullOrWhiteSpace(combobox.Text))
                return;
            if (fileName.GetCommandLinePath().IsFile())
            {
                var ls = fileName.GetCommandLinePath().ReadFileToLines().ToList();
                ls.Add(combobox.Text);
                fileName.GetCommandLinePath().WriteContentToFile(ls.Distinct().ToStringBy());

            }
            else
            {
                fileName.GetCommandLinePath().WriteContentToFile(combobox.Text);

            }
        }
    }
}
