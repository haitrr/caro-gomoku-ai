using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCaro
{
    class CaroNode
    {
        static private Graphics _g;     // graphic để vẽ quân cờ lên bàn cờ
        static private SolidBrush _bg;  // màu nên của bàn cờ
        static private SolidBrush NewNode= new SolidBrush(Color.Yellow);    //màu tô để phân biệt ô mới đánh
        public static SolidBrush Bg
        {
            set { CaroNode._bg = value; }
        }
        public static Graphics G
        {
            set { CaroNode._g = value; }
        }
        static private Image X = Properties.Resources.X, O=Properties.Resources.O;  //ảnh của quân cờ
        private int _Line;                       //cột
        public int Line                                   
        {
            get { return _Line; }
            set { _Line = value; }
        }
        private int _Column;                     //dòng
        public int Column                                  
        {
            get { return _Column; }
            set { _Column = value; }
        }
        private const int _Width = 23;           //chiều rộng của ô cờ
        public static int Width
        {
            get { return _Width; }
        }
        private const int _Height = 23;          //chiều dài của ô cờ
        public static int Height
        {
            get { return _Height; }
        } 
        private Point _Pos;                             //vị trí để vẽ
        public Point Pos
        {
            get { return _Pos; }
            set { _Pos = value; }
        }
        private int _NStatus;                           //trạng thái hiên tại của ô cờ 1= người,-1 = máy, 0= chưa được đánh **2: biên của bàn cờ

        public int NStatus
        {
            get { return _NStatus; }
            set { _NStatus = value; }
        }
        public void Draw()      // hàm vẽ quân cờ
        {
            if (_NStatus == 1)
            {
                _g.DrawImage(O, Pos);
            }
            else if (_NStatus == -1)
            {
                _g.DrawImage(X, Pos);
            }
            else return;
        }
        public void Del()   // xóa quân cờ
        {
            _g.FillRectangle(_bg, Pos.X +1,Pos.Y +1, Width - 1,Height - 1);
        }
        public void PaintNewNode()      //tô màu ô mới đánh
        {
            _g.FillRectangle(NewNode, Pos.X + 1, Pos.Y + 1, Width - 1, Height - 1);
        }
        public CaroNode() { }
        public CaroNode(int Line,int Column,Point Pos, int Status)      //constructor
        {
            _Column = Column;
            _Line = Line;
            _Pos = Pos;
            _NStatus = Status;
        }
    }
}
