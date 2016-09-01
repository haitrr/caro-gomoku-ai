using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCaro
{
    public struct VMove                    //các nước đi và lượng giá của nó
    {
        public VMove(long Value,int d)
        {
            value = Value;
            x = 0;
            y = 0;
            subvalue = 0;
            depth = d;
        }
        public VMove(long c,long d)
        {
            value = c;
            x = 0;
            y = 0;
            subvalue = d;
            depth = 0;
        }
        public VMove(int a,int b,long c, long d)
        {
            value = c;
            x = a;
            y = b;
            subvalue = d;
            depth = 0;
        }
        public int x;       // hàng của nước đi
        public int y;           // cột của nước đi
        public long value;      //lượng giá của nước đi
        public long subvalue;    // lượng giá phụ
        public int depth;           //độ sâu cuối cùng của nước đi
    }
    public struct Area                  // khu vực tìm kiếm
    {
        public int lmin;      //số thứ tự hàng nhỏ nhất
        public int cmin;        //số thứ tự cột nhỏ nhất
        public int lmax;        //số thứ tự hàng lớn nhất
        public int cmax;        //số thứ tự cột lớn nhất
    }
    class Game
    {
        #region define      // khai báo
        int _Result;                                //kết quả của game 1=người thắng , 2= máy thắng,0 = hòa
        private static Graphics _g;             //graphic để vẽ
        public static Graphics G
        {
            set { Game._g = value; }
        }
        public static Pen pen;              //bút vẽ đường chiến thắng
        private static SolidBrush _sbBG;            //màu nền bàn cờ
        public static SolidBrush SbBG
        {
            set { Game._sbBG = value; }
        }
        private CaroBoard _CaroBoard;                       //bàn cờ
        private int _Player;                            //lượt chơi hiện tại 1: người,-1 máy 
        public int Player                           
        {
            get { return _Player; }
        }
        private CaroNode[,] _Nodes;                     //mảng lưu các ô cờ 
        private Stack<CaroNode> _StackMoves;                        //stack lưu các nước đã đi
        private void InitNodes()                //khởi tạo mảng các ô cờ
        {
            _Nodes = new CaroNode[_CaroBoard.LineAmount+2, _CaroBoard.ColumnAmount+2];
            for (int i = 0; i < _CaroBoard.LineAmount+2; i++)
            {
                for (int j = 0; j < _CaroBoard.ColumnAmount+2; j++)
                {
                    if (i == 0 || i == _CaroBoard.LineAmount+1 || j == 0 || j == _CaroBoard.ColumnAmount+1) _Nodes[i, j] = new CaroNode(i, j, new Point(0, 0), 2);  // nếu là biên gán trạng thái là 2
                    else _Nodes[i, j] = new CaroNode(i, j, new Point((j-1) * CaroNode.Width, (i-1) * CaroNode.Height), 0);       //tất cả các ô cờ còn lại trạng thái 0
                }
            }
        }
        public Game()                       //khởi tạo
        {
            _CaroBoard = new CaroBoard();                           //tạo bàn cờ mới  
            InitNodes();
            _StackMoves = new Stack<CaroNode>();
            pen = new Pen(Color.Black);                         //màu để kẻ đường chiến thắng
            pen.Width = 2;                                          //độ rộng của bút                                           
            Random r = new Random();                                //random lượt chơi
            _Player = r.Next(-10,9);
            if(_Player>=0)
            {
                _Player = 1;
            }
            else
            {
                _Player = -1;
            }
        }
        #endregion
        #region Draw            //Vẽ
        public void DrawCaroBoard()               //vẽ bàn cờ
        {
            _CaroBoard.Draw();            //gọi phương thức vẽ của bàn cờ
        }
        public void reDrawMoves()          //vẽ lại các ô cờ đã đánh
        {
            foreach (CaroNode a in _StackMoves)
            {
                a.Draw();
            }
        }
        #endregion
        public bool Move(int x, int y)           //người đi
        {
            if (x % CaroNode.Width == 0 || y % CaroNode.Height == 0) return false;      //nếu đánh vào đường kẻ thì bỏ qua
            int C = x / CaroNode.Width + 1;             //xác định côt
            int L = y / CaroNode.Height + 1;                //xác định dòng
            if (_Nodes[L, C].NStatus != 0) return false;        //nếu ô đó đã được đánh thì bỏ qua
            _Nodes[L, C].NStatus = 1;           //set trạng thái về người
            _Player = -1;                       //nhường lượt đi cho máy
            _Nodes[L, C].PaintNewNode();    //tô màu cho nước mới đánh cho dễ thấy  
            _Nodes[L, C].Draw();             //vẽ ô cờ
            if (_StackMoves.Count > 0)          //làm mất màu nhận dạng của nước trước đó
            {
                _StackMoves.Peek().Del();
                _StackMoves.Peek().Draw();
            }
          
            _StackMoves.Push(_Nodes[L, C]);         //lưu vào stack
            return true;            //thông báo thành công
        }
        public void ComputerMove(int player,int difficulty)          //máy đánh ( có thể đánh cho người hoặc đánh cho máy)
        {
            if (_StackMoves.Count == 0)        //khi máy đánh trước(chưa có quân nào trên bàn cờ) thì đánh vào giữa bàn cờ
            {
                int L = _CaroBoard.LineAmount / 2, C = _CaroBoard.ColumnAmount / 2;
                _Nodes[L, C].NStatus = player;
                _Nodes[L, C].PaintNewNode();  //tô màu cho nước mới đánh cho dễ thấy
                _Nodes[L, C].Draw();
                _Player = -player;
                _StackMoves.Push(_Nodes[L, C]);
                return;
            }
            VMove Move = new VMove(); ;
            switch(difficulty)          //tìm kiếm nước đi tùy theo độ khó đã chọn
            {
                case 1: Move = FindCMove(1, 1, 1, player, -10, 10); break;      //dễ
                case 2: Move = FindCMove(3, 2, 2, player, -10, 10); break;      //thường
                case 3: Move = FindCMove(5, 3, 3, player, -10, 10); break;      //khó
                case 4: Move = FindCMove(8, 3, 3, player, -10, 10); break;      //rất khó
            }
            _Nodes[Move.x, Move.y].NStatus = player;      //set trạng thái 
            _Nodes[Move.x, Move.y].PaintNewNode();      //tô màu và vẽ quân cờ
            _Nodes[Move.x, Move.y].Draw();
            _StackMoves.Peek().Del();           //xóa màu của nước trước
            _StackMoves.Peek().Draw();  
            _Player = -player;          //nhường lượt chơi lại
            _StackMoves.Push(_Nodes[Move.x, Move.y]);     //lưu vào stack
        }
 
        #region Feature
        public void NewGame()      //tạo mới game
        {
            InitNodes();        //khởi tạo lại các ô cờ
            _StackMoves = new Stack<CaroNode>();        //tạo mới stack
            _CaroBoard.Draw();                      //vẽ lại bàn cờ
            Random r = new Random();    //random lượt đi
            _Player = r.Next(-10, 9);  
            if (_Player >= 0)
            {
                _Player = 1;
            }
            else
            {
                _Player = -1;
            }
        }
        public void Undo()     //chức năng undo
        {
            if (_StackMoves.Count > 1)         //chỉ undo khi có hơn 1 nước đi
            {
                if (WinCheck(false))     //nếu đã thắng thì xóa nước đi ra khỏi stack và vẽ lại toàn bàn cờ
                {
                    CaroNode a = _StackMoves.Pop();         //lấy nước đi cuối cùng ra
                    _Nodes[a.Line, a.Column].NStatus = 0;       //set trạng thái thành chưa đánh
                    if (_Result == -1)          //nếu là máy thắng thì lấy thêm 1 nước nữa
                    {
                        a = _StackMoves.Pop();
                        _Nodes[a.Line, a.Column].NStatus = 0;
                    }
                    _g.Clear(_sbBG.Color);      //xóa bàn cờ
                    DrawCaroBoard();        //vẽ lại bàn cờ
                    reDrawMoves();      //vẽ lại các nước đã đi
                }
                else            //nếu không thì chỉ xóa ô cờ bị undo đi 
                {
                    CaroNode a = _StackMoves.Pop();     
                    _Nodes[a.Line, a.Column].NStatus = 0;
                    a.Del();                                //lấy 2 nước cuối cùng ra và set trạng thái về chưa đánh
                    a = _StackMoves.Pop();
                    _Nodes[a.Line, a.Column].NStatus = 0;
                    a.Del();          //xóa các nước đó đi
                }
            }
        }
        #endregion
        #region Check Win
        public void GameOver()              //xử lí khi trò chơi kết thúc
        {
            switch(_Result)
            {
                case 0:
                    MessageBox.Show("Hòa!");
                    break;
                case 1:
                    MessageBox.Show("Thắng!");
                    break;
                case -1:
                    MessageBox.Show("Thua!");
                    break;
            }
        }
        public bool WinCheck(bool Draw)        //kiểm tra xem trò chơi đã kết thúc chưa và vẽ đường chiến thắng hoặc không
        {
            if (_StackMoves.Count < 9) return false;    //đi nhỏ hơn 9 nước thì không thể chiến thắng
            if (_StackMoves.Count == _CaroBoard.LineAmount * _CaroBoard.ColumnAmount)    // nếu đã đi hết toàn bộ bàn cờ thì hòa
            {
                _Result = 0;
                return true;
            }
            foreach (CaroNode a in _StackMoves)     //xét tất cả các nước đã đi
            {
                if(VerticalLine5(a,Draw)|| HorizontalLine5(a,Draw)||DiagonalLineL5(a,Draw)||DiagonalLineR5(a,Draw)) //kiểm tra các phương
                {
                    _Result = a.NStatus;        //trả về người thắng là trạng thái của ô hiện tại
                    return true;
                }
            }
            return false;
        }
        private bool VerticalLine5(CaroNode a,bool Draw)   //đường dọc từ trên xuống
        {
            if (a.Line > _CaroBoard.LineAmount - 4) return false;   //xét xuống không thể đủ 5 quân
            int countN;
            for (countN = 1; countN < 5; countN++)
            {
                if (_Nodes[a.Line + countN, a.Column].NStatus != a.NStatus) return false;   //gặp ô khác trạng thái thì break
            }
            if ((_Nodes[a.Line - 1, a.Column].NStatus == -a.NStatus || _Nodes[a.Line -1, a.Column].NStatus ==2)  && (_Nodes[a.Line + 5, a.Column].NStatus == -a.NStatus || _Nodes[a.Line + 5, a.Column].NStatus ==2 )) return false;    // bị chặn 2 đầu 
            if(Draw)    //có vẽ đường chiến thắng không
            _g.DrawLine(pen, a.Pos.X +CaroNode.Width/2 , a.Pos.Y , _Nodes[a.Line + countN, a.Column].Pos.X+CaroNode.Width/2, _Nodes[a.Line + countN, a.Column].Pos.Y);
            return true;
        }
        private bool HorizontalLine5(CaroNode a, bool Draw)     //đường ngang trái qua phải
        {
            if (a.Column > _CaroBoard.ColumnAmount - 4) return false;   // không thể đủ 5 quân
            int countN;
            for (countN = 1; countN < 5; countN++)
            {
                if (_Nodes[a.Line, a.Column + countN].NStatus != a.NStatus) return false;
            }
            if ((_Nodes[a.Line, a.Column -1].NStatus == -a.NStatus || _Nodes[a.Line, a.Column -1].NStatus == 2 )&& (_Nodes[a.Line, a.Column +5].NStatus == -a.NStatus || _Nodes[a.Line, a.Column +5].NStatus == 2)) return false;       // bị chặn 2 đầu
            if(Draw) //vẽ
            _g.DrawLine(pen, a.Pos.X, a.Pos.Y + CaroNode.Height / 2, _Nodes[a.Line, a.Column + countN].Pos.X, _Nodes[a.Line, a.Column + countN].Pos.Y + CaroNode.Height / 2);
            return true;
        }
        private bool DiagonalLineL5(CaroNode a, bool Draw)     //đường chéo trái
        {
            if (a.Line > _CaroBoard.LineAmount - 4 || a.Column > _CaroBoard.ColumnAmount - 4) return false; //không thể đủ 5 quân
            int countN;
            for (countN = 1; countN < 5; countN++)
            {
                if (_Nodes[a.Line + countN, a.Column + countN].NStatus != a.NStatus) return false;  //gặp quân khác trạng thái
            }
            if (( _Nodes[a.Line -1, a.Column - 1].NStatus == -a.NStatus || _Nodes[a.Line -1, a.Column - 1].NStatus == 2 ) && (_Nodes[a.Line +5, a.Column + 5].NStatus == -a.NStatus || _Nodes[a.Line +5, a.Column + 5].NStatus == 2 )) return false;        // bị chặn 2 đầu
            if(Draw)        //vẽ
            _g.DrawLine(pen, a.Pos.X, a.Pos.Y, _Nodes[a.Line + countN, a.Column + countN].Pos.X, _Nodes[a.Line + countN, a.Column + countN].Pos.Y);
            return true;
        }
        private bool DiagonalLineR5(CaroNode a, bool Draw)     //đường chéo phải
        {
            if (a.Line > _CaroBoard.LineAmount - 4 || a.Column <  5) return false;      //không thể đủ 5 quân
            int countN;
            for (countN = 1; countN < 5; countN++)
            {
                if (_Nodes[a.Line + countN, a.Column - countN].NStatus != a.NStatus) return false;  //gặp quân khác trạng thái
            }
            if ((_Nodes[a.Line - 1, a.Column + 1].NStatus == -a.NStatus || _Nodes[a.Line - 1, a.Column + 1].NStatus == 2 ) && (_Nodes[a.Line + 5, a.Column - 5].NStatus == -a.NStatus || _Nodes[a.Line + 5, a.Column - 5].NStatus == 2 )) return false; //bị chặn 2 đầu
            if(Draw)    //vẽ
            _g.DrawLine(pen, a.Pos.X+CaroNode.Width, a.Pos.Y, _Nodes[a.Line + countN, a.Column - countN].Pos.X+CaroNode.Width, _Nodes[a.Line + countN, a.Column - countN].Pos.Y);
            return true;
        }
        #endregion
        #region Minimax AI
        private long[] AttackPointArray = new long[8] { 0, 16, 670, 11000, 200000, 200000, 200000, 200000 }; //mảng điểm đánh giá tấn công
        private long[] DefendPointArray = new long[8] { 0, 5, 210, 2700, 45000, 45000, 45000, 45000 };    //mảng điểm đánh giá phòng ngự
        private long[] BlockedAttackPointArray = new long[8] { 0, 1, 50, 440, 200000, 200000, 200000, 200000 };
        private long[] BlockedDefendPointArray = new long[8] { 0, 1, 5, 290, 45000, 45000, 45000, 45000 }; 
        private List<CaroNode> SearchArea(int ex)     //tìm khu vực tìm kiếm với ô hiện tại +ex ô
        {
            int temp1;
            List<CaroNode> Rs = new List<CaroNode>();       //list node cần tìm kiếm
            Area temp=new Area();       // biến tạm khu vực
            CaroNode[] s = _StackMoves.ToArray();       //copy stack ra mảng tạm
            for (int k = 0; k < _StackMoves.Count; k++) //xét toàn bộ các nước đi trong mảng
            {
                temp.cmax = s[k].Column+ex;     // tạo khu vực tạm
                temp.cmin = s[k].Column - ex;
                temp.lmax = s[k].Line + ex;
                temp.lmin = s[k].Line - ex;
                if (temp.cmax > _CaroBoard.ColumnAmount ) temp.cmax = _CaroBoard.ColumnAmount ; // nếu vượt quá biên thì chỉnh lại
                if (temp.cmin < 1) temp.cmin = 1;
                if (temp.lmax > _CaroBoard.LineAmount ) temp.lmax = _CaroBoard.LineAmount ;
                if (temp.lmin < 1) temp.lmin = 1;
                for(int i=temp.lmin;i<=temp.lmax;i++)       // nếu trong khu vực đó có 1 ô chưa có trong list thì thêm vào
                {
                    for(int j=temp.cmin;j<=temp.cmax;j++)
                    {
                        if(_Nodes[i,j].NStatus==0)
                        {
                            temp1 = 0;
                            foreach (CaroNode a in Rs)
                            {
                                if (a.Line == _Nodes[i, j].Line && a.Column == _Nodes[i, j].Column)
                                {
                                    temp1 = 1;
                                    break;
                                }
                            }
                            if (temp1 == 0) Rs.Add(_Nodes[i, j]);
                        }
                    }
                }
            }
            return Rs;
        }
        private List<VMove> EValue(int player)      //lượng giá các nước đi trong khu vực tìm kiếm
        {
            List<VMove> ValueBoard = new List<VMove>();             //mảng lưu các nước đi đã được lượng giá
            List<CaroNode> search = SearchArea(2);          // khu vực tìm kiếm 
            foreach (CaroNode a in search)      //lượng giá
            {
                long AttackPoint = AttackPointHorizontal(a.Line, a.Column, player) + AttackPointVertical(a.Line, a.Column, player) + AttackPointDiagonalL(a.Line, a.Column, player) + AttackPointDiagonalR(a.Line, a.Column, player);
                long DefendPoint = DefendPointHorizontal(a.Line, a.Column, player) + DefendPointVertical(a.Line, a.Column, player) + DefendPointDiagonalL(a.Line, a.Column, player) + DefendPointDiagonalR(a.Line, a.Column, player);
                if (AttackPoint >= DefendPoint)        //Nếu điểm tấn công lớn hơn
                {
                    VMove b = new VMove(AttackPoint, DefendPoint);      //khởi tạo và thêm vào list điểm chính là tấn công
                    b.x = a.Line;
                    b.y = a.Column;
                    ValueBoard.Add(b);
                }
                else
                {
                    VMove b = new VMove(DefendPoint,AttackPoint);       //nếu không thì điểm chính là phòng ngư
                    b.x = a.Line;
                    b.y = a.Column;
                    ValueBoard.Add(b);
                }
            }
            return ValueBoard;
        }
        private VMove VlMaxPos(List<VMove> ValueBoard)  //lấy ra nước đi có giá trị lớn nhất
        {
            long temp=long.MinValue;
            VMove rs = new VMove();
            int max=0;
            for(int i=0;i<ValueBoard.Count;i++) //lấy ra nước có điểm cao nhất , nếu bằng thì so sánh điểm phụ
            {
                if (ValueBoard[i].value > temp || (ValueBoard[i].value == temp && ValueBoard[i].subvalue > ValueBoard[max].subvalue)) 
                {
                    rs.x = ValueBoard[i].x;
                    rs.y = ValueBoard[i].y;
                    temp = ValueBoard[i].value;
                    max = i;
                }
            }
            ValueBoard[max] = new VMove(rs.x, rs.y, long.MinValue,0);       //set giá tri xuống để không bị lấy ra lần nữa
            return rs;
        }
        
        
        #region Attack      //các hàm lượng giá điểm tấn công theo các phương
        private long AttackPointHorizontal(int NLine, int NColumn,int player)     //lượng giá tấn công hàng dọc
        {
            long Rs = 0;        //điểm
            int M = 0;      //đếm quân ta
            int temp = 0;       //đếm số khoảng trống đã xét
            int E = 0;       //đêm quân địch
            for (int Count = 1; Count < 7 && NLine + Count < _CaroBoard.LineAmount + 2; Count++)    //xét 6 ô trên xuống 
            {
                if (_Nodes[NLine + Count, NColumn].NStatus == player)   //gặp quân ta thì tăng quân ta
                    M++;
                if (_Nodes[NLine + Count, NColumn].NStatus == -player ||  _Nodes[NLine + Count, NColumn].NStatus ==2)   //gặp quân địch hoặc biên thì tăng quân địch
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine + Count, NColumn].NStatus == 0)    //gặp ô trống
                {
                    if (Count == 1) { temp++; continue;}    //nếu là ô bên cạnh thì tăng đếm lên và tiếp tục 
                    else
                    break;  // không thì break
                }
            }
            for (int Count = 1; Count < 7 && NLine - Count >= 0; Count++)   //xét dưới lên
            {
                if (_Nodes[NLine - Count, NColumn].NStatus == player)   //gặp quân ta thì tăng quân ta
                    M++;
                if (_Nodes[NLine - Count, NColumn].NStatus == -player || _Nodes[NLine - Count, NColumn].NStatus == 2) //gặp quân địch hoặc biên thì tăng quân địch
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine - Count, NColumn].NStatus == 0) //gặp ô trống
                {
                    if (Count == 1) { temp++; continue; } //nếu là ô bên cạnh thì tăng đếm lên và tiếp tục 
                    else
                        break; // không thì break
                }
            }
            if (E == 2 && (M < 4 || temp == 0)) return 0;       //nếu bị chặn 2 đầu thì vô giá trị, kiểm soát lỗi xác định sai bị chặn 2 đầu 
            if (E == 1) 
            {
                Rs += BlockedAttackPointArray[M];       //bị chặn 1 đầu
                if (M == 4 && temp == 2) Rs /= 2;   //kiểm soát lỗi xét ô trống
            }
            else
            {
                Rs += AttackPointArray[M];  //không bị chặn
                if (M == 4 && temp == 2) Rs /= 2;   //kiểm soát lỗi xét ô trống
                if (M == 3 && temp == 2) Rs /= 2;   //kiểm soát lỗi xét ô trống
            }
            return Rs;
        }
        private long AttackPointVertical(int NLine, int NColumn, int player) //lượng giá tấn công hàng ngang tương tự hàng dọc
        {
            long Rs = 0;
            int M=0, temp = 0;
            int E = 0;
            for (int Count = 1; Count < 7 && NColumn + Count < _CaroBoard.ColumnAmount + 2; Count++)
            {
                if (_Nodes[NLine, NColumn + Count].NStatus == player)
                    M++;
                if (_Nodes[NLine, NColumn + Count].NStatus == -player || _Nodes[NLine, NColumn + Count].NStatus == 2)
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine, NColumn + Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue; }
                    else
                    break;
                }
            }
            for (int Count = 1; Count < 7 && NColumn - Count >= 0; Count++)
            {
                if (_Nodes[NLine, NColumn - Count].NStatus == player)
                    M++;
                if (_Nodes[NLine, NColumn - Count].NStatus == -player || _Nodes[NLine, NColumn - Count].NStatus == 2)
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine, NColumn - Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue;}
                    else
                        break;
                }
            }
            if (E == 2 && (M < 4 || temp == 0)) return 0;
            if (E == 1)
            {
                Rs += BlockedAttackPointArray[M];
                if (M == 4 && temp == 2) Rs /= 2;
            }
            else
            {
                Rs += AttackPointArray[M];
                if (M == 4 && temp == 2) Rs /= 2;
                if (M == 3 && temp == 2) Rs /= 2;
            }
            return Rs;
        }
        private long AttackPointDiagonalR(int NLine, int NColumn, int player) //lượng giá tấn công hàng chéo phải tương tự hàng dọc
        {
            long Rs = 0;
            int M=0, temp = 0;
            int E = 0;
            for (int Count = 1; Count < 7 && NColumn + Count < _CaroBoard.ColumnAmount + 2 && NLine - Count >= 0; Count++)
            {
                if (_Nodes[NLine - Count, NColumn + Count].NStatus == player)
                    M++;
                if (_Nodes[NLine - Count, NColumn + Count].NStatus == -player ||  _Nodes[NLine - Count, NColumn + Count].NStatus ==2)
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine - Count, NColumn + Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue;}
                    else
                        break;
                }
            }
            for (int Count = 1; Count < 7 && NColumn - Count >= 0 && NLine + Count < _CaroBoard.LineAmount + 2; Count++)
            {
                if (_Nodes[NLine + Count, NColumn - Count].NStatus == player)
                    M++;
                if (_Nodes[NLine + Count, NColumn - Count].NStatus == -player ||  _Nodes[NLine + Count, NColumn - Count].NStatus ==2)
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine + Count, NColumn - Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue;}
                    else
                        break;
                }
            }
            if (E == 2 && (M < 4 || temp == 0)) return 0;
            if (E == 1)
            {
                Rs += BlockedAttackPointArray[M];
                if (M == 4 && temp == 2) Rs /= 2;
            }
            else
            {
                Rs += AttackPointArray[M];
                if (M == 4 && temp == 2) Rs /= 2;
                if (M == 3 && temp == 2) Rs /= 2;
            }
            return Rs;
        }
        private long AttackPointDiagonalL(int NLine, int NColumn, int player)       //lượng giá tấn công hàng chéo trái tương tự hàng dọc
        {
            long Rs = 0;
            int M=0, temp = 0;
            int E = 0;
            for (int Count = 1; Count < 7 && NColumn + Count < _CaroBoard.ColumnAmount + 2 && NLine + Count < _CaroBoard.LineAmount + 2; Count++)
            {
                if (_Nodes[NLine + Count, NColumn + Count].NStatus == player)
                    M++;
                if (_Nodes[NLine + Count, NColumn + Count].NStatus == -player ||  _Nodes[NLine + Count, NColumn + Count].NStatus ==2)
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine + Count, NColumn + Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue;}
                    else
                        break;
                }
            }
            for (int Count = 1; Count < 7 && NColumn - Count >= 0 && NLine - Count >= 0; Count++)
            {
                if (_Nodes[NLine - Count, NColumn - Count].NStatus == player)
                    M++;
                if (_Nodes[NLine - Count, NColumn - Count].NStatus == -player ||  _Nodes[NLine - Count, NColumn - Count].NStatus ==2)
                {
                    E++;
                    break;
                }
                if (_Nodes[NLine - Count, NColumn - Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue;}
                    else
                        break;
                }
            }
            if (E == 2 && (M < 4 || temp ==0))   return 0;
            if (E == 1)
            {
                Rs += BlockedAttackPointArray[M];
                if (M == 4 && temp == 2) Rs /= 2;
            }
            else
            {
                Rs += AttackPointArray[M];
                if (M == 4 && temp == 2) Rs /= 2;
                if (M == 3 && temp == 2) Rs /= 2;
            }
            return Rs;
        }
        #endregion
        #region Defend
        private long DefendPointHorizontal(int NLine, int NColumn, int player)  // lượng giá phòng ngự hàng dọc
        {
            long Rs = 0;
            int M = 0, temp = 0;
            int E = 0;
            for (int Count = 1; Count < 7 && NLine + Count < _CaroBoard.LineAmount; Count++) //xét 6 ô trên xuống 
            {
                if (_Nodes[NLine + Count, NColumn].NStatus == player || _Nodes[NLine + Count, NColumn].NStatus == 2)       //gặp quân ta, hoặc biên bàn cờ thì tăng số quân ta và break
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine + Count, NColumn].NStatus == -player)      //gặp quân địch thì tăng số quân địch
                {
                    E++;
                }
                if (_Nodes[NLine + Count, NColumn].NStatus == 0)        //gặp ô trống
                {
                    if (Count == 1) { temp++; continue; }        //nếu là ô bên cạnh thì tăng đếm lên và tiếp tục 
                    break;       // không thì break
                }
            }
            for (int Count = 1; Count < 7 && NLine - Count >= 0; Count++)  //xét dưới lên
            {
                if (_Nodes[NLine - Count, NColumn].NStatus == player || _Nodes[NLine + Count, NColumn].NStatus == 2)          //gặp quân ta hoặc biên bàn cờ thì tăng số quân ta và break
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine - Count, NColumn].NStatus == -player)      //gặp quân địch thì tăng số quân địch
                {
                    E++;
                }
                if (_Nodes[NLine - Count, NColumn].NStatus == 0)        //gặp ô trống
                {
                    if (Count == 1) { temp++; continue; }        //nếu là ô bên cạnh thì tăng đếm lên và tiếp tục 
                    break;      // không thì break
                }
            }
            if (M == 2 && (E < 4 || temp == 0)) return 0;    //nếu bị chặn 2 đầu thì vô giá trị, kiểm soát lỗi xác định sai bị chặn 2 đầu 
            if (M == 1)
            {
                Rs += BlockedDefendPointArray[E];  //bị chặn 1 đầu
                if (E == 4 && temp == 2) Rs /= 2;  //kiểm soát lỗi xét ô trống
            }
            else
            {   
                Rs += DefendPointArray[E];      //không bị chặn
                if (E == 4 && temp == 2) Rs /= 2;    //kiểm soát lỗi xét ô trống
                if (E == 3 && temp == 2) Rs /= 2;    //kiểm soát lỗi xét ô trống
            }   
            return Rs;
        }
        private long DefendPointVertical(int NLine, int NColumn, int player)    //lượng giá phòng thủ hàng ngang tương tự hàng dọc
        {
            long Rs = 0;
            int M = 0, temp = 0;
            int E = 0;
            for (int Count = 1; Count < 7 && NColumn + Count < _CaroBoard.ColumnAmount; Count++)
            {
                if (_Nodes[NLine, NColumn + Count].NStatus == player || _Nodes[NLine, NColumn + Count].NStatus == 2)
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine, NColumn + Count].NStatus == -player)
                {
                    E++;
                }
                if (_Nodes[NLine, NColumn + Count].NStatus == 0)
                {

                    if (Count == 1) { temp++; continue; }
                    break;
                }
            }
            for (int Count = 1; Count < 7 && NColumn - Count >= 0; Count++)
            {
                if (_Nodes[NLine, NColumn - Count].NStatus == player || _Nodes[NLine, NColumn - Count].NStatus == 2)
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine, NColumn - Count].NStatus == -player)
                {
                    E++;

                }
                if (_Nodes[NLine, NColumn - Count].NStatus == 0)
                {

                    if (Count == 1) { temp++; continue; }
                    break;
                }
            }
            if (M == 2 && (E < 4 || temp == 0)) return 0;
            if (M == 1)
            {
                Rs += BlockedDefendPointArray[E];
                if (E == 4 && temp == 2) Rs /= 2;
            }
            else
            {
                Rs += DefendPointArray[E];
                if (E == 4 && temp == 2) Rs /= 2;
                if (E == 3 && temp == 2) Rs /= 2;
            }
            return Rs;
        }
        private long DefendPointDiagonalR(int NLine, int NColumn, int player)   //lượng giá phòng thủ hàng chéo phải tương tự hàng dọc
        {
            if ((NLine < 4 && NColumn < 4) || (NColumn >= _CaroBoard.ColumnAmount - 4 && NLine >= _CaroBoard.LineAmount - 4)) return 0;
            long Rs = 0;
            int M = 0, temp = 0;
            int E = 0;
            for (int Count = 1; Count < 7 && NColumn + Count < _CaroBoard.ColumnAmount && NLine - Count >= 0; Count++)
            {
                if (_Nodes[NLine - Count, NColumn + Count].NStatus == player || _Nodes[NLine - Count, NColumn + Count].NStatus == 2)
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine - Count, NColumn + Count].NStatus == -player)
                {
                    E++;
                }
                if (_Nodes[NLine - Count, NColumn + Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue; }
                    break;
                }
            }
            for (int Count = 1; Count < 7 && NColumn - Count >= 0 && NLine + Count < _CaroBoard.LineAmount; Count++)
            {
                if (_Nodes[NLine + Count, NColumn - Count].NStatus == player ||_Nodes[NLine +Count, NColumn - Count].NStatus == 2)
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine + Count, NColumn - Count].NStatus == -player)
                {
                    E++;
                }
                if (_Nodes[NLine + Count, NColumn - Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue; }
                    break;
                }
            }
            if (M == 2 && (E < 4 || temp == 0)) return 0;
            if (M == 1)
            {
                Rs += BlockedDefendPointArray[E];
                if (E == 4 && temp == 2) Rs /= 2;
            }
            else
            {
                Rs += DefendPointArray[E];
                if (E == 4 && temp == 2) Rs /= 2;
                if (E == 3 && temp == 2) Rs /= 2;
            }
            return Rs;
        }
        private long DefendPointDiagonalL(int NLine, int NColumn, int player)       //lượng giá phòng thủ hàng chéo trái tương tự hàng dọc
        {
            if ((NLine >= _CaroBoard.LineAmount - 4 && NColumn < 4) || (NColumn >= _CaroBoard.ColumnAmount - 4 && NLine < 4)) return 0;
            long Rs = 0;
            int M = 0, temp = 0;
            int E = 0;
            for (int Count = 1; Count < 7 && NColumn + Count < _CaroBoard.ColumnAmount && NLine + Count < _CaroBoard.LineAmount; Count++)
            {
                if (_Nodes[NLine + Count, NColumn + Count].NStatus == player || _Nodes[NLine + Count, NColumn + Count].NStatus == 2)
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine + Count, NColumn + Count].NStatus == -player)
                {
                    E++;
                }
                if (_Nodes[NLine + Count, NColumn + Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue; }
                    break;
                }
            }
            for (int Count = 1; Count < 7 && NColumn - Count >= 0 && NLine - Count >= 0; Count++)
            {
                if (_Nodes[NLine - Count, NColumn - Count].NStatus == player)
                {
                    M++;
                    break;
                }
                if (_Nodes[NLine - Count, NColumn - Count].NStatus == -player || _Nodes[NLine - Count, NColumn - Count].NStatus == 2)
                {
                    E++;
                }
                if (_Nodes[NLine - Count, NColumn - Count].NStatus == 0)
                {
                    if (Count == 1) { temp++; continue; }
                    break;
                }
            }
            if (M == 2 && (E < 4 || temp == 0)) return 0;
            if (M == 1)
            {
                Rs += BlockedDefendPointArray[E];
                if (E == 4 && temp == 2) Rs /= 2;
            }
            else
            {
                Rs += DefendPointArray[E];
                if (E == 4 && temp == 2) Rs /= 2;
                if (E == 3 && temp == 2) Rs /= 2;
            }
            return Rs;
        }
        #endregion

        private VMove FindCMove(int depth,int cmoveamount,int hmoveamount,int player,long alpha,long beta)
        {
            if(WinCheck(false)==true)       //kiểm tra thắng thua nhưng không vẽ đường chiến thắng
            {
                if (_Result == -1) return new VMove(1,depth);       //nếu máy thắng thì trả về -1 và độ sâu
                else
                    if (_Result == 1) return new VMove(-1,depth);   // thua thì tra về 1 và độ sâu
                    else return new VMove(0,0);                    //hòa thì trả về 0 
            }
            if (depth == 0) return new VMove(0,0);     //độ sâu giới hạn thì trả về 0
            VMove temp;     //nước đi tạm
            VMove rs=new VMove();   //nước đi kết quả
            if (player == 1)        //nếu là lượt người chơi
            {
                List<VMove> vb =EValue(1);      //lượng giá các nước đi của người chơi
                //hmoveamount = vb.Count();
                VMove Bm = new VMove(2, 10);            //nước đi tôt nhất
                VMove[] hMove = new VMove[hmoveamount];     //mảng lưu các nước đi tiềm năng
                for (int i = 0; i < hmoveamount; i++)       //lấy hmoveamount nước đi tốt nhất từ mảng lượng giá
                {
                    hMove[i] = VlMaxPos(vb);
                }
                for (int i = 0; i < hmoveamount; i++) 
                {
                    _Nodes[hMove[i].x, hMove[i].y].NStatus = 1;         //đi nước đi này
                    _StackMoves.Push(_Nodes[hMove[i].x, hMove[i].y]);       
                    temp = FindCMove(depth - 1, cmoveamount, hmoveamount, -1, alpha, beta); //Phát triển đến độ sâu tiếp theo và trả vê giá trị
                    if (Bm.value > temp.value || (Bm.value == 1 && temp.value == 1 && Bm.depth > temp.depth) || (Bm.value == -1 && temp.value == -1 && Bm.depth < temp.depth))
                    {   //lấy nước đi có giá trị nhỏ nhất ( nếu 2 nước đi dẫn đến chiến thăng (-1) thì depth cao nhất ngược lại dẫn đên thua (1) chon depth thấp nhất
                        rs.value = Bm.value = temp.value;   
                        rs.depth = Bm.depth = temp.depth;
                        rs.x = hMove[i].x;
                        rs.y = hMove[i].y;
                    }
                    _StackMoves.Pop();         //trả lại  nguyên trạng
                    _Nodes[hMove[i].x, hMove[i].y].NStatus = 0;
                    if (beta > Bm.value) beta = Bm.value;   //cắt tỉa alphabeta
                    if (beta <= alpha) break;

                }
                return rs;
            }
            else
            {
                    //nếu là lượt của máy
            }
            {
                List<VMove> vb = EValue(-1);    //lượng giá các nước đi của người chơi
                //cmoveamount = vb.Count();
                VMove Bm = new VMove(-2, -1);     //nước đi tôt nhất
                VMove[] cMove = new VMove[cmoveamount]; //mảng lưu các nước đi tiềm năng
                for (int i = 0; i < cmoveamount; i++)   //lấy cmoveamount nước đi tốt nhất từ mảng lượng giá
                {
                    cMove[i] = VlMaxPos(vb);
                }
                for (int i = 0; i < cmoveamount; i++)
                {
                    _Nodes[cMove[i].x, cMove[i].y].NStatus = -1;
                    _StackMoves.Push(_Nodes[cMove[i].x, cMove[i].y]);       //đi nước đi này
                    temp = FindCMove(depth - 1, cmoveamount, hmoveamount, 1, alpha, beta);   //Phát triển đến độ sâu tiếp theo và trả vê giá trị
                    if (Bm.value < temp.value || (Bm.value == 1 && temp.value == 1 && Bm.depth < temp.depth) || (Bm.value == -1 && temp.value == -1 && Bm.depth > temp.depth))
                    { //lấy nước đi có giá trị nhỏ nhất ( nếu 2 nước đi dẫn đến chiến thăng (1) thì depth cao nhất ngược lại dẫn đên thua (-1) chon depth thấp nhất
                        rs.value = Bm.value = temp.value;
                        rs.depth = Bm.depth = temp.depth;
                        rs.x = cMove[i].x;
                        rs.y = cMove[i].y;
                    }
                    _StackMoves.Pop();  //trả về nguyên trạng
                    _Nodes[cMove[i].x, cMove[i].y].NStatus = 0;
                    if (beta < Bm.value) beta = Bm.value;       //cắt tỉa alphabeta
                    if (beta <= alpha) break;
                }
                return rs;      // trả về nước đi tốt nhất
            }
        }
    }
#endregion
}
