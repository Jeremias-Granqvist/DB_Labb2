﻿using DB_Labb2.Model;
using DB_Labb2.viewModel;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


namespace DB_Labb2.Dialogs
{
    public partial class AddAuthorDialog : Window, INotifyPropertyChanged
    {
        public AddAuthorDialog(AddAuthorDialogViewModel AddAuthorViewModel)
        {
            InitializeComponent();
            this.DataContext = AddAuthorViewModel;
            YearComboBox.ItemsSource = Enumerable.Range(1455, DateTime.UtcNow.Year - 1455).Reverse().ToList();
         
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
