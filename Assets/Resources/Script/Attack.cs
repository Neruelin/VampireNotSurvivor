using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Attack {
    public void Fire(Vector3 direction);
}

public class SprayAttack : MonoBehaviour, Attack {
    public GameObject ProjectilePrefab;
    public GameObject User;
    public Stat[] UserStats;
    public Stat UserAttackDelayStat;
    public Stat UserAttackStat;
    public float AttackDelayMult = 0.3f;
    public float AttackAngle = 15;
    public int ProjectileCount = 2;
    public bool Ready = true;
    private System.Random rnd = new System.Random();

    public void Setup(GameObject User, GameObject ProjectilePrefab) {
        this.User = User;
        this.ProjectilePrefab = ProjectilePrefab;
        UserStats = User.GetComponent<Controller>().Stats;
        UserAttackDelayStat = UserStats[(int) Stat.StatEnum.AttackDelay];
        UserAttackStat = UserStats[(int) Stat.StatEnum.Attack];
    }

    public void Fire(Vector3 direction) {
        if (Ready) {
            for (int i = 0; i < ProjectileCount; i++) {
                float angle = (GetAngleFromVectorFloat(direction) + (rnd.Next() % AttackAngle) - (AttackAngle / 2) + 360) % 360;
                Quaternion QDirection = new Quaternion();
                QDirection.eulerAngles = new Vector3(0,0,angle);
                Vector3 ProjPosition = new Vector3();
                ProjPosition += User.transform.position + (direction * 0.1f);
                ProjPosition.z = -3;
                GameObject ProjObject = GameObject.Instantiate(ProjectilePrefab, ProjPosition, QDirection);
                ProjObject.GetComponent<ProjectileController>().Setup(User, 50, UserStats);
            }
            Ready = false;
            StartCoroutine(SetReady(UserAttackDelayStat.Value() * AttackDelayMult));
        }
    }

    public IEnumerator SetReady(float Delay) {
        yield return new WaitForSeconds(Delay);
        Ready = true;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = Vector3.Normalize(dir);
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}