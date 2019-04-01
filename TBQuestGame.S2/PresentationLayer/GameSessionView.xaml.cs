using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TBQuestGame.PresentationLayer;
using TBQuestGame.Models;
using TBQuestGame.DataLayer;
namespace TBQuestGame.PresentationLayer
{
    /// <summary>
    /// Interaction logic for GameSessionView.xaml
    /// </summary>
    public partial class GameSessionView : Window
    {

        GameSessionViewModel _gameSessionViewModel;
        
        private string Messages; 
        private double PlayerHealth;
        MapDisplay mapWindow = new MapDisplay();
        GameMenuDisplay menuWindow = new GameMenuDisplay(); 
        public GameSessionView(GameSessionViewModel gameSessionViewModel)
        {
            _gameSessionViewModel = gameSessionViewModel;
           // ActiveEnemies.Items.Add("Testing");
            InitializeComponent();
            Messages = _gameSessionViewModel.Messages;
            PlayerHealth = _gameSessionViewModel.PlayerHealth;
            mapWindow.DataContext = gameSessionViewModel;
            menuWindow.DataContext = gameSessionViewModel;
            DataContext = gameSessionViewModel;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void GameOptions_Click(object sender, RoutedEventArgs e)
        {
            menuWindow.Visibility = Visibility.Visible;
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SkillsButton_Click(object sender, RoutedEventArgs e)
        {
            AddEnemyToList("warrior-black");
            AddEnemyToList("warrior");
            AddEnemyToList("wizard");
            AddEnemyToList("scuffedspider");
            AddEnemyToList("mudcrawler");
            AddEnemyToList("bandit");
        }  
        public void AddEnemyToList(string enemyName)
        {
            string nameOfEnemy ="";
            string levelOfEnemy ="";
            string enemyPicturePath = "";
            switch (enemyName.ToLower())
            {
                case "warrior":
                    nameOfEnemy = "Warrior";
                    levelOfEnemy = "{LVL 34}";
                    enemyPicturePath = "warrior-icon.png";
                    break;
                case "warrior-black":
                    nameOfEnemy = "Black Knight";
                    levelOfEnemy = "{LVL 55}";
                    enemyPicturePath = "warrior-black.png";
                    break;
                case "bandit":
                    nameOfEnemy = "Bandit";
                    levelOfEnemy = "{LVL 15}";
                    enemyPicturePath = "Bandit.png";
                    break;
                case "mudcrawler":
                    nameOfEnemy = "MudCrawler";
                    levelOfEnemy = "{LVL 9}";
                    enemyPicturePath = "MudCrawler.png";
                    break;
                case "scuffedspider":
                    nameOfEnemy = "Spider";
                    levelOfEnemy = "{LVL 3}";
                    enemyPicturePath = "scuffedspider.png";
                    break;
                case "wizard":
                    nameOfEnemy = "Wizard";
                    levelOfEnemy = "{LVL 63}";
                    enemyPicturePath = "mage-icon.png";
                    break;
                default:
                    break;
            }
            ListBoxItem item = new ListBoxItem();
            StackPanel new_item = new StackPanel();
            new_item.Orientation = Orientation.Horizontal;
            Image img = new Image();
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();

            bitImage.UriSource = new Uri("/Images/"+enemyPicturePath, UriKind.Relative);

            bitImage.EndInit();
            img.Source = bitImage;
            EnemyPicture.Source = bitImage;
            img.Width = 32;
            img.Height = 32;

            TextBlock entityLevel = new TextBlock();
            entityLevel.Text = levelOfEnemy;
            entityLevel.FontWeight = FontWeights.Bold;
            entityLevel.FontSize = 16;
            entityLevel.VerticalAlignment = VerticalAlignment.Center;

            TextBlock entityName = new TextBlock();
            entityName.Text = nameOfEnemy;
            entityName.FontSize = 15.5;
            entityName.FontWeight = FontWeights.Bold;
            entityName.VerticalAlignment = VerticalAlignment.Center;

            new_item.Children.Add(img);
            new_item.Children.Add(entityLevel);
            new_item.Children.Add(entityName);
            item.Content = new_item;
            item.Background = Brushes.Red;
            item.BorderBrush = Brushes.Black;
            item.BorderThickness = new Thickness(3, 3, 3, 0);

            ActiveEnemies.Items.Add(item);

             
            BitmapImage playerBitImage = new BitmapImage();
            playerBitImage.BeginInit();

            playerBitImage.UriSource = new Uri("/Images/warrior-icon.png", UriKind.Relative);

            playerBitImage.EndInit();
            PlayerPicture.Source = playerBitImage;
        }
          private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerHealth -= 5;
            playerHealthBar.Value = PlayerHealth;
        }
        public void BossBattleStart()
        {
            switch (_gameSessionViewModel.GameMap.CurrentLocation.Name)
            {
                case "The Dark Forest":
                    AddEnemyToList("wizard");
                    _gameSessionViewModel.bossesDefeated.Add(_gameSessionViewModel.GameMap.CurrentLocation);
                    break;
                case "Vickren Dungeon":
                   AddEnemyToList("warrior-black");
                    _gameSessionViewModel.bossesDefeated.Add(_gameSessionViewModel.GameMap.CurrentLocation);
                    break;
                case "Kardon Dungeon":
                   AddEnemyToList("bandit");
                    _gameSessionViewModel.bossesDefeated.Add(_gameSessionViewModel.GameMap.CurrentLocation);
                    break;
                default:
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //
            // North Button
            //
            if (_gameSessionViewModel.GameMap.NorthLocation() != null)
            {
                _gameSessionViewModel.GameMap.MoveNorth();
                LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                if (_gameSessionViewModel.GameMap.CurrentLocation.BossFightRoom)
                {
                    if (!_gameSessionViewModel.bossesDefeated.Contains(_gameSessionViewModel.GameMap.CurrentLocation))
                    {
                        TipsBox.Foreground = Brushes.Red;
                        TipsBox.FontWeight = FontWeights.Bold;
                        TipsBox.Text = "{ YOU'VE ENTERED A BOSS ROOM! FIGHT INITIATED }";
                        BossBattleStart();
                        LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                        DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                        Location.disableControls(this);
                    }
                }
                mapWindow.CurrentLocationDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                mapWindow.LocationDescriptionDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Description;
            } 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //
            // East Button
            //
            if (_gameSessionViewModel.GameMap.EastLocation() != null) {
            _gameSessionViewModel.GameMap.MoveEast();
                LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                if (_gameSessionViewModel.GameMap.CurrentLocation.BossFightRoom)
                {
                    if (!_gameSessionViewModel.bossesDefeated.Contains(_gameSessionViewModel.GameMap.CurrentLocation))
                    {
                        TipsBox.Foreground = Brushes.Red;
                        TipsBox.FontWeight = FontWeights.Bold;
                        TipsBox.Text = "{ YOU'VE ENTERED A BOSS ROOM! FIGHT INITIATED }";
                        BossBattleStart();
                        LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                        DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                        Location.disableControls(this);
                    }
                }
                mapWindow.CurrentLocationDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                mapWindow.LocationDescriptionDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Description;

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //
            // South Button
            //
            if (_gameSessionViewModel.GameMap.SouthLocation() != null)
            {
            _gameSessionViewModel.GameMap.MoveSouth();

                LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                if (_gameSessionViewModel.GameMap.CurrentLocation.BossFightRoom)
                {
                    if (!_gameSessionViewModel.bossesDefeated.Contains(_gameSessionViewModel.GameMap.CurrentLocation))
                    {
                        TipsBox.Foreground = Brushes.Red;
                        TipsBox.FontWeight = FontWeights.Bold;
                        TipsBox.Text = "{ YOU'VE ENTERED A BOSS ROOM! FIGHT INITIATED }";
                        BossBattleStart();
                        LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                        DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                        Location.disableControls(this);
                    }

                }
                mapWindow.CurrentLocationDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                mapWindow.LocationDescriptionDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Description;

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //
            // West Button
            //
            if (_gameSessionViewModel.GameMap.WestLocation() != null)
            {
            _gameSessionViewModel.GameMap.MoveWest();
            LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                if (_gameSessionViewModel.GameMap.CurrentLocation.BossFightRoom)
                {
                    if (!_gameSessionViewModel.bossesDefeated.Contains(_gameSessionViewModel.GameMap.CurrentLocation))
                    {
                        TipsBox.Foreground = Brushes.Red;
                        TipsBox.FontWeight = FontWeights.Bold;
                        TipsBox.Text = "{ YOU'VE ENTERED A BOSS ROOM! FIGHT INITIATED }";
                        BossBattleStart();
                        LocationName.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                        DialogueBox.Text = _gameSessionViewModel.GameMap.CurrentLocation.LocationMessage;
                        Location.disableControls(this);
                    }

                }
                mapWindow.CurrentLocationDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Name;
                mapWindow.LocationDescriptionDisplay.Text = _gameSessionViewModel.GameMap.CurrentLocation.Description;

            }

        }
        private void OpenMap_Click(object sender, RoutedEventArgs e)
        {
            mapWindow.Visibility = Visibility.Visible;
        }

        private void Close_Application(object sender, EventArgs e)
        {
            //If save game method is added, call it here.
            Environment.Exit(0);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //
            //Stats Window Button
            //

        }
    }
}
