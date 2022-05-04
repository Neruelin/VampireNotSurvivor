using System;
public class StatResource : Stat {
    public float Min;
    public float Cap;
    private float Current;
    public float Initial;
    public Stat Delta;

    public StatResource(string Tag, float Base, float Mult, float Flat, float Min, float Cap, float Initial, float DeltaBase, float DeltaMult, float DeltaFlat) : base(Tag, Base, Mult, Flat) {
        this.Cap = Cap;
        this.Initial = Initial;
        this.Current = Initial;
        this.Delta = new Stat("Delta" + Tag, DeltaBase, DeltaMult, DeltaFlat);
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
        this.Current = Math.Min(this.Cap, Math.Max( this.Min, (this.Current + this.Delta.Value())));
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
}