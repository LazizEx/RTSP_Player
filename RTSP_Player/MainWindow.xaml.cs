using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RTSP_Player
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Vlc.DotNet.Wpf.VlcControl vlcControl1;

        public MainWindow()
        {
            InitializeComponent();
            vlcControl1 = new Vlc.DotNet.Wpf.VlcControl();
            vlcControl1.MediaPlayer.BackColor = System.Drawing.Color.Red;
            vlcControl1.MediaPlayer.VlcLibDirectoryNeeded += MediaPlayer_VlcLibDirectoryNeeded;
            //vlcControl1.MediaPlayer.VlcLibDirectory = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.Environment.CurrentDirectory, "Vlc\\x86\\"));
            //vlcControl1.
            Grid.Children.Add(vlcControl1);
            vlcControl1.MediaPlayer.Play(new Uri("rtsp://10.195.155.216:8554/CH001.sdp"));
        }

        private void MediaPlayer_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"vlc\x86\"));
            //e.VlcLibDirectory = new DirectoryInfo(currentDirectory);
            else
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"vlc\x64\"));
            //e.VlcLibDirectory = new DirectoryInfo(currentDirectory);

            if (!e.VlcLibDirectory.Exists)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.Description = "Select Vlc libraries folder.";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vlcControl1 != null)
            {
                vlcControl1.MediaPlayer.Position = 1f;
                vlcControl1.MediaPlayer.Stop();
                Grid.Children.Remove(vlcControl1);
                GC.SuppressFinalize(vlcControl1);
                
                vlcControl1 = null;
                Application.Current.Shutdown();
                Environment.Exit(0);
                //vlcControl1.MediaPlayer.Dispose();
                //vlcControl1.Dispose();
                
            }
        }
    }
}
