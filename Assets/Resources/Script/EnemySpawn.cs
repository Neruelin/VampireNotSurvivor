using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public GameObject enemyPrefab;
    public float spawnTime = 0f;
    public int maxEnemies = 10;

    private List<GameObject> enemies = null;
    private IEnumerator spawnCoroutine;
    private GameObject enemyParent;

    void Awake() {
        enemies = new List<GameObject>();
        enemyParent = new GameObject("Enemies");
        spawnCoroutine = Spawn();
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(spawnCoroutine);
    }

    private IEnumerator Spawn() {
        while (true) {
            if (enemies.Count < maxEnemies) {
                GameObject go = (GameObject)Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                go.SetActive(true);
                go.transform.SetParent(enemyParent.transform);
                enemies.Add(go);
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }
}