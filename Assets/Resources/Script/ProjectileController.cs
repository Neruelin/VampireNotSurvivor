using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Controller
{
    public float TimeToLive = 5;
    public float BaseSpeed = 15;
    public float BaseAttack = 10;
    public int Penetrations = 0;
    public GameObject User;
    Collider playerBody;

    new void Awake() {
        base.Awake();
        Stat Speed = Stats[(int) Stat.StatEnum.Speed];
        Speed.SetBase(BaseSpeed);

        Stat Attack = Stats[(int) Stat.StatEnum.Attack];
        Attack.SetBase(BaseAttack);
    }

    // Update is called once per frame
    new void Update()
    {
        if (IsDead) return;
        Stat Speed = Stats[(int) Stat.StatEnum.Speed];
        transform.position += transform.right * Speed.Value() * Time.deltaTime;
    }

    public void Setup(GameObject User, float NewSpeed, Stat[] UserStats) {
        Stat Speed = Stats[(int) Stat.StatEnum.Speed];
        Speed.SetBase(NewSpeed);
        Stat Attack = Stats[(int) Stat.StatEnum.Attack];
        Attack.SetBase(UserStats[(int) Stat.StatEnum.Attack].Value());
        Stat Penetration = Stats[(int) Stat.StatEnum.Penetration];
        Penetration.SetBase((int) Math.Ceiling(UserStats[(int) Stat.StatEnum.Penetration].Value()));
        this.User = User;
        StartCoroutine(Despawn());
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = Vector3.Normalize(dir);
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    void OnTriggerEnter(Collider collision) {
        if (IsDead) return;
        Stat Attack = Stats[(int) Stat.StatEnum.Attack];
        Stat Penetration = Stats[(int) Stat.StatEnum.Penetration];
        
        if(collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyController>().Damage(User, Attack.Value());
            Penetrations++;
            if (Penetrations >= Penetration.Value()) {
                StartCoroutine(EmitAndDie());
            } else {
                gameObject.GetComponent<ParticleSystem>().Emit(25);
            }
        }
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(TimeToLive);
        HandleDeath();
    }

    IEnumerator EmitAndDie() {
        IsDead = true;
        gameObject.GetComponent<ParticleSystem>().Emit(25);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        HandleDeath();
    }

    protected new void HandleDeath() {
        gameObject.SetActive(false);
    }
}
