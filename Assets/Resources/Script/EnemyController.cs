using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {

    public GameObject target = null;
    public int threshold = 1;
    public float HealthOverride = 1;
    protected Stat Attack = new Stat(Stat.StatEnum.Attack, 10, 1, 0);
    private Rigidbody rb;
    private float DefaultDrag;

    new void Awake() {
        base.Awake();
        Speed.SetBase(15);
        StatResourceLookup[Stat.StatEnum.Health].SetValues(HealthOverride, 1, 0, 0, HealthOverride, HealthOverride);
        StatResourceLookup[Stat.StatEnum.Health].Set(HealthOverride);
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        DefaultDrag = rb.drag;
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (IsDead) return;
        if (target != null) {
            rb.drag = DefaultDrag; 
            Vector3 dirToPlayer = target.transform.position - transform.position;
            float distToPlayer = dirToPlayer.magnitude;
            if (distToPlayer > threshold) {
                Vector3.Normalize(dirToPlayer);
                rb.AddForce(dirToPlayer * Speed.Value() * Time.deltaTime);
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
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Controller>().Damage(Attack.Value());
        }
    }

    void OnCollisionStay(Collision collision) {
        if (IsDead) return;
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Controller>().Damage(Attack.Value());
        }
    }
}
