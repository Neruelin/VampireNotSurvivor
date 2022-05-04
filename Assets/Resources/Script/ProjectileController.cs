using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Controller
{
    public float TimeToLive = 5;
    protected Stat Attack = new Stat("Attack", 50, 1, 0);
    private IEnumerator DespawnCoroutine;

    void Awake() {
        Speed.SetBase(5);
        DespawnCoroutine = Despawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Speed.Value() * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyController>().Damage(Attack.Value());
            HandleDeath();
        }
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(TimeToLive);
        HandleDeath();
    }
}
