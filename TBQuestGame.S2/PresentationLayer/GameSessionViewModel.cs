using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;
using TBQuestGame.PresentationLayer;
using TBQuestGame.DataLayer;
namespace TBQuestGame.PresentationLayer
{
    public class GameSessionViewModel : ObservableObject
    {
        #region FIELDS
        private Player _player;
        private double _playerHealth;
        private double _playerShield;
        private Map _gameMap;
        private Location _currentLocation;
        GameData gameData = new GameData();
        private int currentEnemyID;
        private string _currentLocationName;
        public ObservableCollection<Location> _accessibleLocations;
        public ObservableCollection<Enemy> _currentEnemies;
        
        //
        // used to stop a reoccuring boss battle when user goes back to location
        //
        public List<Location> bossesDefeated = new List<Location>();
        private int _missionLength = 0;
        public ObservableCollection<Enemy> CurrentEnemies
        {
            get { return _currentEnemies; }
            set { _currentEnemies = value;  OnPropertyChanged(nameof(CurrentEnemies)); }
        }
        public int MissionLength
        {
            get { return _missionLength; }
            set { _missionLength = value; }
        }
        public int CurrentEnemyID
        {
            get { return currentEnemyID; }
            set { currentEnemyID = value; OnPropertyChanged(nameof(CurrentEnemyID)); }
        }
        public ObservableCollection<Location> AccessibleLocations
        {
            get { return _accessibleLocations; }
            set { _accessibleLocations = value; OnPropertyChanged(nameof(AccessibleLocations)); }
        }
        public string CurrentLocationName
        {
            get { return _currentLocationName; }
            set { _currentLocationName = value; OnPropertyChanged(nameof(CurrentLocationName)); }
        }


        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; OnPropertyChanged(nameof(CurrentLocation)); }
        }

         
        private List<string> _messages;
        #endregion

        #region PROPERTIES
        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// Return the list of strings converted to a single string
        /// with new lines between each message
        /// </summary>
        public string Messages
        {
            get { return string.Join("\n\n",_messages); }
         }
        public double PlayerHealth
        {
            get { return _playerHealth; }
            set { _playerHealth = value; OnPropertyChanged(nameof(PlayerHealth)); }
        }
        public double PlayerShield
        {
            get { return _playerShield; }
            set { _playerShield = value; OnPropertyChanged(nameof(PlayerShield)); }
        }
        #endregion

        #region METHODS 
        #endregion

        #region CONSTRUCTORS
        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(Player player, List<string>initialMessages, Map gameMap, GameMapCoordinates currentLocationCoordinates, ObservableCollection<Enemy> currentEnemies)
        {
            _playerShield = player.Shield;
            _playerHealth = player.Health;

            currentEnemyID = gameData.currentEnemyID;
            _currentEnemies = currentEnemies;
            _player = player;
            _messages = initialMessages;
            _gameMap = gameMap;
            _gameMap.CurrentLocationCoordinates = currentLocationCoordinates;
            _currentLocation = _gameMap.CurrentLocation; 

        }
        
        #endregion

    }
}
