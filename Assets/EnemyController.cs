using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject target = null;
    public int speed = 1;
    public int threshold = 5;
    private Rigidbody rb;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null) {
            Vector3 dirToPlayer = target.transform.position - transform.position;
            float distToPlayer = dirToPlayer.magnitude;
            if (distToPlayer > threshold) {
                Vector3.Normalize(dirToPlayer);
                rb.AddForce(dirToPlayer * speed);
            }
        } else {
            target = GameObject.FindWithTag("Player");
        }
    }
}
