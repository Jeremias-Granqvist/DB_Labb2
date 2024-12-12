using System.Windows.Input;
using DB_Labb2.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DB_Labb2.Model;

namespace DB_Labb2.viewModel;

public class MainViewModel : ModelBase, ICloseWindows
{

    public MainViewModel()
    {
        
        PressCancelButtonCommand = new DelegateCommand(OnCancelButtonPress);


    }
    public Action Close { get; set; }
    private void OnCancelButtonPress(object obj)
    {
        Close?.Invoke();
    }
    public ICommand PressCancelButtonCommand { get; }

}

interface ICloseWindows
{
    Action Close { get; set; }
}
