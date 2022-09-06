using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    #region Singleton
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public IEnumerator SpawnFromPool(string tag, Tower startPos, Quaternion rotation, Transform destination, float delay)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} does not exist");
            yield break;// null;
        }
        
        yield return new WaitForSeconds(delay);
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        var radiusOfPersonSpawn = StableDatas.radiusOfPersonSpawn;
        objectToSpawn.transform.position = startPos.transform.position + new Vector3(
            Random.Range(-radiusOfPersonSpawn , radiusOfPersonSpawn),
            0,
            Random.Range(-radiusOfPersonSpawn , radiusOfPersonSpawn)
            );
        objectToSpawn.transform.rotation = rotation;
        poolDictionary[tag].Enqueue(objectToSpawn);
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObject != null)
        {
            var speed = StableDatas.personSpeedLevels[startPos.level];
            pooledObject.OnObjectSpawn(speed, destination, startPos, startPos.colorTeam);
        }
        //return objectToSpawn;
    }
}
