using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Reflection;

namespace RiskOfDeath_SetUrlProtocol
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "temp")));
                string s = sr.ReadLine();
                sr.Close();
                sr.Dispose();
                Registry.SetValue("HKEY_CLASSES_ROOT\\ror2mm", null, "URL:ror2mm protocol");
                Registry.SetValue("HKEY_CLASSES_ROOT\\ror2mm", "URL Protocol", "");
                Registry.SetValue("HKEY_CLASSES_ROOT\\ror2mm\\Shell\\Open\\Command", null, string.Format("\"{0}\" \"%1\"", s));
                File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "temp"));
            }
            catch (Exception e) { Console.WriteLine(e.Message + e.StackTrace); Console.ReadLine(); }
        }
    }
}
