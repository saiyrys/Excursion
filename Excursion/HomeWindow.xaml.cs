using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Messaging;

namespace Excursion
{
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
            LoadData();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ExcursionsDB.mdf;Integrated Security=True;Connect Timeout=30");
        DataTable dt = new DataTable("testTable");

        public void LoadData()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Excursions.id,Title,typeName,cost FROM Excursions " + "JOIN Excursionstypes ON Excursions.TypeId = ExcursionsTypes.id", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            dGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void dGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int selectedId = Convert.ToInt32(dt.Rows[dGrid.SelectedIndex][0]);
            con.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Description FROM Excursions WHERE id = {selectedId}", con);
            DescBox.Text = cmd.ExecuteScalar().ToString();
            con.Close();
        }

        private void Deletedbtn_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dGrid.SelectedItem;
                int selectedId = Convert.ToInt32(selectedRow["id"]);

                con.Open();
                SqlCommand cmd = new SqlCommand($"DELETE FROM Excursions WHERE id = {selectedId}", con);
                cmd.ExecuteNonQuery();
                con.Close();

                DataRow[] rows = dt.Select($"id = {selectedId}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    dt.AcceptChanges();
                }
            }
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleEx.Text) || string.IsNullOrWhiteSpace(descEx.Text) ||
        string.IsNullOrWhiteSpace(dateEx.Text) || string.IsNullOrWhiteSpace(timeEx.Text) ||
        typeEx.SelectedItem == null)
            {
                MessageBox.Show("Перед добавлением проверьте, все ли поля заполнены");
                return;
            }
            con.Open();
            var dateTime = Convert.ToDateTime($"{dateEx.Text} {timeEx.Text}");
            SqlCommand cmd = new SqlCommand($"INSERT INTO Excursions VALUES(N'{titleEx.Text}',N'{descEx.Text}','{dateTime.ToString("yyyy-MM-dd HH:mm")}',(" + $"SELECT id FROM ExcursionsTypes WHERE typeName = N'{typeEx.Text}'))", con);
            cmd.ExecuteNonQuery();
            con.Close(); 

            LoadData();
        }
        
    }
}
