using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using CheckersGameClient.ServiceReference2;
using System.ServiceModel;
using System.ComponentModel;

namespace CheckersGameClient
{
    /// <summary>
    /// Interaction logic for Online.xaml
    /// </summary>
    public partial class Online : Window
    {
        public CheckersServiceClient client { get; set; }

        // CheckersService Client = new CheckersService();
        // public ClientCallback Callback { get; set; }
        public ClientCallback Callback;
        public string Username { get; set; }
        public MainWindow board;
        public delegate void myshow();
        myshow show;
        SolidColorBrush clr = new SolidColorBrush();
        public Online()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Callback = new ClientCallback();
            ClientCallback.displayChallenge = DisplayChallenge;
            ClientCallback.updateUsers = UpdateUsers;
            ClientCallback.updateInfo = UpdateInfo;



            InstanceContext instanceContext = new InstanceContext(new ClientCallback());
            client = new CheckersServiceClient(instanceContext);
            client.setClient(Username);


            //PlayersList.ItemsSource = client.getOnlineUsers(Username);

        }

        private void UpdateInfo(string s)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {

                ProfileList.Items.Clear();
                ProfileList.Items.Add(s);

            }
            ));


        }
        private void UpdateUsers(IEnumerable<string> users)
        {

            Dispatcher.BeginInvoke(new Action(() => this.PlayersList.ItemsSource = users));

        }
        private void UpdateNewStep(double loc)
        {

            //board.updateBallStep(loc);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                client.LogOut(Username);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }

        private void Button_Info(object sender, RoutedEventArgs e)
        {
            client.ShowInfo(Username, PlayersList.SelectedItem as string);

        }

        private void Button_Challenge(object sender, RoutedEventArgs e)
        {
            if (ClientCallback.Challange == true) return;

            bool answer = true;
            //board = new MainWindow();
            //board.newStepInBoard += updateNewStepClient;


            if (PlayersList.SelectedItem != null)
            {
                answer = client.SendChallenge(Username,
                       PlayersList.SelectedItem as string);

                if (answer == true)
                {
                    ClientCallback.Challange = true;
                    MessageBox.Show(PlayersList.SelectedItem as string + " Accepted your CHALLENGE!");
                    //      board.username = Username;
                    //      board.Title = "Connected Four - id: " + Username;
                    //       clr.Color = Color.FromRgb(255, 0, 0);


                    //     Dispatcher.BeginInvoke(new Action(() =>
                    {

                        board = new MainWindow();
                        board.online = true;
                        board.onlinePlayer = PlayersList.SelectedItem as string;
                        board.username = Username;
                        board.Title = "Connected Checkers - id: " + Username;
                        board.PlayerLabel.Content = Username + " Turn";
                        board.playerTurn = 0;
                        //clr.Color = Color.FromRgb(255, 255, 0);
                        //board.CurrentPlayerColor = clr;
                        //board.Client = Client;
                        //Client.LogOut(Username);
                        board.Show();
                        //this.Close();

                    }
                    //));

                    //      board.Client = client;
                    //   show = board.mshow;
                    //client.LogOut(Username);
                    //      board.Show();
                    //   show();
                }
                else MessageBox.Show(PlayersList.SelectedItem as string + " Is AFRAID to play.");
            }

            else
            {
                MessageBox.Show("Please choose a player from the list of Online players.");
            }

        }
        private void DisplayChallenge(string fromClient)
        {

            MessageBoxResult res = MessageBoxResult.None;
            //board = new MainWindow();
            //board.newStepInBoard += updateNewStepClient;


            while (res == MessageBoxResult.None)
            {
                res = (MessageBox.Show(fromClient + " Challenged You,do you accept?", "Game resquest",
                                 MessageBoxButton.YesNo));
                if (res == MessageBoxResult.Yes)
                {
                    ClientCallback.Challange = true;

                    Dispatcher.Invoke(new Action(() =>
                    {
                        //ClientCallback.Challange = true;

                        board = new MainWindow();
                        board.username = Username;
                        board.Title = "Connected Four - id: " + Username;
                        board.onlinePlayer = fromClient;
                        board.online = true;
                        board.PlayerLabel.Content = fromClient + " Turn";
                        board.playerTurn = 1;

                        //clr.Color = Color.FromRgb(255, 255, 0);
                        //board.CurrentPlayerColor = clr;
                        //board.Client = Client;
                        //Client.LogOut(Username);

                        board.Show();
                        //this.Close();

                    }
                    ));
                    //  show = board.mshow;
                    //show();
                    //   board.clrLbl.Foreground = clr;
                    //   board.clrLbl.Content = "You Are Yellow! Good luck.";

                }
                if (res == MessageBoxResult.No) ClientCallback.Challange = false;
            }

        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            client.LogOut(this.Username);

        }

    }
}
