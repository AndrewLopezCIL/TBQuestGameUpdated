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
        // player value for Player property
        private Player _player;

        // player health value for PlayerHealth property
        private double _playerHealth;

        // player shield value for PlayerShield property
        private double _playerShield;

        // locationWarningImages value for LocationWarningImage property
        private string _locationWarningImages;

        // gameMap value for GameMap property
        private Map _gameMap; 

        // current location value for CurrentLocation property
        private Location _currentLocation;

        // multiAttackLocation value for MultiAttackLocation property
        private bool _multiAttackLocation;

        // GameData object being instantiated
        GameData _gameData = new GameData();

        // currentEnemyID value for CurrentEnemyID property
        private int currentEnemyID;

        // currentLocationName value for CurrentLocationName property
        private string _currentLocationName;

        // accessibleLocations value for AccessibleLocations property
        public ObservableCollection<Location> _accessibleLocations;

        // currentEnemies list for CurrentEnemies property
        public ObservableCollection<Enemy> _currentEnemies;

        // Current enemy the player's fighting's ID
        private int _currentFightingEnemyID;
        // Current enemy fighting's listplacement ID
        private int _currentFightingEnemyListPlacement;


        //
        // CurrentFightingEnemyID property
        // used for getting the current selected enemy's ID
        // set when player selects enemies from the list
        //
        public int CurrentFightingEnemyID
        {
            get { return _currentFightingEnemyID; }
            set { _currentFightingEnemyID = value; OnPropertyChanged(nameof(CurrentFightingEnemyID)); }
        }
        public int CurrentFightingEnemyListPlacement
        {
            get { return _currentFightingEnemyListPlacement; }
            set { _currentFightingEnemyListPlacement = value; OnPropertyChanged(nameof(CurrentFightingEnemyListPlacement)); }
        }
        //
        // used to stop a reoccuring boss battle when user goes back to location
        //
        public List<Location> bossesDefeated = new List<Location>();
        private int _missionLength = 0;
        //
        // List of current enemies battling, this is binded and used to display the list of enemies in the
        // ActiveEnemies ListBox controls
        //
        public ObservableCollection<Enemy> CurrentEnemies
        {
            get { return _currentEnemies; }
            set { _currentEnemies = value;  OnPropertyChanged(nameof(CurrentEnemies)); }
        }
        private Enemy _selectingEnemy;
        public Enemy SelectingEnemy
        {
            get { return _selectingEnemy; }
            set { _selectingEnemy = value; OnPropertyChanged(nameof(SelectingEnemy)); }
        }
        //
        // Going to be removed in the future
        //
        public int MissionLength
        {
            get { return _missionLength; }
            set { _missionLength = value; }
        }
        //
        // Gets the current enemy's ID 
        // This is used to track which enemies are which, increments everytime an enemy is instantiated
        // Going to be used to track which enemy in the list gets hurt when they're selected in ActiveEnemies control
        //
        public int CurrentEnemyID
        {
            get { return currentEnemyID; }
            set { currentEnemyID = value; OnPropertyChanged(nameof(CurrentEnemyID)); }
        }
        //
        // AccessibleLocations is used to store all of the locations that the player can access
        //
        public ObservableCollection<Location> AccessibleLocations
        {
            get { return _accessibleLocations; }
            set { _accessibleLocations = value; OnPropertyChanged(nameof(AccessibleLocations)); }
        }
        //
        // Returns the current Location's name
        //
        public string CurrentLocationName
        {
            get { return _currentLocationName; }
            set { _currentLocationName = value; OnPropertyChanged(nameof(CurrentLocationName)); }
        }
        //
        // Returns the player's current Location
        //
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; OnPropertyChanged(nameof(CurrentLocation)); }
        }
        //
        // Returns messages for the current Location
        //
        private List<string> _messages;
        #endregion

        #region PROPERTIES
        //
        // Gets / Sets the current GameMap
        //
        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }
        //
        // Gets / Set the current player
        //
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        } 
        //
        // Used to determine which enemy is currently selected on the ListBox ActiveEnemies
        //
        private bool _enemySelected;
        public bool EnemySelected
        {
            get { return _enemySelected; }
            set { _enemySelected = value; OnPropertyChanged(nameof(EnemySelected)); }
        }

        //
        // Return the list of strings converted to a single string
        // with new lines between each message
        //
        public string Messages
        {
            get { return string.Join("\n\n",_messages); }
        }
        //
        // Gets / Sets the player's health
        //
        public double PlayerHealth
        {
            get { return _playerHealth; }
            set { _playerHealth = value; OnPropertyChanged(nameof(PlayerHealth)); }
        }
        //
        // Gets / Sets the player's shield
        //
        public double PlayerShield
        {
            get { return _playerShield; }
            set { _playerShield = value; OnPropertyChanged(nameof(PlayerShield)); }
        }

        #endregion

        #region METHODS 
        public void SelectedEnemySetter(int selected, GameSessionView gsv)
        {
            EnemySelected = true;  
            foreach (Enemy E in CurrentEnemies)
            {

                E.refreshAllEnemiesPositions();
                if (E.listPlacement == selected)
                { 
                        CurrentFightingEnemyID = E.ID;
                        CurrentFightingEnemyListPlacement = E.listPlacement;
                        Player.currentlyAttacking = E;
                        SelectingEnemy = E;
                        if (E.SelectedToFight == false)
                        {
                            E.AttackingPlayer = true;
                            E.startAttackingPlayer();
                            gsv.EnemyPicture.Source = E.PictureSource;
                            E.SelectedToFight = true;
                        }
                        else
                        {
                            E.SelectedToFight = true;
                        }
                 }
                else if (E.listPlacement != selected)
                {
                    if (E.listPlacement != selected && E.SelectedToFight == true)
                    {
                    E.SelectedToFight = false;
                        E.AttackingPlayer = false;
                        E.stopAttackingPlayer();
                    }
                    else
                    {
                        E.SelectedToFight = false;
                    } 
                }
            } 
        }
        #endregion

        #region CONSTRUCTORS
        public GameSessionViewModel()
        {

        }
        //
        // Constructor used for setting the current properties values
        //
        public GameSessionViewModel(Player player, List<string>initialMessages, Map gameMap, GameMapCoordinates currentLocationCoordinates, ObservableCollection<Enemy> currentEnemies)
        {
            // sets the player's shield
            _playerShield = player.Shield;
            // sets the player's health
            _playerHealth = player.Health;  
            // gets currentEnemyID from gameData, and sets the currentEnemyID variable in the view model to equal that
            currentEnemyID = _gameData.currentEnemyID;
            // gets the current enemies list that is passed and sets _currentEnemies in the view model to equal that
            _currentEnemies = currentEnemies;
            // sets the player
            _player = player;
            // sets the messages property
            _messages = initialMessages;
            // sets the current gameMap
            _gameMap = gameMap;
            // sets the player's currentLocationCoordinates in the view model
            _gameMap.CurrentLocationCoordinates = currentLocationCoordinates;
            // sets the currentLocation to equal the gameMap's passed currentLocation
            _currentLocation = _gameMap.CurrentLocation;
            // sets the current locationWarningImage to the gameMap's currentLocation's warning Image
            _locationWarningImages = _gameMap.CurrentLocation.LocationWarningImage;
        }
        
        #endregion

    }
}
