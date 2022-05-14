using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {

    public GameObject target = null;
    public int threshold = 1;
    public float HealthOverride = 1;
    private Rigidbody rb;
    private float DefaultDrag;

    new void Awake() {
        base.Awake();
        Stat Speed = Stats[(int) Stat.StatEnum.Speed];
        Speed.SetBase(10);

        Stat Attack = Stats[(int) Stat.StatEnum.Attack];
        Attack.SetValues(10, 1, 0);

        StatResource Health = Resources[(int) StatResource.ResourceEnum.Health];
        Health.SetValues(HealthOverride, 1, 0, 0, HealthOverride);
        Health.Set(HealthOverride);
    }

    // Start is called before the first frame update
    new void Start() {
        base.Start();
        rb = GetComponent<Rigidbody>();
        DefaultDrag = rb.drag;
        target = GameObject.FindWithTag("Player");
    }

    new void Update() {
        base.Update();
        if (IsDead) return;
        Stat Speed = Stats[(int) Stat.StatEnum.Speed];
        if (target != null) {
            rb.drag = DefaultDrag; 
            Vector3 dirToPlayer = target.transform.position - transform.position;
            float distToPlayer = dirToPlayer.magnitude;
            if (distToPlayer > threshold) {
                Vector3.Normalize(dirToPlayer);
                rb.velocity = dirToPlayer * Speed.Value() * Time.deltaTime;
            } else {
                rb.drag = DefaultDrag * 5;
            }
        } else {
            target = GameObject.FindWithTag("Player");
            rb.drag = DefaultDrag * 5;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (IsDead) return;
        Stat Attack = Stats[(int) Stat.StatEnum.Attack];

        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Controller>().Damage(gameObject, Attack.Value());
        }
    }

    void OnCollisionStay(Collision collision) {
        if (IsDead) return;
        Stat Attack = Stats[(int) Stat.StatEnum.Attack];

        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Controller>().Damage(gameObject, Attack.Value());
        }
    }
}
