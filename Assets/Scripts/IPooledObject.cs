using UnityEngine;

public interface IPooledObject
{
    void OnObjectSpawn(Transform destination, Transform startPos);
}
