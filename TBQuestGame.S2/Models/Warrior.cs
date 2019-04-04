using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Warrior : Enemy
    { 
        private int _level = 34;
        private string _imageString;
        private int health = 125;
        
        public string Image
        {
            get { return _imageString; }
            set { _imageString = value; }
        }
       
        public Warrior()
        {

        }
        public Warrior(double passedhealth, int passedlevel)
        {
            this.Health = health;
            this.Level = _level;
            this._imageString = "warrior-icon.png";

        }
    }
}
