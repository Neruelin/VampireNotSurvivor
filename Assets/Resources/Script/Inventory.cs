using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<Item> items;
    private Dictionary<string, Item> itemDictionary;
    public float experience;

    private void Awake(){
        items = new List<Item>();
        itemDictionary = new Dictionary<string, Item>();
    }
    //TODO: Add stackable item id
    public void AddItem(Item item){
        if(!itemDictionary.ContainsKey(item.id)){
            items.Add(item);
            itemDictionary.Add(item.id, item);
        }
    }

    public void RemoveItem(Item item){
        if(itemDictionary.TryGetValue(item.id, out Item value)){
            items.Remove(value);
            itemDictionary.Remove(value.id);
        }
    }

    public void ClearItems(){
        items = new List<Item>();
    }

    public void GainExperience(float value){
        experience += value;
    }
}