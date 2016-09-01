using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCaro
{
    public partial class FVCaro : Form
    {
        private Stopwatch s;        //đém thời gian suy nghĩ
        private Game _VCaro;        //Game
        private Graphics g; //graphic để vẽ bàn cờ
        public FVCaro()
        {
            InitializeComponent();
            _VCaro = new Game();    //tạo mới game
            g=Game.G=CaroNode.G = CaroBoard.G = PNCaroBoard.CreateGraphics();       //gán graphic của bàn cơ cho các lớp
            BackgroundImage = Properties.Resources.FormBG;  //gán hình nền cho form
            PBCPU.Image = Properties.Resources.CPU;     //load hình minh họa máy
            PBHM.Image = Properties.Resources.Human;        // load hình minh họa người
            PNCaroBoard.BackColor = Color.DarkKhaki;            // Màu nền của bàn cờ
            Game.SbBG=CaroNode.Bg = new SolidBrush(PNCaroBoard.BackColor);  //gán màu để dùng khi xóa quân cờ
            s = new Stopwatch();        //khởi tạo biến để đếm thời gian suy nghĩ
        }
        private void BTNew_Click(object sender, EventArgs e)    //nút NewGame
        {
            g.Clear(PNCaroBoard.BackColor);     //xóa bàn cờ
            _VCaro.NewGame();       //Tạo lại game mới
            if (_VCaro.Player == -1)        //random lượt chơi
            {
                MessageBox.Show("CPU đánh trước");
                _VCaro.ComputerMove(-1, difficulty());
            }
            else
            {
                MessageBox.Show("Bạn đánh trước");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //không cho resize form
            this.MaximizeBox = false;       //khong cho phóng to    
            RBEasy.Checked = true;          //độ khó mặc định là dễ
            //this.MinimizeBox = false;
            _VCaro.DrawCaroBoard();     //Vẽ bàn cờ
            MessageBox.Show("Caro Chess 1.0\nSV:Trần Văn Hải\nMSSV:13520232\nLuật chơi: Ai đánh được 5 quân liên tiếp theo bất kỳ hàng ngang dọc chéo (Không bị chặn 2 đầu) sẽ chiến thắng.");  // thông báo
            if (_VCaro.Player == -1)
            {
                MessageBox.Show("CPU đánh trước");
                _VCaro.ComputerMove(-1, 1);
            }
            else
            {
                MessageBox.Show("Bạn đánh trước");
            }
            s = new Stopwatch();
            s.Start();
        }

        private void PNCaroBoard_Paint(object sender, PaintEventArgs e) //sự kiện Paint
        {
            _VCaro.DrawCaroBoard(); //vẽ lại bàn cờ
            _VCaro.reDrawMoves();   //vẽ lại các nước đi
        }

        private void PNCaroBoard_MouseClick(object sender, MouseEventArgs e)        //click lên bàn cờ(đánh cờ)
        {
            if (_VCaro.WinCheck(false)==true) return;  //nếu trò chơi đã kết thúc  thì thoát
            bool a;
            a= _VCaro.Move(e.X, e.Y);       //dánh ô cờ vào vị trí click
            if (_VCaro.WinCheck(true))      //kiểm tra chiến thắng
            {
                _VCaro.GameOver();
                return;
            }
            if (a)  //nếu đánh cờ thành công
            {
                LBCPUTime.Text = "Find move..";
                s.Reset();  //reset đồng hồ
                s.Start(); //đếm thời gian suy nghĩ của máy
                _VCaro.ComputerMove(-1, difficulty());  //tìm nước đi và đánh với độ khó
                s.Stop();
                LBCPUTime.Text=(s.ElapsedMilliseconds/1000).ToString(); //in ra thời gian suy nghĩ của máy
                s.Reset();  //reset
                s.Start();      //đếm thời gian suy nghĩ của người
            }
            if (_VCaro.WinCheck(true))  //kiểm tra chiến thắng
            {
                _VCaro.GameOver();
                return;
            }
        }

        private void BTUndo_Click(object sender, EventArgs e)       //Nút Undo
        {
            _VCaro.Undo();
        }
        private void BTExit_Click(object sender, EventArgs e)       //Nút thoát
        {
            System.Windows.Forms.Application.Exit();
        }

        private void BTHelp_Click(object sender, EventArgs e)   //Nút Help
        {
            MessageBox.Show("Caro Chess 1.0\nSV:Trần Văn Hải\nMSSV:13520232\nLuật chơi: Ai đánh được 5 quân liên tiếp theo bất kỳ hàng ngang dọc chéo (Không bị chặn 2 đầu) sẽ chiến thắng.");
        }

        private void BTHM_Click(object sender, EventArgs e)     //Nút HelpMe 
        {
            if (_VCaro.WinCheck(false)) return;  //nếu trò chơi đã kêt thúc thì thoát
            _VCaro.ComputerMove(1, difficulty());   //dánh nươc đi cho người
            if (_VCaro.WinCheck(true))  //kiểm tra chiến thắng
            {
                _VCaro.GameOver();
                return;
            }
            _VCaro.ComputerMove(-1, difficulty()); //đánh nước đi cho máy
            if (_VCaro.WinCheck(true))      //kiểm tra chiến thắng
            {
                _VCaro.GameOver();
                return;
            }
        }
        private void TMMoveTime_Tick(object sender, EventArgs e)        //in ra thời gian suy nghĩ mỗi giây
        {
            if(_VCaro.Player==1)
            {
                s.Stop();
                LBHMTime.Text = (s.ElapsedMilliseconds / 1000).ToString();
                s.Start();
            }
        }
        private int difficulty()        //xác định độ khó
        {
            if (RBEasy.Checked) return 1;
            else
                if (RBNormal.Checked) return 2;
                else
                    if (RBHard.Checked) return 3;
                    else
                        return 4;
        }

        }
    
}
