﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RBM21_core;

namespace Visualizzatore_ingressi_RBM21
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            SettingsManager sm = new SettingsManager();
            using (ViewerForm newForm = new ViewerForm(true, sm.SQLiteDB, sm.CameFilePath))           
            {
                newForm.ShowDialog(this);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SettingsForm newForm = new SettingsForm())
            {
                newForm.ShowDialog(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Apri database ingressi RBM21 (sola lettura)";
            fd.Filter = "RBM21 database (*.sqlite)|*.sqlite|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string sFileName = fd.FileName;
                SettingsManager sm = new SettingsManager();
                using (ViewerForm newForm = new ViewerForm(false, fd.FileName, sm.CameFilePath))
                {
                    newForm.ShowDialog(this);
                }
            }
            
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
