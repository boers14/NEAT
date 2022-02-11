using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    public GameObject carPrefab;

    [SerializeField]
    private GameObject startPoint;

    [SerializeField]
    private Finish finish;

    [SerializeField]
    private Text generationNumberText, bestTimeText;

    [SerializeField]
    private int populationSize = 10, timeScale = 8;
    private int generationNumber = 0, amountOfCarsDisabled;
    public int inputs = 7, outputs = 3;
    private Neat neat;
    private List<Driving> carList = null;

    private Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        StartNextGeneration();
    }

    public void StartCheckForNextGeneration()
    {
        StartCoroutine(CheckForNextGeneration());
    }

    private IEnumerator CheckForNextGeneration()
    {
        yield return new WaitForEndOfFrame();
        CallNextGeneration();
    }

    private void CallNextGeneration()
    {
        amountOfCarsDisabled = 0;
        for (int i = 0; i < carList.Count; i++)
        {
            if (!carList[i].gameObject.activeSelf)
            {
                amountOfCarsDisabled++;
            }
        }

        if (amountOfCarsDisabled == carList.Count)
        {
            StartNextGeneration();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = timeScale;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (pathfinding.enabled)
            {
                EnablePathfinding(false);
            }
            else
            {
                EnablePathfinding(true);
            }
        }
    }

    private void StartNextGeneration()
    {
        if (generationNumber == 0)
        {
            InitCarNeuralNetworks();
        }
        else
        {
            neat.Evolve();
            neat.PrintSpecies();
        }

        generationNumber++;

        generationNumberText.text = "Generation number: " + generationNumber;

        CreateCars();
    }

    private void CreateCars()
    {
        if (carList != null)
        {
            for (int i = 0; i < carList.Count; i++)
            {
                Destroy(carList[i].gameObject);
            }

        }

        carList = new List<Driving>();
        for (int i = 0; i < populationSize; i++)
        {
            Driving car = Instantiate(carPrefab, startPoint.transform.position, carPrefab.transform.rotation).GetComponent<Driving>();
            car.Init(neat.GetClient(i), this);
            car.name = "Client " + (i + 1).ToString();
            carList.Add(car);
        }

    }

    void InitCarNeuralNetworks()
    {
        neat = new Neat(inputs, outputs, populationSize);
    }

    public int GetPopulationSize()
    {
        return populationSize;
    }

    public Neat GetNeat()
    {
        return neat;
    }

    public void CalculateTime(float distance)
    {
        distance *= 1.25f;
        float distNotMax = distance / (carList[0].GetSpeedUpdate() * 2) * (carList[0].GetEffectOfSpeed() / 100 + 1);
        distance -= distNotMax;

        float addedTime = distNotMax / (carList[0].GetMaxSpeed() / 2);
        float bestTime = distance / carList[0].GetMaxSpeed();

        bestTime += addedTime;

        bestTime *= finish.GetLapAmount();

        bestTimeText.text = "Best time possible: " + bestTime.ToString();
        finish.SetTimeFactor(bestTime);
    }

    public void EnablePathfinding(bool enable)
    {
        foreach (Transform t in pathfinding.GetGrid().GetTargetList())
        {
            t.gameObject.SetActive(enable);
        }

        pathfinding.GetGrid().enabled = enable;
        pathfinding.enabled = enable;
    }
}
