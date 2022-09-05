using System.Collections;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{
    public bool canSpawn = false;

    public void StartSpawning(Tower destination, Tower startPosTower)
    {
        startPosTower.destinations.Add(destination);
        startPosTower.startMoving = true;
    }
}
