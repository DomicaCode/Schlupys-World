﻿using Engine.Models;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for NameInputTest.xaml
    /// </summary>
    public partial class NameInputTest : Window
    {

        public NameInputTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(txtboxUsername.Text.Trim());
            //main.DataContext = _gameSession;
            main.Show();
            this.Close();
        }
    }
}
