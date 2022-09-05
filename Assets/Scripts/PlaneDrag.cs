using UnityEngine;
public class PlaneDrag : MonoBehaviour
{
    float smoothTime = 0.1F;
    private Vector3 velocity = Vector3.zero;
    Plane plane = new(Vector3.up, Vector3.up * 1); // ground plane
    Transform greenSign;
    SignCollisionDetector detector;
    private void Start()
    {
        greenSign = CommonObjects.Instance.touchSign.transform;
        detector = greenSign.GetComponent<SignCollisionDetector>();
    }

    private void OnMouseDown()
    {
        detector.canStopMoves = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        { 
            greenSign.position = ray.GetPoint(distance); // distance along the ray
            greenSign.gameObject.SetActive(true);
        }
        CommonObjects.Instance.trailRenderer.enabled = true;
    }

    private void OnMouseDrag()
    {
        greenSign.gameObject.SetActive(true);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            greenSign.position = Vector3.SmoothDamp(greenSign.position, targetPosition, ref velocity, smoothTime);

        }
    }

    private void OnMouseUp()
    {
        greenSign.gameObject.SetActive(false);
    }
}
