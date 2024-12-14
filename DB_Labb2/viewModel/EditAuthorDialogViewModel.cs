using DB_Labb2.Command;
using DB_Labb2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Input;

namespace DB_Labb2.viewModel
{
    public class EditAuthorDialogViewModel : ModelBase, ICloseWindows
    {
        private AuthorManager _authorManager;
        public EditAuthorDialogViewModel(AuthorManager authorManager, MainViewModel mainViewModel)
        {
            Authors = mainViewModel.Authors;
            _authorManager = authorManager;
            EditAuthorCommand = new DelegateCommand(OnEditAuthor);
            CancelButtonCommand = new DelegateCommand(OnCancelClick);
        }
        public ICommand EditAuthorCommand { get; }
        public ICommand CancelButtonCommand { get; }
        public Action Close { get; set; }

        private ObservableCollection<Author> _authors;

        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set 
            { 
                _authors = value;
                RaisePropertyChanged();
            }
        }

        private Author _selectedAuthor;

        public Author SelectedAuthor
        {
            get { return _selectedAuthor; }
            set 
            { 
                _selectedAuthor = value;
                RaisePropertyChanged();

                if (_selectedAuthor != null)
                {
                    SelectedYear = _selectedAuthor.Birthdate.Year;
                    SelectedMonth = _selectedAuthor.Birthdate.Month;
                    SelectedDay = _selectedAuthor.Birthdate.Day;
                    
                }
            }
        }



        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set 
            { 
                _firstName = value;
                RaisePropertyChanged();
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set 
            { 
                _lastName = value;
                RaisePropertyChanged();
            }
        }

        private DateOnly _birthDate;

        public DateOnly BirthDate
        {
            get { return _birthDate; }
            set 
            { 
                _birthDate = value;
                RaisePropertyChanged();
            }
        }

        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set 
            { 
                _selectedYear = value;
                RaisePropertyChanged();
            }
        }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set 
            { 
                _selectedMonth = value;
                Month = (Months)value;
                RaisePropertyChanged();
            }
        }

        private Months _month;

        public Months Month
        {
            get { return _month; }
            set 
            { 
                _month = value;
                UpdateDaysInMonth();
                RaisePropertyChanged();
            }
        }

        private int _selectedDay;

        public int SelectedDay
        {
            get { return _selectedDay; }
            set 
            { 
                _selectedDay = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<int> _dayComboBoxItemsSource;

        public ObservableCollection<int> DayComboBoxItemsSource
        {
            get { return _dayComboBoxItemsSource; }
            set 
            { 
                _dayComboBoxItemsSource = value;
                RaisePropertyChanged();
            }
        }


        private void UpdateDaysInMonth()
        {
            if (Month is Months selectedMonth)
            {
                int daysInMonth = GetDaysInMonth(selectedMonth, SelectedYear);
                var localList = Enumerable.Range(1, daysInMonth).ToList();
                DayComboBoxItemsSource = new ObservableCollection<int>(localList);
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
        private void OnEditAuthor(object obj)
        {
        //implementera att skriva över datan med ny när man trycker på save.
        }
        private void OnCancelClick(object obj)
        {
            Close?.Invoke();
        }
    }
}
