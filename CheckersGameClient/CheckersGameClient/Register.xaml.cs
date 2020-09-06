using CheckersGameClient.ServiceReference2;
using CheckersGameServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace CheckersGameClient
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }
        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {   
            CheckersService client = new CheckersService();
            if (client.AddCustomer(tbUsername.Text, tbPassword.Password,tbFirstName.Text,tbLastName.Text,tbCity.Text))
            {
                MessageBox.Show("You have been added to the database,Try logging in.");
                this.Close();
            }
            else
            {
                MessageBox.Show("UserName already exist, please enter another username.");
            }


        }
    }
}
