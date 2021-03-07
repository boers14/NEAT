using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    [SerializeField]
    private int numberOfRounds;

    private float checkpointsDone, timeFactor = 60, bestTimeDone = 0;

    public bool finished = true;

    [SerializeField]
    private GameObject checkPointsList;

    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    [SerializeField]
    private Text finishText, bestTimeDoneText;

    private Client bestClient;

    private void Start()
    {
        for (int i = 0; i < checkPointsList.transform.childCount; i++)
        {
            Transform checkpoint = checkPointsList.transform.GetChild(i);
            checkpoints.Add(checkpoint.GetComponent<Checkpoint>());
        }
    }

    public void ResetFinish()
    {
        finished = false;
        finishText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<Driving>().ReachedFinish() == 0)
            {
                if (other.GetComponent<Driving>().lap < numberOfRounds)
                {
                    other.GetComponent<Driving>().AddRound();
                    ResetFinish();
                    other.GetComponent<Driving>().AddLiveValueFinish(timeFactor);
                }
                else if (other.GetComponent<Driving>().lap >= numberOfRounds)
                {
                    StartCoroutine(Finished(other.GetComponent<Driving>()));
                }
            }
        }
    }

    private IEnumerator Finished(Driving car)
    {
        if (bestTimeDone == 0 || car.GetTime() < bestTimeDone)
        {
            if (bestClient != null)
            {
                bestClient.setBestClient(false);
            }

            bestClient = car.GetClient();
            bestClient.setBestClient(true);
            bestTimeDoneText.text = "Best time done: " + car.GetTime();
            bestTimeDone = car.GetTime();
        }
        
        finished = true;
        finishText.enabled = true;
        finishText.text = "Finished " + car.GetTime().ToString();
        car.AddLiveValueFinish(timeFactor);
        yield return new WaitForSeconds(3);

        car.ResetCar();
        ResetFinish();
    }

    public int GetLapAmount()
    {
        return numberOfRounds;
    }

    public void SetTimeFactor(float timeFactor)
    {
        this.timeFactor = timeFactor;
    }
}
