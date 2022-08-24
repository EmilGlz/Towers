using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FehleController : MonoBehaviour
{
    #region Singleton
    private static FehleController _instance;
    public static FehleController Instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public List<Fehle> fehles;
    public Transform fehlesParent;
    private void Start()
    {
        fehles = fehlesParent.GetComponentsInChildren<Fehle>().ToList();
    }

    public void StartWorking(Transform stone)
    {
        var stoneScript = stone.GetComponent<Stone>();
        if (stoneScript == null) return;
        if (stoneScript.isDestroyed) return;
        var budget = LocalUser.Instance.budget;
        for (int i = 0; i < fehles.Count; i++)
        {
            if (budget - fehles[i].salary < 0)
            {
                break;
            }
            if (!fehles[i].isWorking && !fehles[i].isMoving)
            {
                fehles[i].StartGoingToDestination(stone.position);
                budget -= fehles[i].salary;
            }
            if (!fehles[i].isWorking && fehles[i].isMoving)
            {
                fehles[i].StartGoingToDestination(stone.position);
            }
        }
        LocalUser.Instance.budget = budget;
    }

    public void OnStoneDestroyed(Stone stone)
    {
        LocalUser.Instance.budget += stone.gainingMoney;
    }
}
