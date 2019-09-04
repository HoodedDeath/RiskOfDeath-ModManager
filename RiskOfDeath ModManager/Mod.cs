using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using HoodedDeathHelperLibrary;
using System.Diagnostics;

namespace RiskOfDeath_ModManager
{
    public class ModContainer
    {
        private readonly List<Mod> mods = new List<Mod>();
        private string BEP_UUID => "4c253b36-fd0b-4e6d-b4d8-b227972af4da";
        private Mod bep;
        private string R2API_UUID => "75541a7e-8bfc-4585-a842-5dbf1aae5b3d";
        private Mod r2api;
        private string THIS_UUID => "ab3e2616-672f-4791-91a9-a09647d7f26d";
        private Mod this_mod;
        private string this_vn = "2.1.1";
        public RuleSet Rules { get; private set; }
        private Dictionary<string, MappedInstalledMod> installedmods = new Dictionary<string, MappedInstalledMod>();
        private string ror2;

        /// <summary>
        /// Do not call without calling UpdateModLists first
        /// </summary>
        public List<MiniMod> AvailableMods { get; private set; } = new List<MiniMod>();
        /// <summary>
        /// Do not call without calling UpdateModLists first
        /// </summary>
        public List<InstalledMod> InstalledMods { get; private set; } = new List<InstalledMod>();
        /// <summary>
        /// Do not call without calling UpdateModLists first
        /// </summary>
        public List<InstalledMod> InstalledDependencies { get; private set; } = new List<InstalledMod>(); //CHANGE TO A DEPENDANCY CLASS OF SOME TYPE

        public ModContainer(string jsonFromThunderStore, string ror2Path)
        {
            this.ror2 = ror2Path;
            //Check for old installed json file
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checked")))
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json")))
                {
                    if (MessageBox.Show("There has been a major change to the way installed mods are saved and your current installed mods file can no longer be used. If you choose yes, the current file holding installed mods will be deleted. This manager works best with fresh, unmodified installation of Risk of Rain 2.", "Changes to installation file", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json"));
                    }
                    else throw new CloseEverythingException();
                }
                FileStream temp = File.Create(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checked"));
                temp.Close();
                temp.Dispose();
            }
            //Read installed mods
            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json")));
                this.installedmods = JsonConvert.DeserializeObject<Dictionary<string, MappedInstalledMod>>(sr.ReadToEnd());
                sr.Close();
                sr.Dispose();
            }
            catch (FileNotFoundException) { Console.WriteLine("Installed mods file does not exist."); }
            catch (Exception e) { Console.WriteLine(e.Message + e.StackTrace); }
            //Read RuleSet
            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "RuleSet.json")));
                this.Rules = JsonConvert.DeserializeObject<RuleSet>(sr.ReadToEnd());
                sr.Close();
                sr.Dispose();
            }
            catch (FileNotFoundException) { Console.WriteLine("RuleSet file cannot be found. Downloads may not function."); MessageBox.Show("RuleSet file cannot be found. Downloads may not function."); }
            catch (Exception e) { Console.WriteLine(e.Message + e.StackTrace); new MessageForm().Show(e.Message + e.StackTrace, "An error occurred while loading RuleSet"); }
            DeserializeMods(jsonFromThunderStore);
            if (this.this_mod.GetLatestVersion().VersionNumber.IsNewer(this.this_vn) && MessageBox.Show("There's a new version of Risk of Death available. Would you like to open Thunderstore to download the latest version?", "Update?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            { Process.Start("explorer.exe", "https://thunderstore.io/package/HoodedDeath/RiskOfDeathModManager/"); throw new CloseEverythingException(); }
            if (!CheckBepWithR2API(ror2Path))
            {
                Console.WriteLine("BepInEx/R2API not installed/invalid. Beginning download of latest version.");
                MessageBox.Show("BepInEx not installed/invalid. Will be downloaded and installed.");
                //Download BepInEx and R2API
                //Download(this.bep.Versions[0].LongName, this.r2api.Versions[0].LongName);
                Download(this.bep.GetLatestVersion().LongName, this.r2api.GetLatestVersion().LongName);
                Console.WriteLine("Done downloading BepInEx and R2API.");
            }
        }
        // --NOT CURRENTLY IN USE--
        // --ONLY NEEDED IF SERIALIZING AND WRITING INSTALLED MODS BREAKS DURING DOWNLOAD METHOD--
        /*public void Dispose()
        {
            StreamWriter sw = new StreamWriter(File.Open(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json"), FileMode.OpenOrCreate));
            sw.Write(JsonConvert.SerializeObject(this.installedmods, Formatting.Indented));
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }*/
        private void DeserializeMods(string json)
        {
            List<ModJson> temp = JsonConvert.DeserializeObject<List<ModJson>>(json);
            foreach (ModJson mj in temp)
                if (mj.uuid4 == THIS_UUID)
                    this.this_mod = new Mod(mj);
                else if (mj.uuid4 == BEP_UUID)
                    this.bep = new Mod(mj);
                else if (mj.uuid4 == R2API_UUID)
                    this.r2api = new Mod(mj);
                else
                    this.mods.Add(new Mod(mj));
        }
        private bool CheckBepWithR2API(string instPath)
        {
            //return this.installedmods.ContainsKey(this.bep.LongName) && this.installedmods.ContainsKey(this.r2api.LongName);
            //Check BepInEx
            string bep = System.IO.Path.Combine(instPath, "BepInEx");
            if (!File.Exists(System.IO.Path.Combine(instPath, "doorstop_config.ini"))) return false;
            if (!File.Exists(System.IO.Path.Combine(instPath, "winhttp.dll"))) return false;
            if (!Directory.Exists(bep)) return false;
            //Check config
            string config = System.IO.Path.Combine(bep, "config");
            if (!Directory.Exists(config)) return false;
            if (!File.Exists(System.IO.Path.Combine(config, "BepInEx.cfg"))) return false;
            if (!File.Exists(System.IO.Path.Combine(config, "com.bepis.r2api.cfg"))) return false;
            //Check core
            string core = System.IO.Path.Combine(bep, "core");
            if (!Directory.Exists(core)) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "0Harmony.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "BepInEx.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "BepInEx.Harmony.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "BepInEx.Preloader.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "Mono.Cecil.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "MonoMod.RuntimeDetour.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "MonoMod.Utils.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "0Harmony.xml"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "BepInEx.Harmony.xml"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "BepInEx.Preloader.xml"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "BepInEx.xml"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "MonoMod.RuntimeDetour.xml"))) return false;
            if (!File.Exists(System.IO.Path.Combine(core, "MonoMod.Utils.xml"))) return false;
            //Check monomod
            string monomod = System.IO.Path.Combine(bep, "monomod");
            if (!Directory.Exists(monomod)) return false;
            if (!File.Exists(System.IO.Path.Combine(monomod, "Assembly-CSharp.R2API.mm.dll"))) return false;
            //Check patchers
            string patchers = System.IO.Path.Combine(bep, "patchers");
            if (!Directory.Exists(patchers)) return false;
            //Check BepInEx.MonoMod.Loader within pathers
            string monomodloader = System.IO.Path.Combine(patchers, "BepInEx.MonoMod.Loader");
            if (!Directory.Exists(monomodloader)) return false;
            if (!File.Exists(System.IO.Path.Combine(monomodloader, "MonoMod.exe"))) return false;
            if (!File.Exists(System.IO.Path.Combine(monomodloader, "BepInEx.MonoMod.Loader.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(monomodloader, "Mono.Cecil.Mdb.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(monomodloader, "Mono.Cecil.Pdb.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(monomodloader, "Mono.Cecil.Rocks.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(monomodloader, "BepInEx.MonoMod.Loader.pdb"))) return false;
            //Check plugins
            string plugins = System.IO.Path.Combine(bep, "plugins");
            if (!Directory.Exists(plugins)) return false;
            //Check R2API withing plugins
            string r2api = System.IO.Path.Combine(plugins, this.r2api.GetLatestVersion().DependencyString, "R2API");
            if (!Directory.Exists(r2api)) return false;
            if (!File.Exists(System.IO.Path.Combine(r2api, "MMHOOK_Assembly-CSharp.dll"))) return false;
            if (!File.Exists(System.IO.Path.Combine(r2api, "R2API.dll"))) return false;
            //If nothing failed, BepInEx is installed correctly
            return true;
        }
        [DebuggerStepThrough]
        public Version FindMod(string dependencyString)
        {
            foreach (Version v in this.bep.Versions)
                if (v.DependencyString == dependencyString)
                    return v;
            foreach (Version v in this.r2api.Versions)
                if (v.DependencyString == dependencyString)
                    return v;
            foreach (Mod m in this.mods)
                foreach (Version v in m.Versions)
                    if (v.LongName == dependencyString)
                        return v;
            return null;
        }

        public void Download(bool backupOnOverwrite, params string[] modStrings)
        {
            foreach (string s in modStrings)
                Download(s, backupOnOverwrite);
        }
        public void Download(params string[] modStrings)
        {
            foreach (string s in modStrings)
                Download(s, true);
        }
        public void Download(string dependencyString)
        {
            Download(dependencyString, true);
        }
        //Needs to download dependencies
        //Needs to save what mods are installed
        //possibly needs to handle updates

        //when installing - if there's folders  instead of just dll's, copy contents into subfolders in corrosponding folders (example: contents of plugins for R2API goes to /BepInEx/plugins/<DependencyString>/ )
        public void Download(string dependencyString, bool backupOnOverwrite)
        {
            //Get version
            Version v = FindMod(dependencyString);
            Download(v.Dependencies.ToArray());
            //Test installed
            /*string test = "";
            string[] testar = dependencyString.Split('-');
            for (int i = 0; i < testar.Length - 1; i++)
                test += testar[i] + "-";
            test = test.Substring(0, test.Length - 1);*/
            if (installedmods.ContainsKey(v.ParentMod.LongName) && !new VersionNumber(installedmods[v.ParentMod.LongName].VersionNumber).IsOlder(v.VersionNumber))
                return;
            if (v.ParentMod.IsDeprecated && MessageBox.Show(string.Format("This mod ({0}) is deprecated and may not install and run properly. It is recommended that you find a newer mod instead. Do you want to continue with installing this mod?", v.DependencyString), "Deprecated Mod", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            //Check download path
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "downloads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //Download package zip
            path = Path.Combine(path, v.DependencyString);
            using (var client = new WebClient())
            {
                Console.WriteLine("Downloading {0} ...", v.DependencyString);
                client.DownloadFile(v.DownloadUrl, path + ".zip");
                client.Dispose();
            }
            //Extract package
            Console.WriteLine("Exctracting {0} ...", v.DependencyString);
            ZipFile.ExtractToDirectory(path + ".zip", path);
            string workingdir = path;
            //Files to not worry about copying
            string[] excludedfiles = new string[] { "icon.png", "license.txt", "manifest.json", "readme.md", "instructions.txt", "license", "credits.txt", "icon.psd", "assetplus.xml", "changelog.md", "license.md" };
            //Begin install
            List<string> fileList = new List<string>();
            if (File.Exists(Path.Combine(workingdir, "rules.json"))) //Author defined rules
            {
                //Read rules file
                Console.WriteLine("Installing with author-defined rule set ...");
                StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(workingdir, "rules.json")));
                Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
                sr.Close();
                sr.Dispose();
                //Work through each directory in package
                foreach (string p in Directory.GetDirectories(workingdir, "*", SearchOption.TopDirectoryOnly))
                {
                    string[] parr = p.Split('\\');
                    string work = Path.Combine(workingdir, parr[parr.Length - 1]);
                    string[] modfiles = Directory.GetFiles(work, "*", SearchOption.AllDirectories);
                    foreach (string modf in modfiles)
                    {
                        string s = modf.Substring(work.Length + 1);
                        string t = "";
                        if (dict.ContainsKey(parr[parr.Length - 1]))
                            t = dict[parr[parr.Length - 1]];
                        switch (t.ToLower())
                        {
                            case "bepinex":
                                // bep
                                s = Path.Combine(ror2, "BepInEx", s);
                                break;
                            case "plugins":
                                // bep/plugins
                                s = Path.Combine(ror2, "BepInEx", "plugins", s);
                                break;
                            case "patchers":
                                // bep/patchers
                                s = Path.Combine(ror2, "BepInEx", "patchers", s);
                                break;
                            case "monomod":
                                // bep/monomod
                                s = Path.Combine(ror2, "BepInEx", "monomod", s);
                                break;
                            case "core":
                                // bep/core
                                s = Path.Combine(ror2, "BepInEx", "core", s);
                                break;
                            case "data":
                                // ror2_data
                                s = Path.Combine(ror2, "Risk of Rain 2_Data", s);
                                break;
                            case "language":
                                // ror2_data/language
                                s = Path.Combine(ror2, "Risk of Rain 2_Data", "language", s);
                                break;
                            case "en-us":
                                // ror2_data/language/en
                                s = Path.Combine(ror2, "Risk of Rain 2_Data", "language", "en", s);
                                break;
                            case "managed":
                                // ror2_data/managed
                                s = Path.Combine(ror2, "Risk of Rain 2_Data", "managed", s);
                                break;
                            default:
                                //act as plugins
                                s = Path.Combine(ror2, "BepInEx", "plugins", s);
                                break;
                        }
                        VerifyPathToFile(s);
                        if (backupOnOverwrite && File.Exists(modf))
                            File.Move(s, s + ".bak");
                        File.Copy(modf, s, true);
                        fileList.Add(s);
                        Console.WriteLine("\"{0}\" copied to \"{1}\"", modf, s);

                    }
                }
            }
            else
            {
                // default rules
                SpecialCaseRules sc = null;
                foreach (SpecialCaseRules scr in this.Rules.SpecialCases)
                {
                    if (sc != null)
                        break;
                    if (scr.UUIDS.Contains(v.ParentMod.UUID4))
                        sc = scr;
                }
                if (sc != null)
                {
                    //special case
                    List<string> ignore = new List<string>();
                    foreach (string st in excludedfiles)
                        ignore.Add(st.ToLower());
                    //pick one
                    if (sc.PickOneOfX)
                    {
                        foreach (KeyValuePair<string, string[]> kvp in sc.PickOptions)
                            ignore.Add(Path.Combine(workingdir, kvp.Value[0]).ToLower()); // files.Remove(Path.Combine(workingdir, kvp.Value[0]));
                        KeyValuePair<string, string[]> k;
                        using (var form = new PickForm(v.ParentMod.LongName, sc.PickOptions))
                        {
                            DialogResult res = form.ShowDialog();
                            if (res == DialogResult.Cancel)
                            {
                                Console.WriteLine("Cancelling mod installation after options cancel ...");
                                //Directory.Delete(workingdir);
                                Helper.DeleteDirectory(workingdir);
                                return;
                            }
                            else if (res == DialogResult.OK)
                            {
                                k = form.SelectedKVP;
                                string mod = Path.Combine(workingdir, k.Value[0]);
                                string s = mod.Split('\\')[mod.Split('\\').Length - 1];
                                switch (k.Value[1].ToLower())
                                {
                                    case "plugins":
                                        s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                        break;
                                    default:
                                        s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                        break;
                                }
                                VerifyPathToFile(s);
                                if (backupOnOverwrite && File.Exists(s))
                                    File.Move(s, s + ".bak");
                                File.Copy(mod, s, true);
                                fileList.Add(s);
                                Console.WriteLine("{0} -> Selected {1}", v.ParentMod.LongName, k.Key);
                            }
                            else
                                Console.WriteLine("+\n+\n+\nidk wtf just happened here - options dialog should've only given a dialog result of ok or cancel\n+\n+\n+");
                        }
                    }
                    //Folders
                    foreach (string p in Directory.GetDirectories(workingdir, "*", SearchOption.TopDirectoryOnly))
                    {
                        string[] parr = p.Split('\\');
                        string work = Path.Combine(workingdir, parr[parr.Length - 1]);
                        string[] modfiles = Directory.GetFiles(work, "*", SearchOption.AllDirectories);
                        foreach (string modf in modfiles)
                        {
                            if (ignore.Contains(modf.ToLower()))
                                continue;
                            string s = modf.Substring(work.Length + 1);
                            string t = "";
                            if (sc.Folders.ContainsKey(parr[parr.Length - 1]))
                                t = sc.Folders[parr[parr.Length - 1]];
                            switch (t.ToLower())
                            {
                                case "base":
                                    s = Path.Combine(ror2, s);
                                    break;
                                case "bepinex":
                                    // bep
                                    s = Path.Combine(ror2, "BepInEx", s);
                                    break;
                                case "plugins":
                                    // bep/plugins
                                    s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                    break;
                                case "plugins\\TwitchIntegration":
                                    // bep/plugins/TwitchIntegration
                                    s = Path.Combine(ror2, "BepInEx", "plugins", "TwitchIntegration", s);
                                    break;
                                case "patchers":
                                    // bep/patchers
                                    s = Path.Combine(ror2, "BepInEx", "patchers", s);
                                    break;
                                case "monomod":
                                    // bep/monomod
                                    s = Path.Combine(ror2, "BepInEx", "monomod", s);
                                    break;
                                case "core":
                                    // bep/core
                                    s = Path.Combine(ror2, "BepInEx", "core", s);
                                    break;
                                case "data":
                                    // ror2_data
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", s);
                                    break;
                                case "language":
                                    // ror2_data/language
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", "language", s);
                                    break;
                                case "en-us":
                                    // ror2_data/language/en
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", "language", "en", s);
                                    break;
                                case "managed":
                                    // ror2_data/managed
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", "managed", s);
                                    break;
                                default:
                                    //act as plugins
                                    s = Path.Combine(ror2, "BepInEx", "plugins", s);
                                    break;
                            }
                            VerifyPathToFile(s);
                            if (backupOnOverwrite && File.Exists(s))
                                File.Move(s, s + ".bak");
                            File.Copy(modf, s, true);
                            fileList.Add(s);
                            Console.WriteLine("\"{0}\" copied to \"{1}\"", modf, s);
                            ignore.Add(modf.ToLower());
                        }
                    }
                    //Files
                    foreach (string f in Directory.GetFiles(workingdir, "*", SearchOption.TopDirectoryOnly))
                    {
                        string[] farr = f.Split('\\');
                        if (ignore.Contains(f.ToLower()) || ignore.Contains(farr[farr.Length - 1].ToLower()))
                            continue;
                        farr = farr[farr.Length - 1].Split('.');
                        string s = f.Substring(workingdir.Length + 1);
                        string t = "";
                        if (sc.Files.ContainsKey("." + farr[farr.Length - 1]))
                            t = sc.Files["." + farr[farr.Length - 1]];
                        switch (t.ToLower())
                        {
                            case "plugins":
                                s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                break;
                            default:
                                s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                break;
                        }
                        VerifyPathToFile(s);
                        if (backupOnOverwrite && File.Exists(s))
                            File.Move(s, s + ".bak");
                        File.Copy(f, s, true);
                        fileList.Add(s);
                        Console.WriteLine("\"{0}\" copied to \"{1}\"", f, s);
                        ignore.Add(f.ToLower());
                    }
                }
                else
                {
                    //general rules
                    Console.WriteLine("Installing with default ruleset ...");
                    //folder rules
                    foreach (string p in Directory.GetDirectories(workingdir, "*", SearchOption.TopDirectoryOnly))
                    {
                        string[] parr = p.Split('\\');
                        string work = Path.Combine(workingdir, parr[parr.Length - 1]);
                        string[] modfiles = Directory.GetFiles(work, "*", SearchOption.AllDirectories);
                        foreach (string modf in modfiles)
                        {
                            string s = modf.Substring(work.Length + 1);
                            string t = "";
                            if (this.Rules.Folders.ContainsKey(parr[parr.Length - 1]))
                                t = this.Rules.Folders[parr[parr.Length - 1]];
                            switch (t.ToLower())
                            {
                                case "bepinex":
                                    // bep
                                    s = Path.Combine(ror2, "BepInEx", s);
                                    break;
                                case "plugins":
                                    // bep/plugins
                                    s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                    break;
                                case "patchers":
                                    // bep/patchers
                                    s = Path.Combine(ror2, "BepInEx", "patchers", s);
                                    break;
                                case "monomod":
                                    // bep/monomod
                                    s = Path.Combine(ror2, "BepInEx", "monomod", s);
                                    break;
                                case "core":
                                    // bep/core
                                    s = Path.Combine(ror2, "BepInEx", "core", s);
                                    break;
                                case "data":
                                    // ror2_data
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", s);
                                    break;
                                case "language":
                                    // ror2_data/language
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", "language", s);
                                    break;
                                case "en-us":
                                    // ror2_data/language/en
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", "language", "en", s);
                                    break;
                                case "managed":
                                    // ror2_data/managed
                                    s = Path.Combine(ror2, "Risk of Rain 2_Data", "managed", s);
                                    break;
                                default:
                                    //act as plugins
                                    s = Path.Combine(ror2, "BepInEx", "plugins", s);
                                    break;
                            }
                            VerifyPathToFile(s);
                            if (backupOnOverwrite && File.Exists(s))
                                File.Move(s, s + ".bak");
                            File.Copy(modf, s, true);
                            fileList.Add(s);
                            Console.WriteLine("\"{0}\" copied to \"{1}\"", modf, s);
                        }
                    }
                    //
                    //file rules
                    foreach (string f in Directory.GetFiles(workingdir, "*", SearchOption.TopDirectoryOnly))
                    {
                        string[] farr = f.Split('\\');
                        if (excludedfiles.Contains(farr[farr.Length - 1].ToLower()))
                            continue;
                        farr = farr[farr.Length - 1].Split('.');
                        string s = f.Substring(workingdir.Length + 1);
                        string t = "";
                        if (this.Rules.Files.ContainsKey("." + farr[farr.Length - 1]))
                            t = this.Rules.Files["." + farr[farr.Length - 1]];
                        switch (t.ToLower())
                        {
                            case "plugins":
                                s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                break;
                            default:
                                s = Path.Combine(ror2, "BepInEx", "plugins", v.DependencyString, s);
                                break;
                        }
                        VerifyPathToFile(s);
                        if (backupOnOverwrite && File.Exists(s))
                            File.Move(s, s + ".bak");
                        File.Copy(f, s, true);
                        fileList.Add(s);
                        Console.WriteLine("\"{0}\" copied to \"{1}\"", f, s);
                    }

                }
            }
            //Directory.Delete(workingdir);
            Helper.DeleteDirectory(workingdir);

            //
            //don't forget all folders not covered in ruleset get their contents copied into BepInEx\plugins
            //SeikoML should be replaced by SeikoMLCompatLayer
            MappedInstalledMod map = new MappedInstalledMod() { VersionNumber = v.VersionNumber.ToString(), Files = fileList.ToArray() };
            this.installedmods.Add(v.ParentMod.LongName, map);
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json")))
                File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json"));
            StreamWriter sw = new StreamWriter(File.Open(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json"), FileMode.OpenOrCreate));
            sw.Write(JsonConvert.SerializeObject(this.installedmods, Formatting.Indented));
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        public void Uninstall(string dependencyString)
        {
            Version v = FindMod(dependencyString);
            string[] files = this.installedmods[v.ParentMod.LongName].Files;
            foreach (string file in files)
            {
                File.Delete(file);
                if (File.Exists(file + ".bak"))
                    File.Move(file + ".bak", file);
            }
            this.installedmods.Remove(v.ParentMod.LongName);
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json")))
                File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json"));
            StreamWriter sw = new StreamWriter(File.Open(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "installed.json"), FileMode.OpenOrCreate));
            sw.Write(JsonConvert.SerializeObject(this.installedmods, Formatting.Indented));
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        public void UpdateMod(string dependencyString)
        {
            Uninstall(dependencyString);
            Download(FindMod(dependencyString).ParentMod.GetLatestVersion().DependencyString);
        }
        private void VerifyPathToFile(string path)
        {
            if (path == null || path.Trim() == "")
                throw new ArgumentException("Path input for VerifyPathToFile cannot be null or empty");
            path = path.Trim();
            string[] arr = path.Split('\\');
            string s = arr[0] + "\\";
            for (int i = 1; i < arr.Length - 1; i++)
            {
                s = Path.Combine(s, arr[i]);
                if (!Directory.Exists(s))
                    Directory.CreateDirectory(s);
            }
        }

        public void UpdateModLists()
        {
            this.AvailableMods = new List<MiniMod>();
            this.InstalledMods = new List<InstalledMod>();
            this.InstalledDependencies = new List<InstalledMod>
            {
                new InstalledMod(this.bep, this.bep.GetLatestVersion()),
                new InstalledMod(this.r2api, this.r2api.GetLatestVersion())
            };
            foreach (Mod m in this.mods)
            {
                if (this.installedmods.ContainsKey(m.LongName))
                {
                    Version v = FindMod(string.Format("{0}-{1}", m.LongName, this.installedmods[m.LongName].VersionNumber));
                    this.InstalledMods.Add(new InstalledMod(v.ParentMod, v));
                    foreach (string dep in v.Dependencies)
                    {
                        v = FindMod(dep);
                        if (!this.InstalledDependencies.Contains(new InstalledMod(v.ParentMod, v)))
                            this.InstalledDependencies.Add(new InstalledMod(v.ParentMod, v));
                    }
                }
                else if (!this.Rules.Exclusions.Contains(m.UUID4))
                    this.AvailableMods.Add(new MiniMod(m));
            }
            List<InstalledMod> temp = new List<InstalledMod>();
            foreach (InstalledMod im in this.InstalledMods)
                if (!this.InstalledDependencies.Contains(im))
                    temp.Add(im);
            this.InstalledMods = temp;
        }
        //
        public List<MiniMod> ModsToList
        {
            get {
                List<MiniMod> t = new List<MiniMod>();
                foreach (Mod m in this.mods)
                    if (!this.Rules.Exclusions.Contains(m.UUID4))
                        t.Add(new MiniMod(m));
                return t;
            }
        }
        //
    }
    public class RuleSet
    {
        [JsonProperty("special")]
        public List<SpecialCaseRules> SpecialCases = new List<SpecialCaseRules>();
        [JsonProperty("folders")]
        public Dictionary<string, string> Folders = new Dictionary<string, string>();
        [JsonProperty("files")]
        public Dictionary<string, string> Files = new Dictionary<string, string>();
        [JsonProperty("exclusions")]
        public List<string> Exclusions = new List<string>();
    }
    public class SpecialCaseRules
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("uuids")]
        public List<string> UUIDS;
        [JsonProperty("pick_one_of_x")]
        public bool PickOneOfX;
        [JsonProperty("pick_options")]
        public Dictionary<string, string[]> PickOptions = new Dictionary<string, string[]>();
        [JsonProperty("folders")]
        public Dictionary<string, string> Folders = new Dictionary<string, string>();
        [JsonProperty("files")]
        public Dictionary<string, string> Files = new Dictionary<string, string>();
    }

    public class Mod
    {
        public string Name { get; private set; }
        public string LongName { get; private set; }
        public string Owner { get; private set; }
        public string PkgUrl { get; private set; }
        public string CreatedDate { get; private set; }
        public string UpdatedDate { get; private set; }
        public string UUID4 { get; private set; }
        public bool IsPinned { get; private set; }
        public bool IsDeprecated { get; private set; }
        public List<Version> Versions { get; private set; }
        
        [DebuggerStepThrough]
        public Mod(ModJson mod)
        {
            this.Name = mod.name;
            this.LongName = mod.full_name;
            this.Owner = mod.owner;
            this.PkgUrl = mod.package_url;
            this.CreatedDate = mod.date_created;
            this.UpdatedDate = mod.date_updated;
            this.UUID4 = mod.uuid4;
            this.IsPinned = mod.is_pinned;
            this.IsDeprecated = mod.is_deprecated;
            this.Versions = MakeVersions(mod.versions);
        }
        public List<Version> MakeVersions(List<VersionJson> versions)
        {
            List<Version> t = new List<Version>();
            foreach (VersionJson v in versions)
                t.Add(new Version(v, this));
            return t;
        }

        [DebuggerStepThrough]
        public Version GetLatestVersion()
        {
            Version temp = this.Versions[0];
            for (int i = 1; i < this.Versions.Count; i++)
                if (this.Versions[i].IsNewer(temp)) temp = this.Versions[i];
            return temp;
        }
    }
    public class ModJson
    {
        public string name;
        public string full_name;
        public string owner;
        public string package_url;
        public string date_created;
        public string date_updated;
        public string uuid4;
        public bool is_pinned;
        public bool is_deprecated;
        public List<VersionJson> versions;
    }
    public class MiniMod
    {
        public string Name { get; private set; }
        public string Owner { get; private set; }
        public string PkgUrl { get; private set; }
        public bool IsDeprecated { get; private set; }
        public List<MiniVersion> Versions { get; private set; }

        public MiniMod(Mod m)
        {
            Name = m.Name;
            Owner = m.Owner;
            PkgUrl = m.PkgUrl;
            IsDeprecated = m.IsDeprecated;
            Versions = MakeVersions(m.Versions);
        }
        private List<MiniVersion> MakeVersions(List<Version> versions)
        {
            List<MiniVersion> t = new List<MiniVersion>();
            foreach (Version v in versions)
                t.Add(new MiniVersion(v));
            return t;
        }
    }
    public class InstalledMod
    {
        public string Name { get; private set; }
        public string LongName { get; private set; }
        public string Owner { get; private set; }
        public string HasUpdate { get; private set; }
        public InstalledVersion Version { get; private set; }
        private List<VersionNumber> Versions { get; set; } = new List<VersionNumber>();

        public InstalledMod(Mod m, Version v)
        {
            this.Name = m.Name;
            this.LongName = m.LongName;
            this.Owner = m.Owner;
            this.Version = new InstalledVersion(v, this);
            foreach (Version va in m.Versions)
                this.Versions.Add(va.VersionNumber);

        }

        public bool IsUpToDate()
        {
            bool t = false;
            foreach (VersionNumber v in this.Versions)
            {
                if (t)
                    break;
                t |= v.IsNewer(this.Version.VersionNumber);
            }
            return !t;
        }
    }

    class MappedInstalledMod
    {
        [JsonProperty("version_number")]
        public string VersionNumber { get; set; }
        [JsonProperty("files")]
        public string[] Files { get; set; }
    }
}
