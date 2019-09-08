using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiskOfDeath_ModManager
{
    static class Program
    {
        static Mutex m;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool startdl = false;
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "uritemp")))
                File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "uritemp"));
            //Check already running
            try
            {
                m = new Mutex(true, Application.ProductName.ToString(), out bool first);
                if (first == false && args.Length > 0 && args[0] != null && args[0].StartsWith("ror2mm://"))
                {
                    Console.WriteLine("Application already running, setting up for download ...");
                    WriteProtocolResult(args[0]);
                    Console.WriteLine("Setup done, goodbye");
                    return;
                }
                else if (!first)
                {
                    Console.WriteLine("Instance already running. Press any key to exit.");
                    Console.ReadKey();
                    return;
                }
                else if (args != null && args.Length > 0)
                {
                    if (args[0].StartsWith("ror2mm://"))
                    {
                        startdl = true;
                        WriteProtocolResult(args[0]);
                    }
                    else
                        startdl = false;
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message + e.StackTrace); }

            Console.WriteLine("Checking for Risk Of Rain 2 installation...");
            string s = FindROR2Path();
            if (s == null || s == "" || s == "#UNKNOWN#")
            {
                Console.WriteLine("Unable to find RoR2 folder. Please enter path to your RoR2 folder:");
                for (; ; )
                {
                    string t = Console.ReadLine();
                    if (Directory.Exists(t))
                    {
                        if (Directory.GetFiles(t).Contains(Path.Combine(t, "Risk of Rain 2.exe")))
                        {
                            Console.WriteLine("Thank you.");
                            s = t;
                            break;
                        }
                        else
                            Console.WriteLine("This forlder doesn't contain RoR2.");
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
                    Form1 form = new Form1(s, args.Contains("nolaunch"), args.Contains("hoodeddeath"), startdl);
                    form.ShowDialog();
                    form.Dispose();
                }
                catch (IndexOutOfRangeException)
                {
                    Form1 form = new Form1(s, false, false, startdl);
                    form.ShowDialog();
                    form.Dispose();
                }
            }
            catch (CloseEverythingException) { return; }
            catch (Exception e)
            {
                Console.WriteLine("Caught exeption during execution:\n{0}{1}\n\nPress enter to exit...", e.Message, e.StackTrace);
                Console.ReadLine();
            }
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
        static void WriteProtocolResult(string uri)
        {
            string depStr = "";
            string[] split = uri.Split('/');
            for (int i = split.Length - 4; i < split.Length - 1; i++)
                depStr += split[i] + "-";
            depStr = depStr.Substring(0, depStr.Length - 1);
            Console.WriteLine(depStr);
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "uritemp");
            if (File.Exists(path))
                File.Delete(path);
            StreamWriter sw = new StreamWriter(File.Open(path, FileMode.OpenOrCreate));
            sw.WriteLine(depStr);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
