using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    private GameObject enemyPrefab = null;
    private List<GameObject> enemies = null;
    private float targetTime = 0;
    public int interval = 10;
    public int maxEnemies = 10;
    public float enemySpeed = 1;
    public float enemyDrag = 1;
    public Material enemyDefaultMaterial = null;
    public GameObject enemyModel = null;
    public GameObject enemyTarget = null;

    void Awake() {
        enemies = new List<GameObject>();
        
        enemyPrefab = new GameObject("Enemy");
        enemyPrefab.transform.position = new Vector3(0,0,0);
        EnemyController EC = enemyPrefab.AddComponent<EnemyController>();
        EC.speed = enemySpeed;
        Rigidbody rb = enemyPrefab.AddComponent<Rigidbody>();    
        
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.drag = enemyDrag;
        rb.useGravity = false;

        if (enemyDefaultMaterial == null) {
            enemyDefaultMaterial = new Material(Shader.Find("Transparent/Diffuse"));
            enemyDefaultMaterial.color = Color.red;
        }

        if (enemyModel == null) {
            enemyModel = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            enemyModel.GetComponent<Renderer>().material = enemyDefaultMaterial;
        } else {
            enemyModel = Instantiate(enemyModel, new Vector3(0,0,0), Quaternion.identity);
        }

        enemyModel.transform.position = new Vector3(0,0,-2);
        enemyModel.transform.SetParent(enemyPrefab.transform);

        // enemyPrefab.transform.position += new Vector3(0,0,0);
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
            if ((targetTime - Time.time) < 0) {
                targetTime = Time.time + interval;
                Spawn();
            }
        }
    }
}
