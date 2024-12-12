using DB_Labb2.Command;
using DB_Labb2.Dialogs;
using DB_Labb2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DB_Labb2.viewModel;

public class AddAuthorDialogViewModel : ModelBase
{
    public event EventHandler<Author> AuthorAdded;
    public AddAuthorDialogViewModel()
    {
        SaveCommand = new DelegateCommand(SaveAuthor);
    }
    public ICommand SaveCommand { get; }







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

    private List<int> _dayComboBoxItemsSource;
    public List<int> DayComboBoxItemsSource
    {
        get => _dayComboBoxItemsSource;
        set
        {
            _dayComboBoxItemsSource = value;
            RaisePropertyChanged("DayComboBoxItemsSource");

        }
    }


    private void UpdateDaysInMonth()
    {
        if (Month is Months selectedMonth)
        {
            int daysInMonth = GetDaysInMonth(selectedMonth, Year);
            DayComboBoxItemsSource = Enumerable.Range(1, daysInMonth).ToList();
            Debug.WriteLine($"DayComboBoxItemsSource updated: {string.Join(", ", DayComboBoxItemsSource)}");

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


    private void SaveAuthor(object obj)
    {
        if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname)) 
        {
            MessageBox.Show("Please fill out all fields.");
            return;
        }
        var selectedDate = DateTime.Parse(Year.ToString() + "-" + Month.ToString() + "-" + Day.ToString());


        Author addAuthor = new Author
        {
            Firstname = Firstname,
            Lastname = Lastname,
            Birthdate = selectedDate
        };
        using (var context = new BookstoreContext())
        {
            context.Add(addAuthor);
            context.SaveChanges();

            //  _mainWindow.AddAuthorToOC(addAuthor);

        }
        OnAuthorAdded(addAuthor);
        RaisePropertyChanged("authors");
    }


    protected virtual void OnAuthorAdded(Author author)
    {
        AuthorAdded?.Invoke(this, author);
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
