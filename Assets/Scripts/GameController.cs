using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    [SerializeField] PersonSpawner personSpawner;
    public Tower startTower;
    public Tower endTower;
    public List<Vector2> moves;
    public List<Line> moveLines;

    public void StartMovingAsync()
    {
        //Debug.Log("StartMovingAsync");
        if (startTower == null) return;
        if (endTower == null) return;
        if (endTower == startTower) return;
        int startIndex = CommonObjects.Instance.towers.IndexOf(startTower);
        int endIndex = CommonObjects.Instance.towers.IndexOf(endTower);
        AddMove(startIndex, endIndex);
        endTower.towersSendingPeopleToMe.Add(startTower);
        var newLine = Instantiate(CommonObjects.Instance.linePrefab);
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startTower.transform.position);
        lineRenderer.SetPosition(1, endTower.transform.position);
        Line line = newLine.GetComponent<Line>();
        line._startTower = startTower.transform;
        line._destination = endTower.transform;
        moveLines.Add(line);
        personSpawner.StartSpawning(endTower.transform, startTower);
        endTower = null;
        startTower = null;
    }

    public void SetTowersSigns(int selectedIndex)
    {
        for (int i = 0; i < CommonObjects.Instance.towers.Count; i++)
        {
            if (i == selectedIndex)
            {
                CommonObjects.Instance.towers[i].SetSelectedSign(true);
            }
            else
            {
                CommonObjects.Instance.towers[i].SetSelectedSign(false);
            }
        }
    }

    void AddMove(int startIndex, int endIndex)
    {
        var moveIndex = moves.FindIndex(m => m.x == startIndex);
        if (moveIndex != -1)
        {
            moves[moveIndex] = new Vector2(startIndex, endIndex);
        }
        else
        {
            moves.Add(new Vector2( startIndex, endIndex ));
        }
    }

    public void DeleteLine(Transform startTower, Transform endTower)
    {
        var res = moveLines.FirstOrDefault(l => l._startTower == startTower && l._destination == endTower);
        if (res != null)
        {
            moveLines.Remove(res);
            Destroy(res.gameObject);
        }
    }
}
