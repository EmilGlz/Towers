using UnityEngine;

public interface IPooledObject
{
    void OnObjectSpawn(float speed, Transform destination, Tower startPos);
}
