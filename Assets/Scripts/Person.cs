using System.Threading.Tasks;
using UnityEngine;

public class Person : MonoBehaviour, IPooledObject
{
    public Tower _startTower;
    public Transform _destination;
    public float _speed;
    public float distanceToStop = 1f;
    bool isMoving;
    public ColorTeam colorTeam;
    [SerializeField] GameObject changingColorPart;
    [SerializeField] Animator anim;
    [SerializeField] new BoxCollider collider;
    public void OnObjectSpawn(float speed,Transform destination, Tower startPos, ColorTeam team)
    {
        collider.enabled = true;
        colorTeam = team;
        _speed = speed;
        _destination = destination;
        _startTower = startPos;
        transform.LookAt(_destination);
        isMoving = true;
        startPos.people.Add(this);
        ChangeMaterialByTeamColor();
    }

    private void Update()
    {
        if (!isMoving) return;
        if (_destination == null) return;
        if ((transform.position - _destination.position).magnitude <= distanceToStop)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);
    }

    void ChangeMaterialByTeamColor()
    {
        if (colorTeam == ColorTeam.blue)
        {
            changingColorPart.GetComponent<SkinnedMeshRenderer>().material = CommonObjects.Instance.blueMat;
        }
        else if (colorTeam == ColorTeam.red)
        {
            changingColorPart.GetComponent<SkinnedMeshRenderer>().material = CommonObjects.Instance.redMat;
        }
        else if (colorTeam == ColorTeam.green)
        {
            changingColorPart.GetComponent<SkinnedMeshRenderer>().material = CommonObjects.Instance.greenMat;
        }
        else if (colorTeam == ColorTeam.grey)
        {
            changingColorPart.GetComponent<SkinnedMeshRenderer>().material = CommonObjects.Instance.greyMat;
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Person"))
        {
            Person otherObj = other.GetComponent<Person>();
            if (otherObj.colorTeam == colorTeam) return;
            if (otherObj._startTower.transform != _destination) return;
            if (otherObj._destination != _startTower.transform) return;
            isMoving = false;
            anim.SetTrigger("Dead");
            collider.enabled = false;
            await Task.Delay(900);
            gameObject.SetActive(false);
        }
    }
}
