using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] FehleController fehleController;
    Camera cam;        

    private void Start()
    {
        cam = Camera.main;        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("Stone")))
            {
                var destination = hit.transform;
                fehleController.StartWorking(destination);
            }
        }
    }
}
