﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Grimoire.Utilities;

namespace Grimoire.GUI
{
    public partial class MessageListBox : Form
    {
        XmlManager xMan = XmlManager.Instance;

        public MessageListBox(string title, string msg, string[] filePaths)
        {
            InitializeComponent();
            Text = title;
            this.msg.Text = msg;
            populateList(filePaths);
            //localize();
        }

        private void populateList(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                list.Items.Add(Path.GetFileName(filePath));
            }
        }

        private void yes_btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void no_btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void localize()
        {
            xMan.Localize(this, Localization.Enums.SenderType.GUI);
        }
    }
}
