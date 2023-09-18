
using System.Windows;
using System.Data.SqlClient;
using System.Data;

namespace Excursion
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ExcursionsDB.mdf;Integrated Security=True;Connect Timeout=30"); 
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand($"SELECT password FROM Users WHERE login = '{LoginBox.Text}'",con);
            if(cmd.ExecuteScalar() != null )
            {
                if(cmd.ExecuteScalar().ToString() != passwordBox.Password)
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
                else
                {
                    HomeWindow hw = new HomeWindow();
                    hw.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            con.Close();
        }
    }
}
