using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeCount
{
    public partial class MainWindow : Window
    {
        public static List<string> PosList = new List<string>();
        public static List<string> DepList = new List<string>();
        public static DataRowView Selected;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnect.OpenConnection();
                if (PositionCombo.SelectedIndex == -1 & DepartmentCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("Не заданы параметры выбора", "Внимание", MessageBoxButton.OK);
                }
                else if (PositionCombo.SelectedIndex == -1 & DepartmentCombo.SelectedIndex != -1)
                {

                    DbConnect.sql = "SELECT Employees.Id AS 'Id', Employees.Name AS 'Name', Employees.Surname AS 'Surname', Departments.Name AS 'DepName', Positions.Name AS 'PosName', Employees.Comments AS 'Comments'" +
                   "FROM Departments INNER JOIN Employees ON Departments.Id = Employees.Department_id INNER JOIN " +
                   "Positions ON Employees.Position_id = Positions.Id WHERE Departments.Name = @DS;";
                    DbConnect.cmd.CommandType = CommandType.Text;
                    DbConnect.cmd.Parameters.Clear();
                    DbConnect.cmd.Parameters.AddWithValue("@DS", DepartmentCombo.Text.ToString());
                    DbConnect.cmd.CommandText = DbConnect.sql;
                    DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                    DbConnect.dt = new DataTable();
                    DbConnect.da.Fill(DbConnect.dt);
                    EmployeesGrid.ItemsSource = DbConnect.dt.DefaultView;
                }
                else if (PositionCombo.SelectedIndex != -1 & DepartmentCombo.SelectedIndex == -1)
                {
                    DbConnect.sql = "SELECT  Employees.Id AS 'Id',Employees.Name AS 'Name', Employees.Surname AS 'Surname', Departments.Name AS 'DepName', Positions.Name AS 'PosName', Employees.Comments AS 'Comments'" +
                   "FROM Departments INNER JOIN Employees ON Departments.Id = Employees.Department_id INNER JOIN " +
                   "Positions ON Employees.Position_id = Positions.Id WHERE Positions.Name = @PS;";
                    DbConnect.cmd.CommandType = CommandType.Text;
                    DbConnect.cmd.Parameters.Clear();
                    DbConnect.cmd.Parameters.AddWithValue("@PS", PositionCombo.Text.ToString());
                    DbConnect.cmd.CommandText = DbConnect.sql;
                    DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                    DbConnect.dt = new DataTable();
                    DbConnect.da.Fill(DbConnect.dt);
                    EmployeesGrid.ItemsSource = DbConnect.dt.DefaultView;
                }
                else
                {
                    DbConnect.sql = "SELECT  Employees.Id AS 'Id',Employees.Name AS 'Name', Employees.Surname AS 'Surname', Departments.Name AS 'DepName', Positions.Name AS 'PosName', Employees.Comments AS 'Comments'" +
                   "FROM Departments INNER JOIN Employees ON Departments.Id = Employees.Department_id INNER JOIN " +
                   "Positions ON Employees.Position_id = Positions.Id WHERE Departments.Name = @DS and Positions.Name = @PS;";
                    DbConnect.cmd.CommandType = CommandType.Text;
                    DbConnect.cmd.Parameters.Clear();
                    DbConnect.cmd.Parameters.AddWithValue("@DS", DepartmentCombo.Text.ToString());
                    DbConnect.cmd.Parameters.AddWithValue("@PS", PositionCombo.Text.ToString());
                    DbConnect.cmd.CommandText = DbConnect.sql;
                    DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                    DbConnect.dt = new DataTable();
                    DbConnect.da.Fill(DbConnect.dt);
                    if (DbConnect.dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Сотрудников с выбранными параметрами не существует", "Внимание", MessageBoxButton.OK);
                        EmployeesGrid.ItemsSource = null;
                    }
                    else
                    {
                        EmployeesGrid.ItemsSource = DbConnect.dt.DefaultView;
                    }
                }
                DbConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex, "Ошибка", MessageBoxButton.OK);
            }
        }

        public void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnect.OpenConnection();
                DbConnect.sql = "SELECT Employees.Id AS 'Id', Employees.Name AS 'Name', Employees.Surname AS 'Surname', Departments.Name AS 'DepName', Positions.Name AS 'PosName', Employees.Comments AS 'Comments'" +
                    "FROM Departments INNER JOIN Employees ON Departments.Id = Employees.Department_id INNER JOIN " +
                    "Positions ON Employees.Position_id = Positions.Id";
                DbConnect.cmd.CommandType = CommandType.Text;
                DbConnect.cmd.CommandText = DbConnect.sql;
                DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                DbConnect.dt = new DataTable();
                DbConnect.da.Fill(DbConnect.dt);
                EmployeesGrid.ItemsSource = DbConnect.dt.DefaultView;
                DbConnect.CloseConnection();
                PositionCombo.ItemsSource = PosList;
                DepartmentCombo.ItemsSource = DepList;
                DepartmentCombo.SelectedIndex = -1;
                PositionCombo.SelectedIndex = -1;
                DepLabel.Content = "Выберите подразделение";
                PosLabel.Content = "Выберите должность";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex, "Ошибка", MessageBoxButton.OK);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow window = new AddEmployeeWindow();
            window.Show();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотредника для изменения", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                Selected = (DataRowView)EmployeesGrid.SelectedItem;
                ChangeEmployeeWindow window = new ChangeEmployeeWindow();
                window.Show();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView emp = (DataRowView)EmployeesGrid.SelectedItem;
                string Id = Convert.ToString(emp.Row["Id"]);
                DbConnect.OpenConnection();
                DbConnect.sql = "DELETE FROM Employees WHERE Id=@Id;";
                DbConnect.cmd.CommandType = CommandType.Text;
                DbConnect.cmd.Parameters.Clear();
                DbConnect.cmd.Parameters.AddWithValue("@Id", Id);
                DbConnect.cmd.CommandText = DbConnect.sql;
                DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                DbConnect.dt = new DataTable();
                DbConnect.da.Fill(DbConnect.dt);
                EmployeesGrid.ItemsSource = DbConnect.dt.DefaultView;
                DbConnect.CloseConnection();
                Refresh_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex, "Ошибка", MessageBoxButton.OK);
            }
        }
        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnect.OpenConnection();
                DbConnect.sql = "SELECT Name FROM Positions;";
                DbConnect.cmd.CommandType = CommandType.Text;
                DbConnect.cmd.CommandText = DbConnect.sql;
                DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                using (SqlDataReader reader = DbConnect.cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        PosList.Add(name);
                    }
                }
                PositionCombo.ItemsSource = PosList;
                DbConnect.sql = "SELECT Name FROM Departments;";
                DbConnect.cmd.CommandType = CommandType.Text;
                DbConnect.cmd.CommandText = DbConnect.sql;
                DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                using (SqlDataReader reader = DbConnect.cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        DepList.Add(name);
                    }
                }
                DepartmentCombo.ItemsSource = DepList;
                DbConnect.sql = "SELECT Employees.Id AS 'Id', Employees.Name AS 'Name', Employees.Surname AS 'Surname', Departments.Name AS 'DepName', Positions.Name AS 'PosName', Employees.Comments AS 'Comments'" +
                    "FROM Departments INNER JOIN Employees ON Departments.Id = Employees.Department_id INNER JOIN " +
                    "Positions ON Employees.Position_id = Positions.Id";
                DbConnect.cmd.CommandType = CommandType.Text;
                DbConnect.cmd.CommandText = DbConnect.sql;
                DbConnect.da = new SqlDataAdapter(DbConnect.cmd);
                DbConnect.dt = new DataTable();
                DbConnect.da.Fill(DbConnect.dt);
                EmployeesGrid.ItemsSource = DbConnect.dt.DefaultView;
                DbConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex, "Ошибка", MessageBoxButton.OK);
            }
        }

        private void DepartmentCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentCombo.SelectedIndex != -1)
            {
                DepLabel.Content = "";
            }
        }

        private void PositionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PositionCombo.SelectedIndex != -1)
            {
                PosLabel.Content = "";
            }
        }
    }
}
