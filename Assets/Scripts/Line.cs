using System;
using UnityEngine;
[System.Serializable]
public class Line : MonoBehaviour
{
    public Tower _startTower;
    public Tower _destination;
    LineRenderer lr;
    [SerializeField] BoxCollider childCollider;
    
    public void GenerateBoxCollider()
    {
        lr = GetComponent<LineRenderer>();
        childCollider.transform.position = (lr.GetPosition(0) + lr.GetPosition(1)) / 2f;
        childCollider.transform.LookAt(_destination.transform);
        var distance = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(1));
        childCollider.size = new Vector3(childCollider.size.x, childCollider.size.y, distance - _startTower.transform.localScale.magnitude);
    }
}
