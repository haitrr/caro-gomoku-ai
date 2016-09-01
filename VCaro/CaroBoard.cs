using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCaro
{
    class CaroBoard
    {
        private static Graphics _g;     // graphic để vẽ bàn cờ

        public static Graphics G
        {
            set { _g = value; }
        }
        public CaroBoard()
        {
        }
        private const int _LineAmount = 25;                 //số dòng của bàn cờ

        public int LineAmount
        {
            get { return _LineAmount; }
        }

        private const int _ColumnAmount = 25;               //số cột của bàn cờ

        public int ColumnAmount
        {
            get { return _ColumnAmount; }
        }
        public void Draw()           //vẽ bàn cờ
        {
            Pen pen;
            pen=new Pen(Color.Moccasin);                
            for(int i = 0;i <= _LineAmount; i++)        //vẽ các đường dọc
            {
                _g.DrawLine(pen, i * CaroNode.Width, 0, i * CaroNode.Width, _LineAmount * CaroNode.Height);
            }
            for (int j = 0; j <= _ColumnAmount; j++)                //vẽ các đường ngang
            {
                _g.DrawLine(pen, 0, j * CaroNode.Height, _ColumnAmount * CaroNode.Width, j * CaroNode.Height);
            }
        }
    }
}