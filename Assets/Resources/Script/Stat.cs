using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat {
    public enum StatEnum {
        Speed,
        AttackDelay,
        Health,
        Attack,
        Energy
    }
    
    public StatEnum Tag;
    public float Base;
    private float Mult;
    private float Flat;
    private float Current;
    public List<StatModifier> Mods = new List<StatModifier>(); 
    
    public Stat(StatEnum Tag, float Base, float Mult, float Flat) {
        this.Base = Base;
        this.Mult = Mult;
        this.Flat = Flat;
        this.Tag = Tag;
    }

    public static Stat Default(StatEnum Tag) {
        switch (Tag) {
            case StatEnum.Speed:
                return new Stat(Tag, 1, 1, 0);
            case StatEnum.AttackDelay:
                return new Stat(Tag, 1, 1, 0);
            case StatEnum.Attack:
                return new Stat(Tag, 1, 1, 0);
            default:
                Debug.Log("Invalid Stat Tag");
                return null;
        }
    }

    public void SetValues(float NewBase, float NewMult, float NewFlat) {
        Base = NewBase;
        Mult = NewMult;
        Flat = NewFlat;
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

public class StatResource : Stat {
    public float Min;
    public float Cap;
    private float Current;
    public float Initial;
    public Stat Delta;

    public StatResource(StatEnum Tag, float Base, float Mult, float Flat, float Min, float Cap, float Initial, float DeltaBase, float DeltaMult, float DeltaFlat) : base(Tag, Base, Mult, Flat) {
        this.Cap = Cap;
        this.Initial = Initial;
        this.Current = Initial;
        this.Delta = new Stat(Tag, DeltaBase, DeltaMult, DeltaFlat);
    }

    public static new StatResource Default(StatEnum Tag) { 
        switch (Tag) {
            case StatEnum.Health:
                return new StatResource(Tag, 1, 1, 0, 0, 1, 1, 1, 1, 0);
            case StatEnum.Energy:
                return new StatResource(Tag, 1, 1, 0, 0, 1, 1, 1, 1, 0);
            default:
                Debug.Log("Invalid StatResource Tag");
                return null;
        }
    } 

    public void AddDeltaModifier (StatModifier Mod) {
        Delta.AddModifier(Mod);
    } 

    public bool RemoveDeltaModifier (StatModifier Mod) {
        return Delta.RemoveModifier(Mod);
    }

    public bool RemoveDeltaModifierByTag (string Tag) {
        return Delta.RemoveModifierByTag(Tag);
    }

    public void ApplyDelta(float seconds) {
        this.Current = Math.Min(this.Cap, Math.Max( this.Min, (this.Current + (this.Delta.Value() * seconds))));
    }

    public new float Value() {
        return this.Current;
    }

    public bool Spend(float amount) {
        if (this.Current >= amount) {
            this.Current -= amount;
            return true;
        } 
        return false;
    }
    public float Add(float amount) {
        this.Current = Math.Min(this.Cap, Math.Max( this.Min, (this.Current + amount)));
        return this.Current;
    }
    public float Remove(float amount) {
        this.Current = Math.Min(this.Cap, Math.Max( this.Min, (this.Current - amount)));
        return this.Current;
    }
    public void Set(float amount) {
        this.Current = Math.Min(this.Cap, Math.Max( this.Min, amount));
    }

    public void SetValues(float NewBase, float NewMult, float NewFlat, float NewMin, float NewCap, float NewInitial) {
        base.SetValues(NewBase, NewMult, NewFlat);
        Min = NewMin;
        Cap = NewCap;
        Initial = NewInitial;
    }
    
    public void SetMin(float amount) {
        Min = amount;
    }
    public void SetCap(float amount) {
        Cap = amount;
    }
    public void SetInitial(float amount) {
        Initial = amount;
    }
}