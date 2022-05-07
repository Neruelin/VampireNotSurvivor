using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Stats
    protected Dictionary<Stat.StatEnum, Stat> StatLookup;
    protected Dictionary<Stat.StatEnum, StatResource> StatResourceLookup;
    protected Stat Speed = Stat.Default(Stat.StatEnum.Speed);
    protected StatResource Health = StatResource.Default(Stat.StatEnum.Health);
    public bool IsDead = false;

    protected void Awake() {
        StatLookup = new Dictionary<Stat.StatEnum, Stat>();
        StatResourceLookup = new Dictionary<Stat.StatEnum, StatResource>();
        StatLookup.Add(Speed.Tag, Speed);
        StatResourceLookup.Add(Health.Tag, Health);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatResources();
    }

    void UpdateStatResources () {
        if (IsDead) return;
        foreach (var item in StatResourceLookup) {
            item.Value.ApplyDelta(Time.deltaTime);
        }
    }

    public float[] GetHealthInfo() {
        return new float[] {Health.Value(), Health.Min, Health.Cap};
    }

    public virtual void Damage(float amount) {
        if (IsDead) return;
        if (Health.Remove(amount) <= 0) {
            HandleDeath();
        }
    }

    protected virtual void HandleDeath() {
        IsDead = true;
        Destroy(gameObject);
    }
}
