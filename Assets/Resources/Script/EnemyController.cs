using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {

    public GameObject target = null;
    public int threshold = 5;
    protected Stat Attack = new Stat(Stat.StatEnum.Attack, 10, 1, 0);
    private Rigidbody rb;

    new void Awake() {
        base.Awake();
        Speed.SetBase(5);
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (IsDead) return;
        if (target != null) {
            Vector3 dirToPlayer = target.transform.position - transform.position;
            float distToPlayer = dirToPlayer.magnitude;
            if (distToPlayer > threshold) {
                Vector3.Normalize(dirToPlayer);
                rb.AddForce(dirToPlayer * Speed.Value() * Time.deltaTime);
            }
        } else {
            target = GameObject.FindWithTag("Player");
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
