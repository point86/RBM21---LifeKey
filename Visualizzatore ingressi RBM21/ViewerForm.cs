using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Security.Policy;  //for evidence object
using System.Data.SQLite;
using RBM21_core;

namespace Visualizzatore_ingressi_RBM21
{
    public partial class ViewerForm : Form
    {       
        User[] users;
        DBmanager dbm;
        string databasePath;
        string cameFilePath;

        public ViewerForm(bool allowedChange, string databasePath, string cameFilePath)
        {
            InitializeComponent();
            this.databasePath = databasePath;
            this.cameFilePath = cameFilePath;

            //load files info in the status bar
            toolStripStatusLabel1.Text = databasePath.Replace(@"\\", "\\"); //if filepath is hardcoded, must remove the backslash;
            DateTime lastModified = System.IO.File.GetLastWriteTime(databasePath);
            toolStripStatusLabel2.Text = "Ultima modifica: " + lastModified.ToString("ddd dd MMM, HH:mm");

            button1.Enabled = allowedChange;
            if (allowedChange == false)
                toolTip1.SetToolTip(this.button1, "La sincronizzazione è disabilitata, controlla su \"Impostazioni\" nella schermata precedente");
            toolTip1.SetToolTip(this.button1, "Sincronizzazione");

            //load data
            dataLoader();
                     
            //last column width will fit the parent object.
           // usersListView.AutoResizeColumn(2,ColumnHeaderAutoResizeStyle.HeaderSize);
            entranceList.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        //load data into usersListView
        private void dataLoader()
        {         
            //get data from file
            dbm = new DBmanager(databasePath);
            FileReader fr= new FileReader(cameFilePath);
            users = fr.ParseFile().ToArray();
            //for every user in the camefile, search entrances in sqlite database
            foreach(User usr in users)
                usr.Entrances = dbm.GetEntrances(usr.Key);
            dbm.Close();           

            //display data
            usersListView.Items.Clear();
            foreach (User usr in users ?? Enumerable.Empty<User>())
            {
                string[] row = { usr.Nome, usr.Key };
                ListViewItem listViewItem = new ListViewItem(row);
                listViewItem.Tag = usr;
                usersListView.Items.Add(listViewItem);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void usersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //when user change selection, this method get called (with index == 0), so this had to be managed;
            if (usersListView.SelectedItems.Count == 0)
                return;

            //update userInfo to show info about the selected user
            ListView.SelectedListViewItemCollection lv = this.usersListView.SelectedItems;
            userInfo.Text = ((User)lv[0].Tag).ToVerticalString();

            User u = (User)lv[0].Tag;

            entranceList.Items.Clear();
            foreach (string l in u.Entrances ?? Enumerable.Empty<string>())
                entranceList.Items.Add(new ListViewItem(l));
         
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            string RBM21CorePath = AppDomain.CurrentDomain.BaseDirectory + "RBM21 core.exe";
            
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = RBM21CorePath;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            process.Start();
            process.WaitForExit();// Waits here for the process to exit.

            dataLoader();
            
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void userInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
        

        
    

