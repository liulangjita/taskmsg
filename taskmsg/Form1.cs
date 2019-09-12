using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace taskmsg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            refreshWaitDispatch();
        }

        private void refreshWaitDispatch()
        {
            if (listBox1.Items.Count > 0)
            {
                //清空所有项
                listBox1.Items.Clear();
            }
            FileSystemInfo[] fileinfoList = ToolMoveFiles.Director(ToolMoveFiles.GetRootPath());
            foreach (FileSystemInfo fsinfo in fileinfoList)
            {
                listBox1.Items.Add(fsinfo.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection selectItems = listBox1.SelectedItems;
            string waitselectItem = comboBox2.SelectedItem.ToString();
            for (int i = 0; i < selectItems.Count; i++)
            {
                string item = selectItems[i].ToString();
                string fromfilepath = ToolMoveFiles.GetRootPath() + "\\" + item;
                string toFilepath = ToolMoveFiles.GetWaitManagePath() + "\\" + waitselectItem;
                string result = ToolMoveFiles.CopyFile(fromfilepath, toFilepath);
                if (result.Length > 0)
                {
                    MessageBox.Show(result);
                }
            }
            refreshWaitDispatch();
        }
    }
}
