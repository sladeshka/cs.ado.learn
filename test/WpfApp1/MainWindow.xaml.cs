using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyDB myDB;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void hConnect(object sender, RoutedEventArgs e)
        {
            myDB = new MyDB(Login.Text, Password.Text);
            try
            {
                myDB.connect.Open();
                string query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name";
                using (NpgsqlCommand command = new NpgsqlCommand(query, myDB.connect))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    List<string> tables = new List<string>();

                    tables.Add("Select table");

                    while (reader.Read())
                    {
                        tables.Add(reader["table_name"].ToString());
                    }

                    listTables.ItemsSource = tables;

                    myDB.connect.Close();
                }

                MessageBox.Show("Connected to database");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }

        private void hGetTableRows(object sender, RoutedEventArgs e)
        {
            myDB.tableName = listTables.SelectedValue.ToString();
            myDB.setColumns();
            string query = $"SELECT * FROM {myDB.tableName};";
            try
            {
                myDB.connect.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, myDB.connect))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        WindowTableEditor window = new();
                        window.myDB = this.myDB;
                        window.dataGrid.ItemsSource = dataTable.DefaultView;
                        window.Show();

                        myDB.connect.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading data from table: " + ex.Message);
            }
        }

        private void listTables_Initialized(object sender, EventArgs e)
        {
            listTables.ItemsSource = new List<string>() { "Select table" };
            listTables.SelectedIndex = 0;
        }

        private void listTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getTableRows.IsEnabled = listTables.SelectedValue != "Select table";
        }
    }
}