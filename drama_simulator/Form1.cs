using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace drama_simulator
{
    public partial class Form1 : Form
    {
        List<string> filteredFiles = new List<string>();
        FolderBrowserDialog browserDialog = new FolderBrowserDialog();
        int currentFile = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadFolderEvent(object sender, EventArgs e)
        {
            VideoPlayer.Ctlcontrols.stop();

            if(filteredFiles.Count > 1)
            {
                filteredFiles.Clear();
                filteredFiles = null;
                PlayList.Items.Clear();

                currentFile = 0;
            }

            DialogResult result = browserDialog.ShowDialog();

            if ( result == DialogResult.OK)
            {
                filteredFiles = Directory.GetFiles(browserDialog.SelectedPath, "*.*").Where(file => file.ToLower().EndsWith("webm")
                || file.ToLower().EndsWith("mp4") 
                || file.ToLower().EndsWith("wmv") 
                || file.ToLower().EndsWith("mkv") 
                || file.ToLower().EndsWith("avi")).ToList();

                LoadPlayList();
            }
        }

        private void ShowAboutEvent(object sender, EventArgs e)
        {
            MessageBox.Show("This app is made by mingde wu" + Environment.NewLine + "Hope you can enjoying yourself");
        }

        private void MediaPlayerStateChangeEvent(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if(e.newState == 0)
            {
                lblDuration.Text = "Media Player is Ready to be loaded";
            }
            else if(e.newState ==1)
            {
                lblDuration.Text = "Media Player Stopped";
            }
            else if(e.newState ==3)
            {
                lblDuration.Text = "Duration:" + VideoPlayer.currentMedia.durationString;
            }
            else if(e.newState==8)
            {
                if (currentFile>= filteredFiles.Count-1)
                {
                    currentFile = 0;
                }
                else
                {
                    currentFile += 1;
                }
                PlayList.SelectedIndex = currentFile;

                ShowFileName(FileName);
            }

        }

        private void PlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFile = PlayList.SelectedIndex;
            PlayFile(PlayList.SelectedItem.ToString());
            ShowFileName(FileName);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            VideoPlayer.Ctlcontrols.play();
            timer1.Stop();
        }

        private void LoadPlayList() 
        {
            VideoPlayer.currentPlaylist = VideoPlayer.newPlaylist("PlayList", "");

            foreach(String videos in filteredFiles)
            {
                VideoPlayer.currentPlaylist.appendItem(VideoPlayer.newMedia(videos));
                PlayList.Items.Add(videos);
            }

            if (filteredFiles.Count > 0) 
            {
                FileName.Text = "Files Found" + filteredFiles.Count;

                PlayList.SelectedIndex = currentFile;

                PlayFile(PlayList.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("No Video Files Found in this folder");
            }
        }

        private void PlayFile(String url)
        {
            VideoPlayer.URL = url;
        }
        private void ShowFileName(Label name)
        {
            String file = Path.GetFileName(PlayList.SelectedItem.ToString());
            name.Text = "Currently Playing:" + file;
        }
    }
}
