using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public GameObject enemyPrefab;
    public float spawnTime = 0f;
    public int maxEnemies = 10;
    private int spawned = 0;

    private List<GameObject> enemies = null;
    private GameObject enemyParent;

    void Awake() {
        enemies = new List<GameObject>();
        enemyParent = new GameObject("Enemies");
    }

    // Start is called before the first frame update
    public void Start() {
        // StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        while (true) {
            yield return new WaitForSeconds(spawnTime);
            if (maxEnemies == -1 || spawned < maxEnemies) {
                Vector3 position = new Vector3();
                position += transform.position;
                position.z = -1;
                GameObject go = (GameObject)Instantiate(enemyPrefab, position, Quaternion.identity);
                go.SetActive(true);
                go.transform.SetParent(enemyParent.transform);
                spawned++;
            }
        }
    }

    void OnEnable() {
        StartCoroutine(Spawn());
    }
}
