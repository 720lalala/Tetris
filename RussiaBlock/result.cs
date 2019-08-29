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
    public partial class result : Form
    {

        public result()
        {
            InitializeComponent();
        }
        public result(int player1result, int player2result)
        {
            InitializeComponent();
            this.player1.Text = "玩家一：" + player1result + "分";
            this.player2.Text = "玩家二：" + player2result + "分";
            if (player1result > player2result) label1.Text = "玩家一胜利！";
            else if (player1result < player2result) label1.Text = "玩家二胜利！";
            else label1.Text = "平局！！！";

        }
        private void result_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    MessageBox.Show("您按下了回车键");
                    break;
                case Keys.Space:
                    break;
                default:
                    break;
            }
        }
    }
}
