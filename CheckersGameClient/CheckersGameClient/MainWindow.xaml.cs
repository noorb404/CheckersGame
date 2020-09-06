using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CheckersGameClient.ServiceReference2;
using System.ServiceModel;
using System.ComponentModel;

namespace CheckersGameClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int tempI;
        private int tempJ;
        private int i2;
        private int j2;
        private int kingI;
        private int turn;
        public int playerTurn;
        private int flag;
        private int doubleEatFlag;
        private int EatCount;

        public CheckersServiceClient Client { get; set; }

        CheckersPiece[,] Board = new CheckersPiece[8, 8];
        public MainWindow()
        {
            flag = 0;
            turn = 0;//white starts
            doubleEatFlag = 0;
            InitializeComponent();
            FillBoard();
            ClientCallback.newStep = UpdateNewStep;
            InstanceContext instanceContext = new InstanceContext(new ClientCallback());
            Client = new CheckersServiceClient(instanceContext);
            Closing += OnWindowClosing;
        }

        public bool online = false; /*true means online . false means offline*/
        public string username { get; set; }
        public string onlinePlayer = "";
        public int onetimeflag = 1;

        public void UpdateNewStep(double x, double y)
        {

            this.Dispatcher.Invoke((Action)(() =>
            {


                int loopFlag = 0;
                if (EatCount >= 10)
                    CheckWinner();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        CheckWinner();
                        if (Board[i, j] != null)
                        {
                            if (flag == 1)
                            {


                                if (Board[i, j].PiecePositionCheck == true && Board[i, j].PieceColor != "empty" && Board[i, j].PieceColor != "brown" && turn == Board[i, j].Turn)
                                {
                                    if (x >= Board[i, j].X - 35 && x <= Board[i, j].X + 35)
                                    {
                                        if (y >= Board[i, j].Y - 35 && y <= Board[i, j].Y + 35)
                                        {
                                            ImageBrush myBrush = new ImageBrush();
                                            myBrush.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackSelected.jpg", UriKind.Relative));
                                            ImageBrush myBrush2 = new ImageBrush();
                                            myBrush2.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteSelected.jpg", UriKind.Relative));
                                            ImageBrush myBrush3 = new ImageBrush();
                                            myBrush3.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackKingPieceSelected.jpg", UriKind.Relative));
                                            ImageBrush myBrush4 = new ImageBrush();
                                            myBrush4.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteKingPieceSelected.jpg", UriKind.Relative));

                                            ImageBrush myBrush11 = new ImageBrush();
                                            myBrush11.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackP.jpg", UriKind.Relative));
                                            ImageBrush myBrush22 = new ImageBrush();
                                            myBrush22.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteP.jpg", UriKind.Relative));
                                            ImageBrush myBrush44 = new ImageBrush();
                                            myBrush44.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteKingPiece.jpg", UriKind.Relative));
                                            ImageBrush myBrush33 = new ImageBrush();
                                            myBrush33.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackKingPiece.jpg", UriKind.Relative));

                                            if (Board[i, j].PieceColor.Equals("red"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush2;
                                                if (Board[tempI, tempJ].PieceColor.Equals("red"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush22;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush44;
                                            }
                                            if (Board[i, j].PieceColor.Equals("black"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush;
                                                if (Board[tempI, tempJ].PieceColor.Equals("black"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush11;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush33;
                                            }
                                            if (Board[i, j].PieceColor.Equals("kingRed"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush4;
                                                if (Board[tempI, tempJ].PieceColor.Equals("kingRed"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush44;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush22;

                                            }
                                            if (Board[i, j].PieceColor.Equals("kingBlack"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush3;
                                                if (Board[tempI, tempJ].PieceColor.Equals("kingBlack"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush33;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush11;
                                            }
                                            Board[i, j].PieceSelectedCheck = true;
                                            tempI = i;
                                            tempJ = j;
                                            flag = 1;
                                            loopFlag = 1;

                                        }
                                    }
                                }




                                else if (x >= Board[i, j].X - 10 && x <= Board[i, j].X + 50)
                                {
                                    if (y >= Board[i, j].Y - 10 && y <= Board[i, j].Y + 50)
                                    {
                                        if (((Board[tempI, tempJ].PieceColor == "red" || Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed") && ((tempI == i - 1 && tempJ == j - 1) || (tempI == i - 1 && tempJ == j + 1)))
                                            || ((Board[tempI, tempJ].PieceColor == "black" || Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed")
                                            && ((tempI == i + 1 && tempJ == j - 1) || (tempI == i + 1 && tempJ == j + 1))))
                                        {
                                            if (Board[i, j].PieceColor == "empty")
                                            {
                                                kingI = i;
                                                doubleEatFlag = 0;
                                                Console.WriteLine(i + " " + j);
                                                //MessageBox.Show(i + " " + j+ ","+tempI+" "+tempJ);
                                                // sendDataToServer(tempI + "," + tempJ + "," + i + ","+j);
                                                ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                                                loopFlag = 1;
                                                break;
                                            }
                                        }
                                        /////////////////////


                                        ////////////////////////////////////////////////
                                        if ((Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed") && (checkRadius(i, j) == true))
                                        {
                                            if (Board[i, j].PieceColor == "empty")
                                            {
                                                kingI = i;
                                                doubleEatFlag = 0;
                                                ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                                                loopFlag = 1;
                                                break;
                                            }

                                        }

                                        doubleEatFlag = 1;
                                        if ((Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed") && (eatRadius(i, j) == true))
                                        {
                                            i2 = i;
                                            j2 = j;
                                            loopFlag = 1;
                                            break;
                                        }
                                        //////////////////////////////////////////////////
                                        doubleEatFlag = 1;
                                        if (EatPiece(i, j) == 1)
                                        {

                                            i2 = i;
                                            j2 = j;
                                            loopFlag = 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (flag != 1)
                            {
                                if (turn == Board[i, j].Turn)
                                {
                                    if (Board[i, j].PiecePositionCheck == true && Board[i, j].PieceColor != "empty" && Board[i, j].PieceColor != "brown")
                                    {
                                        if (x >= Board[i, j].X - 35 && x <= Board[i, j].X + 35)
                                        {
                                            if (y >= Board[i, j].Y - 35 && y <= Board[i, j].Y + 35)
                                            {
                                                ImageBrush myBrush = new ImageBrush();
                                                myBrush.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackSelected.jpg", UriKind.Relative));
                                                ImageBrush myBrush2 = new ImageBrush();
                                                myBrush2.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteSelected.jpg", UriKind.Relative));
                                                ImageBrush myBrush3 = new ImageBrush();
                                                myBrush3.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackKingPieceSelected.jpg", UriKind.Relative));
                                                ImageBrush myBrush4 = new ImageBrush();
                                                myBrush4.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteKingPieceSelected.jpg", UriKind.Relative));
                                                if (Board[i, j].PieceColor.Equals("red"))
                                                    Board[i, j].Piece.Fill = myBrush2;
                                                if (Board[i, j].PieceColor.Equals("black"))
                                                    Board[i, j].Piece.Fill = myBrush;
                                                if (Board[i, j].PieceColor.Equals("kingRed"))
                                                    Board[i, j].Piece.Fill = myBrush4;
                                                if (Board[i, j].PieceColor.Equals("kingBlack"))
                                                    Board[i, j].Piece.Fill = myBrush3;
                                                Board[i, j].PieceSelectedCheck = true;
                                                tempI = i;
                                                tempJ = j;
                                                flag = 1;
                                                loopFlag = 1;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (loopFlag == 1) break;
                    }
                    if (loopFlag == 1)
                    {
                        loopFlag = 0;
                        break;
                    }
                }
            }));
        }

        public void FillBoard()
        {
            /*  Array Board Start  */
            int j = 0, k = 0, i = 0, xplus = 0, yplus = 0, tempPlus = 0;
            string color = "red";
            for (j = 0; j < 9; j++)
            {
                if (j == 8)
                {
                    i++;
                    if (i == 3) { color = "empty"; }
                    if (i == 5) { color = "black"; }
                    if (i == 8) break;
                    j = 0;
                    xplus = 0;
                    tempPlus = 0;
                    yplus += 55;
                }

                if (i % 2 == 0)
                {
                    if (j % 2 == k)
                    {
                        Board[i, j] = new CheckersPiece(color);
                        if (color != "empty")
                            Board[i, j].PiecePositionCheck = true;
                        else
                            Board[i, j].PiecePositionCheck = false;
                        Board[i, j].X = 33 + xplus;
                        Board[i, j].Y = 33 + yplus;
                        xplus += 110;
                        xplus += tempPlus;
                        tempPlus += 2;
                    }
                    else
                    {
                        Board[i, j] = new CheckersPiece("brown");
                        Board[i, j].PiecePositionCheck = false;
                        Board[i, j].X = 0;
                        Board[i, j].Y = 0;
                    }
                }
                else
                {
                    if (j % 2 == k + 1)
                    {
                        Board[i, j] = new CheckersPiece(color);
                        if (color != "empty")
                            Board[i, j].PiecePositionCheck = true;
                        else
                            Board[i, j].PiecePositionCheck = false;
                        Board[i, j].X = 88 + xplus;
                        Board[i, j].Y = 33 + yplus;
                        xplus += 110;
                        xplus += tempPlus;
                        tempPlus += 2;
                    }
                    else
                    {
                        Board[i, j] = new CheckersPiece("brown");
                        Board[i, j].PiecePositionCheck = false;
                        Board[i, j].X = 0;
                        Board[i, j].Y = 0;

                    }
                }
            }
            j = 0;
            for (i = 0; i < 9; i++)
            {
                if (i == 8)
                {
                    j++;

                    if (j == 8) break;
                    i = 0;
                }
                if (Board[i, j] != null)
                    if (Board[i, j].PiecePositionCheck == true)
                    {
                        //Console.WriteLine(Board[i, j].PieceColor);
                        Canvas.SetTop(Board[i, j].Piece, Board[i, j].Y);
                        Canvas.SetLeft(Board[i, j].Piece, Board[i, j].X);
                        BoardCanvas.Children.Add(Board[i, j].Piece);
                    }
            }
        }
        CheckersPiece removePiece(CheckersPiece piece)
        {

            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\empty.jpg", UriKind.Relative));
            piece.Piece.Fill = myBrush;
            piece.Piece.Width = 1;
            piece.Piece.Height = 1;
            piece.PieceColor = "empty";
            piece.PiecePositionCheck = false;
            return piece;
        }
        //check if eating is impossible after setting of tempi, tempj 
        int checkPiece()
        {
            // Console.WriteLine("TEMPIII== " + tempI + "  TEMPJJJ==" + tempJ);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if ((Board[tempI, tempJ].PieceColor == "red" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                                        && tempI == i - 2 && tempJ == j - 2)
                    {
                        if (Board[i - 1, j - 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i - 1, j - 1].PieceColor != "empty")
                        {
                            if (Board[i, j].PieceColor == "empty")
                            {
                                return 1;
                            }
                        }
                    }
                    else if ((Board[tempI, tempJ].PieceColor == "red" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                        && tempI == i - 2 && tempJ == j + 2)
                    {
                        if (Board[i - 1, j + 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i - 1, j + 1].PieceColor != "empty")
                        {
                            if (Board[i, j].PieceColor == "empty")
                            {
                                return 1;
                            }
                        }
                    }
                    else if ((Board[tempI, tempJ].PieceColor == "black" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                        && tempI == i + 2 && tempJ == j + 2)
                    {
                        if (Board[i + 1, j + 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i + 1, j + 1].PieceColor != "empty")
                        {
                            if (Board[i, j].PieceColor == "empty")
                            {
                                return 1;
                            }
                        }
                    }
                    else if ((Board[tempI, tempJ].PieceColor == "black" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                        && tempI == i + 2 && tempJ == j - 2)
                    {
                        if (Board[i + 1, j - 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i + 1, j - 1].PieceColor != "empty")
                        {
                            if (Board[i, j].PieceColor == "empty")
                            {
                                return 1;
                            }
                        }
                    }
                }
            }
            return 0;
        }
        //eat after and move from tempi, tempj to i,j 
        int EatPiece(int i, int j)
        {
            /*CODE FOR DELETING PIECE (EATING)*/
            if ((Board[tempI, tempJ].PieceColor == "red" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                                        && tempI == i - 2 && tempJ == j - 2)
            {
                if (Board[i - 1, j - 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i - 1, j - 1].PieceColor != "empty")
                {
                    if (Board[i, j].PieceColor == "empty")
                    {
                        kingI = i;
                        Board[i - 1, j - 1] = removePiece(Board[i - 1, j - 1]);
                        Canvas.SetTop(Board[i - 1, j - 1].Piece, Board[i - 1, j - 1].Y);
                        Canvas.SetLeft(Board[i - 1, j - 1].Piece, Board[i - 1, j - 1].X);
                        ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                        return 1;
                    }
                }
            }
            else if ((Board[tempI, tempJ].PieceColor == "red" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                && tempI == i - 2 && tempJ == j + 2)
            {
                if (Board[i - 1, j + 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i - 1, j + 1].PieceColor != "empty")
                {
                    if (Board[i, j].PieceColor == "empty")
                    {
                        kingI = i;
                        Board[i - 1, j + 1] = removePiece(Board[i - 1, j + 1]);
                        Canvas.SetTop(Board[i - 1, j + 1].Piece, Board[i - 1, j + 1].Y);
                        Canvas.SetLeft(Board[i - 1, j + 1].Piece, Board[i - 1, j + 1].X);
                        ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                        return 1;
                    }
                }
            }
            else if ((Board[tempI, tempJ].PieceColor == "black" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                && tempI == i + 2 && tempJ == j + 2)
            {
                if (Board[i + 1, j + 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i + 1, j + 1].PieceColor != "empty")
                {
                    if (Board[i, j].PieceColor == "empty")
                    {
                        kingI = i;
                        Board[i + 1, j + 1] = removePiece(Board[i + 1, j + 1]);
                        Canvas.SetTop(Board[i + 1, j + 1].Piece, Board[i + 1, j + 1].Y);
                        Canvas.SetLeft(Board[i + 1, j + 1].Piece, Board[i + 1, j + 1].X);
                        ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                        return 1;
                    }
                }
            }
            else if ((Board[tempI, tempJ].PieceColor == "black" || Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack")
                && tempI == i + 2 && tempJ == j - 2)
            {
                if (Board[i + 1, j - 1].PieceColor != Board[tempI, tempJ].PieceColor && Board[i + 1, j - 1].PieceColor != "empty")
                {
                    if (Board[i, j].PieceColor == "empty")
                    {
                        kingI = i;
                        Board[i + 1, j - 1] = removePiece(Board[i + 1, j - 1]);
                        Canvas.SetTop(Board[i + 1, j - 1].Piece, Board[i + 1, j - 1].Y);
                        Canvas.SetLeft(Board[i + 1, j - 1].Piece, Board[i + 1, j - 1].X);
                        ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                        return 1;

                    }
                }
            }
            return 0;
        }
        private void CheckWinner()
        {
            int blackCount = 0, WhiteCount = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j].PieceColor == "red" || Board[i, j].PieceColor == "kingRed") WhiteCount++;
                    if (Board[i, j].PieceColor == "black" || Board[i, j].PieceColor == "kingBlack") blackCount++;
                }
            }
            if (blackCount == 0)
            {


                if (playerTurn == 0)
                {
                    Client.UpdateWin(username);
                    Client.UpdateLose(onlinePlayer);
                    WinnerLabel.Content = username + " Wins!";
                }
                else WinnerLabel.Content = onlinePlayer + " Wins!";


            }
            else if (WhiteCount == 0)
            {

                if (playerTurn == 0)
                {
                    Client.UpdateWin(onlinePlayer);
                    Client.UpdateLose(username);
                    WinnerLabel.Content = onlinePlayer + " Wins!";
                }
                else WinnerLabel.Content = username + " Wins!";

            }
        }
        /* CHECK SELECTED PIECE AND MARK IT */
        private void BoardCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (turn == playerTurn || !online)
            {

                System.Windows.Point p = e.GetPosition(BoardCanvas);
                if (online == true) Client.AddStepToTheBoard(p.X, p.Y, this.username);
                int loopFlag = 0;
                if (EatCount >= 10)
                    CheckWinner();


                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Board[i, j] != null)
                        {
                            if (flag == 1)
                            {

                                //select other piece 
                                if (Board[i, j].PiecePositionCheck == true && Board[i, j].PieceColor != "empty" && Board[i, j].PieceColor != "brown" && turn == Board[i, j].Turn)
                                {
                                    if (p.X >= Board[i, j].X - 35 && p.X <= Board[i, j].X + 35)
                                    {
                                        if (p.Y >= Board[i, j].Y - 35 && p.Y <= Board[i, j].Y + 35)
                                        {
                                            ImageBrush myBrush = new ImageBrush();
                                            myBrush.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackSelected.jpg", UriKind.Relative));
                                            ImageBrush myBrush2 = new ImageBrush();
                                            myBrush2.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteSelected.jpg", UriKind.Relative));
                                            ImageBrush myBrush3 = new ImageBrush();
                                            myBrush3.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackKingPieceSelected.jpg", UriKind.Relative));
                                            ImageBrush myBrush4 = new ImageBrush();
                                            myBrush4.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteKingPieceSelected.jpg", UriKind.Relative));

                                            ImageBrush myBrush11 = new ImageBrush();
                                            myBrush11.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackP.jpg", UriKind.Relative));
                                            ImageBrush myBrush22 = new ImageBrush();
                                            myBrush22.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteP.jpg", UriKind.Relative));
                                            ImageBrush myBrush44 = new ImageBrush();
                                            myBrush44.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteKingPiece.jpg", UriKind.Relative));
                                            ImageBrush myBrush33 = new ImageBrush();
                                            myBrush33.ImageSource =
                                                new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackKingPiece.jpg", UriKind.Relative));

                                            if (Board[i, j].PieceColor.Equals("red"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush2;
                                                if (Board[tempI, tempJ].PieceColor.Equals("red"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush22;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush44;
                                            }
                                            if (Board[i, j].PieceColor.Equals("black"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush;
                                                if (Board[tempI, tempJ].PieceColor.Equals("black"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush11;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush33;
                                            }
                                            if (Board[i, j].PieceColor.Equals("kingRed"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush4;
                                                if (Board[tempI, tempJ].PieceColor.Equals("kingRed"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush44;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush22;

                                            }
                                            if (Board[i, j].PieceColor.Equals("kingBlack"))
                                            {
                                                Board[i, j].Piece.Fill = myBrush3;
                                                if (Board[tempI, tempJ].PieceColor.Equals("kingBlack"))
                                                    Board[tempI, tempJ].Piece.Fill = myBrush33;
                                                else Board[tempI, tempJ].Piece.Fill = myBrush11;
                                            }
                                            Board[i, j].PieceSelectedCheck = true;
                                            tempI = i;
                                            tempJ = j;
                                            flag = 1;
                                            loopFlag = 1;

                                        }
                                    }
                                }




                                else if (p.X >= Board[i, j].X - 10 && p.X <= Board[i, j].X + 50)
                                {
                                    if (p.Y >= Board[i, j].Y - 10 && p.Y <= Board[i, j].Y + 50)
                                    {
                                        if (((Board[tempI, tempJ].PieceColor == "red" || Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed") && ((tempI == i - 1 && tempJ == j - 1) || (tempI == i - 1 && tempJ == j + 1)))
                                            || ((Board[tempI, tempJ].PieceColor == "black" || Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed")
                                            && ((tempI == i + 1 && tempJ == j - 1) || (tempI == i + 1 && tempJ == j + 1))))
                                        {
                                            if (Board[i, j].PieceColor == "empty")
                                            {
                                                kingI = i;
                                                doubleEatFlag = 0;
                                                Console.WriteLine(i + " " + j);
                                                //MessageBox.Show(i + " " + j+ ","+tempI+" "+tempJ);
                                                // sendDataToServer(tempI + "," + tempJ + "," + i + ","+j);
                                                ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                                                loopFlag = 1;
                                                break;
                                            }
                                        }
                                        /////////////////////


                                        ////////////////////////////////////////////////
                                        if ((Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed") && (checkRadius(i,j)==true))
                                        {
                                            if (Board[i, j].PieceColor == "empty")
                                            {
                                                kingI = i;
                                                doubleEatFlag = 0;
                                                ThreadPool.QueueUserWorkItem(movePiece, Board[i, j]);
                                                loopFlag = 1;
                                                break;
                                            }

                                        }

                                        doubleEatFlag = 1;
                                        if ((Board[tempI, tempJ].PieceColor == "kingBlack" || Board[tempI, tempJ].PieceColor == "kingRed") && (eatRadius(i, j) == true))
                                        {
                                            i2 = i;
                                            j2 = j;
                                            loopFlag = 1;
                                            break;
                                        }
                                        //////////////////////////////////////////////////
                                        doubleEatFlag = 1;
                                        if (EatPiece(i, j) == 1)
                                        {

                                            i2 = i;
                                            j2 = j;
                                            loopFlag = 1;
                                            break;
                                        }
                                        ////////////
                                    }
                                }
                            }
                            if (flag != 1)
                            {
                                if (turn == Board[i, j].Turn)
                                {
                                    if (Board[i, j].PiecePositionCheck == true && Board[i, j].PieceColor != "empty" && Board[i, j].PieceColor != "brown")
                                    {
                                        if (p.X >= Board[i, j].X - 35 && p.X <= Board[i, j].X + 35)
                                        {
                                            if (p.Y >= Board[i, j].Y - 35 && p.Y <= Board[i, j].Y + 35)
                                            {
                                                ImageBrush myBrush = new ImageBrush();
                                                myBrush.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackSelected.jpg", UriKind.Relative));
                                                ImageBrush myBrush2 = new ImageBrush();
                                                myBrush2.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteSelected.jpg", UriKind.Relative));
                                                ImageBrush myBrush3 = new ImageBrush();
                                                myBrush3.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackKingPieceSelected.jpg", UriKind.Relative));
                                                ImageBrush myBrush4 = new ImageBrush();
                                                myBrush4.ImageSource =
                                                    new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteKingPieceSelected.jpg", UriKind.Relative));
                                                if (Board[i, j].PieceColor.Equals("red"))
                                                    Board[i, j].Piece.Fill = myBrush2;
                                                if (Board[i, j].PieceColor.Equals("black"))
                                                    Board[i, j].Piece.Fill = myBrush;
                                                if (Board[i, j].PieceColor.Equals("kingRed"))
                                                    Board[i, j].Piece.Fill = myBrush4;
                                                if (Board[i, j].PieceColor.Equals("kingBlack"))
                                                    Board[i, j].Piece.Fill = myBrush3;
                                                Board[i, j].PieceSelectedCheck = true;
                                                tempI = i;
                                                tempJ = j;
                                                flag = 1;
                                                loopFlag = 1;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (loopFlag == 1) break;
                    }
                    if (loopFlag == 1)
                    {
                        loopFlag = 0;
                        break;
                    }
                }

            }
        }

        int checkKingPiece()
        {

            if (!(Board[tempI, tempJ].PieceColor == "kingRed" || Board[tempI, tempJ].PieceColor == "kingBlack"))
                return 0;

            int vectumI = -1;
            int vectumJ = -1;


            for (int i = (tempI + 1), j = (tempJ + 1); i < 7 && i>0 && j<7 && j>0; ++i, ++j)
            {
                if (Board[i, j].PieceColor != "empty" && (Board[i + 1, j + 1].PieceColor == "empty"))
                {
                    vectumI = i;
                    vectumJ = j;
                    break;
                }
            }
            if (vectumI != -1 && Board[vectumI, vectumJ].PieceColor != Board[tempI, tempJ].PieceColor)
                return 1;


            vectumI = -1; vectumJ = -1;
            for (int i = (tempI - 1), j = (tempJ - 1); i < 7 && i > 0 && j < 7 && j > 0; i -= 1, j -= 1)    
            {
                if (Board[i, j].PieceColor != "empty"&& (Board[i - 1, j - 1].PieceColor == "empty"))
                {
                    vectumI = i;
                    vectumJ = j;
                    break;
                }
            }
            if (vectumI != -1 && Board[vectumI, vectumJ].PieceColor != Board[tempI, tempJ].PieceColor)
                return 1;


            vectumI = -1; vectumJ = -1;
            for (int i = (tempI - 1), j = (tempJ + 1); i < 7 && i > 0 && j < 7 && j > 0; i -= 1, j+= 1)
            {
                if (Board[i, j].PieceColor != "empty" && (Board[i - 1, j + 1].PieceColor == "empty"))
                {
                    vectumI = i;
                    vectumJ = j;
                    break;
                }
            }
            if (vectumI != -1 && Board[vectumI, vectumJ].PieceColor != Board[tempI, tempJ].PieceColor)
                return 1;


            vectumI = -1; vectumJ = -1;
            for (int i = (tempI + 1), j = (tempJ - 1); i < 7 && i > 0 && j < 7 && j > 0; i += 1, j -= 1)
            {
                if (Board[i, j].PieceColor != "empty" && (Board[i + 1, j - 1].PieceColor == "empty"))
                {
                    vectumI = i;
                    vectumJ = j;
                    break;
                }
            }
            if (vectumI != -1 && Board[vectumI, vectumJ].PieceColor != Board[tempI, tempJ].PieceColor)
                return 1;

            return 0;
        }

        bool checkRadius(int newI, int newJ)
        {
            if (!((newI - tempI == newJ - tempJ)|| (newI - tempI == -(newJ - tempJ)))) return false;
            int vectumI = -1;
            int vectumJ = -1;
            int checkJ;
            int checkI;

            if (newJ > tempJ) checkJ = 1;
            else checkJ = -1;

            if (newI > tempI) checkI = 1;
            else checkI = -1;

            for (int i = (tempI+checkI), j = (tempJ+checkJ); i != newI; i += checkI, j += checkJ)
            {
                if (Board[i, j].PieceColor != "empty")
                {
                    if (vectumI != -1) return false;

                    vectumI = i;
                    vectumJ = j;
                }
            }
            if (vectumI != -1) return false;
            else return true;
        }

        bool eatRadius(int newI, int newJ)
        {
            if (!((newI - tempI == newJ - tempJ) || (newI - tempI == -(newJ - tempJ)))) return false;
            int vectumI = -1;
            int vectumJ = -1;
            int checkJ;
            int checkI;

            if (newJ > tempJ) checkJ = 1;
            else checkJ = -1;

            if (newI > tempI) checkI = 1;
            else checkI = -1;

            for (int i = (tempI + checkI), j = (tempJ + checkJ); i != newI; i += checkI, j += checkJ)
            {
                if (Board[i, j].PieceColor != "empty")
                {
                    if (vectumI != -1) return false;

                    vectumI = i;
                    vectumJ = j;
                }
            }

            if (vectumI != -1 && Board[newI, newJ].PieceColor == "empty" && Board[vectumI, vectumJ].PieceColor != Board[tempI, tempJ].PieceColor)
            {
                if (Board[vectumI, vectumJ].PieceColor == "red" && Board[tempI, tempJ].PieceColor == "kingRed")
                    return false;
                if (Board[vectumI, vectumJ].PieceColor == "black" && Board[tempI, tempJ].PieceColor == "kingBlack")
                    return false;

                kingI = newI;
                Board[vectumI, vectumJ] = removePiece(Board[vectumI, vectumJ]);
                Canvas.SetTop(Board[vectumI, vectumJ].Piece, Board[vectumI, vectumJ].Y);
                Canvas.SetLeft(Board[vectumI, vectumJ].Piece, Board[vectumI, vectumJ].X);
                ThreadPool.QueueUserWorkItem(movePiece, Board[newI, newJ]);
                return true;
            }

            return false;
        }
        void movePiece(object obj)
        {
            CheckersPiece checkersPiece = (CheckersPiece)obj;

            if (!checkersPiece.PieceColor.Equals("brown"))
            {
                this.Dispatcher.Invoke((Action)(() =>
                {

                    checkersPiece.Piece = Board[tempI, tempJ].Piece;
                    checkersPiece.PieceColor = Board[tempI, tempJ].PieceColor;
                    checkersPiece.Turn = Board[tempI, tempJ].Turn;
                    checkersPiece.PiecePositionCheck = true;
                    Board[tempI, tempJ].PieceColor = "empty";
                    Board[tempI, tempJ].PiecePositionCheck = false;

                    if (doubleEatFlag == 1)
                    {
                        EatCount++;
                        tempI = i2;
                        tempJ = j2;
                    }
                    if (checkPiece() == 0 && checkKingPiece()==0)
                    {
                        flag = 0;
                        checkersPiece.PieceSelectedCheck = false;
                        if (turn == 0)
                        {
                            turn = 1;
                            if (!online)

                                PlayerLabel.Content = "Black Turn";
                            else
                            {
                                if (playerTurn == 1)
                                    PlayerLabel.Content = username + " Turn";
                                else PlayerLabel.Content = onlinePlayer + " Turn";
                            }
                            PlayerLabel.Foreground = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            turn = 0;
                            if (!online)
                                PlayerLabel.Content = "White Turn";
                            else
                            {
                                if (playerTurn == 0)
                                    PlayerLabel.Content = username + " Turn";
                                else PlayerLabel.Content = onlinePlayer + " Turn";
                            }
                            PlayerLabel.Foreground = new SolidColorBrush(Colors.White);

                        }
                        ImageBrush myBrush = new ImageBrush();
                        myBrush.ImageSource =
                            new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackP.jpg", UriKind.Relative));
                        ImageBrush myBrush2 = new ImageBrush();
                        myBrush2.ImageSource =
                            new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteP.jpg", UriKind.Relative));
                        ImageBrush myBrush3 = new ImageBrush();
                        myBrush3.ImageSource =
                            new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\whiteKingPiece.jpg", UriKind.Relative));
                        ImageBrush myBrush4 = new ImageBrush();
                        myBrush4.ImageSource =
                            new BitmapImage(new Uri(@"" + CheckersPiece.path + "\\blackKingPiece.jpg", UriKind.Relative));


                        if (checkersPiece.PieceColor.Equals("red"))
                        {

                            if (kingI == 7)
                            {
                                checkersPiece.PieceColor = "kingRed";
                                checkersPiece.Piece.Fill = myBrush3;
                            }
                            else
                            {
                                checkersPiece.Piece.Fill = myBrush2;
                            }
                        }
                        else if (checkersPiece.PieceColor.Equals("kingRed"))
                        {
                            checkersPiece.Piece.Fill = myBrush3;
                        }
                        else if (checkersPiece.PieceColor.Equals("kingBlack"))
                        {
                            checkersPiece.Piece.Fill = myBrush4;
                        }
                        else if (checkersPiece.PieceColor.Equals("black"))
                        {
                            if (kingI == 0)
                            {
                                checkersPiece.PieceColor = "kingBlack";
                                checkersPiece.Piece.Fill = myBrush4;
                            }
                            else
                            {
                                checkersPiece.Piece.Fill = myBrush;
                            }
                        }
                    }

                    Canvas.SetTop(checkersPiece.Piece, checkersPiece.Y);
                    Canvas.SetLeft(checkersPiece.Piece, checkersPiece.X);
                }));

            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (online == true) ClientCallback.Challange = false;
            Client.outOfChallenge(this.username, onlinePlayer);
        }
    }


}
