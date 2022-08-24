using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonObjects : MonoBehaviour
{
    #region Singleton
    public static CommonObjects Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public GameObject touchSign;
    public LineRenderer line;
    public List<Tower> towers;

    public Material greenMat;
    public Material blueMat;
    public Material yellowMat;
    public Material redMat;
    public Material greyMat;

    public GameObject linePrefab;
}


public enum TowerState
{
    myTower,
    oppTower,
    pending
}