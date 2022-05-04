using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Stats
    protected Stat Speed = new Stat("Speed", 1, 1, 0);
    protected StatResource Health = new StatResource("Health", 1, 1, 0, 0, 100, 100, 0, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetHealthValue() {
        return Health.Value();
    }

    public void Damage(float amount) {
        if (Health.Remove(amount) <= 0) {
            HandleDeath();
        }

    }

    protected void HandleDeath() {
        Destroy(gameObject);
    }
}
