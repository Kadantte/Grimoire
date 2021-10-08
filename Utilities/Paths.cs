﻿using System.IO;
using System.Windows.Forms;

namespace Grimoire.Utilities
{
    public static class Paths
    {
        public static string DefaultDirectory;
        public static string DefaultFileName = string.Empty;
        static string title;

        public static string Title
        {
            set
            {
                title = value;
            }
        }

        static string description;
        public static string Description
        {
            set
            {
                description = value;
            }
        }

        public static bool FileMultiSelect = false;
        public static DialogResult FileResult;
        public static DialogResult SaveResult;
        public static DialogResult FolderResult;

        private static string[] filePaths
        {
            get
            {
                title = (title == null) ? "Please select desired file" : title;
                using (OpenFileDialog ofDlg = new OpenFileDialog()
                {
                    DefaultExt = "*",
                    Title = title,
                    InitialDirectory = DefaultDirectory,
                    Multiselect = FileMultiSelect,
                    FileName = DefaultFileName
                })
                {
                    if ((FileResult = ofDlg.ShowDialog(Grimoire.GUI.Main.Instance)) == DialogResult.OK)
                        return File.Exists(ofDlg.FileName) ? ofDlg.FileNames : null;
                }

                return new string[1] { null };
            }
        }

        public static string FilePath => filePaths?[0];

        public static string[] FilePaths => filePaths;

        public static string SavePath
        {
            get
            {
                title = (title == null) ? "Please select save location and file name" : title;
                using (SaveFileDialog svDlg = new SaveFileDialog()
                {
                    DefaultExt = "*",
                    Title = title,
                    InitialDirectory = DefaultDirectory,
                    FileName = DefaultFileName
                })
                {
                    if ((SaveResult = svDlg.ShowDialog(Grimoire.GUI.Main.Instance)) == DialogResult.OK)
                        return svDlg.FileName;
                }

                return null;
            }
        }

        public static string FolderPath
        {
            get
            {
                using (FolderBrowserDialog fbDlg = new FolderBrowserDialog()
                {
                    Description = description ?? "Please select desired folder"
                })
                {
                    if ((FolderResult = fbDlg.ShowDialog(Grimoire.GUI.Main.Instance)) == DialogResult.OK)
                        return Directory.Exists(fbDlg.SelectedPath) ? fbDlg.SelectedPath : null;
                }

                return null;
            }
        }
    }
}
