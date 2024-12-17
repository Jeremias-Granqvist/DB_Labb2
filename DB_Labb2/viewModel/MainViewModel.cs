using System.Windows.Input;
using DB_Labb2.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DB_Labb2.Model;
using DB_Labb2.Dialogs;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DB_Labb2.viewModel;

public class MainViewModel : ModelBase, ICloseWindows
{


    private AuthorManager _authorManager;
    private BookManager _bookManager;
    public MainViewModel()
    {
        _authorManager = new AuthorManager();
        _authors = new ObservableCollection<Author>();
        _books = new ObservableCollection<Book>();
        _bookManager = new BookManager();

        _authorManager.AuthorAdded += AuthorManager_AuthorAdded;
        _authorManager.AuthorEdited += AuthorManager_AuthorEdited;
        _authorManager.AuthorDeleted += AuthorManager_AuthorDeleted;
        _bookManager.BookAdded += BookManager_BookAdded;
        _bookManager.BookEdited += BookManager_BookEdited;
        _bookManager.BookDeleted += BookManager_BookDeleted;

        PressCancelButtonCommand = new DelegateCommand(OnCancelButtonPress);
        AddAuthorCommand = new DelegateCommand(OnAddAuthorClick);
        AlterAuthorCommand = new DelegateCommand(OnAlterAuthorClick);
        
        AddBookCommand = new DelegateCommand(OnAddBookClick);
        EditBookCommand = new DelegateCommand(OnEditBookClick);
        SetDataGrids();  

    }

    public ICommand PressCancelButtonCommand { get; }
    public ICommand AddAuthorCommand { get; }
    public ICommand AlterAuthorCommand { get; }
    public ICommand AddBookCommand { get; }
    public ICommand EditBookCommand { get; }
    public Action Close { get; set; }


    private ObservableCollection<Inventory> _inventories;

    public ObservableCollection<Inventory> Inventories
    {
        get { return _inventories; }
        set { _inventories = value; }
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

    private ObservableCollection<Book> _books;
    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            _books = value;
            RaisePropertyChanged();
        }
    }
    private void SetDataGrids()
    {
        using var db = new BookstoreContext();
        Authors = new ObservableCollection<Author>(db.Authors);
        Books = new ObservableCollection<Book>(db.Books.Include(b => b.Authors));
        Inventories = new ObservableCollection<Inventory>(db.Inventory
            .Include(i => i.book)
            .Include(i => i.store)
            .ToList());
    }

    private void OnAddAuthorClick(object obj)
    {
        var addAuthorModel = new AddAuthorDialogViewModel(_authorManager);
        var addAuthorWindow = new AddAuthorDialog(addAuthorModel) 
        { 
            DataContext = addAuthorModel
        };

        if (addAuthorWindow.DataContext is ICloseWindows dialogviewmodel)
        {
            dialogviewmodel.Close = new Action(addAuthorWindow.Close);
        }
        addAuthorWindow.Show();
    }

    private void OnAlterAuthorClick(object obj)
    {
        var editAuthorModel = new EditAuthorDialogViewModel(_authorManager, this);
        var editAuthorWindow = new EditAuthorDialog(editAuthorModel) 
        {
            DataContext = editAuthorModel 
        };

        if (editAuthorWindow.DataContext is ICloseWindows dialogViewModel)
        {
            dialogViewModel.Close = new Action(editAuthorWindow.Close);
        }
        editAuthorWindow.Show();
    }

    private void OnAddBookClick(object obj)
    {
        var addBookModel = new AddBookDialogViewModel(_bookManager, this);
        var addBookWindow = new AddBookDialog(addBookModel) 
        {
            DataContext = addBookModel 
        };
        if (addBookWindow.DataContext is ICloseWindows dialogViewModel)
        {
            dialogViewModel.Close = new Action(addBookWindow.Close);
        }
        addBookWindow.Show();
    }

    private void OnEditBookClick(object obj)
    {
        var editBookModel = new EditBookDialogViewModel(_bookManager, this);
        var editBookWindow = new EditBookDialog(editBookModel) 
        {
            DataContext = editBookModel 
        };
        if (editBookWindow.DataContext is ICloseWindows dialogViewModel)
        {
            dialogViewModel.Close = new Action(editBookWindow.Close);
        }
        editBookWindow.Show();
    }

    private void AuthorManager_AuthorAdded(object sender, Author author)
    {
        Authors.Add(author);
        RaisePropertyChanged("Authors");
    }
    private void AuthorManager_AuthorEdited(object? sender, Author author)
    {
        var authorInCollection = Authors.FirstOrDefault(a => a.AuthorID == author.AuthorID);
        if(authorInCollection != null)
        {
            authorInCollection.Firstname = author.Firstname;
            authorInCollection.Lastname = author.Lastname;
            authorInCollection.Birthdate = author.Birthdate;
        }
        RaisePropertyChanged("Author");
        RaisePropertyChanged("Authors");
        
    }
    private void AuthorManager_AuthorDeleted(object? sender, int authorID)
    {
        var authorToDelete = Authors.FirstOrDefault(a => a.AuthorID == authorID);
        if (authorToDelete != null)
        {
            Authors.Remove(authorToDelete);
        }
        RaisePropertyChanged("Authors");
    }

    private void BookManager_BookAdded(object sender, Book book)
    {
        Books.Add(book);
        RaisePropertyChanged("Books");
    }
    private void BookManager_BookEdited(object? sender, Book book)
    {
        var bookInCollection = Books.FirstOrDefault(b => b.ISBN13 == book.ISBN13);
        if (bookInCollection != null)
        {
            bookInCollection.ISBN13 = book.ISBN13;
            bookInCollection.Title = book.Title;
            bookInCollection.ReleaseDate = book.ReleaseDate;
            bookInCollection.Language = book.Language;
            bookInCollection.Price = book.Price;
            bookInCollection.Authors = book.Authors;
        }
        RaisePropertyChanged("Books");
        RaisePropertyChanged("Authors");
    }
    private void BookManager_BookDeleted(object? sender, long isbn)
    {
        var bookToDelete = Books.FirstOrDefault(b => b.ISBN13 == isbn);
        if (bookToDelete != null)
        {
            Books.Remove(bookToDelete);
            RaisePropertyChanged("Books");
        }
    }

    private void OnCancelButtonPress(object obj)
    {
        Close?.Invoke();
    }
}
interface ICloseWindows
{
    Action Close { get; set; }
}
