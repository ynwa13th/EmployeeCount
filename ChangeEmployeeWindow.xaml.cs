using System;
using System.Windows;
using System.Data;
using System.Data.SqlClient;


namespace EmployeeCount
{
    public partial class ChangeEmployeeWindow : Window
    {
        string NameToChange;
        string SurnameToChange;
        string SelectedPos;
        string SelectedDep;
        string SelectedComments;
        string SelectedId;
        public ChangeEmployeeWindow()
        {
            InitializeComponent();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((NameBox.Text != NameToChange) ||
                   (SurnameBox.Text != SurnameToChange) ||
                   (PosCombo.SelectedItem.ToString() != SelectedPos) ||
                   (DepCombo.SelectedItem.ToString() != SelectedDep) ||
                   (CommentBox.Text != SelectedComments))
                {
                    DbConnect.OpenConnection();
                    DbConnect.sql = "UPDATE Employees " +
                        "SET Name=@Name, Surname=@Surname, Department_Id=(SELECT Id FROM Departments WHERE Name = @DS), " +
                        "Position_Id=(SELECT Id FROM Positions WHERE Name = @PS) , Comments=@Comment " +
                        "WHERE Employees.Id = @Id";
                    DbConnect.cmd.CommandType = CommandType.Text;
                    DbConnect.cmd.CommandText = DbConnect.sql;
                    DbConnect.cmd.Parameters.Clear();
                    DbConnect.cmd.Parameters.AddWithValue("@Name", NameBox.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@Surname", SurnameBox.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@DS", DepCombo.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@PS", PosCombo.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@Comment", CommentBox.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@Id", SelectedId.ToString());
                    DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                    DbConnect.dt = new DataTable();
                    DbConnect.da.Fill(DbConnect.dt);
                    DbConnect.CloseConnection();
                    MessageBox.Show("Сотрудник изменен", "Внимание", MessageBoxButton.OK);
                    Close();
                }
                else MessageBox.Show("Внесите изменения", "Внимание", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex, "Ошибка", MessageBoxButton.OK);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PosCombo.ItemsSource = MainWindow.PosList;
                DepCombo.ItemsSource = MainWindow.DepList;
                SelectedId = Convert.ToString(MainWindow.Selected.Row["Id"]);
                NameToChange = Convert.ToString(MainWindow.Selected.Row["Name"]);
                SurnameToChange = Convert.ToString(MainWindow.Selected.Row["Surname"]);
                SelectedPos = Convert.ToString(MainWindow.Selected.Row["PosName"]);
                SelectedDep = Convert.ToString(MainWindow.Selected.Row["DepName"]);
                SelectedComments = Convert.ToString(MainWindow.Selected.Row["Comments"]);
                NameBox.Text = NameToChange;
                SurnameBox.Text = SurnameToChange;
                PosCombo.SelectedItem = SelectedPos;
                DepCombo.SelectedItem = SelectedDep;
                CommentBox.Text = SelectedComments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex, "Ошибка", MessageBoxButton.OK);
            }
        }
        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
