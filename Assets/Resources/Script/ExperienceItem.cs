using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : Item
{
    public float experience;
    public override void Add(Inventory inventory){
        inventory.GainExperience(experience);
    }

    public override void Remove(Inventory inventory){

    }
}
