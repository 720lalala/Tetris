using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using  System.Runtime.Serialization.Formatters.Binary;


namespace RussiaBlock
{
    public class MainForm : System.Windows.Forms.Form
    {
        #region ����
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button music;       
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private AxWMPLib.AxWindowsMediaPlayer MediaPlayer1;
        private AxWMPLib.AxWindowsMediaPlayer deleterow;
        private AxWMPLib.AxWindowsMediaPlayer uplevelmusic;
        private AxWMPLib.AxWindowsMediaPlayer gameover;
        private System.ComponentModel.IContainer components;
        private Block block;       // ����ʵ�� 
        private Block nextBlock;   // ��һ������ʵ��
        private int nextShapeNO;   // ��һ��������״��
        private bool paused;       //����ͣ
        private DateTime atStart;  //��ʼʱ�� 
        private DateTime atPause;  //��ͣʱ��
        private TimeSpan pauseTime;//��ͣ���ʱ��        
        private int level;         
        private ControlForm sform;
        private Keys[] keys;
        private int startLevel;    // ��ʼ����
        private int rowDelNum;     //��������
        private bool failed; 
        private Button help;
        private double speed;
        private Button exit;
        private int upgrade;        //������������
        private Label top;
        private int rememberrow;    //��¼��һ����������
        private AxWMPLib.AxWindowsMediaPlayer sound;
        private Timer timer2;
        private int toprow;
        #endregion
        public MainForm()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        //��ʼ��
        private void Initiate()
        {
            //�򿪵�ʱ�����setting.ini
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"setting.ini");
                XmlNodeList nodes = doc.DocumentElement.ChildNodes;
                this.startLevel = Convert.ToInt32(nodes[0].InnerText);
                this.toprow = Convert.ToInt32(nodes[1].InnerText);
                this.level = this.startLevel;
                keys = new Keys[6];
                for (int i = 0; i < nodes[2].ChildNodes.Count; i++)
                {
                    KeysConverter kc = new KeysConverter();
                    this.keys[i] = (Keys)(kc.ConvertFromString(nodes[2].ChildNodes[i].InnerText));
                }
            }
            catch //��������ָ�������װ�á�
            {
                keys = new Keys[6];
                keys[0] = Keys.Left;
                keys[1] = Keys.Right;
                keys[2] = Keys.Down;    //��һ��
                keys[3] = Keys.Up;      //��ת
                keys[4] = Keys.Space;   //����
                keys[5] = Keys.S;       //��ͣ
                this.level = 1;
                this.startLevel = 1;
                this.toprow = 0; ;

            }

            this.timer1.Interval = 500 - 50 * (level - 1);   //���ö�ʱ���ļ��
            this.label4.Text = "���� " + this.startLevel;  //����ԭʼ����
            this.timer2.Interval = 1;
            this.timer2.Enabled = true;
        }
        //��Ϸ��ʼ
        private void Start()                           //��ʼ
        {
            //��ʼ����Ч            
            MediaPlayer1.URL = Application.StartupPath + @"//���˶�ս��������.MP3";
            deleterow.URL = Application.StartupPath + @"//����.MP3";
            uplevelmusic.URL = Application.StartupPath + @"//����.MP3";
            gameover.URL = Application.StartupPath + @"//����.MP3";
            deleterow.Ctlcontrols.stop();
            uplevelmusic.Ctlcontrols.stop();
            gameover.Ctlcontrols.stop();
            sound.Ctlcontrols.stop();
            //�ж��Ƿ��п���Ч
            if (music.Text == "��Ч��")
            {

                MediaPlayer1.Ctlcontrols.stop();
            }
            else
            {
                MediaPlayer1.Ctlcontrols.play();

            }
            upgrade = 3;
            this.block = null;
            this.nextBlock = null;
            this.label1.Text = "������\n�������\n�Ѿ���Խ��\n0%����";
            this.label3.Text = "������ 0";
            this.label4.Text = "���� " + this.startLevel;
            this.top.Text = "�������\n" + toprow;
            this.level = this.startLevel;
            //�������ɵ�ʱ����
            this.timer1.Interval = 500 - 50 * (level - 1);
            this.paused = false;
            this.failed = false;
            //���ú��������ɫ�Ļ���
            this.panel1.Invalidate();
            this.panel2.Invalidate(); 
            this.nextShapeNO = 0;
            //���ɵ�ǰ����
            this.CreateBlock();
            //�����¸�����
            this.CreateNextBlock();
            //Timer.Enabled=False����Timer������,Ҳ������Timerֹͣ��ʱ
            this.timer1.Enabled = true;
            this.timer2.Enabled = false;
            this.atStart = DateTime.Now;
            this.pauseTime = new TimeSpan(0);
        }
        //��Ϸ����
        private void Fail()    
        {
            this.failed = true;
            //ǿ�ƿؼ��ػ�
            this.panel1.Invalidate(new Rectangle(0, 0, panel1.Width, 100));
            this.timer1.Enabled = false;
            this.timer2.Enabled = true;
            this.paused = true; 
            //��ͣ�������֣���������Ч��������²��Ž�������
            MediaPlayer1.Ctlcontrols.stop();
            if (music.Text == "��Ч��")
            {

                gameover.Ctlcontrols.stop();
            }
            else
            {
                gameover.Ctlcontrols.play();

            }
        }
        //������
        private bool CreateBlock()
        {
            Point firstPos;
            Color color;
            //û������ʱ�򣬲��������
            if (this.nextShapeNO == 0)
            {
                Random rand = new Random();
                this.nextShapeNO = rand.Next(1, 8);
            }
            //���������������ķ���
            switch (this.nextShapeNO)
            {
                case 1://��
                    firstPos = new Point(4, 0);
                    color = Color.Cyan;
                    break;
                case 2://һ
                    firstPos = new Point(3, 0);
                    color = Color.White;
                    break;
                case 3://��
                    firstPos = new Point(4, 0);
                    color = Color.Chartreuse;
                    break;
                case 4://z
                    firstPos = new Point(4, 0);
                    color = Color.Crimson;
                    break;
                case 5://��z
                    firstPos = new Point(4, 1);
                    color = Color.Yellow;
                    break;
                case 6://L
                    firstPos = new Point(4, 0);
                    color = Color.BlueViolet;
                    break;
                default://��L
                    firstPos = new Point(4, 0);
                    color = Color.OrangeRed;
                    break;
            } 
            //�����¿�
            if (this.block == null)
            {
                block = new Block(this.panel1, 9, 19, 25, this.nextShapeNO, firstPos, color);
            }
            else
            {
                if (!block.GeneBlock(this.nextShapeNO, firstPos, color)) 
                {
                    return false;
                }
            }
            block.EraseLast();
            block.Move(2);
            return true;
        } 
        //������һ��
        private void CreateNextBlock()
        {
            Random rand = new Random();
            this.nextShapeNO = rand.Next(1, 8);
            Point firstPos;
            Color color;
            switch (this.nextShapeNO)
            {
                case 1://��
                    firstPos = new Point(1, 0);
                    color = Color.Cyan;
                    break;
                case 2://һ
                    firstPos = new Point(0, 1);
                    color = Color.White;
                    break;
                case 3://��
                    firstPos = new Point(0, 0);
                    color = Color.Chartreuse;
                    break;
                case 4://z
                    firstPos = new Point(0, 0);
                    color = Color.	Crimson;
                    break;
                case 5://��z
                    firstPos = new Point(0, 1);
                    color = Color.Yellow;
                    break;
                case 6://L
                    firstPos = new Point(0, 0);
                    color = Color.BlueViolet;
                    break;
                default://��L
                    firstPos = new Point(0, 0);
                    color = Color.Red;
                    break;
            }
            if (nextBlock == null)
                nextBlock = new Block(this.panel2, 3, 1, 20, this.nextShapeNO, firstPos, color);
            else
            {
                nextBlock.GeneBlock(this.nextShapeNO, firstPos, color);
                nextBlock.EraseLast();
            }
        }
        // �������ͼ�������¿�
        private void FixAndCreate()
        {
            block.FixBlock();
            speed = 150 * Math.Round((double)block.BlockNum / ((TimeSpan)(DateTime.Now - this.atStart)).Subtract(this.pauseTime).TotalSeconds, 3);
            if (speed >= 99) speed = 99;
            this.label1.Text = "������\n�������\n�Ѿ�����\n" + speed + "%����";
            this.label3.Text = "������ " + block.RowDelNum;
            //��������
            if (upgrade * (this.level) <= block.RowDelNum && this.level < 10)
            {
                upgrade++;
                this.level++;
                this.timer1.Interval = 500 - 50 * (level - 1);
                this.label4.Text = "����  " + this.level;
                //����Ч����ʱ�򲥷�����
                uplevelmusic.Ctlcontrols.stop();
                if (music.Text == "��Ч��")
                {

                    uplevelmusic.Ctlcontrols.stop();
                }
                else
                {
                    uplevelmusic.Ctlcontrols.play();

                }
            }
            //��������
            else if (rememberrow != block.RowDelNum && block.RowDelNum != 0)
            {
                deleterow.Ctlcontrols.stop();
                if (music.Text == "��Ч��")
                {

                    deleterow.Ctlcontrols.stop();
                }
                else
                {
                    deleterow.Ctlcontrols.play();

                }
               // deleterow.Ctlcontrols.play();
            }
            rememberrow = block.RowDelNum;
            if (this.level < 10 && block.RowDelNum - this.rowDelNum >= 30)
            {
                this.rowDelNum += 30;
                this.level++;
                this.timer1.Interval = 500 - 50 * (level - 1);
                this.label4.Text = "����  " + this.level;
            }
            //����Ƽ�¼ �Ǿͼ�¼�ֽ��������
            if(block.RowDelNum>toprow)
            {
                toprow = block.RowDelNum;
                this.top.Text = "�������\n" + toprow;
            }
            bool createOK = this.CreateBlock();
            this.CreateNextBlock();
            if (!createOK)
                this.Fail();
        }
        //��������
        private void SaveSetting()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "gb2312", null);

                XmlElement setting = doc.CreateElement("SETTING");
                doc.AppendChild(setting);

                XmlElement level = doc.CreateElement("LEVEL");
                level.InnerText = this.startLevel.ToString();
                setting.AppendChild(level);

                XmlElement top = doc.CreateElement("TOP");
                top.InnerText = this.toprow.ToString();
                setting.AppendChild(top);

                XmlElement keys = doc.CreateElement("KEYS");
                setting.AppendChild(keys);
                foreach (Keys k in this.keys)
                {
                    KeysConverter kc = new KeysConverter();
                    XmlElement x = doc.CreateElement("SUBKEYS");
                    x.InnerText = kc.ConvertToString(k);
                    keys.AppendChild(x);
                }

                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDec, root);
                doc.Save(@"setting.ini");
            }
            catch (Exception xe)
            {
                MessageBox.Show(xe.Message);
            }
        }

        #region Windows ������������ɵĴ���
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.help = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.MediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.deleterow = new AxWMPLib.AxWindowsMediaPlayer();
            this.uplevelmusic = new AxWMPLib.AxWindowsMediaPlayer();
            this.gameover = new AxWMPLib.AxWindowsMediaPlayer();
            this.music = new System.Windows.Forms.Button();
            this.top = new System.Windows.Forms.Label();
            this.sound = new AxWMPLib.AxWindowsMediaPlayer();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleterow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uplevelmusic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameover)).BeginInit();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Salmon;
            this.label1.Font = new System.Drawing.Font("��Բ", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(268, 207);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(83, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "������";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Salmon;
            this.label3.Font = new System.Drawing.Font("��Բ", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(268, 158);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(83, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "������";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("����ϸ��", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(288, 308);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 32);
            this.button1.TabIndex = 10;
            this.button1.Text = "��ʼ";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("����ϸ��", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(288, 369);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 32);
            this.button3.TabIndex = 7;
            this.button3.Text = "����";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("����ϸ��", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.ForeColor = System.Drawing.Color.Red;
            this.button4.Location = new System.Drawing.Point(288, 431);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(72, 32);
            this.button4.TabIndex = 8;
            this.button4.Text = "��ͣ";
            this.button4.Click += new System.EventHandler(this.button4_Click);
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
            this.textBox1.Location = new System.Drawing.Point(402, 455);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(0, 21);
            this.textBox1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightCoral;
            this.label4.Font = new System.Drawing.Font("��Բ", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(268, 109);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(83, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "����";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Salmon;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(270, 485);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 23);
            this.label5.TabIndex = 12;
            this.label5.Text = "�������119";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            this.label5.MouseEnter += new System.EventHandler(this.label5_MouseEnter);
            this.label5.MouseLeave += new System.EventHandler(this.label5_MouseLeave);
            // 
            // help
            // 
            this.help.BackColor = System.Drawing.Color.White;
            this.help.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.help.ForeColor = System.Drawing.Color.Crimson;
            this.help.Location = new System.Drawing.Point(410, 455);
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
            this.exit.ForeColor = System.Drawing.Color.Crimson;
            this.exit.Location = new System.Drawing.Point(410, 484);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(72, 23);
            this.exit.TabIndex = 19;
            this.exit.Text = "�ص���ҳ��";
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // MediaPlayer1
            // 
            this.MediaPlayer1.Enabled = true;
            this.MediaPlayer1.Location = new System.Drawing.Point(443, 330);
            this.MediaPlayer1.Name = "MediaPlayer1";
            this.MediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MediaPlayer1.OcxState")));
            this.MediaPlayer1.Size = new System.Drawing.Size(0, 0);
            this.MediaPlayer1.TabIndex = 23;
            this.MediaPlayer1.Enter += new System.EventHandler(this.MediaPlayer1_Enter);
            // 
            // deleterow
            // 
            this.deleterow.Enabled = true;
            this.deleterow.Location = new System.Drawing.Point(443, 424);
            this.deleterow.Name = "deleterow";
            this.deleterow.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("deleterow.OcxState")));
            this.deleterow.Size = new System.Drawing.Size(0, 0);
            this.deleterow.TabIndex = 0;
            this.deleterow.Enter += new System.EventHandler(this.good_Enter);
            // 
            // uplevelmusic
            // 
            this.uplevelmusic.Enabled = true;
            this.uplevelmusic.Location = new System.Drawing.Point(443, 248);
            this.uplevelmusic.Name = "uplevelmusic";
            this.uplevelmusic.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("uplevelmusic.OcxState")));
            this.uplevelmusic.Size = new System.Drawing.Size(0, 0);
            this.uplevelmusic.TabIndex = 25;
            // 
            // gameover
            // 
            this.gameover.Enabled = true;
            this.gameover.Location = new System.Drawing.Point(443, 222);
            this.gameover.Name = "gameover";
            this.gameover.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gameover.OcxState")));
            this.gameover.Size = new System.Drawing.Size(0, 0);
            this.gameover.TabIndex = 26;
            // 
            // music
            // 
            this.music.BackColor = System.Drawing.Color.White;
            this.music.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.music.ForeColor = System.Drawing.Color.Crimson;
            this.music.Location = new System.Drawing.Point(410, 424);
            this.music.Name = "music";
            this.music.Size = new System.Drawing.Size(72, 23);
            this.music.TabIndex = 28;
            this.music.Text = "��Ч��";
            this.music.UseVisualStyleBackColor = false;
            this.music.Click += new System.EventHandler(this.music_Click);
            // 
            // top
            // 
            this.top.AutoSize = true;
            this.top.BackColor = System.Drawing.Color.LightCoral;
            this.top.Font = new System.Drawing.Font("��Բ", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.top.ForeColor = System.Drawing.Color.White;
            this.top.Location = new System.Drawing.Point(390, 9);
            this.top.Name = "top";
            this.top.Padding = new System.Windows.Forms.Padding(2);
            this.top.Size = new System.Drawing.Size(97, 24);
            this.top.TabIndex = 30;
            this.top.Text = "�������";
            this.top.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sound
            // 
            this.sound.Enabled = true;
            this.sound.Location = new System.Drawing.Point(427, 330);
            this.sound.Name = "sound";
            this.sound.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("sound.OcxState")));
            this.sound.Size = new System.Drawing.Size(0, 0);
            this.sound.TabIndex = 32;
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(494, 517);
            this.Controls.Add(this.sound);
            this.Controls.Add(this.top);
            this.Controls.Add(this.music);
            this.Controls.Add(this.uplevelmusic);
            this.Controls.Add(this.deleterow);
            this.Controls.Add(this.MediaPlayer1);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.help);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gameover);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "����˹����";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleterow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uplevelmusic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameover)).EndInit();
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
        //��������
        private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (block != null && this.paused == false && !this.failed)
            {
                if (e.KeyCode == this.keys[0])
                {
                    if (block.Move(0))
                    {
                        block.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[1])
                {
                    if (block.Move(1))
                    {
                        block.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[2])
                {
                    if (!block.Move(2))
                    {
                        this.FixAndCreate();
                    }
                    else
                    {
                        block.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[3])
                {
                    if (block.Rotate())
                    {
                        block.EraseLast();
                    }
                }
                else if (e.KeyCode == this.keys[4])
                {
                    block.Drop();
                    block.EraseLast();
                    this.FixAndCreate();
                }
                else if (e.KeyCode == this.keys[5])
                {
                    this.button4_Click(null, null);
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                this.Start();
            }
        }
        // ��Ϸ�������
        private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (block != null)
            {
                block.DrawBlocks(e.ClipRectangle);
            }
            if (this.failed)
            {
                Graphics gra = e.Graphics;
                gra.DrawString("Game Over", new Font("Arial Black", 25f), new SolidBrush(Color.Gray), 30, 30);
            }
        }
        //��һ��ʱ��ִ��һ��
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (block != null && !this.failed)
            {
                //���ƿ�ʼ������ʱ��ı�������ѭ������
                if ((int)MediaPlayer1.playState == 1&&music.Text=="��Ч��")
                {
                    MediaPlayer1.Ctlcontrols.play();
                }
                if (!block.Move(2))
                {
                    this.FixAndCreate();
                }
                else
                {
                    block.EraseLast();
                }
            }
        }
        // ��һ�鴰�����
        private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (nextBlock != null)
            {
                nextBlock.DrawBlocks(e.ClipRectangle);
            }
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            if (!this.failed)
            {
                if (paused)
                {
                    this.pauseTime += DateTime.Now - this.atPause;
                    paused = false;
                    this.timer1.Start();
                }
                else
                {
                    this.atPause = DateTime.Now;
                    paused = true;
                    this.timer1.Stop();
                }
            }
            this.textBox1.Focus();
        }
        // ���û���
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (!paused)
            {
                this.atPause = DateTime.Now;
                this.paused = true;
                this.timer1.Stop();
            }
            sform = new ControlForm();
            sform.SetOptions(this.keys, this.startLevel);
            sform.DialogResult = DialogResult.Cancel;

            sform.ShowDialog();
            if (sform.DialogResult == DialogResult.OK)
            {
                sform.GetOptions(ref this.keys, ref this.startLevel);
                this.level = this.startLevel;
                this.label4.Text = "���� " + this.level;
                this.timer1.Interval = 500 - 50 * (level - 1);
            }
            this.paused = false;
            this.pauseTime += DateTime.Now - this.atPause;
            this.timer1.Start();

            this.textBox1.Focus();
        }
        //����ʱ����
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            sound.URL = Application.StartupPath + @"//����ʱ����.MP3";
            this.Initiate();
            this.BackgroundImage = Image.FromFile(@"33.jpg");
        }
        //�ر���Ϸ
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.SaveSetting();

            Application.Exit();
        }

        private void label5_MouseEnter(object sender, System.EventArgs e)
        {
            this.label5.Text = "16�����119����";
        }

        private void label5_MouseLeave(object sender, System.EventArgs e)
        {
            this.label5.Text = "�������119";
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
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
        //�ص���ҳ��
        private void exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.SaveSetting();
            this.Close();
            MainForm0.eMainForm0.Show();
        }

        private void MediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void good_Enter(object sender, EventArgs e)
        {

        }
        //��Ч����
        private void music_Click(object sender, EventArgs e)
        {
            if (music.Text == "��Ч��")
            {
                if(timer1.Enabled==true)
                MediaPlayer1.Ctlcontrols.play();
                music.Text = "��Ч��";
                textBox1.Focus();
            }
            else
            {
                MediaPlayer1.Ctlcontrols.stop();
                deleterow.Ctlcontrols.stop();
                uplevelmusic.Ctlcontrols.stop();
                gameover.Ctlcontrols.stop();
                sound.Ctlcontrols.stop();
                textBox1.Focus();
                music.Text = "��Ч��";
            }
        }
        //����ҳ��
        private void help_Click(object sender, EventArgs e)
        {
            help help1 = new help();
            help1.Show();
        }
        //���ƽ���ҳ�����δ��ʼ�ͽ�������δ��һ�ο�ʼ������
        private void timer2_Tick(object sender, EventArgs e)
        {
            if ((int)sound.playState == 1 && music.Text == "��Ч��")
                sound.Ctlcontrols.play();
            else if (music.Text == "��Ч��")
                sound.Ctlcontrols.stop();
        }
    }
}