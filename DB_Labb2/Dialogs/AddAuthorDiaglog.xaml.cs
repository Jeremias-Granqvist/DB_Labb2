using DB_Labb2.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


namespace DB_Labb2.Dialogs
{
    public partial class AddAuthorDiaglog : Window, INotifyPropertyChanged
    {
        MainWindow main;
        public int Year;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Firstname => FirstNameTB.Text;
        public string Lastname => LastNameTB.Text;
        public AddAuthorDiaglog()
        {
            InitializeComponent();
            DataContext = this;
            YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
            main = new MainWindow();
            
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

            Months selectedMonth = (Months)MonthComboBox.SelectedItem;
            int monthValue = (int) selectedMonth;

            Author addAuthor = new Author
            {
                Firstname = FirstNameTB.Text,
                Lastname = LastNameTB.Text,  
                Birthdate = DateTime.Parse(YearComboBox.Text.ToString() + "-" + monthValue.ToString() + "-" + DayComboBox.Text.ToString())
            };
            using (var context = new BookstoreContext())
            {
                context.Add(addAuthor);
                context.SaveChanges();

                main.AddAuthorToOC(addAuthor);

                RaisePropertyChanged("authors");          
            }
            this.Close();
        }
    }

    }
