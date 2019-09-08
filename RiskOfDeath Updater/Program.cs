using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Reflection;
using HoodedDeathHelperLibrary;
using System.IO.Compression;

namespace RiskOfDeath_Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fetching mod from thunderstore ...");
            WebRequest request = WebRequest.Create("https://thunderstore.io/api/v1/package/ab3e2616-672f-4791-91a9-a09647d7f26d");
            WebResponse resp = request.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            Mod mod = JsonConvert.DeserializeObject<Mod>(sr.ReadToEnd());
            sr.Close();
            sr.Dispose();
            resp.Close();
            Console.WriteLine("Downloading update ...");
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string working = Path.Combine(path, mod.versions[0].full_name);
            using (WebClient cli = new WebClient())
            {
                cli.Proxy = null;
                cli.DownloadFile(mod.versions[0].download_url, working + ".zip");
                cli.Dispose();
            }
            Console.WriteLine("Extracting version {0} ...", mod.versions[0].version_number);
            if (Directory.Exists(working))
                Helper.DeleteDirectory(working);
            ZipFile.ExtractToDirectory(working + ".zip", working);
            string tworking = Path.Combine(working, "Risk of Death");
            foreach (string file in Directory.GetFiles(tworking, "*", SearchOption.TopDirectoryOnly))
            {
                string tpath = path.Substring(0, path.Length - 8);
                string temp = tpath + file.Remove(0, working.Length + 14);
                Console.WriteLine("Copying \"{0}\" to \"{1}\" ...", file, temp);
                try { File.Copy(file, temp, true); } catch (Exception) { }
            }
            Helper.DeleteDirectory(working);
            File.Delete(Path.Combine(path, mod.versions[0].full_name) + ".zip");
            Console.WriteLine("Update finished. Thank you for updating, press any key to finish.");
            Console.ReadKey();
        }
    }

    public class Mod
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
        public List<Version> versions;
    }
    public class Version
    {
        public string name;
        public string full_name;
        public string description;
        public string icon;
        public string version_number;
        public List<string> dependencies;
        public string download_url;
        public int downloads;
        public string date_created;
        public string website_url;
        public bool is_active;
        public string uuid4;
    }
}
