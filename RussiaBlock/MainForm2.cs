using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;


namespace RussiaBlock
{
	public class MainForm2 : System.Windows.Forms.Form
	{
		#region 变量
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button music;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button exit;
		private System.Windows.Forms.Timer timer1;
        private Block block;      //玩家1方块实例 
        private Block block2;     //玩家2方块实例
        private Block nextBlock;  //玩家1下一基本块实例
        private Block nextBlock2; //玩家2下一基本块实例
        private int nextShapeNO;  //玩家1下一基本块形状号
        private int nextShapeNO2; //玩家2下一基本块形状号
        private bool paused;      //已暂停
        private DateTime atStart; //开始时间
        private DateTime atPause; //暂停时间
        private TimeSpan pauseTime;  //暂停间隔时间
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private AxWMPLib.AxWindowsMediaPlayer gameover2;
        private AxWMPLib.AxWindowsMediaPlayer uplevelmusic2;
        private AxWMPLib.AxWindowsMediaPlayer MediaPlayer2;
        private System.ComponentModel.IContainer components;
		private ControlForm2 sform;
		private Keys[] keys;		
		private int level;
		private int startLevel; // 开始级别
        private int rowDelNum;  //玩家1所消行数
        private int rowDelNum2; //玩家2所消行数
        private int DeductMark; //玩家1扣分
        private int DeductMarks2; //玩家2扣分
        private bool failed;				
        private int upgrade;    //用于升级操作   
        private int RememberRow;     //记录玩家1上一次所消行数 
        private int RememberRow2;    //记录玩家2上一次所消行数  
        private int score;           //玩家1分数
        private AxWMPLib.AxWindowsMediaPlayer sound;
        private Timer timer2;
        private int score2;           //玩家2分数
        #endregion
        public MainForm2()
		{
			InitializeComponent();
		}
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
        //初始化
        private void Initiate()
		{
			try 
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(@"setting2.ini");
				XmlNodeList nodes=doc.DocumentElement.ChildNodes;
				this.startLevel=Convert.ToInt32(nodes[0].InnerText);
				this.level=this.startLevel;				
				keys=new Keys[10];
				for(int i=0;i<nodes[1].ChildNodes.Count;i++)
				{
					KeysConverter kc=new KeysConverter();
					this.keys[i]=(Keys)(kc.ConvertFromString(nodes[1].ChildNodes[i].InnerText));
				}			
			}
            catch  //有问题则恢复“出厂装置”
            {
				keys=new Keys[10];
                keys[0] = Keys.A;
                keys[1] = Keys.D;
                keys[2] = Keys.S;      //快一点
                keys[3] = Keys.W;      //旋转
                keys[4] = Keys.Space;  //丢下
                keys[5] = Keys.Left;
				keys[6] = Keys.Right;
				keys[7] = Keys.Down;     //快一点
				keys[8] = Keys.Up;       //旋转
                keys[9] = Keys.NumPad0;  //丢下
                this.level=1;
				this.startLevel=1;
                
			}
			
			this.timer1.Interval=500-50*(level-1); //设置定时器的间隔
            this.label4.Text="级别： "+this.startLevel;
            this.timer2.Interval = 1;
            this.timer2.Enabled = true;
        }
        //游戏开始
        private void Start()                           //开始
		{
            KeyPreview = true;
            upgrade = 3;
            //初始化音效 
            MediaPlayer2.URL = Application.StartupPath + @"//双人对战.MP3";
            uplevelmusic2.URL = Application.StartupPath + @"//升级.MP3";
            gameover2.URL = Application.StartupPath + @"//结束.MP3";
            uplevelmusic2.Ctlcontrols.stop();
            gameover2.Ctlcontrols.stop();
            MediaPlayer2.Ctlcontrols.play();
            sound.Ctlcontrols.stop();
            upgrade = 3;
            //判断是否有开音效
            if (music.Text == "音效开")
            {

                MediaPlayer2.Ctlcontrols.stop();
            }
            else
            {
                MediaPlayer2.Ctlcontrols.play();

            }
            this.block=null;
            this.block2 = null;
            this.nextBlock=null;
            this.nextBlock2 = null;
            this.label3.Text="分数： 0";
            this.label13.Text = "分数： 0";
            this.label4.Text="级别： "+this.startLevel;
			this.level=this.startLevel;
            //方块生成的时间间隔
			this.timer1.Interval=500-50*(level-1);
            this.paused=false;
			this.failed=false;
            //画好后面四块黑色的画布
			this.panel1.Invalidate();
			this.panel2.Invalidate();
            this.panel4.Invalidate();
            this.panel5.Invalidate(); 
            this.nextShapeNO=0;
            this.nextShapeNO2 = 0;
            //生成当前方块
            this.CreateBlock();
            this.CreateBlock2();
            //生成下个方块
            this.CreateNextBlock();
            this.CreateNextBlock2();
            this.RememberRow = 0;
            this.RememberRow2 = 0;
            this.score = 0;
            this.score2 = 0;
            this.timer1.Enabled=true;
            this.timer2.Enabled = false;
            this.atStart=DateTime.Now;
			this.pauseTime=new TimeSpan(0);
		} 
        //失败
		private void Fail()   
		{
            //强制控件重绘
			this.panel1.Invalidate(new Rectangle(0,0,panel1.Width,100));
            this.panel4.Invalidate(new Rectangle(0, 0, panel4.Width, 100));
            //暂停背景音乐，并且在音效开的情况下播放结束音乐
            MediaPlayer2.Ctlcontrols.stop();
            if (music.Text == "音效开")
            {

                gameover2.Ctlcontrols.stop();
            }
            else
            {
                gameover2.Ctlcontrols.play();

            }
            KeyPreview = false;
            this.timer1.Enabled=false;
            this.timer2.Enabled = true;
            this.paused=true; //停顿
            //游戏结束后的传递参数
            result s1 = new result(score,score2);
            s1.Show();
        }
        //玩家1产生块
        private bool CreateBlock() 
		{
			Point firstPos;
			Color color;
			if(this.nextShapeNO==0)//没有满的时候
			{
				Random rand=new Random();		
				this.nextShapeNO=rand.Next(1,8);
			}
			switch(this.nextShapeNO)
			{
				case 1://田
					firstPos=new Point(4,0);
					color=Color.Cyan;
					break;
				case 2://一
					firstPos=new Point(3,0);
					color=Color.White;
					break;
				case 3://土
					firstPos=new Point(4,0);
					color=Color.Chartreuse;
					break;
				case 4://z
					firstPos=new Point(4,0);
					color=Color.Crimson;
					break;
				case 5://倒z
					firstPos=new Point(4,1);
					color=Color.Yellow;
					break;
				case 6://L
					firstPos=new Point(4,0);
					color=Color.BlueViolet;
					break;
				default://倒L
					firstPos=new Point(4,0);
					color=Color.OrangeRed;
					break;
			} 
            //玩家1产生新块
			if(this.block==null)
			{
				block=new Block(this.panel1,9,19,25,this.nextShapeNO,firstPos,color);
			}
			else
			{
				if(!block.GeneBlock(this.nextShapeNO,firstPos,color)) 
				{
					return false;
				}
			}
			block.EraseLast();
			block.Move(2);
			return true;
		}
        //玩家2产生块
        private bool CreateBlock2() 
        {
            Point firstPos2;
            Color color2;
            if (this.nextShapeNO2 == 0)//没有满的时候
            {
                Random rand2 = new Random();
                this.nextShapeNO2 = rand2.Next(1, 8);
            }
            switch (this.nextShapeNO2)
            {
                case 1://田
                    firstPos2 = new Point(4, 0);
                    color2 = Color.Cyan;
                    break;
                case 2://一
                    firstPos2 = new Point(3, 0);
                    color2 = Color.White;
                    break;
                case 3://土
                    firstPos2 = new Point(4, 0);
                    color2 = Color.Chartreuse;
                    break;
                case 4://z
                    firstPos2 = new Point(4, 0);
                    color2 = Color.Crimson;
                    break;
                case 5://倒z
                    firstPos2 = new Point(4, 1);
                    color2 = Color.Yellow;
                    break;
                case 6://L
                    firstPos2 = new Point(4, 0);
                    color2 = Color.BlueViolet;
                    break;
                default://倒L
                    firstPos2 = new Point(4, 0);
                    color2 = Color.OrangeRed;
                    break;
            }
            //玩家2产生新块
            if (this.block2 == null) 
            {
                block2 = new Block(this.panel4, 9, 19, 25, this.nextShapeNO2, firstPos2, color2);
            }
            else
            {
                if (!block2.GeneBlock(this.nextShapeNO2, firstPos2, color2)) 
                {
                    return false;
                }
            }
            block2.EraseLast();
            block2.Move(2);
            return true;
        }
        //玩家1生成下一块
        private void CreateNextBlock() 
		{
			Random rand=new Random();		
			this.nextShapeNO=rand.Next(1,8);
			Point firstPos;
			Color color;
			switch(this.nextShapeNO)
			{
				case 1://田
					firstPos=new Point(1,0);
					color=Color.Cyan;
					break;
				case 2://一
					firstPos=new Point(0,1);
					color=Color.White;
					break;
				case 3://土
					firstPos=new Point(0,0);
					color=Color.Chartreuse;
					break;
				case 4://z
					firstPos=new Point(0,0);
					color=Color.Crimson;
					break;
				case 5://倒z
					firstPos=new Point(0,1);
					color=Color.Yellow;
					break;
				case 6://L
					firstPos=new Point(0,0);
					color=Color.BlueViolet;
					break;
				default://倒L
					firstPos=new Point(0,0);
					color=Color.Red;
					break;
			}
			if(nextBlock==null)
				nextBlock=new Block(this.panel2,3,1,20,this.nextShapeNO,firstPos,color);
			else
			{
				nextBlock.GeneBlock(this.nextShapeNO,firstPos,color);
				nextBlock.EraseLast();
			}
		}
        //玩家2生成下一块
        private void CreateNextBlock2() 
        {
            Random rand2 = new Random();
            this.nextShapeNO2 = rand2.Next(1, 8);
            Point firstPos2;
            Color color2;
            switch (this.nextShapeNO2)
            {
                case 1://田
                    firstPos2 = new Point(1, 0);
                    color2 = Color.Cyan;
                    break;
                case 2://一
                    firstPos2 = new Point(0, 1);
                    color2 = Color.White;
                    break;
                case 3://土
                    firstPos2 = new Point(0, 0);
                    color2 = Color.Chartreuse;
                    break;
                case 4://z
                    firstPos2 = new Point(0, 0);
                    color2 = Color.Crimson;
                    break;
                case 5://倒z
                    firstPos2 = new Point(0, 1);
                    color2 = Color.Yellow;
                    break;
                case 6://L
                    firstPos2 = new Point(0, 0);
                    color2 = Color.BlueViolet;
                    break;
                default://倒L
                    firstPos2 = new Point(0, 0);
                    color2 = Color.Red;
                    break;
            }
            if (nextBlock2 == null)
                nextBlock2 = new Block(this.panel5, 3, 1, 20, this.nextShapeNO2, firstPos2, color2);
            else
            {
                nextBlock2.GeneBlock(this.nextShapeNO2, firstPos2, color2);
                nextBlock2.EraseLast();
            }
        }
        //玩家1填补方块视图并建立新块
        private void FixAndCreate() 
		{
			block.FixBlock();
            //如果消行了进行加分数操作
            score = score + (block.RowDelNum - RememberRow) * 100;
            this.label3.Text="分数： "+score;
            //扣分操作
            if(block.RowDelNum-RememberRow>=3)
            {
                score2 = score2 - 200;
                this.label13.Text = "分数： " + score2;
            }
            //升级操作
            if(upgrade* (this.level)<= block.RowDelNum && this.level < 10)
            {
                upgrade++;
                this.level++;
                this.timer1.Interval = 500 - 50 * (level - 1);
                this.label4.Text = "级别：  " + this.level;
                uplevelmusic2.Ctlcontrols.stop();
                if (music.Text == "音效开")
                {

                    uplevelmusic2.Ctlcontrols.stop();
                }
                else
                {
                    uplevelmusic2.Ctlcontrols.play();

                }
            }
			if(this.level<10 && block.RowDelNum-this.rowDelNum>=30)
			{
				this.rowDelNum+=30;
				this.level++;
				this.timer1.Interval=500-50*(level-1);
                this.label4.Text="级别：  "+this.level;
			}
            RememberRow = block.RowDelNum;
            bool createOK=this.CreateBlock();
			this.CreateNextBlock();				
			if(!createOK)
				this.Fail();
		}
        //玩家2填补方块视图并建立新块
        private void FixAndCreate2()
        {
            block2.FixBlock();
            this.label13.Text = "行数： " + block2.RowDelNum;
            score2 = score2 + (block2.RowDelNum - RememberRow2) * 100;
            this.label13.Text = "分数： " + score2;
            //扣分操作
            if (block2.RowDelNum - RememberRow2 >= 3)
            {
                score = score - 200;
                this.label13.Text = "分数： " + score;
            }
            //升级操作
            if (upgrade * (this.level) <= block2.RowDelNum && this.level < 10)
            {
                upgrade++;
                this.level++;
                this.timer1.Interval = 500 - 50 * (level - 1);
                this.label4.Text = "级别：  " + this.level;
                if (music.Text == "音效开")
                {

                    uplevelmusic2.Ctlcontrols.stop();
                }
                else
                {
                    uplevelmusic2.Ctlcontrols.play();

                }
            }
            if (this.level < 10 && block2.RowDelNum - this.rowDelNum2 >= 30)
            {
                this.rowDelNum2 += 30;
                this.level++;
                this.timer1.Interval = 500 - 50 * (level - 1);
                this.label4.Text = "级别：  " + this.level;
            }
            bool createOK2 = this.CreateBlock2();
            this.RememberRow2 = block2.RowDelNum;
            this.CreateNextBlock2();
            if (!createOK2)
                this.Fail();
        }
        //保存设置
        private void SaveSetting()
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				XmlDeclaration xmlDec=doc.CreateXmlDeclaration ("1.0","gb2312",null);

				XmlElement setting=doc.CreateElement("SETTING");
				doc.AppendChild(setting);

				XmlElement level=doc.CreateElement("LEVEL");
				level.InnerText=this.startLevel.ToString();
				setting.AppendChild(level);
                	
				XmlElement keys=doc.CreateElement("KEYS");    
				setting.AppendChild(keys);
				foreach(Keys k in this.keys)
				{
					KeysConverter kc=new KeysConverter();	
					XmlElement x=doc.CreateElement("SUBKEYS");
					x.InnerText=kc.ConvertToString(k);
					keys.AppendChild(x);
				}
				XmlElement root=doc.DocumentElement;
				doc.InsertBefore(xmlDec,root);
				doc.Save(@"setting2.ini");
			}
			catch(Exception xe)
			{
				MessageBox.Show(xe.Message);
			}
		}

		#region Windows 窗体设计器生成的代码
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.help = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.gameover2 = new AxWMPLib.AxWindowsMediaPlayer();
            this.uplevelmusic2 = new AxWMPLib.AxWindowsMediaPlayer();
            this.MediaPlayer2 = new AxWMPLib.AxWindowsMediaPlayer();
            this.music = new System.Windows.Forms.Button();
            this.sound = new AxWMPLib.AxWindowsMediaPlayer();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameover2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uplevelmusic2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sound)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCoral;
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 501);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Location = new System.Drawing.Point(14, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(86, 53);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Salmon;
            this.label3.Font = new System.Drawing.Font("幼圆", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(265, 187);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(83, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "分数：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("华文细黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(392, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 39);
            this.button1.TabIndex = 10;
            this.button1.Text = "开始";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("华文细黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(392, 365);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 39);
            this.button3.TabIndex = 7;
            this.button3.Text = "设置";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Tomato;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Location = new System.Drawing.Point(272, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(112, 72);
            this.panel3.TabIndex = 9;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(610, 457);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(1, 21);
            this.textBox1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightSalmon;
            this.label4.Font = new System.Drawing.Font("幼圆", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(388, 115);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(83, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "级别：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Salmon;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(286, 486);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 23);
            this.label5.TabIndex = 12;
            this.label5.Text = "火灾请打119";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            this.label5.MouseEnter += new System.EventHandler(this.label5_MouseEnter);
            this.label5.MouseLeave += new System.EventHandler(this.label5_MouseLeave);
            // 
            // help
            // 
            this.help.BackColor = System.Drawing.Color.White;
            this.help.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.help.ForeColor = System.Drawing.Color.Firebrick;
            this.help.Location = new System.Drawing.Point(539, 457);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(72, 23);
            this.help.TabIndex = 17;
            this.help.Text = "help";
            this.help.UseVisualStyleBackColor = false;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.Color.White;
            this.exit.ForeColor = System.Drawing.Color.Firebrick;
            this.exit.Location = new System.Drawing.Point(539, 486);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(72, 23);
            this.exit.TabIndex = 19;
            this.exit.Text = "回到主页面";
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightCoral;
            this.panel4.Location = new System.Drawing.Point(629, 8);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(251, 501);
            this.panel4.TabIndex = 23;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Tomato;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Location = new System.Drawing.Point(502, 8);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(112, 72);
            this.panel6.TabIndex = 24;
            this.panel6.Paint += new System.Windows.Forms.PaintEventHandler(this.panel6_Paint);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Location = new System.Drawing.Point(14, 12);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(86, 53);
            this.panel5.TabIndex = 10;
            this.panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.panel5_Paint);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Salmon;
            this.label13.Font = new System.Drawing.Font("幼圆", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(476, 187);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(2);
            this.label13.Size = new System.Drawing.Size(83, 25);
            this.label13.TabIndex = 25;
            this.label13.Text = "分数：";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gameover2
            // 
            this.gameover2.Enabled = true;
            this.gameover2.Location = new System.Drawing.Point(392, 234);
            this.gameover2.Name = "gameover2";
            this.gameover2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gameover2.OcxState")));
            this.gameover2.Size = new System.Drawing.Size(0, 0);
            this.gameover2.TabIndex = 28;
            this.gameover2.Enter += new System.EventHandler(this.axWindowsMediaPlayer2_Enter);
            // 
            // uplevelmusic2
            // 
            this.uplevelmusic2.Enabled = true;
            this.uplevelmusic2.Location = new System.Drawing.Point(396, 189);
            this.uplevelmusic2.Name = "uplevelmusic2";
            this.uplevelmusic2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("uplevelmusic2.OcxState")));
            this.uplevelmusic2.Size = new System.Drawing.Size(0, 0);
            this.uplevelmusic2.TabIndex = 29;
            // 
            // MediaPlayer2
            // 
            this.MediaPlayer2.Enabled = true;
            this.MediaPlayer2.Location = new System.Drawing.Point(408, 178);
            this.MediaPlayer2.Name = "MediaPlayer2";
            this.MediaPlayer2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MediaPlayer2.OcxState")));
            this.MediaPlayer2.Size = new System.Drawing.Size(0, 0);
            this.MediaPlayer2.TabIndex = 30;
            // 
            // music
            // 
            this.music.BackColor = System.Drawing.Color.White;
            this.music.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.music.ForeColor = System.Drawing.Color.Firebrick;
            this.music.Location = new System.Drawing.Point(539, 428);
            this.music.Name = "music";
            this.music.Size = new System.Drawing.Size(72, 23);
            this.music.TabIndex = 31;
            this.music.Text = "音效关";
            this.music.UseVisualStyleBackColor = false;
            this.music.Click += new System.EventHandler(this.music_Click);
            // 
            // sound
            // 
            this.sound.Enabled = true;
            this.sound.Location = new System.Drawing.Point(518, 337);
            this.sound.Name = "sound";
            this.sound.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("sound.OcxState")));
            this.sound.Size = new System.Drawing.Size(0, 0);
            this.sound.TabIndex = 33;
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // MainForm2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(892, 517);
            this.Controls.Add(this.sound);
            this.Controls.Add(this.music);
            this.Controls.Add(this.MediaPlayer2);
            this.Controls.Add(this.uplevelmusic2);
            this.Controls.Add(this.gameover2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.help);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "MainForm2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "俄罗斯方块";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gameover2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uplevelmusic2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sound)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Start();
			this.textBox1.Focus();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.textBox1.Focus();
		}
        //按键操作
        private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(block!=null && this.paused==false && !this.failed&&block2!=null)
			{
				if(e.KeyCode==this.keys[0])
				{
					if(block.Move(0))
					{
						block.EraseLast();
					}
				}
				else if(e.KeyCode==this.keys[1])
				{
					if(block.Move(1))
					{
						block.EraseLast();
					}
				}
				else if(e.KeyCode==this.keys[2])
				{
					if(!block.Move(2))
					{
						this.FixAndCreate();
					}
					else
					{
						block.EraseLast();
					}
				}
				else if(e.KeyCode==this.keys[3])
				{
					if(block.Rotate())
					{
						block.EraseLast();
					}
				}
				else if(e.KeyCode==this.keys[4])
				{
					block.Drop();
					block.EraseLast();
					this.FixAndCreate();
				}
                else if (e.KeyCode == this.keys[5])
                {
                    if (block2.Move(0))
                    {
                        block2.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[6])
                {
                    if (block2.Move(1))
                    {
                        block2.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[7])
                {
                    if (!block2.Move(2))
                    {
                        this.FixAndCreate2();
                    }
                    else
                    {
                        block2.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[8])
                {
                    if (block2.Rotate())
                    {
                        block2.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[9])
                {
                    block2.Drop();
                    block2.EraseLast();
                    this.FixAndCreate2();
                }

            }
		}
        // 玩家1游戏窗格绘制
        private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(block!=null)
			{
				block.DrawBlocks(e.ClipRectangle);
			}
			if(this.failed)
			{
				Graphics gra=e.Graphics;
				gra.DrawString("Game Over",new Font("Arial Black",25f),new SolidBrush(Color.Gray),30,30);
			}
		}
        // 玩家2游戏窗格绘制
        private void panel4_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (block2!= null)
            {
                block2.DrawBlocks(e.ClipRectangle);
            }
            if (this.failed)
            {
                Graphics gra2 = e.Graphics;
                gra2.DrawString("Game Over", new Font("Arial Black", 25f), new SolidBrush(Color.Gray), 30, 30);
            }
        }
        //过一段时间就执行一次
        private void timer1_Tick(object sender, System.EventArgs e)
		{
			if(block!=null && !this.failed&&block2 != null )
			{
                if((int)MediaPlayer2.playState==1&& music.Text == "音效关")
                {
                    MediaPlayer2.Ctlcontrols.play();
                }
				if(!block.Move(2))
				{
					this.FixAndCreate();
				}	
				else
				{
					block.EraseLast();
				}
                if (!block2.Move(2))
                {
                    this.FixAndCreate2();
                }
                else
                {
                    block2.EraseLast();
                }
            }
		}

        // 玩家1下一块窗格绘制
        private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{		
			if(nextBlock!=null)
			{
				nextBlock.DrawBlocks(e.ClipRectangle);
			}
		}
        // 玩家2下一块窗格绘制
        private void panel5_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (nextBlock2 != null)
            {
                nextBlock2.DrawBlocks(e.ClipRectangle);
            }
        }
        // 设置环境
        private void button3_Click(object sender, System.EventArgs e)
		{
            if (!paused)
            {
                this.atPause = DateTime.Now;
                this.paused = true;
                this.timer1.Stop();
            }
            sform = new ControlForm2();
            sform.SetOptions(this.keys, this.startLevel);
            sform.DialogResult = DialogResult.Cancel;

            sform.ShowDialog();
            if (sform.DialogResult == DialogResult.OK)
            {
                sform.GetOptions(ref this.keys, ref this.startLevel);
                this.level = this.startLevel;
                this.label4.Text = "级别： " + this.level;
                this.timer1.Interval = 500 - 50 * (level - 1);
            }
            this.paused = false;
            this.pauseTime += DateTime.Now - this.atPause;
            this.timer1.Start();
            this.textBox1.Focus();
        }
        //加载时操作
        private void MainForm_Load(object sender, System.EventArgs e)
		{
            sound.URL = Application.StartupPath + @"//加载时音乐.MP3";
            this.Initiate();
            this.BackgroundImage = Image.FromFile(@"33.jpg");
           
        }
        //关闭游戏
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            SaveSetting();
            
             Application.Exit();
        }

        private void label5_MouseEnter(object sender, System.EventArgs e)
        {
            this.label5.Text = "16网络班119制作";
        }

        private void label5_MouseLeave(object sender, System.EventArgs e)
        {
            this.label5.Text = "火灾请打119";
        }
	
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void menuItem1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void exit_Click(object sender, EventArgs e)
        {
            SaveSetting();
            this.Dispose();
            this.Close();
            MainForm0.eMainForm0.Show();     
        }

        private void axWindowsMediaPlayer2_Enter(object sender, EventArgs e)
        {

        }

        private void music_Click(object sender, EventArgs e)
        {
            if (music.Text == "音效开")
            {
                if(timer1.Enabled==true)
                MediaPlayer2.Ctlcontrols.play();
                music.Text = "音效关";
                textBox1.Focus();
            }
            else
            {
                uplevelmusic2.Ctlcontrols.stop();
                gameover2.Ctlcontrols.stop();
                MediaPlayer2.Ctlcontrols.stop();
                sound.Ctlcontrols.stop();
                music.Text = "音效开";
                textBox1.Focus();
            }
        }
        //回到主页面
        private void help_Click(object sender, EventArgs e)
        {
            help help1 = new help();
            help1.Show();
        }
        //循环播放加载音效
        private void timer2_Tick(object sender, EventArgs e)
        {
            if ((int)sound.playState == 1 && music.Text == "音效关")
                sound.Ctlcontrols.play();
            else if (music.Text == "音效开")
                sound.Ctlcontrols.stop();
        }
    }
}