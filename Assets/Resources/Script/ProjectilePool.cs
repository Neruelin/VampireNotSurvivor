using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool SharedInstance;
    public List<GameObject> pooledProjectiles;
    public GameObject projectileToPool;
    public int amountToPool;

    void Awake(){
        SharedInstance = this;
    }

    void Start(){
        pooledProjectiles = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++){
            tmp = Instantiate(projectileToPool);
            tmp.SetActive(false);
            pooledProjectiles.Add(tmp);
        }
    }

    public GameObject GetPooledProjectile(){
        for(int i = 0; i < amountToPool; i++){
            if(!pooledProjectiles[i].activeInHierarchy){
                return pooledProjectiles[i];
            }
        }
        return null;
    }
}
