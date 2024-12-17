using DB_Labb2.Command;
using DB_Labb2.Model;
using System.Collections.ObjectModel;
using System.Windows;
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

                    OriginalFirstName = _selectedAuthor.Firstname;
                    OriginalLastName = _selectedAuthor.Lastname;
                    OriginalBirthDate = _selectedAuthor.Birthdate;
                }
            }
        }

        private string _originalFirstName;

        public string OriginalFirstName
        {
            get { return _originalFirstName; }
            set { _originalFirstName = value; }
        }

        private string _originalLastName;

        public string OriginalLastName
        {
            get { return _originalLastName; }
            set { _originalLastName = value; }
        }
        private DateOnly _originalBirthDate;

        public DateOnly OriginalBirthDate
        {
            get { return _originalBirthDate; }
            set
            {
                _originalBirthDate = value;
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

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
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
            if (IsSelected)
            {
                string messageBoxText = $"This will permanently remove {SelectedAuthor.FullName} from the database, do you wish to continue?";
                string caption = "Warning";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        DeleteAuthorFromDB(SelectedAuthor);
                        break;
                    case MessageBoxResult.No:
                        IsSelected = !IsSelected;
                        break;
                    default:
                        break;
                }
            }
            if (!IsSelected)
            {
                UpdateAuthorInformation(SelectedAuthor);
            }
            Close?.Invoke();
        }

        private void UpdateAuthorInformation(Author author)
        {
            SelectedAuthor.Birthdate = DateOnly.Parse(SelectedYear.ToString() + "-" + SelectedMonth.ToString() + "-" + SelectedDay.ToString());
            _authorManager.EditAuthor(SelectedAuthor);
        }

        private void DeleteAuthorFromDB(Author author)
        {
            var authorToRemove = Authors.FirstOrDefault(a => a.AuthorID == author.AuthorID);
            _authorManager.DeleteAuthor(authorToRemove.AuthorID);
        }

        private void OnCancelClick(object obj)
        {
            if (_selectedAuthor != null)
            {
                _selectedAuthor.Firstname = _originalFirstName;
                _selectedAuthor.Lastname = _originalLastName;
                _selectedAuthor.Birthdate = _originalBirthDate;
            }
            Close?.Invoke();
        }
    }
}
