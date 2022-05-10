public abstract class Effect {
    // Apply adds effects, Remove must reverse the effects
    public abstract void Apply(Controller Target);
    public abstract void Remove(Controller Target);
}

// Effects:
    // Movement Speed
    // Bullet Size
    // 

public class SpeedEffectFactory {
    public static Effect MakeEffect(bool flat, float amount) {
        if (flat) {
            return new SpeedFlatEffect(amount);
        } else {
            return new SpeedMultEffect(amount);
        }
    }
}

public class SpeedFlatEffect : Effect {
    public float Amount = 5;
    public SpeedFlatEffect (float amount) {
        Amount = amount;
    }
    public override void Apply (Controller Target) {
        Stat Speed = Target.Stats[(int) Stat.StatEnum.Speed];
        Speed.SetFlat(Speed.Value() + Amount);
    }
    public override void Remove (Controller Target) {
        Stat Speed = Target.Stats[(int) Stat.StatEnum.Speed];
        Speed.SetFlat(Speed.Value() - Amount);
    }
}

public class SpeedMultEffect : Effect {
    public float Amount = 1.5f;
    public SpeedMultEffect (float amount) {
        Amount = amount;
    }
    public override void Apply (Controller Target) {
        Stat Speed = Target.Stats[(int) Stat.StatEnum.Speed];
        Speed.SetFlat(Speed.Value() * Amount);
    }
    public override void Remove (Controller Target) {
        Stat Speed = Target.Stats[(int) Stat.StatEnum.Speed];
        Speed.SetFlat(Speed.Value() / Amount);
    }
}

public class AttackEffectFactory {
    public static Effect MakeEffect(bool flat, float amount) {
        if (flat) {
            return new AttackFlatEffect(amount);
        } else {
            return new AttackMultEffect(amount);
        }
    }
}

public class AttackFlatEffect : Effect {
    public float Amount = 5;
    public AttackFlatEffect (float amount) {
        Amount = amount;
    }
    public override void Apply (Controller Target) {
        Stat Attack = Target.Stats[(int) Stat.StatEnum.Attack];
        Attack.SetFlat(Attack.Value() + Amount);
    }
    public override void Remove (Controller Target) {
        Stat Attack = Target.Stats[(int) Stat.StatEnum.Attack];
        Attack.SetFlat(Attack.Value() - Amount);
    }
}

public class AttackMultEffect : Effect {
    public float Amount = 1.5f;
    public AttackMultEffect (float amount) {
        Amount = amount;
    }
    public override void Apply (Controller Target) {
        Stat Attack = Target.Stats[(int) Stat.StatEnum.Attack];
        Attack.SetFlat(Attack.Value() * Amount);
    }
    public override void Remove (Controller Target) {
        Stat Attack = Target.Stats[(int) Stat.StatEnum.Attack];
        Attack.SetFlat(Attack.Value() / Amount);
    }
}

public class PenetrationEffectFactory {
    public static Effect MakeEffect(bool flat, float amount) {
        if (flat) {
            return new PenetrationFlatEffect(amount);
        } else {
            return new PenetrationMultEffect(amount);
        }
    }
}

public class PenetrationFlatEffect : Effect {
    public float Amount = 5;
    public PenetrationFlatEffect (float amount) {
        Amount = amount;
    }
    public override void Apply (Controller Target) {
        Stat Penetration = Target.Stats[(int) Stat.StatEnum.Penetration];
        Penetration.SetFlat(Penetration.Value() + Amount);
    }
    public override void Remove (Controller Target) {
        Stat Penetration = Target.Stats[(int) Stat.StatEnum.Penetration];
        Penetration.SetFlat(Penetration.Value() - Amount);
    }
}

public class PenetrationMultEffect : Effect {
    public float Amount = 1.5f;
    public PenetrationMultEffect (float amount) {
        Amount = amount;
    }
    public override void Apply (Controller Target) {
        Stat Penetration = Target.Stats[(int) Stat.StatEnum.Penetration];
        Penetration.SetFlat(Penetration.Value() * Amount);
    }
    public override void Remove (Controller Target) {
        Stat Penetration = Target.Stats[(int) Stat.StatEnum.Penetration];
        Penetration.SetFlat(Penetration.Value() / Amount);
    }
}

public class LevelUpEffectFactory {
    public static Effect MakeEffect(bool flat, float amount){
        if(flat){
            return new LevelUpFlatEffect(amount);
        } else {
            return new LevelUpMultEffect(amount);
        }
    }
}

public class LevelUpFlatEffect : Effect {
    public float Amount = 5; // 5 exp
    public LevelUpFlatEffect(float amount){
        Amount = amount; //also can do this.Amount = amount
    }
    public override void Apply (Controller Target) {
        Stat Experience = Target.Stats[(int) Stat.StatEnum.Experience];
        Experience.SetFlat(Experience.Value + amount);
    }
    public override void Remove (Controller Target){
        Stat Experience = Target.Stats[(int) Stat.StatEnum.Experience];
        Experience.SetFlat(Experience.Value - amount);
    }
}

public class LevelUpMultEffect : Effect {
    public gloat Amount = 1.5f;
    public LevelUpMultEffect (float amount){
            Amount = amount;
    }
    public override void Apply (Controller Target) {
        Stat Experience = Target.Stats[(int) Stat.StatEnum.Experience];
        Experience.SetFlat(Experience.Value() * Amount);
    }
    public override void Remove (Controller Target) {
        Stat Experience = Target.Stats[(int) Stat.StatEnum.Experience];
        Experience.SetFlat(Experience.Value() / Amount);
    }
}
