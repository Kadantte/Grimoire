﻿using System;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using Grimoire.Tabs;
using Grimoire.Configuration;

namespace Grimoire.Structures
{
    //TODO: Changing output directory needs to update open rdb tabs
    //TODO: changing save with ascii/encrypted needs to update open rdb tabs

    class Settings
    {
        ConfigManager configMan = GUI.Main.Instance.ConfigMan;
        Tabs.Manager tabMan = Tabs.Manager.Instance;

        //[Description("A list of styles that can be choosen from the 'New' drop-down (Delimited by ',')"), Category("Tab Style"), DisplayName("Styles")]
        //public string Styles { get => configMan["Styles"]; set => configMan["Styles"] = value; }

        [Description("Default Tab Style to be loaded when starting Grimoire (if any)"), Category("Tab Style"), DisplayName("Default Style"), DefaultValue(Tabs.Style.NONE)]
        public Tabs.Style DefaultStyle
        {
            get => (Tabs.Style)Enum.Parse(typeof(Style), configMan["DefaultStyle", "Tab"]);
            set => configMan["DefaultStyle", "Tab"] = value.ToString();
        }

        [Description("The IP at which your database can be connected to"), Category("Database")]
        public string IP { get => configMan["IP"]; set => configMan["IP"] = value; }

        [Description("The Port (if any) your database is behind"), Category("Database"), DefaultValue(1433)]
        public int Port { get => (int)configMan["Port"]; set => configMan["Port"] = value; }

        [Description("Determines if Grimoire will use Windows Authentication to connect to the database, this does not require Username/Password."), Category("Database"), DisplayName("Trusted Connection")]
        public bool Trusted { get => configMan["Trusted", "DB"]; set => configMan["Trusted", "DB"] = value; }

        [Description("The database name of your Arcadia (world database) e.g. ....Arcadia or 62World"), Category("Database"), DefaultValue("Arcadia"), DisplayName("Arcadia Name")]
        public string WorldName { get => configMan["WorldName"]; set => configMan["WorldName"] = value; }

        [Description("The username used to connect to the world database. Leave blank if Trusted Connection is true"), Category("Database"), DefaultValue("sa"), DisplayName("Arcadia Username")]
        public string WorldUser { get => configMan["WorldUser"]; set => configMan["WorldUser"] = value; }

        [Description("The password used to connect to the world database. Leave blank if Trusted Connection is true"), Category("Database"), DisplayName("Arcadia Password")]
        public string WorldPass { get => configMan["WorldPass"]; set => configMan["WorldPass"] = value; }

        [Description("Determines if the target table of the save operation will be backed up before inserting the .rdb data by creating a script (.sql) of the tables data in the /scripts/ folder"), Category("Database"), DisplayName("Backup Table")]
        public bool BackupTable { get => configMan["Backup", "DB"]; set => configMan["Backup", "DB"] = value; }

        [Description("Determines if the target table of any SQL save operation will be dropped and recreated or truncated before inserting the .rdb data"), Category("Database"), DisplayName("Drop Table")]
        public bool DropTable { get => configMan["DropOnExport"]; set => configMan["DropOnExport"] = value; }

        [Description("Directory where sql related scripts will be stored."), Category("Database"), DisplayName("Scripts Directory")]
        public string ScriptsDirectory { get => configMan["ScriptsDirectory", "DB"]; set => configMan["ScriptsDirectory", "DB"] = value; }

        [Description("Determines the period of time (in seconds) a SQL Connection attempt will last before timing out (expiring)."), Category("Database"), DisplayName("Connection Timeout")]
        public int ConnTimeout { get => (int)configMan["Timeout", "DB"]; set => configMan["Timeout", "DB"] = value; }

        [Description("If defined, this is where any exported/built files will be placed. If not defined Grimoire will use/create the 'Output' folder."), Category("Data/RDB Utility"), DisplayName("Build Directory"), EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string BuildDirectory 
        { 
            get => configMan["BuildDirectory", "Grim"];
            set
            {
                configMan["BuildDirectory", "Grim"] = value;

                string[] keys = tabMan.GetKeysByStyle(Style.RDB) ?? new string[0];

                foreach (string key in keys)
                    tabMan.RDBTabByKey(key).BuildDirectory = value;
            }
        }

        // SPR / Dump utilities

        [Description("Directory where a previously generated data dump is located"), DisplayName("Dump Directory"), Category("SPR Generator / Dump Updater"), EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DumpDirectory { get => configMan["DumpDirectory", "Grim"]; set => configMan["DumpDirectory", "Grim"] = value; }

        // data
        [Description("The default directory displayed when opening local files in the Data Utility"), Category("Data Utility"), DisplayName("Default Directory"), EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DataLoadDirectory { get => configMan["LoadDirectory", "Data"]; set => configMan["LoadDirectory", "Data"] = value; }

        [Description("Determines if Grimoire will use a modified xor encryption key to load/alter client data files"), Category("Data Utility"), DisplayNameAttribute("Use Modified XOR Key")]
        public bool UseModifiedXOR { get => configMan["UseModifiedXOR"]; set => configMan["UseModifiedXOR"] = value; }

        [Description("Determines if files like data.000-008 will be backed up before any changes are made to them. (RECOMMENDED)"), Category("Data Utility"), DisplayName("Backups")]
        public bool Backups 
        { 
            get => configMan["Backup", "Data"];
            set
            {
                configMan["Backup", "Data"] = value;

                string[] keys = tabMan.GetKeysByStyle(Style.DATA) ?? new string[0];

                foreach (string key in keys)
                {
                    DataCore.Core core = tabMan.DataCoreByKey(key);

                    if (core != null)
                        core.Backups = value;
                }
            }       
        }

        [Description("Determines if the DataCore.Core will be cleared after a successful 'New' client has been created. If set to False the newly created client will be displayed as if loaded."), Category("Data Utility"), DisplayName("Clear on Create")]
        public bool ClearCreate { get => configMan["ClearOnCreate"]; set => configMan["ClearOnCreate"] = value; }

        // rdb

        [Description("Determines if the structure you select will be loaded the moment you select it or manually with \"Load\" button"), Category("RDB Utility"), DisplayName("Load on Select")]
        public bool AutoLoad { get => configMan["Struct_AutoLoad"]; set => configMan["Struct_AutoLoad"] = value; }
        
        [Description("The path where RDB Structure .lua Files are stored. Likely /structures/"), Category("RDB Utility"), DisplayName("Structures Directory"), EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string RDBStructureDirectory { get => configMan["Directory", "RDB"]; set => configMan["Directory", "RDB"] = value; }

        [Description("The default directory displayed when opening local files in the RDB Utility"), Category("RDB Utility"), DisplayName("Default Directory"), EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string RDBLoadDirectory { get => configMan["LoadDirectory", "RDB"]; set => configMan["LoadDirectory", "RDB"] = value; }

        [Description("Determines if (ascii) is appended to file names being loaded or saved"), Category("RDB Utility"), DefaultValue(false), DisplayName("Use ASCII")]
        public bool UseASCII
        {
            get => configMan["AppendASCII"];
            set
            {
                if (Manager.Instance.RDBTab != null)
                    Manager.Instance.RDBTab.UseASCII = value;

                configMan["AppendASCII"] = value;
            }
        }

        [Description("Determines if newly created .RDB files will be saved in their hash name version"), Category("RDB Utility"), DefaultValue(false), DisplayName("Save Hashed")]
        public bool SaveHashed
        {
            get => configMan["SaveHashed", "RDB"];
            set
            {
                if (Manager.Instance.RDBTab != null)
                    Manager.Instance.RDBTab.SaveEncrypted = value;

                configMan["SaveHashed", "RDB"] = value;
            }
        }

        [Description("The directory where all .csv will be saved to."), Category("RDB Utility"), DisplayName("CSV Directory"), EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CSVDirectory { get => configMan["CSV_Directory"]; set => configMan["CSV_Directory"] = value; }

        // hasher
        [Description("Determines if the file entries will be cleared from the grid after conversion"), Category("Hasher Utility"), DisplayName("Auto Clear")]
        public bool AutoClear { get => configMan["AutoClear", "Hash"]; set => configMan["AutoClear", "Hash"] = value; }

        [Description("Determines if files are automatically converted after being added to the grid."), Category("Hasher Utility"), DisplayName("Auto Convert")]
        public bool AutoConvert { get => configMan["AutoConvert", "Hash"]; set => configMan["AutoConvert", "Hash"] = value; }

        // logger
        //[Description("Determines the level'(s) of messages to be displayed/written by the logging system"), Category("Logger"), DisplayName("Logging Level")]
        //public int LogLevel { get { return OPT.GetInt("log.level"); } set { OPT.UpdateSetting("log.evel", value.ToString()); } }

        // use flag
        [Description("Directory where flag files are stored"), DisplayName("Flag Files Directory"), Category("Flag Editor"), EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string FlagDirectory { get => configMan["Directory", "Flag"]; set => configMan["Directory", "Flag"] = value; }

        [Description("Path of the flag file to be loaded by default"), Category("Flag Editor"), DisplayName("Flag List Path"), EditorAttribute(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string DefaultPath { get => configMan["Default", "Flag"]; set => configMan["Default", "Flag"] = value; }

        [Description("Determines if the flag selections are reset when changing flag lists!"), DisplayName("Clear on List Changed"), Category("Flag Editor")]
        public bool ClearOnChange { get => configMan["ClearOnChange", "Flag"]; set => configMan["ClearOnChange", "Flag"] = value; }

        // Dump Updater

        [Description("Determines if existing files (in the dump) will be overwritten (without warning) by files being copied into it."), DisplayName("Overwrite Existing Files"), Category("Dump Updater")]
        public bool OverwriteExisting { get => configMan["OverwriteExisting", "DumpUpdater"]; set => configMan["OverwriteExisting", "DumpUpdater"] = value; }

        // Spr Generator

        [Description("Determines if images whose size does not match the expected size will be processed"), DisplayName("Ignore Invalid Size"), Category("SPR Generator")]
        public bool IgnoreSize { get => configMan["IgnoreSize", "SPR"]; set => configMan["IgnoreSize", "SPR"] = value; }

        [Description("Determines if a warning will be shown to the caller during generating"), DisplayName("Show Warnings"), Category("SPR Generator")]
        public bool ShowWarnings { get => configMan["ShowWarnings", "SPR"]; set => configMan["ShowWarnings", "SPR"] = value; }

        [Description("Determines if unverified/ignored spr icons will be listed at the end of generating"), DisplayName("Show Ignored"), Category("SPR Generator")]
        public bool ShowIgnored { get => configMan["ShowIgnored", "SPR"]; set => configMan["ShowIgnored", "SPR"] = value; }

        // locale

        [Description("Determines the displayed language of Grimoire"), DisplayName("Locale"), Category("Language")]
        public string Locale { get => configMan["Locale", "Localization"]; set => configMan["Locale", "Localization"] = value; }

        [Description("Directory where files relevant to the localization engine are located"), DisplayName("Localization Directory"), Category("Language")]
        public string LocaleDirectory { get => configMan["Directory", "Localization"]; set => configMan["Directory", "Localization"] = value; }
    }
}