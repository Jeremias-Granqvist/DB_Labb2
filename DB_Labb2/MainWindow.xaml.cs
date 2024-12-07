
using System.Windows;
using Microsoft.EntityFrameworkCore;
using DB_Labb2.Model;
using DB_Labb2.Dialogs;

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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var AddAuthor = new AddAuthorDiaglog();
            AddAuthor.Show();
            AddAuthor.YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
            
            if (AddAuthor.ShowDialog() == true)
            {
                Author newAuthor = new Author
                {
                    Firstname = AddAuthor.Firstname,
                    Lastname = AddAuthor.Lastname
                    //implement , Birthdate = addAuthor.Birthdate.value
                };

                MessageBox.Show($"New author added to database: {newAuthor.Firstname} {newAuthor.Lastname} Birthdate {newAuthor.Birthdate}"); //implement birthdate!!! maybe newauthor.birthdate.toshortdatestring()
            }
        }
    }
}