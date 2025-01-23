using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    #region Variables

    public static ObjectPooling Instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public Transform objectPoolParent;
    public int amountToPool;

    #endregion
    
    #region Unity Methods

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool,objectPoolParent);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    #endregion

    #region ObjectPooling Methods

    /// <summary>
    /// get a object out of the pool if one is in it
    /// </summary>
    /// <returns>a object in the pool or null</returns>
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    #endregion
    
}
