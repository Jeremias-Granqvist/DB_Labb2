
using System.Windows;
using Microsoft.EntityFrameworkCore;
using DB_Labb2.Model;

namespace DB_Labb2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetDataGrids();
        }

        private void SetDataGrids()
        {
            
            using var db = new BookstoreContext();
            var authors = db.Authors.ToList();
            AuthorDataGrid.ItemsSource = authors;

            var books = db.Books.ToList();
            BooksDataGrid.ItemsSource = books;

            
        }
    }
}