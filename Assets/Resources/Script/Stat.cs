using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat {
    public enum StatEnum : int {
        Speed,
        AttackDelay,
        Attack,
        Penetration,
    }
    
    public float Base;
    private float Mult;
    private float Flat;
    private float _Base;
    private float _Mult;
    private float _Flat;
    
    public Stat(float Base, float Mult, float Flat) {
        this.Base = Base;
        this.Mult = Mult;
        this.Flat = Flat;
        this._Base = Base;
        this._Mult = Mult;
        this._Flat = Flat;
    }

    public static Stat Default(StatEnum Tag) {
        switch (Tag) {
            case StatEnum.Speed:
                return new Stat(1, 1, 0);
            case StatEnum.AttackDelay:
                return new Stat(1, 1, 0);
            case StatEnum.Attack:
                return new Stat(1, 1, 0);
            case StatEnum.Penetration:
                return new Stat(1, 1, 0);
            default:
                Debug.Log("Invalid Stat Tag");
                return null;
        }
    }

    public void SetValues(float NewBase, float NewMult, float NewFlat) {
        Base = NewBase;
        Mult = NewMult;
        Flat = NewFlat;
        _Base = NewBase;
        _Mult = NewMult;
        _Flat = NewFlat;
    }
    public void SetBase(float value) {
        Base = value;
        _Base = value;
    }
    public void SetMult(float value) {
        Mult = value;
        _Mult = value;
    }
    public void SetFlat(float value) {
        Flat = value;
        _Flat = value;
    }

    public void Reset() {
        Base = _Base;
        Mult = _Mult;
        Flat = _Flat;
    }

    public float Value() {
        return (Base * Mult) + Flat;
    }
}

public class StatResource : Stat {

    public enum ResourceEnum : int { 
        Health,
        Energy,
    }

    public float Min;
    public float Cap;
    private float Current;
    public float Initial;
    private float _Min;
    private float _Cap;
    public Stat Delta;

    public StatResource(float Base, float Mult, float Flat, float Min, float Cap, float Initial, float DeltaBase, float DeltaMult, float DeltaFlat) : base(Base, Mult, Flat) {
        this.Cap = Cap;
        this.Min = Min;
        this.Initial = Initial;
        this.Current = Initial;
        this._Cap = Cap;
        this._Min = Min;
        this.Delta = new Stat(DeltaBase, DeltaMult, DeltaFlat);
    }

    public static StatResource Default(ResourceEnum Tag) { 
        switch (Tag) {
            case ResourceEnum.Health:
                return new StatResource(1, 1, 0, 0, 1, 1, 1, 1, 0);
            case ResourceEnum.Energy:
                return new StatResource(1, 1, 0, 0, 1, 1, 1, 1, 0);
            default:
                Debug.Log("Invalid StatResource Tag");
                return null;
        }
    } 

    public void ApplyDelta(float seconds) {
        this.Current = Math.Min(this.Cap, Math.Max( this.Min, (this.Current + (this.Delta.Value() * seconds))));
    }

    public new float Value() {
        return this.Current;
    }

    public new void Reset() {
        float percent = Current / Cap;
        Cap = _Cap;
        Min = _Min;
        Current = (percent * (Cap - Min)) + Min;
        Delta.Reset();
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

    public void SetValues(float NewBase, float NewMult, float NewFlat, float NewMin, float NewCap) {
        base.SetValues(NewBase, NewMult, NewFlat);
        float percent = Current / Cap;
        Min = NewMin;
        Cap = NewCap;
        Current = (percent * (Cap - Min)) + Min;
        _Min = NewMin;
        _Cap = NewCap;
    }
    
    public void SetMin(float amount) {
        Min = amount;
        _Min = amount;
    }
    public void SetCap(float amount) {
        Cap = amount;
        _Cap = amount;
    }
}