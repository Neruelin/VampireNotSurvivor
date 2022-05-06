using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    public float invincibilityFramesTimer;
    public bool isInvincible = false;
    public GameObject ProjectilePrefab;
    protected Stat AttackDelay = Stat.Default(Stat.StatEnum.AttackDelay);
    private IEnumerator SpawnProjectileCoroutine;
    private Rigidbody rb;
    private float DefaultDrag;
    private bool InControl = true;

    new void Awake() {
        base.Awake();
        StatLookup.Add(AttackDelay.Tag, AttackDelay);
        StatResourceLookup[Stat.StatEnum.Health].SetValues(100, 1, 0, 0, 100, 100);
        StatResourceLookup[Stat.StatEnum.Health].Set(100);
        SpawnProjectileCoroutine = SpawnProjectile();
        Speed.SetBase(5);
        // isInvincible = true;
        // StartCoroutine(RemoveInvincibility());
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DefaultDrag = rb.drag;
        StartCoroutine(SpawnProjectileCoroutine);
    }

    // Update is called once per frame
    void Update() {
        if (IsDead) return;

        Vector3 direction = new Vector3(0, 0, 0);
        rb.drag = DefaultDrag; 

        if (InControl) {
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
            if (Input.GetKey(KeyCode.W)) direction += Vector3.up;
            if (Input.GetKey(KeyCode.S)) direction += Vector3.down;
        }

        if (direction == Vector3.zero) {
            rb.drag = DefaultDrag * 5;
        } else {
            Vector3.Normalize(direction);
            rb.AddForce(direction * Speed.Value());
        }
    }

    private IEnumerator SpawnProjectile() {
        while (true) {
            if (IsDead) yield break;
            Instantiate(ProjectilePrefab, gameObject.transform.position + Vector3.up, Quaternion.identity);
            yield return new WaitForSeconds(AttackDelay.Value());
        }
    }
  
    public override void Damage (float amount) {
        if (IsDead) return;
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

    protected override void HandleDeath() {
        Debug.Log("Player is Dead");
        InControl = false;
        IsDead = true;
    }
}
