using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {

    public GameObject target = null;
    public int threshold = 5;
    protected Stat Attack = new Stat("Attack", 10, 1, 0);
    private Rigidbody rb;

    void Awake() {
        Speed.SetBase(5);
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (target != null) {
            Vector3 dirToPlayer = target.transform.position - transform.position;
            float distToPlayer = dirToPlayer.magnitude;
            if (distToPlayer > threshold) {
                Vector3.Normalize(dirToPlayer);
                rb.AddForce(dirToPlayer * Speed.Value() * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().Damage(Attack.Value());
        }
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().Damage(Attack.Value());
        }
    }
}
