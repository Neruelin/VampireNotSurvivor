using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Controller
{
    public float TimeToLive = 5;
    protected Stat Attack = new Stat(Stat.StatEnum.Attack, 50, 1, 0);
    private IEnumerator DespawnCoroutine;

    Collider playerBody;
    new void Awake() {
        base.Awake();
        Speed.SetBase(15);
        DespawnCoroutine = Despawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnCoroutine);
    }

    
    public void SetBulletDirection(Vector3 direction) 
    {
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
        }
        HandleDeath();
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(TimeToLive);
        HandleDeath();
    }
}
