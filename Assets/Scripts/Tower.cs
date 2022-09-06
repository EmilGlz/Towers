using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class Tower : MonoBehaviour
{
    [SerializeField] List<MeshRenderer> towerMeshes; 
    [SerializeField] GameObject selectedSign;
    [SerializeField] TextMesh personCountText;
    readonly int maxPersonCount = 60;
    int currentPersonCount = 10;
    [SerializeField] bool isFull;
    float delay = 1f;
    [HideInInspector] public int level;
    [HideInInspector] public bool isEmpty;
    public List<Tower> towersSendingPeopleToMe;
    [HideInInspector] public List<Tower> destinations;
    [HideInInspector] public bool startMoving;
    [HideInInspector] public List<Person> people;
    ObjectPooler _objectPooler;
    float _timePassed = 0f;
    public ColorTeam colorTeam;

    private void Start()
    {
        if (currentPersonCount == 0) isEmpty = true;
        else isEmpty = false;
        destinations = new List<Tower>();
        _objectPooler = ObjectPooler.Instance;
        personCountText.text = currentPersonCount.ToString();
        UpdateLevelByPersonCount();
        UpdateTowerColor();
        UpdateTowerModel();
    }

    public void SetSelectedSign(bool selected)
    {
        selectedSign.SetActive(selected);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Person"))
        {
            Person person = other.GetComponent<Person>();
            if (person._startTower == this)
            {
                return;
            }
            OnPersonEnteredMe(person);
            person._startTower.people.Remove(person);
        }
    }

    public void OnPersonEnteredMe(Person person)
    {
        if (colorTeam == person.colorTeam)
        {
            // count++
            if (currentPersonCount < maxPersonCount)
            {
                currentPersonCount++;
                personCountText.text = currentPersonCount.ToString();
                isEmpty = false;
            }
            else
            {
                isFull = true;
            }

            UpdateLevelByPersonCount();
        }
        else
        {
            // count--
            if (currentPersonCount > 0)
            {
                currentPersonCount--;
                personCountText.text = currentPersonCount.ToString();
            }
            else
            {
                isEmpty = true;
                colorTeam = person.colorTeam;
                UpdateTowerColor();
            }
            UpdateLevelByPersonCount();
        }
    }

    public void OnPersonLeftMe(Tower destination)
    {
        StartCoroutine(_objectPooler.SpawnFromPool("Person", this, Quaternion.identity, destination.transform, 0f));
        if (isFull)
        {
            int towersToMeCount = towersSendingPeopleToMe.Count;
            for (int i = 0; i < towersToMeCount; i++)
            {
                StartCoroutine(_objectPooler.SpawnFromPool("Person", this, Quaternion.identity, destination.transform, delay / 2));
            }
        }
    }

    void UpdateTowerColor()
    {

        if (colorTeam == ColorTeam.blue)
        {
            towerMeshes[level-1].material = CommonObjects.Instance.blueMat;
        }
        else if (colorTeam == ColorTeam.red)
        {
            towerMeshes[level - 1].material = CommonObjects.Instance.redMat;
        }
        else if (colorTeam == ColorTeam.green)
        {
            towerMeshes[level - 1].material = CommonObjects.Instance.greenMat;
        }
        else if (colorTeam == ColorTeam.grey)
        {
            towerMeshes[level - 1].material = CommonObjects.Instance.greyMat;
        }
    }

    public void UpdateTowerModel()
    {
        for (int i = 0; i < towerMeshes.Count; i++)
        {
            if (i == level-1)
            {
                towerMeshes[i].gameObject.SetActive(true);
            }
            else
            {
                towerMeshes[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (startMoving)
        {
            _timePassed += Time.deltaTime;
            if (_timePassed >= delay)
            {
                for (int i = 0; i < destinations.Count; i++)
                {
                    if (isEmpty)
                    {
                        for (int j = 0; j < destinations.Count; j++)
                        {
                            destinations[j].GetComponent<Tower>().towersSendingPeopleToMe.Remove(this);
                            GameController.Instance.DeleteMove(this, destinations[j]);
                        }
                        destinations = new List<Tower>();
                        startMoving = false;
                        break;
                    }
                    OnPersonLeftMe(destinations[i]);
                    _timePassed = 0f;
                }
            }
        }
    }

    public void StopSendingPeople(Tower destination)
    {
        destinations.Remove(destination);
        destination.towersSendingPeopleToMe.Remove(this);
    }

    void UpdateLevelByPersonCount()
    {
        var previousLevel = level;
        if (currentPersonCount < 10)
        {
            level = 1;
        }
        else if (currentPersonCount < 30)
        {
            level = 2;
        }
        else
        {
            level = 3;
        }
        if (previousLevel != level)
        {
            // level changed
            UpdateTowerModel();
            UpdateTowerColor();
        }
        delay = StableDatas.delayLevels[level];
        foreach (var item in people)
        {
            item._speed = StableDatas.personSpeedLevels[level];
        }
    }
}
