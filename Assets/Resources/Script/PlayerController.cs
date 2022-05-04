using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    public int speed;
    public float invincibilityFramesTimer;
    private System.Random rnd = new System.Random();
    public bool isInvincible = false;

    public GameObject ProjectilePrefab;
    protected Stat AttackDelay = new Stat("AttackDelay", 1, 1, 0);
    private IEnumerator SpawnProjectileCoroutine;

    void Awake() {
        SpawnProjectileCoroutine = SpawnProjectile();
        Speed.SetBase(5);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnProjectileCoroutine);
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

    private IEnumerator SpawnProjectile() {
        while (true) {
            Instantiate(ProjectilePrefab, gameObject.transform.position + Vector3.up, Quaternion.identity);
            yield return new WaitForSeconds(AttackDelay.Value());
        }
    }
  
    public new void Damage (float amount) {
        if (!isInvincible) {
            isInvincible = true;
            base.Damage(amount);
            StartCoroutine(RemoveInvincibility());   
        }
    }

    public IEnumerator RemoveInvincibility() {
        yield return new WaitForSeconds(invincibilityFramesTimer);
        isInvincible = false;
    }
}
