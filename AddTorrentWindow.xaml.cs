using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Utorrents
{
    /// <summary>
    /// Interaction logic for AddTorrentWindow.xaml
    /// </summary>
    public partial class AddTorrentWindow : Window
    {
        public string _torrentPath;
        public string _downloadPath;
        public AddTorrentWindow()
        {
            InitializeComponent();
        }

        private void Browse_Click_Open(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog myDialog = new Microsoft.Win32.OpenFileDialog();
            myDialog.Filter = ".torrent files (*.torrent)|*.torrent";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                textBoxTorrentPath.Text = myDialog.FileName;
            }
        }

        private void Browse_Click_Save(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog();
            folderBrowseDialog.ShowDialog();
            textBoxDownloadPath.Text = folderBrowseDialog.SelectedPath;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            _torrentPath = textBoxTorrentPath.Text;
            _downloadPath = textBoxDownloadPath.Text;
            this.Close();
        }
    }
}
