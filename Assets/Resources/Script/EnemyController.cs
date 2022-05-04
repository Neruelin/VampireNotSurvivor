using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller
{

    public GameObject target = null;
    public int threshold = 5;
    public float damage;
    private Rigidbody rb;

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
    
    public void Damage(float amount) {
        DestroyImmediate(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Controller>().Damage(damage);
        }
    }
}

