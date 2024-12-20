using DB_Labb2.Command;
using DB_Labb2.Dialogs;
using DB_Labb2.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace DB_Labb2.viewModel;

public class MainViewModel : ModelBase, ICloseWindows
{

    private AuthorManager _authorManager;
    private BookManager _bookManager;
    private InventoryManager _inventoryManager;
    public MainViewModel()
    {
        _authorManager = new AuthorManager();
        _bookManager = new BookManager();
        _inventoryManager = new InventoryManager();

        _authors = new ObservableCollection<Author>();
        _books = new ObservableCollection<Book>();


        _authorManager.AuthorAdded += AuthorManager_AuthorAdded;
        _authorManager.AuthorEdited += AuthorManager_AuthorEdited;
        _authorManager.AuthorDeleted += AuthorManager_AuthorDeleted;
        _bookManager.BookAdded += BookManager_BookAdded;
        _bookManager.BookEdited += BookManager_BookEdited;
        _bookManager.BookDeleted += BookManager_BookDeleted;
        _inventoryManager.InventoryAdded += InventoryManager_InventoryAdded;
        _inventoryManager.InventoryEdited += InventoryManager_InventoryEdited;
        _inventoryManager.InventoryDeleted += InventoryManager_InventoryDeleted;
        
        
        PressCancelButtonCommand = new DelegateCommand(OnCancelButtonPress);
        
        AddAuthorCommand = new DelegateCommand(OnAddAuthorClick);
        AlterAuthorCommand = new DelegateCommand(OnAlterAuthorClick);

        AddBookCommand = new DelegateCommand(OnAddBookClick);
        EditBookCommand = new DelegateCommand(OnEditBookClick);
        
        PressAddToInventoryCommand = new DelegateCommand(OnAddToInventoryClick);
        PressRemoveFromInventoryCommand = new DelegateCommand(OnRemoveFromInventoryClick);

        PressBookSearchCommand = new DelegateCommand(OnBookSearchClick);
        PressAuthorSearchCommand = new DelegateCommand(OnAuthorSearchClick);
        PressResetCommand = new DelegateCommand(OnResetFilterClick);
        SetDataGrids();

    }

    //startup settings and commands
    public ICommand PressResetCommand { get; }
    public ICommand PressBookSearchCommand { get; }
    public ICommand PressAuthorSearchCommand { get; }
    public ICommand PressAddToInventoryCommand { get; }
    public ICommand PressRemoveFromInventoryCommand { get; }
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

    private ObservableCollection<Store> _stores;
    public ObservableCollection<Store> Stores
    {
        get { return _stores; }
        set
        {
            _stores = value;
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
            .Where(i => i.Amount > 0)
            .ToList());
        Stores = new ObservableCollection<Store>(db.Stores);

        FilteredAuthors = Authors;
        FilteredBooks = Books;
    
    }
    //Search handling

    private string _authorSearchString;

    public string AuthorSearchString
    {
        get { return _authorSearchString; }
        set 
        { 
            _authorSearchString = value;
            RaisePropertyChanged();
        }
    }


    private ObservableCollection<Author> _filteredAuthors;
    public ObservableCollection<Author> FilteredAuthors
    {
        get { return _filteredAuthors; }
        set 
        { 
            _filteredAuthors = value;
            RaisePropertyChanged();
        }
    }

    private string _bookSearchString;

    public string BookSearchString
    {
        get { return _bookSearchString; }
        set 
        { 
            _bookSearchString = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Book> _filteredBooks;
    public ObservableCollection<Book> FilteredBooks
    {
        get { return _filteredBooks; }
        set 
        { 
            _filteredBooks = value;
            RaisePropertyChanged();
        }
    }
    private void OnBookSearchClick(object obj)
    {
        var filteredBook = string.IsNullOrWhiteSpace(BookSearchString)
                        ? Books
                        : new ObservableCollection<Book>(Books.Where(b => b.Title.ToLower().Contains(BookSearchString.ToLower())));
        FilteredBooks = filteredBook;
    }

    private void OnAuthorSearchClick(object obj)
    {
        var filteredAuthor = string.IsNullOrWhiteSpace(AuthorSearchString)
                        ? Authors
                        : new ObservableCollection<Author>(Authors.Where(b => b.FullName.ToLower().Contains(AuthorSearchString.ToLower())));
        FilteredAuthors = filteredAuthor;
    }

    private void OnResetFilterClick(object obj)
    {
        SetDataGrids();
    }
    //Inventory updating

    private Store _selectedStore;

    public Store SelectedStore
    {
        get { return _selectedStore; }
        set
        {
            _selectedStore = value;
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
            RaisePropertyChanged();
        }
    }

    private int _addToAmount;

    public int AddToAmount
    {
        get { return _addToAmount; }
        set
        {
            _addToAmount = value;
            RaisePropertyChanged();
        }
    }

    private void OnAddToInventoryClick(object obj)
    {
        bool? existsInList = Inventories.Any(i => i.InventoryISBN13 == SelectedBook.ISBN13 && i.StoreID == SelectedStore.StoreID);

        if (existsInList == true)
        {
            foreach (var inventory in Inventories)
            {
                if (inventory.InventoryISBN13 == SelectedBook.ISBN13 && inventory.StoreID == SelectedStore.StoreID)
                {
                    inventory.Amount = inventory.Amount + AddToAmount;
                    RaisePropertyChanged();
                    _inventoryManager.EditInventory(inventory);
                }
            }
        }
        else
        {
            var newAdditionToInventory = new Inventory() 
            { 
                Amount = AddToAmount, 
                StoreID = SelectedStore.StoreID, 
                store = SelectedStore, 
                book = SelectedBook, 
                InventoryISBN13 = SelectedBook.ISBN13 
            };
            RaisePropertyChanged("Inventories");
        _inventoryManager.AddInventory(newAdditionToInventory);
        }
    }

    private void OnRemoveFromInventoryClick(object obj)
    {
        bool existsInList = Inventories.Any(i => i.InventoryISBN13 == SelectedBook.ISBN13 && i.StoreID == SelectedStore.StoreID);

        if (existsInList)
        {
            foreach (var inventory in Inventories)
            {
                if (inventory.InventoryISBN13 == SelectedBook.ISBN13 && inventory.StoreID == SelectedStore.StoreID)
                {
                    inventory.Amount = inventory.Amount - AddToAmount;
                    if (inventory.Amount < 0)
                    {
                        inventory.Amount = 0;
                        _inventoryManager.DeleteInventory(inventory);
                    }
                    else
                    {
                    _inventoryManager.EditInventory(inventory);
                    }
                    RaisePropertyChanged("Inventories");
                        break;
                }
            }
        }
        else
        {
            string messageBoxText = $"This book does not exist in your inventory, doublecheck your storename and book title.";
            string caption = "Warning";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
        }
    }


    //menu button clicks
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




    //eventhandling and updates

    private void AuthorManager_AuthorAdded(object sender, Author author)
    {
        Authors.Add(author);
        RaisePropertyChanged("Authors");
    }
    private void AuthorManager_AuthorEdited(object? sender, Author author)
    {
        var authorInCollection = Authors.FirstOrDefault(a => a.AuthorID == author.AuthorID);
        if (authorInCollection != null)
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

    private void InventoryManager_InventoryAdded(object? sender, Inventory inventory)
    {
        Inventories.Add(inventory);
        RaisePropertyChanged("Inventories");
    }
    private void InventoryManager_InventoryEdited(object? sender, Inventory inventory)
    {
        var inventoryInCollection = Inventories.FirstOrDefault(i => i.InventoryISBN13 == inventory.InventoryISBN13 && i.StoreID == inventory.StoreID);
        if (inventoryInCollection != null)
        {
            inventoryInCollection.Amount = inventory.Amount;
        }
        if (inventoryInCollection == null)
        {
            Inventories.Add(inventory);
        }
        RaisePropertyChanged("Inventories");

    }
    private void InventoryManager_InventoryDeleted(object? sender, Inventory inventory)
    {
        var inventoryToDelete = Inventories.FirstOrDefault(i => i.InventoryISBN13 == inventory.InventoryISBN13 && i.StoreID == inventory.StoreID);
        if (inventoryToDelete != null)
        {
            Inventories.Remove(inventoryToDelete);
            RaisePropertyChanged("Inventories");
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
