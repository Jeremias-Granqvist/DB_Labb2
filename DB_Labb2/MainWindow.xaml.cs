
using DB_Labb2.Dialogs;
using DB_Labb2.Model;
using DB_Labb2.viewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DB_Labb2;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
        var viewModel = new MainViewModel();
        DataContext = viewModel;

    }

    public ObservableCollection<Author> authors { get; set; } = new ObservableCollection<Author>();
    public ObservableCollection<Book> books { get; set; } = new ObservableCollection<Book>();




    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        SetDataGrids();
        LoadCloseWindows();

    }

    private void LoadCloseWindows()
    {
        if (DataContext is ICloseWindows vm)
        {
                vm.Close = new Action(this.Close); 
        }
    }

    private void SetDataGrids()
    {

        using var db = new BookstoreContext();

        authors = new ObservableCollection<Author>(db.Authors);
        AuthorDataGrid.ItemsSource = authors;

        books = new ObservableCollection<Book>(db.Books);
        BooksDataGrid.ItemsSource = books;

        RaisePropertyChanged("authors");
        RaisePropertyChanged("books");

    }

    public void AddAuthorToOC(Author author)
    {
        authors.Add(author);
    }

    private void AddAuthor_Click(object sender, RoutedEventArgs e)
    {

        var AddAuthor = new AddAuthorDialog(new AddAuthorDialogViewModel());
        AddAuthor.DataContext = new AddAuthorDialogViewModel();

        if (AddAuthor.DataContext is ICloseWindows dialogViewModel)
        {
            dialogViewModel.Close = new Action(AddAuthor.Close);
        }
        AddAuthor.Show();
        AddAuthor.YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();

    }
    private void AddBook_Click(object sender, RoutedEventArgs e)
    {
        var AddBook = new AddBookDialog(this);
        AddBook.Show();
        AddBook.YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}