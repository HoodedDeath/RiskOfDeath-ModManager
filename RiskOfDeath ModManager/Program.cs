using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiskOfDeath_ModManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Directory.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "downloads")))
                Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "downloads"));
            Console.WriteLine("Checking for Risk Of Rain 2 installation...");
            string s = FindROR2Path();
            if (s == null || s == "" || s == "#UNKNOWN#")
            {
                Console.WriteLine("Unable to find Risk Of Rain 2 folder. Please enter path to your Risk Of Rain 2 folder:");
                for (; ; )
                {
                    string t = Console.ReadLine();
                    if (Directory.Exists(t))
                    {
                        Console.WriteLine("Thank you.");
                        s = t;
                        break;
                    }
                    else
                        Console.WriteLine("Please enter the path to a valid folder.");
                }
            }
            else
                Console.WriteLine("Risk of Rain 2 installation found at path \"{0}\"", s);
            Console.WriteLine("Please wait while loading.");
            try
            {
                try
                {
                    //Application.EnableVisualStyles();
                    //Application.SetCompatibleTextRenderingDefault(false);
                    //Application.Run(new Form1(s, args.Contains("nolaunch"), args.Contains("hoodeddeath")));
                    Form1 form = new Form1(s, args.Contains("nolaunch"), args.Contains("hoodeddeath"));
                    form.ShowDialog();
                    form.Dispose();
                }
                catch (IndexOutOfRangeException)
                {
                    //Application.EnableVisualStyles();
                    //Application.SetCompatibleTextRenderingDefault(false);
                    //Application.Run(new Form1(s, false, false));
                    Form1 form = new Form1(s, false, false);
                    form.ShowDialog();
                    form.Dispose();
                }
            }
            catch (CloseEverythingException) { return; }
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Console.WriteLine("Goodbye");
        }
        static string FindROR2Path()
        {
            string ret = "#UNKNOWN#";

            object reg = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", "#NO_VAL#");
            if (reg != null && (string)reg != "#NO_VAL#" && (string)reg != "")
            {
                string path = (string)reg + "\\steamapps";
                string[] files = Directory.GetFiles(path, "*.acf");
                if (files.Length > 0)
                {
                    string find = "";
                    foreach (string s in files)
                    {
                        string[] t = s.Split('\\');
                        if (t[t.Length - 1] == "appmanifest_632360.acf")
                        {
                            find = s;
                            break;
                        }
                    }
                    if (find != null && find != "")
                    {
                        return path + "\\common\\Risk of Rain 2";
                    }
                    else
                        return FindROR2Path_LibFolder(path);
                }
                else
                    return FindROR2Path_LibFolder(path);
            }

            return ret;
        }
        static string FindROR2Path_LibFolder(string path)
        {
            string ret = "#UNKNOWN#";

            path = Path.Combine(path, "libraryfolders.vdf");
            if (File.Exists(path))
            {
                try
                {
                    List<string> folders = new List<string>();
                    StreamReader sr = new StreamReader(File.OpenRead(path));
                    //Padding to reach the first listed library folder
                    sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                    //Infinite loop to read to end of file
                    for (; ; )
                    {
                        string temp = sr.ReadLine();
                        //Breaks at end of file
                        if (temp == null || temp == "" || temp == "}") break;
                        string[] t = temp.Split('"');
                        //Saves the path of the library folder
                        folders.Add(t[t.Length - 2]);
                    }
                    sr.Close();
                    sr.Dispose();
                    foreach (string folder in folders)
                    {
                        string tpath = folder + "\\steamapps";
                        string[] files = Directory.GetFiles(tpath, "*.acf");
                        if (files.Length > 0)
                        {
                            string find = "";
                            foreach (string s in files)
                            {
                                string[] t = s.Split('\\');
                                if (t[t.Length - 1] == "appmanifest_632360.acf")
                                {
                                    find = s;
                                    break;
                                }
                            }
                            if (find != null && find != "")
                            {
                                return tpath + "\\common\\Risk of Rain 2";
                            }
                        }
                    }
                }
                catch (Exception) { }
            }

            return ret;
        }
    }
}
