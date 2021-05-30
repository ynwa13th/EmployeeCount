using System;
using System.Linq;
using System.Windows;
using System.Data;
using System.Data.SqlClient;


namespace EmployeeCount
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PosCombo.ItemsSource = MainWindow.PosList;
            DepCombo.ItemsSource = MainWindow.DepList;
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NameBox.Text.Contains(' ') | NameBox.Text.Length <= 1)
                {
                    MessageBox.Show("Введите имя", "Внимание", MessageBoxButton.OK);
                }
                else if (SurnameBox.Text.Contains(' ') | SurnameBox.Text.Length <= 1)
                {
                    MessageBox.Show("Введите фамилию", "Внимание", MessageBoxButton.OK);
                }
                else if (DepCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите подразделение", "Внимание", MessageBoxButton.OK);
                }
                else if (PosCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите должность", "Внимание", MessageBoxButton.OK);
                }
                else
                {
                    DbConnect.OpenConnection();
                    DbConnect.sql = "INSERT INTO Employees(Name, Surname, Department_Id, Position_Id , Comments) " +
                        "VALUES(@Name, @Surname, " +
                        "(SELECT Id FROM Departments WHERE Name = @DS)," +
                        "(SELECT Id FROM Positions WHERE Name = @PS)," +
                        "@Comment);";
                    DbConnect.cmd.CommandType = CommandType.Text;
                    DbConnect.cmd.CommandText = DbConnect.sql;
                    DbConnect.cmd.Parameters.Clear();
                    DbConnect.cmd.Parameters.AddWithValue("@Name", NameBox.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@Surname", SurnameBox.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@DS", DepCombo.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@PS", PosCombo.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("Comment", CommentBox.Text.ToString());
                    DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                    DbConnect.dt = new DataTable();
                    DbConnect.da.Fill(DbConnect.dt);
                    DbConnect.CloseConnection();
                    MessageBox.Show("Сотрудник добавлен", "Внимание", MessageBoxButton.OK);
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось добавить сотрудника", "Внимание", MessageBoxButton.OK);
            }
        }
    }
}
