using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DB_Labb2.Dialogs
{
    /// <summary>
    /// Interaction logic for AddBookDialog.xaml
    /// </summary>
    public partial class AddBookDialog : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ISBN => ISBN13TB.Text;
        public string BookTitle => TitleTB.Text;

        private MainWindow _mainWindow;
        public int Year;

        public AddBookDialog(MainWindow mainWindow)
        {
            InitializeComponent();
            this.DataContext = mainWindow.DataContext;
            YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
            _mainWindow = mainWindow;
        }

    }
}
