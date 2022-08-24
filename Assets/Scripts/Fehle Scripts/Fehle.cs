using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Fehle : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int salary;
    public int damage;
    public bool isWorking;
    public bool isMoving = false;
    public Vector3 _destinationPos;
    [SerializeField]Vector3 startPos;
    Quaternion startRot;
    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    public Fehle(int salary, bool canWork)
    {
        this.salary = salary;
        this.isWorking = canWork;
    }

    private void Update()
    {
        if (!isMoving) return;
        var distance = Vector3.Distance(_destinationPos, transform.position);
        if (distance <= 1) // already reached the destination
        {
            isMoving = false;
            return;
        }
        //var direction = (_destination.position - transform.position).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }

    public void StartGoingToDestination(Vector3 destinationPos)
    {
        transform.LookAt(destinationPos);
        _destinationPos = destinationPos;
        isMoving = true;
    }

    IEnumerator StartDamaginStone(Stone stone)
    {
        isWorking = true;
        while (isWorking)
        {
            yield return new WaitForSeconds(2);
            DamageStone(stone);
        }
        Debug.Log("Work finished for fehle: " + transform.name);
        StartGoingToDestination(startPos);
    }

    void DamageStone(Stone stone)
    {
        isWorking = stone.BeDamagedBy(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stone"))
        {
            var stone = other.GetComponent<Stone>();
            StartCoroutine(StartDamaginStone(stone));
        }
    }
}
