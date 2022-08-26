using UnityEngine;
public class PlaneDrag : MonoBehaviour
{
    Plane plane = new Plane(Vector3.up, Vector3.up * 1); // ground plane
    Transform greenSign;
    private void Start()
    {
        greenSign = CommonObjects.Instance.touchSign.transform;
    }

    private void OnMouseDrag()
    {
        greenSign.gameObject.SetActive(true);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            greenSign.position = ray.GetPoint(distance); // distance along the ray
        }
    }

    private void OnMouseUp()
    {
        greenSign.gameObject.SetActive(false);
    }
}
