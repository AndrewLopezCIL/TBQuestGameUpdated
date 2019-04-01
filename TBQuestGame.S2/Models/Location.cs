using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.PresentationLayer;

namespace TBQuestGame.Models
{
   public class Location
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;

        private string locationMessage;

        public string LocationMessage
        {
            get { return locationMessage; }
            set { locationMessage = value; }
        }

        private bool _fightChance;

        public bool ChanceOfFight
        {
            get { return _fightChance; }
            set { _fightChance = value; }
        }


        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private bool _accessible;

        public bool Accessible
        {   
            get { return _accessible; }
            set { _accessible = value; }
        }

        private bool _fightRoom;

        public bool BossFightRoom
        {
            get { return _fightRoom; }
            set { _fightRoom = value; }
        }

       
        public static void enableControls(GameSessionView view)
        {
            view.North_Button.IsEnabled = true;
            view.East_Button.IsEnabled = true;
            view.South_Button.IsEnabled = true;
            view.West_Button.IsEnabled = true;
        }
        public static void disableControls(GameSessionView view)
        {
            view.North_Button.IsEnabled = false;
            view.East_Button.IsEnabled = false;
            view.West_Button.IsEnabled = false;
            view.South_Button.IsEnabled = false;
        }

      


    }
}
