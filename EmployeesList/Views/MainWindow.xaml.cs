using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EmployeesList
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateEmployeesWindows(Deserialization());
        }

        public string Filename;

        public static List<Employee> Deserialization(string jsonFile = @".\EmployeesDataFile.json")
        {
            var jsonString = File.ReadAllText(jsonFile);
            var deserializedEmployees = JsonConvert.DeserializeObject<List<Employee>>(jsonString);
            return deserializedEmployees;
        }

        public static void Serialization(List<Employee> employees, string jsonFile = @".\EmployeesDataFile.json")
        {
            File.WriteAllText(jsonFile, JsonConvert.SerializeObject(employees));
        }


        public void CreateEmployeesWindows(List<Employee> employees)
        {
            if (employees is null)
                return;
            for (int i = 0; i < employees.Count; i++)
            {
                TextBlock employeeData = new TextBlock();
                employeeData.Text = $"{employees[i].Surname} {employees[i].Name} {employees[i].Patronymic}\nДолжность: {employees[i].Job}";
                employeeData.FontSize = 13;
                employeeData.Margin = new Thickness(5);
                Border border = new Border();
                SolidColorBrush brush = new SolidColorBrush(Colors.Black);
                border.BorderBrush = brush;
                border.BorderThickness = new Thickness(1);
                border.Height = 50;
                border.Margin = new Thickness(0);
                border.Child = employeeData;
                WorkList.Items.Add(border);
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var employee = new Employee
            {
                Surname = $"{SurnameData.Text}",
                Name = $"{NameData.Text}",
                Patronymic = $"{PatronymicData.Text}",
                Job = $"{JobData.Text}"
            };

            var employees = Deserialization(Filename);

            WorkList.Items.Clear();

            employees.Add(employee);
            Serialization(employees);
            CreateEmployeesWindows(employees);

            SurnameData.Text = "";
            NameData.Text = "";
            PatronymicData.Text = "";
            JobData.Text = "";
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var employees = Deserialization(Filename);
            var index = WorkList.SelectedIndex;
            employees.Remove(employees[index]);
            Serialization(employees);
            WorkList.Items.Clear();
            CreateEmployeesWindows(employees);
        }

        private void ChooseElement(object sender, SelectionChangedEventArgs e)
        {
            if (WorkList.SelectedIndex == -1)
                return;
            var employees = Deserialization(Filename);
            SurnameData.Text = employees[WorkList.SelectedIndex].Surname;
            NameData.Text = employees[WorkList.SelectedIndex].Name;
            PatronymicData.Text = employees[WorkList.SelectedIndex].Patronymic;
            JobData.Text = employees[WorkList.SelectedIndex].Job;
        }

        private void ChangeButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var employees = Deserialization(Filename);
                var index = WorkList.SelectedIndex;
                employees[index].Surname = SurnameData.Text;
                employees[index].Name = NameData.Text;
                employees[index].Patronymic = PatronymicData.Text;
                employees[index].Job = JobData.Text;
                Serialization(employees, Filename);
                WorkList.Items.Clear();
                CreateEmployeesWindows(employees);
            }
            catch (ArgumentOutOfRangeException)
            {
                WorkList.SelectedIndex = 0;
            }
        }

        private void DownoladButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON (.json)|*.json";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                WorkList.Items.Clear();
                Filename = dialog.FileName;
                CreateEmployeesWindows(Deserialization(filename));
            }
        }
    }
}
