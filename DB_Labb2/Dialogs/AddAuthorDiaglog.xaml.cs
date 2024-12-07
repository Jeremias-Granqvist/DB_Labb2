using DB_Labb2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DB_Labb2.Dialogs
{
    /// <summary>
    /// Interaction logic for AddAuthorDiaglog.xaml
    /// </summary>
    public partial class AddAuthorDiaglog : Window
    {
        public List<Months> Months { get; }
        public int Year;

        public string Firstname => FirstNameTB.Text;
        public string Lastname => LastNameTB.Text;
        //public DateTime? Birthdate => BirthdatePicker.SelectedDate; Behöver implementeras!!!!
        public AddAuthorDiaglog()
        {
            InitializeComponent();
            DataContext = this;
            Months = Enum.GetValues(typeof(Months)).Cast<Months>().ToList();
            YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
        }

        public List<int> Years { get; }


        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Year = (int)YearComboBox.SelectedItem;
        }
        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedItem is Months selectedMonth)
            {
                int daysInMonth = GetDaysInMonth(selectedMonth, Year);
                DayComboBox.ItemsSource = Enumerable.Range(1, daysInMonth).ToList();
            }
        }

        private int GetDaysInMonth(Months month, int year)
        {
            switch (month)
            {
                case Model.Months.February:
                    return DateTime.IsLeapYear(year) ? 29 : 28;
                case Model.Months.April:
                case Model.Months.June:
                case Model.Months.September:
                case Model.Months.November:
                    return 30;
                default:
                    return 31;
            }
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname)) //add !Birthdate.HasValue
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }
            this.DialogResult = true;
            this.Close();
        }
    }

    }
