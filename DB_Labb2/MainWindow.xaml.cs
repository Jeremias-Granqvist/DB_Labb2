
using System.Windows;
using Microsoft.EntityFrameworkCore;
using DB_Labb2.Model;
using DB_Labb2.Dialogs;
using System.Windows.Input;
using DB_Labb2.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace DB_Labb2
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            DataContext = this;

        }
        public ObservableCollection<Author> authors { get; set; } = new ObservableCollection<Author>();
        public ObservableCollection<Book> books { get; set; } = new ObservableCollection<Book>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetDataGrids();
        }

        private void SetDataGrids()
        {
            
            using var db = new BookstoreContext();

            authors = new ObservableCollection<Author>(db.Authors);
            AuthorDataGrid.ItemsSource = authors;

            books = new ObservableCollection<Book>(db.Books);
            BooksDataGrid.ItemsSource = books;

            RaisePropertyChanged("authors");
            RaisePropertyChanged("authors");
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var AddAuthor = new AddAuthorDiaglog(this);
            AddAuthor.Show();
            AddAuthor.YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();

        }

        public void AddAuthorToOC(Author author)
        {
            //SetDataGrids();

            authors.Add(author);
            //RaisePropertyChanged("authors");

            //AuthorDataGrid.ItemsSource = null;
            //AuthorDataGrid.ItemsSource = authors;
        }
    }
}