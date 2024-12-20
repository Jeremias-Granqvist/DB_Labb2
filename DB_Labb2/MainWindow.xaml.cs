
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
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        LoadCloseWindows();
    }
    private void LoadCloseWindows()
    {
        if (DataContext is ICloseWindows vm)
        {
                vm.Close = new Action(this.Close); 
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}