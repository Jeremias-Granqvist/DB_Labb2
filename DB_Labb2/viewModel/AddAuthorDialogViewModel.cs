using DB_Labb2.Command;
using DB_Labb2.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DB_Labb2.viewModel;

public class AddAuthorDialogViewModel : ModelBase, ICloseWindows
{

    private AuthorManager _authorManager;
    public AddAuthorDialogViewModel(AuthorManager authorManager)
    {
        _authorManager = authorManager;
        AddAuthorCommand = new DelegateCommand(OnAddAuthor);
        CancelButtonCommand = new DelegateCommand(OnCancelClick);
    }


    public ICommand CancelButtonCommand { get; }
    public ICommand AddAuthorCommand { get; }
    public Action Close { get; set; }

    private void OnCancelClick(object obj)
    {
        Close?.Invoke();
    }


    private string _firstname;
    public string Firstname
    {
        get => _firstname;
        set
        {
            _firstname = value;
            RaisePropertyChanged();
        }
    }

    private string _lastname;
    public string Lastname
    {
        get => _lastname;
        set
        {
            _lastname = value;
            RaisePropertyChanged();
        }
    }

    private int _year;
    public int Year
    {
        get => _year;
        set
        {
            _year = value;
            RaisePropertyChanged();
        }
    }

    private Months _month;
    public Months Month
    {
        get => _month;
        set
        {
            _month = value;
            RaisePropertyChanged();
            UpdateDaysInMonth();
            RaisePropertyChanged();
            RaisePropertyChanged("DayComboBoxItemsSource");
        }
    }

    private int _day;
    public int Day
    {
        get => _day;
        set
        {
            _day = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<int> _dayComboBoxItemsSource;
    public ObservableCollection<int> DayComboBoxItemsSource
    {
        get => _dayComboBoxItemsSource;
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
            int daysInMonth = GetDaysInMonth(selectedMonth, Year);
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

    private void OnAddAuthor(object obj)
    {
        if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname) || new [] { Year, (int)Month, Day }.Any(val => val == 0))
        {
            MessageBox.Show("Please fill out all fields.");
            return;
        }
        var selectedDate = DateOnly.Parse(Year.ToString() + "-" + Month.ToString() + "-" + Day.ToString());
        
        Author newAuthor = new Author
        {
            Firstname = Firstname,
            Lastname = Lastname,
            Birthdate = selectedDate
        };

        _authorManager.AddAuthor(newAuthor);
        Close?.Invoke();
    }





}
