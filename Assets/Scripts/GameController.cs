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
        if (startTower.isEmpty) return;
        int startIndex = CommonObjects.Instance.towers.IndexOf(startTower);
        int endIndex = CommonObjects.Instance.towers.IndexOf(endTower);
        bool moveIsAllowed = MoveIsAllowed(startIndex, endIndex);
        if (!moveIsAllowed) return;
        AddMove(startIndex, endIndex);
        endTower.towersSendingPeopleToMe.Add(startTower);
        var newLine = Instantiate(CommonObjects.Instance.linePrefab);
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startTower.transform.position);
        lineRenderer.SetPosition(1, endTower.transform.position);
        Line line = newLine.GetComponent<Line>();
        line._startTower = startTower;
        line._destination = endTower;
        line.GenerateBoxCollider();
        moveLines.Add(line);
        personSpawner.StartSpawning(endTower, startTower);
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

    public void DeleteMove(Tower startTower, Tower endTower)
    {
        DeleteLine(startTower.transform, endTower.transform);
        int startIndex = CommonObjects.Instance.towers.IndexOf(startTower);
        int endIndex = CommonObjects.Instance.towers.IndexOf(endTower);
        var res = moves.FirstOrDefault(m=> m.x==startIndex && m.y==endIndex);
        moves.Remove(res);
    }

    void DeleteLine(Transform startTower, Transform endTower)
    {
        var res = moveLines.FirstOrDefault(l => l._startTower.transform == startTower && l._destination.transform == endTower);
        if (res != null)
        {
            moveLines.Remove(res);
            Destroy(res.gameObject);
        }
    }

    bool MoveIsAllowed(int startIndex, int finishIndex)
    {
        if (moves.Count == 0) return true;
        var res = moves.FirstOrDefault(m=>m.x == finishIndex && m.y == startIndex);
        if (res == Vector2.zero) return true;
        if (res != null) return false;
        var res1 = moves.FirstOrDefault(m => m.x == startIndex && m.y == finishIndex);
        if (res1 == Vector2.zero) return true;
        if (res1 != null) return false;
        return true;
    }

}
