using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour, IPooledObject
{
    public Transform _startTower;
    public Transform _destination;
    public float speed;
    public float distanceToStop = 1f;
    bool isMoving;
    public void OnObjectSpawn(Transform destination, Transform startPos)
    {
        _destination = destination;
        _startTower = startPos;
        transform.LookAt(_destination);
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;
        if (_destination == null) return;
        if ((transform.position - _destination.position).magnitude <= distanceToStop)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * speed);
    }
}
