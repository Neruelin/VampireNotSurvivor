using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    public float invincibilityFramesTimer;
    public bool isInvincible = false;
    public GameObject ProjectilePrefab;
    private GameObject PlayerModel;
    protected Stat AttackDelay = Stat.Default(Stat.StatEnum.AttackDelay);
    protected Stat Attack = Stat.Default(Stat.StatEnum.Attack);
    private IEnumerator SpawnProjectileCoroutine;
    private Rigidbody rb;
    private float DefaultDrag;
    private bool InControl = true;
    private SprayAttack SAtk;

    new void Awake() {
        base.Awake();
        StatLookup.Add(AttackDelay.Tag, AttackDelay);
        StatLookup.Add(Attack.Tag, Attack);
        StatResourceLookup[Stat.StatEnum.Health].SetValues(100, 1, 0, 0, 100, 100);
        StatResourceLookup[Stat.StatEnum.Health].Set(100);
        AttackDelay.SetBase(0.2f);
        SpawnProjectileCoroutine = SpawnProjectile();
        Speed.SetBase(5);
        PlayerModel = transform.Find("tinker_original").gameObject;
        SAtk = gameObject.AddComponent<SprayAttack>();
        SAtk.Setup(gameObject, ProjectilePrefab);
        // isInvincible = true;
        // StartCoroutine(RemoveInvincibility());
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DefaultDrag = rb.drag;
        // StartCoroutine(SpawnProjectileCoroutine);
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
            direction = Vector3.right;
            PlayerModel.transform.eulerAngles = new Vector3(0, 0, ProjectileController.GetAngleFromVectorFloat(direction));
        } else {
            Vector3.Normalize(direction);
            PlayerModel.transform.eulerAngles = new Vector3(0, 0, ProjectileController.GetAngleFromVectorFloat(direction));
            rb.AddForce(direction * Speed.Value());
        }

        if (SAtk.Ready) SAtk.Fire(direction);
    }

    private IEnumerator SpawnProjectile() {
        while (true) {
            if (IsDead) yield break;
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
            if (direction == Vector3.zero) {
                direction = Vector3.right;
            }
            Vector3 position = new Vector3();
            position += direction;
            position *= 0.1f;
            position += gameObject.transform.position;
            position.z = -3;

            GameObject projObject = Instantiate(ProjectilePrefab, position, DirToZQuat(direction));

            yield return new WaitForSeconds(AttackDelay.Value());
        }
    }

    public static Quaternion DirToZQuat (Vector3 direction) {
        float angle = GetAngleFromVectorFloat(direction);
        Quaternion QDirection = new Quaternion();
        QDirection.eulerAngles = new Vector3(0,0,angle);
        return QDirection;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = Vector3.Normalize(dir);
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
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
        PlayerModel.transform.eulerAngles = new Vector3(0, 90, 0);
    }
}
