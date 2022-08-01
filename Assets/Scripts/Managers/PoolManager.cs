using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDic;

    private void Awake()
    {
        if(Instance != null) 
        {
            Debug.Log($"More than one instance for {this}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        poolDic = new Dictionary<string, Queue<GameObject>>();
    }

    public GameObject ReleaseFromThePool(GameObject obj, Vector3 spawnPosition)
    {
        if(poolDic.TryGetValue(obj.name, out Queue<GameObject> objList)) 
        {
            if(objList.Count > 0)
            {
                GameObject spawnedObj = objList.Dequeue();
                spawnedObj.transform.position = spawnPosition;
                spawnedObj.SetActive(true);
                return spawnedObj;
            }
            else
            {
                return CreateGameobject(obj, spawnPosition);
            }
        } 
        else
        {
            return CreateGameobject(obj, spawnPosition);
        } 
    }

    public void ReturnToPool(GameObject obj)
    {
        if(poolDic.TryGetValue(obj.name, out Queue<GameObject> objList))
        {
            objList.Enqueue(obj);
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            newQueue.Enqueue(obj);
            poolDic.Add(obj.name, newQueue);
        }

        obj.SetActive(false);
    }

    private GameObject CreateGameobject(GameObject obj, Vector3 position) 
    {
        GameObject newGameobject = Instantiate(obj);
        newGameobject.name = obj.name;
        newGameobject.transform.position = position;
        return newGameobject;
    }
}
