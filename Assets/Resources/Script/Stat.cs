using System.Collections;
using System.Collections.Generic;
public class Stat {
    public string Tag;
    public float Base;
    private float Mult;
    private float Flat;
    private float Current;
    public List<StatModifier> Mods = new List<StatModifier>(); 
    
    public Stat(string Tag, float Base, float Mult, float Flat) {
        this.Base = Base;
        this.Mult = Mult;
        this.Flat = Flat;
        this.Tag = Tag;
    }

    public void SetBase(float value) {
        Base = value;
    }
    public void SetMult(float value) {
        Mult = value;
    }
    public void SetFlat(float value) {
        Flat = value;
    }

    public void AddModifier (StatModifier Mod) {
        Mods.Add(Mod);
        Mult *= Mod.Mult;
        Flat *= Mod.Flat;
    }

    public bool RemoveModifier (StatModifier Mod) {
        return Mods.Remove(Mod);
    }

    public bool RemoveModifierByTag (string Tag) {
        bool success = false;
        for (int i = 0; i < Mods.Count; i++) {
            if (Mods[i].Tag == Tag) {
                Mods.RemoveAt(i);
                success = true;
                break;
            }
        }
        return success;
    }

    public float Value() {
        return (Base * Mult) + Flat;
    }
}