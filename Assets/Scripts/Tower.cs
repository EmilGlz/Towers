using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class Tower : MonoBehaviour
{
    public int maxPersonCount;
    public int currentPersonCount;
    public bool isFull;
    private bool isEmpty;
    public List<Tower> towersSendingPeopleToMe;
    public TowerState towerState;
    [SerializeField] private int delayInMilliSeconds;
    [SerializeField] List<Transform> destinations;
    [SerializeField] GameObject selectedSign;
    [SerializeField] TextMesh personCountText;
    ObjectPooler _objectPooler;

    private void Start()
    {
        destinations = new List<Transform>();
        _objectPooler = ObjectPooler.Instance;
        personCountText.text = currentPersonCount.ToString();
        UpdateTowerColor();
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
            if (person._startTower == transform)
            {
                return;
            }
            OnPersonEnteredMe();
        }
    }

    public void OnPersonEnteredMe()
    {
        if (towerState == TowerState.myTower)
        {
            // count++
            if (currentPersonCount < maxPersonCount)
            {
                currentPersonCount++;
                personCountText.text = currentPersonCount.ToString();
            }
            else
            {
                isFull = true;
            }
        }
        else if (towerState == TowerState.pending || towerState == TowerState.oppTower)
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
                towerState = TowerState.myTower;
                UpdateTowerColor();
            }
        }
    }

    public void OnPersonLeftMe()
    {
        if (currentPersonCount > 0)
        {
            currentPersonCount--;
            personCountText.text = currentPersonCount.ToString();
            isEmpty = false;
        }
        else
        {
            currentPersonCount = 0;
            personCountText.text = currentPersonCount.ToString();
            isEmpty = true;
        }
    }

    public async void StartMovingFromMe(Transform destination)
    {
        if (destinations.Contains(destination)) return;
        destinations.Add(destination);
        bool canSpawn = true;
        while (canSpawn)
        {
            for (int i = 0; i < destinations.Count; i++)
            {
                OnPersonLeftMe();
                if (isEmpty)
                {
                    for (int j = 0; j < destinations.Count; j++)
                    {
                        destinations[j].GetComponent<Tower>().towersSendingPeopleToMe.Remove(this);
                    }
                    destinations = new List<Transform>();
                    canSpawn = false;
                    GameController.Instance.DeleteLine(transform, destination);
                    break;
                }
                _objectPooler.SpawnFromPool("Person", transform, Quaternion.identity, destinations[i]);
            }
            await Task.Delay(delayInMilliSeconds);
        }    
    }

    public void UpdateTowerColor()
    {
        if (towerState == TowerState.myTower)
        {
            gameObject.GetComponent<Renderer>().material = CommonObjects.Instance.blueMat;
        }
        else if (towerState == TowerState.pending)
        {
            gameObject.GetComponent<Renderer>().material = CommonObjects.Instance.greyMat;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = CommonObjects.Instance.redMat;
        }
        
    }
}
