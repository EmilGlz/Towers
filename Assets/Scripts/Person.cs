using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour, IPooledObject
{
    public Tower _startTower;
    public Transform _destination;
    public float _speed;
    public float distanceToStop = 1f;
    bool isMoving;
    public void OnObjectSpawn(float speed,Transform destination, Tower startPos)
    {
        _speed = speed;
        _destination = destination;
        _startTower = startPos;
        transform.LookAt(_destination);
        isMoving = true;
        startPos.people.Add(this);
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
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);
    }
}
