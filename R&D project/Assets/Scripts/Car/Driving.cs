using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Driving : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float maxSpeed, speedUpdate, rotationForce, brakeForce, carSlowDown, effectOfSpeedSteering, effectOfSpeedSpeed, timeForSpeeding, timeForDeletingCP;

    private Finish finish;

    [SerializeField]
    private bool canDriveManually = false;

    [SerializeField]
    private LayerMask mask;

    private GameObject checkPointsList;

    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private float carSpeed, liveValue, steerPower, time, prevCarSpeed, liveValueAdded = 1, rayCastAddZ, rayCastAddX, timeForGiveGas, deleteCP;
    public float lap;

    private bool driving, canSteer, giveGas;

    private Client client;

    private Manager manager;

    private Vector3 startPos;

    [SerializeField]
    private int amountOfInputs = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (canDriveManually)
        {
            startPos = transform.position;
        }
        mask = ~mask;
        rb = GetComponent<Rigidbody>();
        finish = GameObject.Find("Start").GetComponent<Finish>();
        rayCastAddZ = transform.localScale.z/2;
        rayCastAddX = transform.localScale.x/2;
        checkPointsList = GameObject.Find("Checkpoints");
        AddCheckpoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (canDriveManually)
        {
            DriveWithInputs();
        }
        else
        {
            DriveWithoutInputs();
        }


        if (!driving)
        {
            carSpeed *= carSlowDown;
        }

        if (carSpeed > 0.8f)
        {
            canSteer = true;
        }
        else
        {
            canSteer = false;
        }

        rb.velocity = transform.forward * carSpeed;
        driving = false;
        time += Time.deltaTime;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall" && !finish.finished)
        {
            ResetCar();
            finish.ResetFinish();
        }
    }

    private void DriveWithoutInputs()
    {
        double[] outputs = client.Calculate(DecideOutput());

        for (int i = 0; i < amountOfInputs; i++)
            {
            int input = directionFromOutput(outputs);
            outputs[input] = -100;

            if (input == 0)
            {
                GiveGas();
            }
            else if (input == 1)
            {
                SteerRight();
            }
            else if (input == 2)
            {
                SteerLeft();
            }
            else if (input == 3)
            {
                Brake();
            }
            else
            {

            }
        }

        if (carSpeed <= 1 && !giveGas)
        {
            timeForGiveGas += Time.deltaTime;
            if (timeForGiveGas > timeForSpeeding)
            {
                liveValueAdded /= 2;
                giveGas = true;
            }
        }
        else if (giveGas)
        {
            timeForGiveGas += Time.deltaTime;
            GiveGas();
            if (timeForGiveGas > timeForSpeeding * 1.5f)
            {
                giveGas = false;
            }
        }
        else
        {
            timeForGiveGas = 0;
        }

        deleteCP += Time.deltaTime;
        if (deleteCP > timeForDeletingCP)
        {
            ResetCar();
            finish.ResetFinish();
        }
    }

    private double[] DecideOutput()
    {
        double[] outputs = new double[manager.inputs];

        if (Physics.Raycast(transform.position + new Vector3(0, 0, rayCastAddZ), transform.forward, out RaycastHit hit1, Mathf.Infinity, mask))
        {
            if (hit1.transform.tag == "Wall")
            {
                outputs[0] = hit1.distance;
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, rayCastAddX), transform.right, out RaycastHit hit2, Mathf.Infinity, mask))
        {
            if (hit2.transform.tag == "Wall")
            {
                outputs[1] = hit2.distance;
            }
        }

        if (Physics.Raycast(transform.position - new Vector3(0, 0, rayCastAddX), -transform.right, out RaycastHit hit3, Mathf.Infinity, mask))
        {
            if (hit3.transform.tag == "Wall")
            {
                outputs[2] = hit3.distance;
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, rayCastAddZ), transform.right + transform.forward, out RaycastHit hit4, Mathf.Infinity, mask))
        {
            if (hit4.transform.tag == "Wall")
            {
                outputs[3] = hit4.distance;
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, -rayCastAddZ), -transform.right + transform.forward, out RaycastHit hit5, Mathf.Infinity, mask))
        {
            if (hit5.transform.tag == "Wall")
            {
                outputs[4] = hit5.distance;
            }
        }


        outputs[5] = (outputs[0] + outputs[2]) / 2;

        outputs[6] = (outputs[0] + outputs[1]) / 2;

        return outputs;
    }

    private int directionFromOutput(double[] output)
    {
        int index = 0;
        for (int i = 1; i < output.Length; i++){
            if (output[i] > output[index]){
                index = i;
            }
        }

        return index;
    }

    private void DriveWithInputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            GiveGas();
        }

        if (Input.GetKey(KeyCode.S))
        {
            Brake();
        }

        if (Input.GetKey(KeyCode.A))
        {
            SteerLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            SteerRight();
        }
    }

    public void GiveGas()
    {
        driving = true;
        if (carSpeed < 0)
        {
            carSpeed = 0;
        }

        carSpeed += (speedUpdate / ((carSpeed + maxSpeed) / maxSpeed)) / effectOfSpeedSpeed;
        if (carSpeed >= maxSpeed)
        {
            carSpeed = maxSpeed;
        }
    }

    public void Brake()
    {
        driving = true;
        carSpeed -= brakeForce;
        if (carSpeed < 0)
        {
            carSpeed = 0;
        }
    }

    public void SteerLeft()
    {
        if (canSteer)
        {
            steerPower = (rotationForce / ((carSpeed + maxSpeed) / maxSpeed)) / effectOfSpeedSteering;
            transform.Rotate(0, -steerPower, 0);
        }
    }

    public void SteerRight()
    {
        if (canSteer)
        {
            steerPower = (rotationForce / ((carSpeed + maxSpeed) / maxSpeed)) / effectOfSpeedSteering;
            transform.Rotate(0, steerPower, 0);
        }
    }

    public void AddRound()
    {
        lap++;
        AddCheckpoints();
    }

    public void AddLiveValue(Checkpoint checkpoint)
    {
        deleteCP = 0;

        checkpoints.Remove(checkpoint);

        if (!canDriveManually)
        {
            liveValue += liveValueAdded / time;
            client.SetScore(liveValue);
            liveValueAdded *= 2;
        }
    }

    public void AddLiveValueFinish(float timeFactor)
    {
        liveValue += (time / ((time + timeFactor) / timeFactor)) * 50;
        if (!canDriveManually)
        {
            client.SetScore(liveValue);
        }
    }

    public void ResetCar()
    {
        if (!canDriveManually)
        {
            gameObject.SetActive(false);
            manager.CallNextGeneration();
        }
        else
        {
            transform.position = startPos;
        }
    }

    public void Init(Client client, Manager manager)
    {
        this.client = client;
        this.manager = manager;
    }

    public float GetTime()
    {
        return time;
    }

    public void AddCheckpoints()
    {
        for (int i = 0; i < checkPointsList.transform.childCount; i++)
        {
            Transform checkpoint = checkPointsList.transform.GetChild(i);
            checkpoints.Add(checkpoint.GetComponent<Checkpoint>());
        }
    }

    public List<Checkpoint> GetCheckpoints()
    {
        return checkpoints;
    }

    public int ReachedFinish()
    {
        if (checkPointsList != null)
        {
            return checkpoints.Count;
        } else
        {
            return -1;
        }
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetSpeedUpdate()
    {
        return speedUpdate;
    }

    public float GetEffectOfSpeed()
    {
        return effectOfSpeedSpeed;
    }

    public Client GetClient()
    {
        return client;
    }
}
