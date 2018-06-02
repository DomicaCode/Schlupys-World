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
using Engine.ViewModels;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for CharacterCreation.xaml
    /// </summary>6
    public partial class CharacterCreation : Window
    {
        private readonly GameSession _gameSession = new GameSession();
        public string Username;
        public CharacterCreation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.DataContext = _gameSession;
            string Username = txtboxUser.Text;
            _gameSession.ChracterCreation(Username);
            main.Show();
        }
    }
}
