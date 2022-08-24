using System.Collections;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{
    readonly float delay = 0.5f;
    //ObjectPooler objectPooler;
    public bool canSpawn = false;
    private void Start()
    {
        //objectPooler = ObjectPooler.Instance;
    }

    public void StartSpawning(Transform destination, Tower startPosTower)
    {
        startPosTower.StartMovingFromMe(destination);
    }
}
