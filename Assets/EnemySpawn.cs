using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public int maxEnemies = 10;
    private List<GameObject> enemies = null;
    public GameObject enemyTarget = null;
    public GameObject enemyPrefab = null;
    public Material mat = null;
    public int interval = 10;
    public float targetTime = 0;

    void Awake() {
        enemies = new List<GameObject>();

        enemyPrefab = new GameObject("Enemy");
        enemyPrefab.transform.position = new Vector3(0,0,0);
        enemyPrefab.AddComponent<EnemyController>();
        Rigidbody rb = enemyPrefab.AddComponent<Rigidbody>();    
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.drag = 5;
        rb.useGravity = false;

        GameObject model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        model.transform.position = new Vector3(0,0,0);
        model.GetComponent<Renderer>().material = mat;
        model.transform.SetParent(enemyPrefab.transform);

        enemyPrefab.transform.position += new Vector3(0,0,-1);
        enemyPrefab.SetActive(false);
    }

    void Spawn() {
        GameObject go = (GameObject) Instantiate(enemyPrefab, new Vector3(0,0,0), Quaternion.identity);
        go.SetActive(true);
        go.GetComponent<EnemyController>().target = enemyTarget;
        enemies.Add(go);
    }

    // Start is called before the first frame update
    void Start() {
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count < maxEnemies) {
            Debug.Log((targetTime - Time.time));
            if ((targetTime - Time.time) < 0) {
                targetTime = Time.time + interval;
                Spawn();
            }
        }
    }
}
