using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Stat[] Stats = new Stat[Enum.GetNames(typeof(Stat.StatEnum)).Length];
    public StatResource[] Resources = new StatResource[Enum.GetNames(typeof(StatResource.ResourceEnum)).Length];
    public List<Effect> ActiveEffects = new List<Effect>();
    public int Kills = 0;
    public bool IsDead = false;

    protected void Awake() {
        foreach (Stat.StatEnum type in Enum.GetValues(typeof(Stat.StatEnum))) {
            Stats[(int) type] = Stat.Default(type);
        }
        foreach (StatResource.ResourceEnum type in Enum.GetValues(typeof(StatResource.ResourceEnum))) {
            Resources[(int) type] = StatResource.Default(type);
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateStatResources();
    }

    protected void Move() {

    }

    void UpdateStatResources () {
        if (IsDead) return;
        foreach (StatResource item in Resources) {
            item.ApplyDelta(Time.deltaTime);
        }
    }

    public void AddEffect(Effect eff) {
        ActiveEffects.Add(eff);
        eff.Apply(this);
    }

    public void RemoveEffect(int position) {
        ActiveEffects[position].Remove(this);
        ActiveEffects.RemoveAt(position);
    }

    public void ResetEffects() {
        ActiveEffects = new List<Effect>();
        ResetStats();
        ResetResources();
    }

    void ResetStats() {
        foreach (Stat item in Stats) { item.Reset(); }
    }

    void ResetResources() {
        foreach (StatResource item in Resources) { item.Reset(); }
    }

    public float[] GetHealthInfo() {
        StatResource Health = Resources[(int) StatResource.ResourceEnum.Health];
        return new float[] {Health.Value(), Health.Min, Health.Cap};
    }

    public virtual void Damage(GameObject Attacker, float amount) {
        StatResource Health = Resources[(int) StatResource.ResourceEnum.Health];
        if (IsDead) return;
        if (Health.Remove(amount) <= 0) {
            Attacker.GetComponent<Controller>().OnKill(gameObject);
            HandleDeath();
        }
    }

    public virtual void OnKill(GameObject target) {
        Debug.Log(gameObject.name + " killed " + target.name);
        Kills++;
    }

    protected virtual void HandleDeath() {
        IsDead = true;
        Destroy(gameObject);
    }
}
