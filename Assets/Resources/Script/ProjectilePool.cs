using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolSingleton
{
    public static ProjectilePoolSingleton SharedInstance;
    public List<GameObject> pooledProjectiles;
    public GameObject projectileToPool;
    public int amountToPool;

    private ProjectilePoolSingleton(GameObject pooled, int size){
        amountToPool = size;
        projectileToPool = pooled;
        pooledProjectiles = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++){
            tmp = GameObject.Instantiate(projectileToPool);
            tmp.SetActive(false);
            pooledProjectiles.Add(tmp);
        }
    }

    public static ProjectilePoolSingleton GetInstance(GameObject pooled, int size){
        if (SharedInstance == null) {
            SharedInstance = new ProjectilePoolSingleton(pooled, size);
        }
        return SharedInstance; 
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
