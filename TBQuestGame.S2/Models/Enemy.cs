using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public abstract class Enemy
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
        //
        // Contains all of the enemie objects the player is currently fighting
        //
        public ObservableCollection<Enemy> attackingEnemies = new ObservableCollection<Enemy>();

        

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
        #endregion

        #region METHODS
        public double getCriticalAttack()
        { 
            _criticalAttack = _baseAttack + (_baseAttack * .30);
            return _criticalAttack;
        }
        #endregion

        #region CONSTRUCTORS

        #endregion


    }
}
