using System;
using System.Drawing;
using System.Windows.Forms;

namespace RussiaBlock
{
	/// <summary>
	/// Block ��ժҪ˵����
	/// </summary>
	public class Block
	{
		public Block(Control con,int leftBorder,int bottomBorder,int unitPix,int shapeNO,Point firstPos,Color color)
		{
			this.con=con;
			this.leftBorder=leftBorder;
			this.bottomBorder=bottomBorder;
			this.unitPix=unitPix;
			this.SetPos(shapeNO,firstPos);
			this.color=color;
			this.huji=new bool[leftBorder+1,bottomBorder+1];
			this.iori=new Color[leftBorder+1,bottomBorder+1];
			this.lastPos=new Point[4];
		}
		private int shapeNO;//��״��
		private Control con;//��ͼ�ؼ�
		private Point[] pos;//��ǰλ��
		private Point[] lastPos;//��һ��λ��
		private int leftBorder;//��߽�
		private int bottomBorder;//�±߽�
		private int unitPix;//ÿ��������
		private int blockNum=0;
		private int rowDelNum=0;
		private bool[,] huji;
		private Color[,] iori;
		private Color color;//��ǰ����ɫ
        //��󷽿�λ�õ���ʾ
        public void EraseLast()
		{
			foreach(Point p in this.lastPos)
			{
				this.con.Invalidate(new Rectangle(p.X*unitPix,p.Y*unitPix,unitPix+1,unitPix+1));
			}
		}
        //���÷�������λ��
        private void SetLastPos()
		{
			for(int i=0;i<this.pos.Length;i++)
			{
				this.lastPos[i]=this.pos[i];
			}
        }
        //���÷���ĵ�ǰλ��,shapeNO�����,firstPos��ʼλ��
        private void SetPos(int shapeNO,Point firstPos)
		{
			this.shapeNO=shapeNO;
			this.pos=new Point[4];
			pos[0]=firstPos;
			switch(shapeNO)
			{
				case 1:
					pos[1]=new Point(firstPos.X+1,firstPos.Y);
					pos[2]=new Point(firstPos.X,firstPos.Y+1);
					pos[3]=new Point(firstPos.X+1,firstPos.Y+1);
					break;
				case 2:
					pos[1]=new Point(firstPos.X+1,firstPos.Y);
					pos[2]=new Point(firstPos.X+2,firstPos.Y);
					pos[3]=new Point(firstPos.X+3,firstPos.Y);
					break;
				case 3:
					pos[1]=new Point(firstPos.X+1,firstPos.Y);
					pos[2]=new Point(firstPos.X+1,firstPos.Y+1);
					pos[3]=new Point(firstPos.X+2,firstPos.Y);
					break;
				case 4:
					pos[1]=new Point(firstPos.X+1,firstPos.Y);
					pos[2]=new Point(firstPos.X+1,firstPos.Y+1);
					pos[3]=new Point(firstPos.X+2,firstPos.Y+1);
					break;
				case 5:
					pos[1]=new Point(firstPos.X+1,firstPos.Y);
					pos[2]=new Point(firstPos.X+1,firstPos.Y-1);
					pos[3]=new Point(firstPos.X+2,firstPos.Y-1);
					break;
				case 6:
					pos[1]=new Point(firstPos.X,firstPos.Y+1);
					pos[2]=new Point(firstPos.X+1,firstPos.Y);
					pos[3]=new Point(firstPos.X+2,firstPos.Y);
					break;
				default:
					pos[1]=new Point(firstPos.X+1,firstPos.Y);
					pos[2]=new Point(firstPos.X+2,firstPos.Y);
					pos[3]=new Point(firstPos.X+2,firstPos.Y+1);
					break;
			}
		}
        //���Ƿ񻹿ɼ����ƶ������º�����
        private bool CanMove(int direction)
		{
			bool canMove=true;
			if(direction==0)
			{
				foreach(Point p in this.pos)
				{
					if(p.X-1<0 || this.huji[p.X-1,p.Y])
					{
						canMove=false;
						break;
					}
				}
			}
			else if(direction==1)
			{
				foreach(Point p in this.pos)
				{
					if(p.X+1>this.leftBorder || this.huji[p.X+1,p.Y])
					{
						canMove=false;
						break;
					}
				}
			}
			else 
			{
				foreach(Point p in this.pos)
				{
					if(p.Y+1>this.bottomBorder || this.huji[p.X,p.Y+1])
					{
						canMove=false;
						break;
					}
				}
			}
			return canMove;
		}
        //�жϵ�ǰ���Ƿ��ǿ��Խ��з�ת,λ��ȷ������,��ûȷ�������
        private bool CanRotate(Point[] pos)
		{
			bool canRotate=true;
			foreach(Point p in pos)
			{
				if(p.X<0 || p.X>this.leftBorder || p.Y<0 || p.Y>this.bottomBorder || this.huji[p.X,p.Y])
				{
					canRotate=false;
					break;
				}
			}
			if(canRotate==true)
				this.SetLastPos();
			return canRotate;
		}
        //һ�������и�����,ɾ������
        private void DelRows()
		{
			int count=0;
			int highRow=20;
			int lowRow=-1;
			int[] delRow={-1,-1,-1,-1};
			foreach(Point p in this.pos)
			{
				if(p.Y==highRow || p.Y==lowRow)
					continue;
				int i;
				for(i=0;i<this.huji.GetLength(0);i++)
					if(huji[i,p.Y]==false)
						break;
				if(i==this.huji.GetLength(0))
				{
					delRow[count]=p.Y;
					if(p.Y<highRow)
						highRow=p.Y;
					if(p.Y>lowRow)
						lowRow=p.Y;
					count++;
				}
			}
			
			if(count>0)
			{		
				//-----------------------------------------------------------------
				Graphics gra=con.CreateGraphics();
				foreach(Point p in this.lastPos)
				{
					gra.FillRectangle(new SolidBrush(con.BackColor),p.X*this.unitPix,p.Y*unitPix,25,25);
				}
				foreach(Point p in this.pos)
				{
					this.DrawOne(p.X,p.Y,this.color,gra);
				}	
				foreach(int i in delRow)
				{
					if(i>0)
					{
						for(int j=0;j<this.huji.GetLength(0);j++)
						{
							gra.FillRectangle(new SolidBrush(Color.FromArgb(60,Color.Black)),j*this.unitPix,i*unitPix,25,25);
						}
					}
				}
				System.Threading.Thread.CurrentThread.Join(180);
			    
				//-----------------------------------------------------------------
				
				if(count==2 && lowRow-highRow>1)
				{
					for(int i=lowRow;i>highRow+1;i--)
					{
						for(int j=0;j<this.huji.GetLength(0);j++)
						{
							this.huji[j,i]=this.huji[j,i-1];
							this.iori[j,i]=this.iori[j,i-1];
						}
					}
					for(int i=highRow;i>=count;i--)
					{
						for(int j=0;j<this.huji.GetLength(0);j++)
						{
							this.huji[j,i+1]=this.huji[j,i-1];
							this.iori[j,i+1]=this.iori[j,i-1];
						}
					}
				}
				else if(count==3 && lowRow-highRow>2)
				{
					int midRow=-1;
					foreach(int row in delRow)
					{
						if(row!=highRow && row!=lowRow)
						{
							midRow=row;
							break;
						}
					}
					for(int j=0;j<this.huji.GetLength(0);j++)
					{
						this.huji[j,lowRow]=this.huji[j,lowRow+highRow-midRow];
					}
					for(int i=highRow;i>=count;i--)
					{
						for(int j=0;j<this.huji.GetLength(0);j++)
						{
							this.huji[j,i+2]=this.huji[j,i-1];
							this.iori[j,i+2]=this.iori[j,i-1];
						}
					}
				}
				else
				{
					for(int i=lowRow;i>=count;i--)
					{
						for(int j=0;j<this.huji.GetLength(0);j++)
						{
							this.huji[j,i]=this.huji[j,i-count];
							this.iori[j,i]=this.iori[j,i-count];
						}
					}
				}	
				for(int i=0;i<count;i++)
				{
					for(int j=0;j<this.huji.GetLength(0);j++)
					{
						this.huji[j,i]=false;
					}
				}
				con.Invalidate(new Rectangle(0,0,con.Width,(lowRow+1)*this.unitPix));
				this.rowDelNum+=count;
			}
		}
        //���
        public void FixBlock()
		{
			this.blockNum++;
			foreach(Point p in this.pos)
			{
				this.huji[p.X,p.Y]=true;
				this.iori[p.X,p.Y]=this.color;
			}
			this.DelRows();
		}
        //������һ����
        public bool GeneBlock(int shapeNO,Point firstPos,Color color)
		{
			this.SetLastPos();
			this.EraseLast();
			this.SetPos(shapeNO,firstPos);			
			if(!this.CanRotate(this.pos))
			{
				this.pos=null;
				return false;
			}
			else 
			{
				this.color=color;
				return true;
			}
		}
        //��ת����
        public bool Rotate()
		{
			bool rotated=true;
			Point[] temp={pos[0],pos[1],pos[2],pos[3]};
			switch(this.shapeNO)
			{
				case 1:
					rotated=false;
					break;
				case 2:
					temp[0].Offset(2,2);
					temp[1].Offset(1,1);
					temp[3].Offset(-1,-1);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(2,2);
						this.pos[1].Offset(1,1);
						this.pos[3].Offset(-1,-1);
						this.shapeNO=8;
					}
					else 
						rotated=false;
					break;
				case 3:
					temp[0].Offset(1,-1);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(1,-1);
						this.shapeNO=9;
					}
					else 
						rotated=false;
					break;
				case 4:
					temp[0].Offset(2,0);
					temp[1].Offset(0,2);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(2,0);
						this.pos[1].Offset(0,2);
						this.shapeNO=12;
					}
					else
					{
						rotated=false;
					}
					break;
				case 5:
					temp[2].Offset(-1,0);
					temp[3].Offset(-1,2);
					if(this.CanRotate(temp))
					{
						this.pos[2].Offset(-1,0);
						this.pos[3].Offset(-1,2);
						this.shapeNO=13;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 6:
					temp[0].Offset(1,1);
					temp[1].Offset(2,0);
					temp[3].Offset(-1,-1);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(1,1);
						this.pos[1].Offset(2,0);
						this.pos[3].Offset(-1,-1);
						this.shapeNO=14;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 7:
					temp[0].Offset(1,1);
					temp[2].Offset(-1,-1);
					temp[3].Offset(0,-2);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(1,1);
						this.pos[2].Offset(-1,-1);
						this.pos[3].Offset(0,-2);
						this.shapeNO=17;
					}
					else 
					{
						rotated=false;
					}
					break;

				case 8:
					temp[0].Offset(-2,-2);
					temp[1].Offset(-1,-1);
					temp[3].Offset(1,1);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(-2,-2);
						this.pos[1].Offset(-1,-1);
						this.pos[3].Offset(1,1);
						this.shapeNO=2;
					}
					else 
						rotated=false;
					break;
				case 9:
					temp[2].Offset(-1,-1);
					if(this.CanRotate(temp))
					{
						this.pos[2].Offset(-1,-1);
						this.shapeNO=10;
					}
					else 
						rotated=false;
					break;
				case 10:
					temp[3].Offset(-1,1);
					if(this.CanRotate(temp))
					{
						this.pos[3].Offset(-1,1);
						this.shapeNO=11;
					}
					else 
						rotated=false;
					break;
				case 11:
					temp[0].Offset(-1,1);
					temp[2].Offset(1,1);
					temp[3].Offset(1,-1);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(-1,1);
						this.pos[2].Offset(1,1);
						this.pos[3].Offset(1,-1);
						this.shapeNO=3;
					}
					else
						rotated=false;
					break;
				case 12:
					temp[0].Offset(-2,0);
					temp[0].Offset(0,-2);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(-2,0);
						this.pos[1].Offset(0,-2);
						this.shapeNO=4;
					}
					else
					{
						rotated=false;
					}
					break;
				case 13:
					temp[2].Offset(1,0);
					temp[3].Offset(1,-2);
					if(this.CanRotate(temp))
					{
						this.pos[2].Offset(1,0);
						this.pos[3].Offset(1,-2);
						this.shapeNO=5;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 14:
					temp[2].Offset(1,0);
					temp[3].Offset(-1,2);
					if(this.CanRotate(temp))
					{
						this.pos[2].Offset(1,0);
						this.pos[3].Offset(-1,2);
						this.shapeNO=15;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 15:			
					temp[1].Offset(-1,-1);
					temp[2].Offset(-1,-1);
					temp[3].Offset(0,-2);
					if(this.CanRotate(temp))
					{			
						this.pos[1].Offset(-1,-1);
						this.pos[2].Offset(-1,-1);
						this.pos[3].Offset(0,-2);
						this.shapeNO=16;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 16:
					temp[0].Offset(-1,-1);
					temp[1].Offset(-1,1);
					temp[2].Offset(0,1);
					temp[3].Offset(2,1);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(-1,-1);
						this.pos[1].Offset(-1,1);
						this.pos[2].Offset(0,1);
						this.pos[3].Offset(2,1);
						this.shapeNO=6;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 17:
					temp[0].Offset(1,-1);
					temp[2].Offset(-1,1);
					temp[3].Offset(-2,0);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(1,-1);
						this.pos[2].Offset(-1,1);
						this.pos[3].Offset(-2,0);
						this.shapeNO=18;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 18:
					temp[0].Offset(-1,-1);
					temp[2].Offset(1,1);
					temp[3].Offset(0,2);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(-1,-1);
						this.pos[2].Offset(1,1);
						this.pos[3].Offset(0,2);
						this.shapeNO=19;
					}
					else 
					{
						rotated=false;
					}
					break;
				case 19:
					temp[0].Offset(-1,1);
					temp[2].Offset(1,-1);
					temp[3].Offset(2,0);
					if(this.CanRotate(temp))
					{
						this.pos[0].Offset(-1,1);
						this.pos[2].Offset(1,-1);
						this.pos[3].Offset(2,0);
						this.shapeNO=7;
					}
					else 
					{
						rotated=false;
					}
					break;
			}
			return rotated;
		}
        //�ƶ� 
        public bool Move(int direction)
		{
			int offx=0,offy=0;
			if(direction==0 && this.CanMove(0))//��
			{
				offx=-1;
				offy=0;
			}
			else if(direction==1 && this.CanMove(1))//��
			{
				offx=1;
				offy=0;
			}
			else if(direction==2 && this.CanMove(2))//��
			{
				offx=0;
				offy=1;
			}
			else 
			{
				return false;
			}
			this.SetLastPos();
			for(int i=0;i<this.pos.Length;i++)
			{
				pos[i].Offset(offx,offy);
			}
			return true;
		}
        //ֱ��
        public void Drop()
		{
			if(this.CanMove(2))
				this.SetLastPos();
			while(this.CanMove(2))
			{
				for(int i=0;i<this.pos.Length;i++)
				{
					pos[i].Offset(0,1);
				}
			}
		}

		private void DrawOne(int x,int y,Color color,Graphics gra)
		{
			gra.FillRectangle(new SolidBrush(color),x*unitPix+1,y*unitPix+1,this.unitPix-1,this.unitPix-1);
			gra.DrawRectangle(new Pen(Color.Transparent,1),x*unitPix,y*unitPix,unitPix,unitPix);
        }
        //��ָ����λ�û��Ʒ���,������С��
        public void DrawBlocks(Rectangle rec)
		{
			Graphics gra=this.con.CreateGraphics();
			if(this.pos!=null)
			{
				foreach(Point p in this.pos)
				{
					this.DrawOne(p.X,p.Y,this.color,gra);
				}
			}
			int x=rec.Height-1;//special for eraselast()'s size+1
			int y=rec.Width-1;
			if((x==this.unitPix && y==4*this.unitPix) || (x==2*unitPix && y==2*unitPix) || (x==2*unitPix && y==3*unitPix) || (x==3*unitPix && y==2*unitPix) || (x==4*unitPix && y==unitPix) )
				return;
			else
			{
				for(int i=this.huji.GetLength(0)-1;i>=0;i--)
				{
					for(int j=this.huji.GetLength(1)-1;j>=0;j--)
					{
						if(huji[i,j]==true && i*unitPix-rec.Left>-unitPix && i*unitPix<rec.Right && j*unitPix-rec.Top>-unitPix && j*unitPix<rec.Bottom)
						{
							this.DrawOne(i,j,this.iori[i,j],gra);
						}
					}
				}
			}	
		}

		public int BlockNum
		{
			get
			{
				return this.blockNum;
			}
		}
		public int RowDelNum
		{
			get
			{
				return this.rowDelNum;
			}
		}

	}
}
