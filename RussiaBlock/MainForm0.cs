using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RussiaBlock
{
    public partial class MainForm0 : Form
    {
        public static MainForm0 eMainForm0 = null;
        public MainForm0()
        {
            eMainForm0 = this;
            InitializeComponent();
            this.timer1.Start();
        }
        //单击单人模式所执行的操作
        private void single_Click(object sender, EventArgs e)
        {
            music0.Ctlcontrols.stop();
            timer1.Stop();
            MainForm SingleGame = new MainForm();//首先实例化
            SingleGame.Show();                   //Show方法
            this.Hide();                         //隐藏窗口
        }
        //单击对战模式所执行的操作
        private void couple_Click(object sender, EventArgs e)
        {
            music0.Ctlcontrols.stop();
            timer1.Stop();
            MainForm2 CoupleGame = new MainForm2();//首先实例化
              CoupleGame.Show();                   //Show方法
            this.Hide();                           //隐藏窗口
        }
        private void MainForm0_Load(object sender, EventArgs e)
        {
            music0.URL = Application.StartupPath + @"//单人对战背景音乐.MP3";
            music0.Ctlcontrols.play();
            this.timer1.Start();
            this.timer1.Interval =1;
        }
        //设置音乐开关
        private void music_Click(object sender, EventArgs e) 
        {
            if (music.Text == "1")
            {
                music0.Ctlcontrols.stop();
                music.Text ="0";
            }
            else
            {
                
                music0.Ctlcontrols.play();
                music.Text = "1";
            }

        }
        //设置帮助开关
        private void help3_Click(object sender, EventArgs e)
        {
            help help1 = new help();
            help1.Show();
        }
        //循环播放音乐
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((int)music0.playState == 1 && music.Text == "1")
            {
                music0.Ctlcontrols.play();
            }
        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

        }
    }
}
