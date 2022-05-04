using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    public int speed;
    public float invincibilityFramesTimer;

    private System.Random rnd = new System.Random();
    private bool isInvincible = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 direction = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A))
            direction += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            direction += Vector3.right;
        if (Input.GetKey(KeyCode.W))
            direction += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            direction += Vector3.down;
        Vector3.Normalize(direction);
        gameObject.GetComponent<Rigidbody>().AddForce(direction * Speed.Value());
    }

    public IEnumerator damagePlayer(float damage) {
        if (!isInvincible) {
            Damage(damage);
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityFramesTimer);
            isInvincible = false;
        }
    }
}
