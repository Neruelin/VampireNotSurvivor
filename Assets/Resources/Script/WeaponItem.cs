using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    public override void Add(Inventory inventory){
        inventory.AddItem(this);
    }

    public override void Remove(Inventory inventory){

    }
}
