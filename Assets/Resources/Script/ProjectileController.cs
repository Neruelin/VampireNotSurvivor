using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Controller
{
    public float TimeToLive = 5;
    protected Stat Attack = new Stat("Attack", 50, 1, 0);
    private IEnumerator DespawnCoroutine;

    Collider playerBody;

    void Awake() {
        Speed.SetBase(15);
        DespawnCoroutine = Despawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnCoroutine);
        SetBulletDirection();
    }

    
    void SetBulletDirection()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A))
            direction += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            direction += Vector3.right;
        if (Input.GetKey(KeyCode.W))
            direction += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            direction += Vector3.down;
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
            direction += Vector3.up;

        Vector3.Normalize(direction);
        Setup(direction);
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = Vector3.Normalize(dir);
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    private Vector3 shootDirection;
    // Start is called before the first frame update
    public void Setup(Vector3 shootDir)
    {
       this.shootDirection = shootDir;
       transform.eulerAngles = new Vector3(0, 0, ProjectileController.GetAngleFromVectorFloat(shootDir));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += shootDirection * Speed.Value() * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyController>().Damage(Attack.Value());
            HandleDeath();
        }
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(TimeToLive);
        HandleDeath();
    }
}
