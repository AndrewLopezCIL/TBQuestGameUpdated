using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TBQuestGame.PresentationLayer;

namespace TBQuestGame.Models
{
    public abstract class Enemy : ObservableObject
    {

        #region FIELDS
 
        private double _health;
        private double _baseAttack = 2.9;
        private double _criticalAttack;
        private double _xpDrop;
        private double _itemDrop;
        private int _level;
        private string _name; 
        private int _listPlacement;
        public int currentID = 0;
        private int _id;
        private bool _isBoss;
        private bool _selectedToFight = false;
        GameSessionViewModel gameSessionViewModel;
       public DispatcherTimer attackTimer = new System.Windows.Threading.DispatcherTimer();
        //
        // Contains all of the enemie objects the player is currently fighting
        //
        public static ObservableCollection<Enemy> attackingEnemies = new ObservableCollection<Enemy>();

        

        #endregion

        #region PROPERTIES
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public bool IsBoss
        {
            get { return _isBoss; }
            set { _isBoss = value; }
        }
        public double Health
        {
            get { return _health; }
            set { _health = value; }
        } 
        public double XPDrop
        {
            get { return _xpDrop; }
            set { _xpDrop= value; }
        }
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public double ItemDrop
        {
            get { return _itemDrop; }
            set { _itemDrop = value; }
        } 
        public double BaseAttack
        {
            get { return _baseAttack; }
            set { _baseAttack = value; }
        }
        public int listPlacement
        {
            get { return _listPlacement; }
            set { _listPlacement = value; }
        }
        private bool _attacking;
        public bool AttackingPlayer
        {
            get { return _attacking; }
            set { _attacking = value; }
        } 
        public bool SelectedToFight
        {
            get { return _selectedToFight; }
            set { _selectedToFight = value; OnPropertyChanged(nameof(SelectedToFight)); }
        }
        #endregion

        #region METHODS
        public double getCriticalAttack()
        { 
            _criticalAttack = _baseAttack + (_baseAttack * .30);
            return _criticalAttack;
        }
        
        public void startAttackingPlayer()
        {
            attackTimer.Tick += new EventHandler(AttackTimerTick);
            attackTimer.Interval = new TimeSpan(0, 0, 1);
            attackTimer.Start();
        }
        public void stopAttackingPlayer()
        {
            attackTimer.Stop();
        }
        #endregion

        #region CONSTRUCTORS
        public Enemy()
        {

        }
        public Enemy(GameSessionViewModel _gameSessionViewModel)
        {
            this.gameSessionViewModel = _gameSessionViewModel;
            if (_gameSessionViewModel.CurrentLocation.MultiAttackLocation)
            {

                startAttackingPlayer();
            }
            else if (SelectedToFight == true)
            {
                startAttackingPlayer();
            }
            if (AttackingPlayer == true)
            {
                startAttackingPlayer();
            }
        }

        private void AttackTimerTick(object sender, EventArgs e)
        {
             
                //
                // If the player's shield is less than the base attack (can't withstand another hit)
                // And the shield value is more than 0, then set the shield to 0 and take the difference from
                // The player's health
                //
                if (gameSessionViewModel.PlayerShield < this.BaseAttack && gameSessionViewModel.PlayerShield > 0)
                {
                    double difference = this.BaseAttack - gameSessionViewModel.PlayerShield;
                    gameSessionViewModel.PlayerShield = 0;
                    gameSessionViewModel.PlayerHealth -= difference;
                }
                //
                // If the player's shield is more than the base attack (sufficient to withstand a hit) 
                // then decrement shield
                //
                else if (gameSessionViewModel.PlayerShield > this.BaseAttack)
                {
                    gameSessionViewModel.PlayerShield -= this.BaseAttack;
                }   //
                    // If the player's shield is 0/gone, move onto health decrement process
                    //
                else if (gameSessionViewModel.PlayerShield == 0)
                {
                    //
                    // If the player has more than 0 health and is alive, then handle health decrement process
                    //
                    if (gameSessionViewModel.PlayerHealth > 0 && gameSessionViewModel.Player.IsAlive)
                    {
                        gameSessionViewModel.PlayerHealth -= BaseAttack;
                        //
                        //If the player has less than 0 health after the attack, then set it to 0 and set isAlive to false
                        //
                        if (gameSessionViewModel.PlayerHealth < 0)
                        {
                            gameSessionViewModel.PlayerHealth = 0;
                            gameSessionViewModel.Player.IsAlive = false;
                        }
                    }
              
            }
        }
        #endregion


    }
}
