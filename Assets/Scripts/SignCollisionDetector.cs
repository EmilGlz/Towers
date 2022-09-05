using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignCollisionDetector : MonoBehaviour
{
    public bool canStopMoves;
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
    }
}
