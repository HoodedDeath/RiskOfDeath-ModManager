using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskOfDeath_ModManager
{
    public class Version
    {
        public string Name { get; private set; }
        public string LongName { get; private set; }
        public string DependencyString => LongName;
        public string Description { get; private set; }
        public string IconUrl { get; private set; }
        public VersionNumber VersionNumber { get; private set; }
        public List<string> Dependencies { get; private set; }
        public string DownloadUrl { get; private set; }
        public int Downloads { get; private set; }
        public string CreatedDate { get; private set; }
        public string WebsiteUrl { get; private set; }
        public bool IsActive { get; private set; }
        public string UUID4 { get; private set; }
        public Mod ParentMod { get; private set; }

        public Version(VersionJson v, Mod parent)
        {
            this.ParentMod = parent;
            Name = v.name;
            LongName = v.full_name;
            Description = v.description;
            IconUrl = v.icon;
            VersionNumber = new VersionNumber(v.version_number);
            Dependencies = v.dependencies;
            DownloadUrl = v.download_url;
            Downloads = v.downloads;
            WebsiteUrl = v.website_url;
            IsActive = v.is_active;
            UUID4 = v.uuid4;
        }

        public bool IsNewer(Version v) => VersionNumber.IsNewer(v.VersionNumber);
        public bool IsOlder(Version v) => VersionNumber.IsOlder(v.VersionNumber);
    }
    public class VersionJson
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
    public class VersionNumber
    {
        public int MajorBuild { get; private set; }
        public int MinorBuild { get; private set; }
        public int Revision { get; private set; }
        public string FullVersionNumber { get { return string.Format("{0}.{1}.{2}", MajorBuild, MinorBuild, Revision); } }

        public VersionNumber(string s)
        {
            string[] split = s.Split('.');
            if (split.Length != 3) throw new ArgumentException(string.Format("Version number {0} is invalid.", s));
            MajorBuild = Convert.ToInt32(split[0]);
            MinorBuild = Convert.ToInt32(split[1]);
            Revision = Convert.ToInt32(split[2]);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", MajorBuild, MinorBuild, Revision);
        }

        /// <summary>
        /// Tests if this VersionNumber is newer than the given VersionNumber
        /// </summary>
        /// <param name="vn">The VersionNumber to test</param>
        /// <returns>True if the calling VersionNumber is newer than the parameter, false if it is equal to or older than the paramete</returns>
        public bool IsNewer(VersionNumber vn)
        {
            if (this.MajorBuild < vn.MajorBuild)
                return false;
            if (this.MajorBuild > vn.MajorBuild)
                return true;
            if (this.MinorBuild < vn.MinorBuild)
                return false;
            if (this.MinorBuild > vn.MinorBuild)
                return true;
            if (this.Revision < vn.Revision)
                return false;
            if (this.Revision > vn.Revision)
                return true;

            /*if (this.MajorBuild <= vn.MajorBuild) return false;
            if (this.MinorBuild <= vn.MinorBuild) return false;
            if (this.Revision <= vn.Revision) return false;*/
            return false;
        }
        /// <summary>
        /// Tests if this VersionNumber is newer than the given VersionNumber
        /// </summary>
        /// <param name="vn">The string representation of the VersionNumber to test</param>
        /// <returns>True if the calling VersionNumber is newer than the parameter, false if it is equal to or older than the paramete</returns>
        public bool IsNewer(string vn) => IsNewer(new VersionNumber(vn));

        /// <summary>
        /// Tests if this VersionNumber is older than the given VersionNumber
        /// </summary>
        /// <param name="vn">The VersionNumber to test</param>
        /// <returns>True if the calling VersionNumber is older than the parameter, false if it is equal to or newer than the paramete</returns>
        public bool IsOlder(VersionNumber vn)
        {
            if (this.MajorBuild >= vn.MajorBuild) return false;
            if (this.MinorBuild >= vn.MinorBuild) return false;
            if (this.Revision >= vn.Revision) return false;
            return true;
        }
        /// <summary>
        /// Tests if this VersionNumber is older than the given VersionNumber
        /// </summary>
        /// <param name="vn">The string representation of the VersionNumber to test</param>
        /// <returns>True if the calling VersionNumber is older than the parameter, false if it is equal to or newer than the paramete</returns>
        public bool IsOlder(string vn) => IsOlder(new VersionNumber(vn));
    }
    public class MiniVersion
    {
        public string DependencyString { get; private set; }
        public string Description { get; private set; }
        public string IconUrl { get; private set; }
        public VersionNumber VersionNumber { get; private set; }
        
        public MiniVersion(Version v)
        {
            DependencyString = v.DependencyString;
            Description = v.Description;
            IconUrl = v.IconUrl;
            VersionNumber = v.VersionNumber;
        }
    }
    public class InstalledVersion
    {
        public string Name { get; private set; }
        public string LongName { get; private set; }
        public string DependencyString => LongName;
        public string Description { get; private set; }
        public string IconUrl { get; private set; }
        public VersionNumber VersionNumber { get; private set; }
        public List<string> Dependencies { get; private set; }
        public InstalledMod ParentMod { get; private set; }

        public InstalledVersion(Version v, InstalledMod parent)
        {
            this.ParentMod = parent;
            Name = v.Name;
            LongName = v.LongName;
            Description = v.Description;
            IconUrl = v.IconUrl;
            VersionNumber = v.VersionNumber;
            Dependencies = v.Dependencies;
        }

        public bool IsNewest { get { return this.ParentMod.IsUpToDate(); } }
    }
}
