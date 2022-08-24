using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalUser : MonoBehaviour
{
    #region Singleton
    private static LocalUser _instance;
    public static LocalUser Instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public int budget;
    public int level;
}
