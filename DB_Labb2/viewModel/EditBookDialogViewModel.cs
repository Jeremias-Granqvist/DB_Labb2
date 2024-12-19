﻿using DB_Labb2.Command;
using DB_Labb2.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DB_Labb2.viewModel
{
    public class EditBookDialogViewModel : ModelBase, ICloseWindows
    {
        private BookManager _bookManager;
        public EditBookDialogViewModel(BookManager bookManager, MainViewModel mainViewModel)
        {
            _bookManager = bookManager;
            Books = mainViewModel.Books;
            Authors = mainViewModel.Authors;


            CancelButtonCommand = new DelegateCommand(OnCancelClick);
            SaveButtonCommand = new DelegateCommand(OnSaveClick);
        }




        public ICommand SaveButtonCommand { get; }
        public ICommand CancelButtonCommand { get; }

        public Book LoadBookForEditing(long isbn)
        {
            if (SelectedBook != null)
            {
                using (var context = new BookstoreContext())
                {
                    var book = context.Books
                        .Where(b => b.ISBN13 == isbn)
                        .Include(b => b.Authors)
                        .FirstOrDefault();

                    if (book != null)
                    {

                        SelectedAuthor.Clear();

                        foreach (var author in book.Authors)
                        {
                            SelectedAuthor.Add(author);
                        }

                        return book;
                    }

                }
            }
            return null;
        }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                RaisePropertyChanged();
            }
        }


        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;

                if (_selectedBook != null)
                {

                    SelectedYear = _selectedBook.ReleaseDate.Year;
                    SelectedMonth = _selectedBook.ReleaseDate.Month;
                    SelectedDay = _selectedBook.ReleaseDate.Day;
                    
                    _selectedBook = LoadBookForEditing(SelectedBook.ISBN13);

                    _originalISBN = _selectedBook.ISBN13;
                    _originalTitle = _selectedBook.Title;
                    _originalLanguage = _selectedBook.Language;
                    _originalPrice = _selectedBook.Price;
                    _originialReleaseDate = _selectedBook.ReleaseDate;
                    _originalAuthor = _selectedBook.Authors;

                }
                RaisePropertyChanged();
            }
        }




        private Author _selectedAuthorForBook;
        public Author SelectedAuthorForBook
        {
            get { return _selectedAuthorForBook; }
            set
            {
                _selectedAuthorForBook = value;

                if (_selectedAuthor.Count == 0)
                {
                    _selectedAuthor.Add(SelectedAuthorForBook);
                }
                RaisePropertyChanged();
            }
        }

        private ICollection<Author> _selectedAuthor = new List<Author>();

        public ICollection<Author> SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                RaisePropertyChanged();
            }
        }

        private ICollection<Author> _originalAuthor;
        public ICollection<Author> OriginalAuthor
        {
            get { return _originalAuthor; }
            set
            {
                _originalAuthor = value;
                RaisePropertyChanged();
            }
        }


        private long _originalISBN;
        public long OriginalISBN
        {
            get { return _originalISBN; }
            set
            {
                _originalISBN = value;
                RaisePropertyChanged();
            }
        }

        private string _originalTitle;
        public string OriginalTitle
        {
            get { return _originalTitle; }
            set
            {
                _originalTitle = value;
                RaisePropertyChanged();
            }
        }

        private int _originalPrice;
        public int OriginalPrice
        {
            get { return _originalPrice; }
            set
            {
                _originalPrice = value;
                RaisePropertyChanged();
            }
        }

        private string _originalLanguage;
        public string OriginalLanguage
        {
            get { return _originalLanguage; }
            set
            {
                _originalLanguage = value;
                RaisePropertyChanged();
            }
        }
        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged();
            }
        }

        private DateOnly _originialReleaseDate;
        public DateOnly OriginalReleaseDate
        {
            get { return _originialReleaseDate; }
            set
            {
                _originialReleaseDate = value;
                RaisePropertyChanged();
            }
        }

        private DateOnly _selectedReleaseDate;
        public DateOnly SelectedReleaseDate
        {
            get { return _selectedReleaseDate; }
            set
            {
                _selectedReleaseDate = value;
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



        private void OnSaveClick(object obj)
        {
            if (IsSelected)
            {
                string messageBoxText = $"This will permanently remove {SelectedBook.Title} from the database, do you wish to continue?";
                string caption = "Warning";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        DeleteBookFromDB(SelectedBook);
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
                if (SelectedBook.ISBN13.ToString().Length != 13)
                {
                    string messageBoxText = $"Your ISBN number is not 13 characters long, please doublecheck your ISBN number.";
                    string caption = "Error";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                }
                UpdateBookInformation(SelectedBook);
            }
            Close?.Invoke();
        }

        private void UpdateBookInformation(Book book)
        {
            SelectedBook.ReleaseDate = DateOnly.Parse(SelectedYear.ToString() + "-" + SelectedMonth.ToString() + "-" + SelectedDay.ToString());
            SelectedBook.Authors = SelectedAuthor;
            _bookManager.EditBook(SelectedBook, _originalISBN);
        }
        private void DeleteBookFromDB(Book book)
        {
            var bookToRemove = Books.FirstOrDefault(b => b.ISBN13 == book.ISBN13);
            _bookManager.DeleteBook(bookToRemove.ISBN13);
        }
        private void OnCancelClick(object obj)
        {
            if (_selectedBook != null)
            {
                _selectedBook.ISBN13 = _originalISBN;
                _selectedBook.Title = _originalTitle;
                _selectedBook.Language = _originalLanguage;
                _selectedBook.Price = _originalPrice;
                _selectedReleaseDate = _originialReleaseDate;
                _selectedAuthor = _originalAuthor;
            }
            Close?.Invoke();
        }

        public Action Close { get; set; }
    }
}
