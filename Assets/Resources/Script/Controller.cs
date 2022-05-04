using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public int speed;
    public int maxHealth = 100;
    public int currentHealth;
    private System.Random rnd = new System.Random();

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
        gameObject.GetComponent<Rigidbody>().AddForce(direction * speed);
    }

    public void damagePlayer(int damage)
    {
        currentHealth -= damage;
        // Set character to inactive if less than 0 health
        if(currentHealth == 0) {
            this.gameObject.SetActive(false);
        }
    }
}
