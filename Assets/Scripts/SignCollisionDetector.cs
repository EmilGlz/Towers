using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignCollisionDetector : MonoBehaviour
{
    public bool canStopMoves;
    public bool insideOfTower;
    [SerializeField] LineRenderer line;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Line"))
        {
            // destroy the line
            if (!canStopMoves) return;
            Line line = other.GetComponentInParent<Line>();
            var startTower = line._startTower;
            var destination = line._destination;
            if (startTower == null || destination == null) return;
            GameController.Instance.DeleteMove(startTower, destination);
            line._startTower.StopSendingPeople(line._destination);
        }
        else if (other.CompareTag("Tower"))
        {
            Tower t = other.GetComponent<Tower>();
            if (GameController.Instance.startTower != t)
            {
                insideOfTower = true;
                GameController.Instance.endTower = t;
                line.SetPosition(1, t.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            insideOfTower = false;
            GameController.Instance.endTower = null;
        }
    }
}
