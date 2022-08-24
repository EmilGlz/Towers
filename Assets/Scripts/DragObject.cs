using System.Collections.Generic;
using UnityEngine;
public class DragObject : MonoBehaviour
{
    [SerializeField] Transform greenSign;
    Plane plane = new Plane(Vector3.up, Vector3.up * 1); // ground plane
    Tower myTower;
    GameController gameController;
    LineRenderer line;
    private void Start()
    {
        line = CommonObjects.Instance.line;
        gameController = GameController.Instance;
        myTower = GetComponent<Tower>();
        greenSign = CommonObjects.Instance.touchSign.transform;
        greenSign.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (myTower.towerState != TowerState.myTower) return;
        gameController.startTower = myTower;
        line.SetWidth(3f,3f);
        line.gameObject.SetActive(true);
    }

    void OnMouseDrag()
    {
        if (myTower.towerState != TowerState.myTower) return;
        greenSign.gameObject.SetActive(true);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            greenSign.position = ray.GetPoint(distance); // distance along the ray
            line.SetPosition(0, transform.position);
            line.SetPosition(1, greenSign.position);
        }
        var selectedTowerIndex = FindClosestTowerWithMaxDistance(15f);
        gameController.SetTowersSigns(selectedTowerIndex);
        if (selectedTowerIndex != -1)
        {
            var currentEndPos = CommonObjects.Instance.towers[selectedTowerIndex];
            greenSign.position = currentEndPos.transform.position;
            line.SetPosition(1, greenSign.position);
        }
        else
        {
            gameController.endTower = null;
        }
        if (Physics.Raycast(transform.position, (greenSign.position - transform.position).normalized, out RaycastHit hit, Vector3.Distance(greenSign.position, transform.position) - 10))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                line.material = CommonObjects.Instance.blueMat;
            }
        }
        else
        {
            line.material = CommonObjects.Instance.greenMat;
            if (selectedTowerIndex != -1)
            {
                gameController.endTower = CommonObjects.Instance.towers[selectedTowerIndex];
            }
        }
    }

    private void OnMouseUp()
    {
        //Debug.Log("OnMouseUp: " + this.name);
        greenSign.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
        gameController.StartMovingAsync();
    }

    int FindClosestTower()
    {
        List<Tower> towers = CommonObjects.Instance.towers;
        int resIndex = 0;
        float minDifference = Vector3.Distance(greenSign.position, towers[0].transform.position);
        for (int i = 0; i < towers.Count; i++)
        {
            float currentDistance = Vector3.Distance(greenSign.position, towers[i].transform.position);
            if (currentDistance < minDifference)
            {
                resIndex = i;
                minDifference = currentDistance;
            }
        }
        return resIndex;
    }

    int FindClosestTowerWithMaxDistance(float distance)
    {
        List<Tower> towers = CommonObjects.Instance.towers;
        int resIndex = 0;
        float minDifference = Vector3.Distance(greenSign.position, towers[0].transform.position);
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == myTower)
            {
                continue;
            }
            float currentDistance = Vector3.Distance(greenSign.position, towers[i].transform.position);
            if (currentDistance < minDifference)
            {
                resIndex = i;
                minDifference = currentDistance;
            }
        }
        if (minDifference <= distance)
        {
            return resIndex;
        }
        return -1;
    }
}