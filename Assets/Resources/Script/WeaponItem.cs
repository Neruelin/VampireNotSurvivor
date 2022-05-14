using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponItem : Item
{
    public override void OnPickUp(Inventory inventory){
        inventory.AddItem(this);
    }

    public override void OnRemove(Inventory inventory){

    }
}
