using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public GameObject[] objectsToPool;
    public int[] amountToPool;
    Dictionary<PlatformType, int> startingIndices;

    void Awake()
    {
        startingIndices = new Dictionary<PlatformType, int>();
    }

    void Start()
    {
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        var startingIdx = 0;
        for (var i = 0; i < amountToPool.Length; i++) {
            startingIndices.Add(objectsToPool[i].gameObject.GetComponent<Platform>().platformType, startingIdx);
            for (var j = 0; j < amountToPool[i]; j++) {
                var obj = Instantiate(objectsToPool[i], transform, true);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }

            startingIdx += amountToPool[i];
        }
    }

    public GameObject GetPooledObject(PlatformType type)
    {
        var startingIndex = startingIndices[type];
        // For as many objects as are in the pooledObjects list
        for (var i = startingIndex; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].gameObject.CompareTag(type.ToString())) {
                return null;
            }

            // if the pooled objects is NOT active, return that object
            if (!pooledObjects[i].activeInHierarchy) {
                return pooledObjects[i];
            }
        }

        // otherwise, return null   
        return null;
    }
}