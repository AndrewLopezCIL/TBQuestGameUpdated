using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;
using TBQuestGame.PresentationLayer;
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
        private string _currentLocationName;
        private ObservableCollection<Location> _accessibleLocations;
        public List<Location> bossesDefeated = new List<Location>();
        private int _missionLength = 0;

        public int MissionLength
        {
            get { return _missionLength; }
            set { _missionLength = value; }
        }


        public string CurrentLocationName
        {
            get { return _currentLocationName; }
            set { _currentLocationName = value; OnPropertyChanged(nameof(CurrentLocationName)); }
        }


        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; }
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
            set { _playerShield = value; }
        }
        #endregion

        #region METHODS 
        #endregion

        #region CONSTRUCTORS
        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(Player player, List<string>initialMessages, Map gameMap, GameMapCoordinates currentLocationCoordinates)
        {
            _playerShield = player.Shield;
            _playerHealth = player.Health;


            _player = player;
            _messages = initialMessages;
            _gameMap = gameMap;
            _gameMap.CurrentLocationCoordinates = currentLocationCoordinates;
            _currentLocation = _gameMap.CurrentLocation; 

        }
        
        #endregion

    }
}
