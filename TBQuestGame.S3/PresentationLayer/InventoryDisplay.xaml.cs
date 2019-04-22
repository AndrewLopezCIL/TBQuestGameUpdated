using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TBQuestGame.Models;
using TBQuestGame.Models.Items;

namespace TBQuestGame.PresentationLayer
{
    /// <summary>
    /// Interaction logic for InventoryDisplay.xaml
    /// </summary>
    public partial class InventoryDisplay : Window
    {
        GameSessionViewModel gsm;
        GameSessionView gsv;
        public int _LocationLootListItemSelected = 0;
        public int _PlayerInventoryItemSelected = 0;
        private bool stopLoop = false;
        public InventoryDisplay(GameSessionViewModel gsm, GameSessionView GSV)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.gsm = gsm;
            this.gsv = GSV;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void LocationLootList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            for (int item = 0; item < gsm.GameMap.CurrentLocation.LootableItems.Count; item++)
            {
                if (gsm.GameMap.CurrentLocation.LootableItems[item].HasListSelection == true)
                {
                    if (gsm.Player.Inventory.Count >= 1)
                    {
                        bool nameFoundCheck = false;
                        for (int obj = 0; obj < gsm.Player.Inventory.Count; obj++)
                        {
                        if (gsm.Player.Inventory[obj].Name == gsm.GameMap.CurrentLocation.LootableItems[item].Name && gsm.GameMap.CurrentLocation.LootableItems[item].SpecialObject == false)
                            {
                                gsm.Player.Inventory[obj].ItemStackCount += gsm.GameMap.CurrentLocation.LootableItems[item].ItemStackCount;
                                gsm.GameMap.CurrentLocation.LootableItems.RemoveAt(item);
                                nameFoundCheck = true;
                                break;
                            }
                            else if (gsm.Player.Inventory[obj].Name == gsm.GameMap.CurrentLocation.LootableItems[item].Name && gsm.GameMap.CurrentLocation.LootableItems[item].SpecialObject == true)
                            {
                                gsm.Player.Inventory.Add(gsm.GameMap.CurrentLocation.LootableItems[item]);
                                gsm.GameMap.CurrentLocation.LootableItems.RemoveAt(item);
                                nameFoundCheck = true;
                                break;

                            } 
                        //
                        // LOGIC ERROR, it checks whether or not the name of the object is equal to the other objects in the list
                        // if not then it adds it to the player's inventory as a seperate object
                        // The error is, what if the first object it's looping through doesn't have the same name as the current object?
                        // Then will it just add since it's not the same
                        //
                            else if(gsm.Player.Inventory[obj].Name != gsm.GameMap.CurrentLocation.LootableItems[item].Name)
                            {
                                nameFoundCheck = false;  
                            }
                        }
                        if (nameFoundCheck == false)
                        {
                            gsm.Player.Inventory.Add(gsm.GameMap.CurrentLocation.LootableItems[item]);
                            gsm.GameMap.CurrentLocation.LootableItems.RemoveAt(item);
                        }
                      
                    }
                     if (gsm.Player.Inventory.Count == 0)
                    {
                        gsm.Player.Inventory.Add(gsm.GameMap.CurrentLocation.LootableItems[item]);
                        gsm.GameMap.CurrentLocation.LootableItems.RemoveAt(item);
                    }
                   
                  //  gsm.Player.Inventory.Add(gsm.GameMap.CurrentLocation.LootableItems[item]); 
                    

                }
                
            }
        }
        //
        // Gets the currently selected vicinity / loot area item's position
        //
        private void LocationLootListSelectorSetter(int selected)
        {
            _LocationLootListItemSelected = selected;
            for (int item = 0; item < gsm.GameMap.CurrentLocation.LootableItems.Count; item++)
            {
                if (gsm.GameMap.CurrentLocation.LootableItems[item].LocationListPlacement == selected)
                {
                    gsm.GameMap.CurrentLocation.LootableItems[item].HasListSelection = true;
                }
                else if (gsm.GameMap.CurrentLocation.LootableItems[item].LocationListPlacement != selected)
                {
                    gsm.GameMap.CurrentLocation.LootableItems[item].HasListSelection = false;
                }
            }
        }
        //
        // Gets the currently selected player inventory item's position
        // 
        private void PlayerInventorySelectorSetter(int selected)
        {
            _PlayerInventoryItemSelected = selected;
            for (int item = 0; item < gsm.Player.Inventory.Count; item++)
            {
                if (gsm.Player.Inventory[item].PlayerInventoryListPlacement == selected)
                {
                    gsm.Player.Inventory[item].HasPlayerInventorySelection = true;
                }
                else if (gsm.Player.Inventory[item].PlayerInventoryListPlacement != selected)
                {
                    gsm.Player.Inventory[item].HasPlayerInventorySelection = false;
                }
            }
        }
        //
        // Updates the list placement of lootable items
        //
        private void UpdateListPlacement()
        {
            for (int item = 0; item < gsm.GameMap.CurrentLocation.LootableItems.Count; item++)
            {
                gsm.GameMap.CurrentLocation.LootableItems[item].LocationListPlacement = item;
            }
        }
        //
        // Updates the player's inventory list placement of items
        //
        private void UpdatePlayerInventoryListPlacement()
        {
            for (int item = 0; item < gsm.Player.Inventory.Count; item++)
            {
                gsm.Player.Inventory[item].PlayerInventoryListPlacement = item;
            }
        }
        //
        // Selection changed handler for location loot
        //
        private void LocationLootList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;
            UpdateListPlacement();
           LocationLootListSelectorSetter(item.SelectedIndex);
        }
        //
        // Selection changed handler for players inventory
        //
        private void PlayerInventoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender; 
            UpdatePlayerInventoryListPlacement();
            PlayerInventorySelectorSetter(item.SelectedIndex);
        }
        
        //
        // Player inventory item used/consumed feature
        //
        private void PlayerInventoryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            stopLoop = false;
            for (int item = 0; item < gsm.Player.Inventory.Count; item++)
            { 
               
                if (stopLoop == true)
                {
                    break;
                }
                if (gsm.Player.Inventory[item].HasPlayerInventorySelection == true)
                {
                    //
                    // If the item in player's inventory is a consumable, and it hasn't been consumed then use.
                    //
                    if (gsm.Player.Inventory[item].Consumable == true && gsm.Player.Inventory[item].ConsumableUsed == false)
                    {
                        gsm.Player.Inventory[item].ConsumableUsed = true;
                        switch (gsm.Player.Inventory[item].Name)
                        {
                            case "Basic Healing Potion":
                                if (gsm.Player.Inventory[item].ItemStackCount > 1)
                                {
                                    gsm.Player.Inventory[item].ItemStackCount -= 1;
                                }
                                else if (gsm.Player.Inventory[item].ItemStackCount == 1)
                                { 
                                gsm.Player.Inventory.RemoveAt(item);
                                }

                                BasicHealingPotion potion = new BasicHealingPotion(gsm, gsv);
                                potion.potionUsedWithCooldown(gsm);
                                break; 
                            default:
                                break;
                        }
                    }
                    else if(gsm.Player.Inventory[item].CanEquip == true){
                        switch (gsm.Player.Inventory[item].Name)
                        {
                            case "Ruby Sword":
                                #region EquippingSword

                                //gsm.PlayerBaseAttack = gsm.Player.Inventory[item].dama
                                //
                                // Set the currently equiped weapon property in player's class to equal
                                // the Ruby Sword, then add the old equiped weapon to the player's inventory
                                // Remove Ruby Sword from player's inventory when equiped 
                                //
                                if (gsm.Player.EquippedWeapon == null) {

                                    if (gsm.Player.Inventory[item].ItemStackCount > 1)
                                    {
                                        gsm.Player.Inventory[item].ItemStackCount -= 1;
                                        gsm.Player.Inventory[item].Equipped = true;
                                        gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                        gsm.PlayerEquippedItems.Add(gsm.Player.Inventory[item]);
                                        stopLoop = true;

                                        break;
                                        //gsm.Player.Inventory.RemoveAt(gsm.Player.Inventory[item].PlayerInventoryListPlacement);
                                    }
                                    else if (gsm.Player.Inventory[item].ItemStackCount == 1)
                                    {
                                        gsm.Player.EquippedItems.Add(gsm.Player.Inventory[item]);
                                        gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                        gsm.Player.Inventory[item].Equipped = true;
                                        gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                        stopLoop = true;

                                        break;
                                    }
                                }
                                else if (gsm.Player.EquippedWeapon != null)
                                {
                                    bool foundInv = false;
                                    for (int inv = 0; inv < gsm.Player.Inventory.Count; inv++)
                                    {
                                        //
                                        // If there's an item in the player's inventory that equals the currently equipped item's name and it's not special
                                        // then increment stack value, set equipped to false, and set the equipped item to Ruby Sword
                                        //
                                        if (gsm.Player.EquippedWeapon.SpecialObject == false) {
                                            if (gsm.Player.Inventory[inv].Name == gsm.Player.EquippedWeapon.Name)
                                            {
                                                //    gsm.Player.Inventory[inv].ItemStackCount += 1;
                                                //    gsm.Player.EquippedWeapon.Equipped = false;
                                                //    // if doesnt work then add for loop
                                                //    gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                //    gsm.Player.EquippedWeapon = gsm.Player.Inventory[inv];
                                                foundInv = true;
                                                stopLoop = true;

                                                break;
                                            } 
                                            else if (gsm.Player.Inventory[item].Name != gsm.Player.EquippedWeapon.Name)
                                            {
                                                if (gsm.Player.Inventory[item].SpecialObject == false) {
                                                    if (gsm.Player.Inventory[inv].ItemStackCount > 1)
                                                    {
                                                        gsm.Player.Inventory[inv].ItemStackCount -= 1;
                                                        gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                        gsm.Player.EquippedWeapon = gsm.Player.Inventory[inv];
                                                        gsm.Player.EquippedWeapon.Equipped = true;
                                                        stopLoop = true;
                                                        foundInv = true;
                                                        break;
                                                    }
                                                    else if (gsm.Player.Inventory[inv].ItemStackCount == 1)
                                                    {
                                                        gsm.Player.Inventory.Add(gsm.Player.EquippedWeapon);
                                                        gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                        gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                                        gsm.Player.EquippedItems.Add(gsm.Player.EquippedWeapon);
                                                        gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                                        stopLoop = true;
                                                        foundInv = true;
                                                        break;
                                                    } }
                                                else if (gsm.Player.Inventory[item].SpecialObject == true)
                                                {

                                                }
                                            }

                                        }
                                        //
                                        // Last edits made in this area ( Re-arranging code might be the problem)
                                        //
                                        else if (gsm.Player.EquippedWeapon.SpecialObject == true)
                                        {
                                            if (gsm.Player.Inventory[inv].Name == gsm.Player.EquippedWeapon.Name)
                                            {
                                                gsm.Player.Inventory.Add(gsm.Player.EquippedWeapon);
                                                gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                                gsm.Player.EquippedItems.Add(gsm.Player.EquippedWeapon);
                                                gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                               
                                                foundInv = true;
                                                stopLoop = true;

                                                break;
                                            }
                                            else if (gsm.Player.Inventory[item].Name != gsm.Player.EquippedWeapon.Name)
                                            { 
                                                 
                                                    gsm.Player.Inventory.Add(gsm.Player.EquippedWeapon);
                                                    gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                    gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                                    gsm.Player.EquippedItems.Add(gsm.Player.EquippedWeapon);
                                                    gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                                    stopLoop = true;
                                                    foundInv = true;
                                                    break;
                                            
                                            } }
                                    }
                                    if (foundInv == false)
                                    {
                                        gsm.Player.Inventory.Add(gsm.Player.EquippedWeapon);
                                        gsm.Player.EquippedWeapon.Equipped = false;
                                        stopLoop = true;
                                        break;
                                    }
                                }
                                #endregion
                                
                                break;

                            case "Excalibur":
                                #region EquippingSword

                                //gsm.PlayerBaseAttack = gsm.Player.Inventory[item].dama
                                //
                                // Set the currently equiped weapon property in player's class to equal
                                // the Ruby Sword, then add the old equiped weapon to the player's inventory
                                // Remove Ruby Sword from player's inventory when equiped 
                                //
                                if (gsm.Player.EquippedWeapon == null)
                                {

                                    if (gsm.Player.Inventory[item].ItemStackCount > 1)
                                    {
                                        gsm.Player.Inventory[item].ItemStackCount -= 1;
                                        gsm.Player.Inventory[item].Equipped = true;
                                        gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                        gsm.PlayerEquippedItems.Add(gsm.Player.Inventory[item]);
                                        stopLoop = true;

                                        break;
                                        //gsm.Player.Inventory.RemoveAt(gsm.Player.Inventory[item].PlayerInventoryListPlacement);
                                    }
                                    else if (gsm.Player.Inventory[item].ItemStackCount == 1)
                                    {
                                        gsm.Player.EquippedItems.Add(gsm.Player.Inventory[item]);
                                        gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                        gsm.Player.Inventory[item].Equipped = true;
                                        gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                        stopLoop = true;

                                        break;
                                    }
                                }
                                else if (gsm.Player.EquippedWeapon != null)
                                {
                                    bool foundInv = false;
                                    for (int inv = 0; inv < gsm.Player.Inventory.Count; inv++)
                                    {
                                        //
                                        // If there's an item in the player's inventory that equals the currently equipped item's name and it's not special
                                        // then increment stack value, set equipped to false, and set the equipped item to Ruby Sword
                                        //
                                        if (gsm.Player.EquippedWeapon.SpecialObject == false)
                                        {
                                            if (gsm.Player.Inventory[inv].Name == gsm.Player.EquippedWeapon.Name)
                                            {
                                                //    gsm.Player.Inventory[inv].ItemStackCount += 1;
                                                //    gsm.Player.EquippedWeapon.Equipped = false;
                                                //    // if doesnt work then add for loop
                                                //    gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                //    gsm.Player.EquippedWeapon = gsm.Player.Inventory[inv];
                                                foundInv = true;
                                                stopLoop = true;

                                                break;
                                            }
                                            else if (gsm.Player.Inventory[item].Name != gsm.Player.EquippedWeapon.Name)
                                            {
                                                if (gsm.Player.Inventory[inv].ItemStackCount > 1)
                                                {
                                                    gsm.Player.Inventory[inv].ItemStackCount -= 1;
                                                    gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                    gsm.Player.EquippedWeapon = gsm.Player.Inventory[inv];
                                                    gsm.Player.EquippedWeapon.Equipped = true;
                                                    stopLoop = true;
                                                    foundInv = true;
                                                    break;
                                                }
                                                else if (gsm.Player.Inventory[inv].ItemStackCount == 1)
                                                {
                                                    gsm.Player.Inventory.Add(gsm.Player.EquippedWeapon);
                                                    gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                                    gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                                    gsm.Player.EquippedItems.Add(gsm.Player.EquippedWeapon);
                                                    gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                                    stopLoop = true;
                                                    foundInv = true;
                                                    break;
                                                }
                                            }

                                        }
                                        else if (gsm.Player.EquippedWeapon.SpecialObject == true)
                                        {
                                            gsm.Player.Inventory.Add(gsm.Player.EquippedWeapon);
                                            gsm.Player.EquippedWeapon = gsm.Player.Inventory[item];
                                            gsm.Player.EquippedItems.Remove(gsm.Player.EquippedWeapon);
                                            foundInv = true;
                                            stopLoop = true;

                                            break;
                                        }
                                    }
                                    if (foundInv == false)
                                    {
                                        gsm.Player.Inventory.Add(gsm.Player.EquippedWeapon);
                                        gsm.Player.EquippedWeapon.Equipped = false;
                                        stopLoop = true;
                                        break;
                                    }
                                }
                                #endregion

                                break;

                            default:
                         break;
	}    
                    }
                }
            }
        }
          //
          // enabling the drop item function
          //
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DropItem.IsEnabled = true;
            DropAll.IsEnabled = true;
        }
        //
        // Disabling the drop item functions
        //
        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DropItem.IsEnabled = false;
            DropAll.IsEnabled = false;
        }
         //
         // Dropping one item at a time
         //
        private void DropItem_Click(object sender, RoutedEventArgs e)
        {
            for (int item = 0; item < gsm.Player.Inventory.Count; item++)
            { 
                if (gsm.Player.Inventory[item].HasPlayerInventorySelection == true)
                {
                    if (gsm.Player.Inventory[item].ItemStackCount > 1)
                    {
                        gsm.Player.Inventory[item].ItemStackCount -= 1;
                         bool found = false;
                        for (int items = 0; items < gsm.GameMap.CurrentLocation.LootableItems.Count; items++)
                        {
                            
                            if (gsm.GameMap.CurrentLocation.LootableItems[items].Name == gsm.Player.Inventory[item].Name)
                            {
                                gsm.GameMap.CurrentLocation.LootableItems[items].ItemStackCount += 1;
                                found = true;
                                break;
                            }
                            else if(gsm.GameMap.CurrentLocation.LootableItems[items].Name != gsm.Player.Inventory[item].Name)
                            {
                                found = false;
                            }
                        }
                        if (found == false)
                        {
                            gsm.GameMap.CurrentLocation.LootableItems.Add(gsm.Player.Inventory[item]);
                        }
                    } 
                    else if (gsm.Player.Inventory[item].ItemStackCount == 1)
                    {

                        bool found = false;
                        for (int items = 0; items < gsm.GameMap.CurrentLocation.LootableItems.Count; items++)
                        { 
                            if (gsm.GameMap.CurrentLocation.LootableItems[items].Name == gsm.Player.Inventory[item].Name && gsm.Player.Inventory[item].SpecialObject == false)
                            {
                                gsm.GameMap.CurrentLocation.LootableItems[items].ItemStackCount += 1;
                                found = true;
                                gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                break;
                            }
                            else if (gsm.GameMap.CurrentLocation.LootableItems[items].Name == gsm.Player.Inventory[item].Name && gsm.Player.Inventory[item].SpecialObject == true)
                            {
                                gsm.GameMap.CurrentLocation.LootableItems.Add(gsm.Player.Inventory[item]);
                                found = true;
                                gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                                break;
                            }
                            else if (gsm.GameMap.CurrentLocation.LootableItems[items].Name != gsm.Player.Inventory[item].Name)
                            {
                                found = false;
                            }

                        }
                        if (found == false)
                        {
                            gsm.GameMap.CurrentLocation.LootableItems.Add(gsm.Player.Inventory[item]);
                            gsm.Player.Inventory.Remove(gsm.Player.Inventory[item]);
                            break;
                        } 

                    }
                } 
            }
        }
        //
        // Dropping all of an object's stack at a time
        //
        private void DropAll_Click(object sender, RoutedEventArgs e)
        {
            for (int item = 0; item < gsm.Player.Inventory.Count; item++)
            {
                if (gsm.Player.Inventory[item].HasPlayerInventorySelection == true)
                {
                    if (gsm.Player.Inventory[item].ItemStackCount > 1)
                    {
                         
                        bool found = false;
                        for (int items = 0; items < gsm.GameMap.CurrentLocation.LootableItems.Count; items++)
                        {

                            if (gsm.GameMap.CurrentLocation.LootableItems[items].Name == gsm.Player.Inventory[item].Name)
                            {
                                gsm.GameMap.CurrentLocation.LootableItems[items].ItemStackCount += gsm.Player.Inventory[item].ItemStackCount;
                                found = true; 
                                break;
                            }
                            else if (gsm.GameMap.CurrentLocation.LootableItems[items].Name != gsm.Player.Inventory[item].Name)
                            {
                                found = false;
                            }
                        }
                        if (found == false)
                        {
                            gsm.GameMap.CurrentLocation.LootableItems.Add(gsm.Player.Inventory[item]);
                        }
                    }
                    else if (gsm.Player.Inventory[item].ItemStackCount == 1)
                    {

                        bool found = false;
                        for (int items = 0; items < gsm.GameMap.CurrentLocation.LootableItems.Count; items++)
                        {

                            if (gsm.GameMap.CurrentLocation.LootableItems[items].Name == gsm.Player.Inventory[item].Name)
                            {
                                gsm.GameMap.CurrentLocation.LootableItems[items].ItemStackCount += gsm.Player.Inventory[item].ItemStackCount;
                                found = true;
                                break;
                            }
                            else if (gsm.GameMap.CurrentLocation.LootableItems[items].Name != gsm.Player.Inventory[item].Name)
                            {
                                found = false;
                            }
                        }
                        if (found == false)
                        {
                            gsm.GameMap.CurrentLocation.LootableItems.Add(gsm.Player.Inventory[item]);
                        }

                    }
                        gsm.Player.Inventory.RemoveAt(item);
                }
            }
        }

        private void Label_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {

        }

        private void EquippedList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
