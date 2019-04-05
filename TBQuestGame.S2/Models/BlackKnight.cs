using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.PresentationLayer;

namespace TBQuestGame.Models
{
    public class BlackKnight : Enemy
    {
        private int _level = 45;
        private string _imageString;
        private int health = 145;

        public string Image
        {
            get { return _imageString; }
            set { _imageString = value; }
        }

        public BlackKnight()
        {

        }
        public BlackKnight(bool isBoss, GameSessionViewModel _gameSessionViewModel)
        {
            this.Health = health;
            this.Level = _level;
            this.BaseAttack = this.BaseAttack += (this.Level / 100) + .50;
            this._imageString = "warrior-black.png";
            this.Name = "Black Knight";
            _gameSessionViewModel.CurrentEnemyID += 1;
            this.ID = _gameSessionViewModel.CurrentEnemyID;
            //
            // if passed isBoss bool value is true, then set the property to true, otherwise set the property to false
            //
            isBoss = true ? IsBoss = isBoss : IsBoss = isBoss;
        }
    }
}
