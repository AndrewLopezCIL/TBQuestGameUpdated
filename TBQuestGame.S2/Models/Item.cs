using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public abstract class Item
    {
        //
        // Current Trackable ID of the Item
        //
        private int _itemID;

        public int ID
        {
            get { return _itemID; }
            set { _itemID = value; }
        }
        //
        // The item's display name
        //
        private string _itemName;

        public string Name
        {
            get { return _itemName; }
            set { _itemName = value; }
        }
        //
        // How much the item is worth in Gold
        //
        private int _itemValue;

        public int Value
        {
            get { return _itemValue; }
            set { _itemValue = value; }
        }
        //
        // To keep track of how much the player is currently carrying
        //
        private int _itemStackCount;

        public int ItemStackCount
        {   
            get { return _itemStackCount; }
            set { _itemStackCount = value; }
        }
        //
        // Whether or not the player can equip the item
        //
        private bool _equipable;

        public bool CanEquip
        {
            get { return _equipable; }
            set { _equipable = value; }
        }
        //
        // Whether or not the item can be consumed/used ( as in a buff type way not equipable )
        //
        private bool _consumable;

        public bool Consumable
        {
            get { return _consumable; }
            set { _consumable = value; }
        }

        //
        // If the item is in the player's inventory/was collected by the player
        //
        private bool _collected;
            
        public bool Collected
        {
            get { return _collected; }
            set { _collected = value; }
        }


    }
}
