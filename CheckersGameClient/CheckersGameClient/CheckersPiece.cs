using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckersGameClient
{
    class CheckersPiece
    {
        private Ellipse piece;
        private String color;
        private int x;
        private int y;
        private int turn;
        private Boolean piecePositionCheck;
        private Boolean pieceSelectedCheck;
        public static string path = "C:\\Users\noor2\\Desktop\\WEB + DAMA\\FinalC#\\project v3\\CheckersGameClient\\CheckersGameClient\\pictures";


        public String PieceColor
        {
            get { return color; }
            set { color = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Turn
        {
            get { return turn; }
            set { turn = value; }
        }
        public Ellipse Piece
        {
            get { return piece; }
            set { piece = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public Boolean PiecePositionCheck
        {
            get { return piecePositionCheck; }
            set { piecePositionCheck = value; }
        }
        public Boolean PieceSelectedCheck
        {
            get { return pieceSelectedCheck; }
            set { pieceSelectedCheck = value; }
        }
        public CheckersPiece()
        {
        }
        public CheckersPiece(String color)
        {
            Piece = new Ellipse();
            piece.Width = 45;
            piece.Height = 45;
            this.piecePositionCheck = false;
            X = 0;
            y = 0;
            PieceColor = color;
            if (color == "black")
            {
                turn = 1;
                ImageBrush myBrush = new ImageBrush();
                
                myBrush.ImageSource =
                    new BitmapImage(new Uri(@""+path+"\\blackP.jpg", UriKind.Relative));
                piece.Fill = myBrush;
            }
            if (color == "red")
            {
                turn = 0;
                ImageBrush myBrush = new ImageBrush();
                //string path= System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                myBrush.ImageSource =
                    new BitmapImage(new Uri(@"" + path + "\\whiteP.jpg", UriKind.Relative));
                piece.Fill = myBrush;
            }
            if (color == "empty")
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource =
                    new BitmapImage(new Uri(@"" + path + "\\empty.jpg", UriKind.Relative));
                piece.Fill = myBrush;
                piece.Width = 1;
                piece.Height = 1;
            }
        }
    }
}
