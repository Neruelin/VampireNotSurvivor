using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public int[] items;
    private Dictionary<int, Item> itemDictionary;

    // Idea: Make arrays size = total number of item id.
    // When pick up item, go to item array, and increase the int value to represent the number of times item has been picked up.
    // Modify stats by X value/percentage
    //
    // Have a list of recently picked up items. (For displaying on UI?)
    // Add to list, when user has picked up recently. 
    // Update list when user has picked up the same item, to upgrade.
    //
    // sent to UI -> list, with item data

    // Or have dictionary of only recently picked up items <id, item data>
    // UI displays data dictionary.
    // finds item count from id
    // Add when user picks up new item
    // update -> check if item in dictionary
    // --> Add/Update -> Update UI -> UI iterates through dictionary -> display Item Data -> Use item.id (key) to find Items to determine the number have
    private void Awake(){
        items = new int[10];
        itemDictionary = new Dictionary<int, Item>();
    }
    //TODO: Add stackable item id
    public void AddItem(Item item){
        // Update item array count
        items[item.id] += items[item.id]+1;

        if(!itemDictionary.ContainsKey(item.id)){
            itemDictionary.Add(item.id, item);
        }
    }

    public void RemoveItem(Item item){
        if(itemDictionary.TryGetValue(item.id, out Item value)){
            items[item.id] = 0;
            itemDictionary.Remove(value.id);
        }
    }

    public void ClearItems(){
        for(int i = 0; i < items.Length; i++){
            items[i] = 0;
        }
        itemDictionary = new Dictionary<int, Item>();
    }
}