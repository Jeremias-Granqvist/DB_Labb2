﻿using DB_Labb2.Command;
using DB_Labb2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DB_Labb2.viewModel
{
    public class AddBookDialogViewModel : ModelBase, ICloseWindows
    {
        private BookManager _bookManager;
        public AddBookDialogViewModel(BookManager bookmanager, MainViewModel mainViewModel)
        {
            Authors = mainViewModel.Authors;
            _bookManager = bookmanager;

            CancelButtonCommand = new DelegateCommand(OnCancelClick);
            AddBookCommand = new DelegateCommand(OnSaveBook);
        }
        public ICommand AddBookCommand { get; }
        public ICommand CancelButtonCommand {get;}
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
        
        private long _ISBN13;
        public long ISBN13
        {
            get { return _ISBN13; }
            set 
            { 
                _ISBN13 = value;
                RaisePropertyChanged();
            }
        }
        private string? _title;
        public string? Title
        {
            get { return _title; }
            set 
            { 
                _title = value;
                RaisePropertyChanged();
            }
        }
        private int _price;

        public int Price
        {
            get { return _price; }
            set 
            { 
                _price = value;
                RaisePropertyChanged();
            }
        }

        private Language _language;

        public Language Language
        {
            get { return _language; }
            set 
            { 
                _language = value;
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

        private Author _author;

        public Author Author
        {
            get { return _author; }
            set 
            { 
                _author = value;
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


        private void OnSaveBook(object obj)
        {
            if (string.IsNullOrEmpty(Title)|| string.IsNullOrEmpty(Language.ToString()) || Price == 0 || new[] { Year, (int)Month, Day }.Any(val => val == 0))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }
            if (ISBN13.ToString().Length != 13)
            {
                MessageBox.Show("Please ensure your ISBN is 13 characters long.");
                return;
            }

            var selectedDate = DateOnly.Parse(Year.ToString() + "-" + Month.ToString() + "-" + Day.ToString());

            var newBook = new Book()
            {
                ISBN13 = ISBN13,
                Title = Title,
                Price = Price,
                ReleaseDate = selectedDate,
                Language = Language.ToString()
            };
            _bookManager.AddBook(newBook);
            Close?.Invoke();
        }

        private void OnCancelClick(object obj)
        {
            Close?.Invoke();
        }
    }
}
