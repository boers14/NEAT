using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<Driving>().GetCheckpoints().Contains(this))
            {
                other.GetComponent<Driving>().AddLiveValue(this);
            }
        }
    }
}
