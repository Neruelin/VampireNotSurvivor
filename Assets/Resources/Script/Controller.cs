using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Speed
    private Stat Speed = new Stat("Speed", 1, 1, 0);
   
    //Health
    private StatResource Health = new StatResource("Health", 1, 1, 0, 0, 100, 100, 0, 1, 0);
    public float health = 100;

    public int speed;
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityFramesTimer;

    private System.Random rnd = new System.Random();
    private bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        Vector3 direction = new Vector3(0,0,0);
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

    public IEnumerator damagePlayer(int damage) {
        if (!isInvincible) {
            currentHealth -= damage;
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityFramesTimer);
            isInvincible = false;
        }

    }
}
