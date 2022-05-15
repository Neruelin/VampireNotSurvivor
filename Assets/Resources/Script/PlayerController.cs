using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    public float invincibilityFramesTimer;
    public bool isInvincible = false;
    public GameObject ProjectilePrefab;
    private GameObject PlayerModel;
    private Rigidbody rb;
    private float DefaultDrag;
    private bool InControl = true;
    public float HealthOverride = 100;
    private Vector3 Direction = Vector3.right;
    private SprayAttack SAtk;

    new void Awake() {
        base.Awake();
        Stat AttackDelay = Stats[(int) Stat.StatEnum.AttackDelay];
        AttackDelay.SetBase(0.2f);

        Stat Speed = Stats[(int) Stat.StatEnum.Speed];
        Speed.SetBase(5);

        StatResource Health = Resources[(int) StatResource.ResourceEnum.Health];
        Health.SetValues(HealthOverride, 1, 0, 0, HealthOverride);
        Health.Set(HealthOverride);
        
        PlayerModel = transform.Find("tinker_original").gameObject;
        
        SAtk = gameObject.AddComponent<SprayAttack>();
        SAtk.Setup(gameObject, ProjectilePrefab);

        AddEffect(SpeedEffectFactory.MakeEffect(true, 20));
        AddEffect(PenetrationEffectFactory.MakeEffect(true, 20));
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        DefaultDrag = rb.drag;
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
        if (IsDead) return;
        Stat Speed = Stats[(int) Stat.StatEnum.Speed];
        Vector3 direction = new Vector3(0, 0, 0);
        rb.drag = DefaultDrag; 

        if (InControl) {
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
            if (Input.GetKey(KeyCode.W)) direction += Vector3.up;
            if (Input.GetKey(KeyCode.S)) direction += Vector3.down;
        }

        if (direction == Vector3.zero) {
            rb.drag = DefaultDrag * 25;
            // direction = Vector3.right;
            // PlayerModel.transform.eulerAngles = new Vector3(0, 0, ProjectileController.GetAngleFromVectorFloat(direction));
        } else {
            Direction = direction.normalized;
            rb.velocity = Direction * Speed.Value();
        }
        float DesiredAngle = ProjectileController.GetAngleFromVectorFloat(Direction);
        float CurrentAngle = PlayerModel.transform.eulerAngles.z;
        float AngleCorrection = DesiredAngle - CurrentAngle;
        if (AngleCorrection > 180) AngleCorrection -= 360;
        if (AngleCorrection < -180) AngleCorrection += 360;

        Vector3 DirectionCorrection = new Vector3(0, 0, AngleCorrection);
        
        PlayerModel.transform.eulerAngles += DirectionCorrection * Time.deltaTime * 10;

        // if (SAtk.Ready) SAtk.Fire(PlayerModel.transform.right);
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

    public override void Damage (GameObject Attacker, float amount) {
        if (IsDead) return;
        if (!isInvincible) {
            isInvincible = true;
            base.Damage(Attacker, amount);
            StartCoroutine(RemoveInvincibility());   
        }
    }

    public IEnumerator RemoveInvincibility() {
        yield return new WaitForSeconds(invincibilityFramesTimer);
        isInvincible = false;
    }

    public override void OnKill(GameObject target)
    {
        base.OnKill(target);
        AddEffect(AttackEffectFactory.MakeEffect(true, 10));
        Debug.Log("Attack: " + Stats[(int) Stat.StatEnum.Attack].Value());
    }

    protected override void HandleDeath() {
        Debug.Log("Player is Dead");
        InControl = false;
        IsDead = true;
        PlayerModel.transform.eulerAngles = new Vector3(0, 90, 0);
    }
}
