using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetPos : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private LayerMask hitLayers;

    [SerializeField]
    private Pathfinding pathfinding;

    private int inputKey;

    private void Start()
    {
        cam = Camera.main;
        hitLayers = ~hitLayers;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.X) && Input.GetKey(inputKey.ToString()))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayers))
            {
                transform.position = hit.point;
            }
            pathfinding.ResetPathfinding();
        }
    }

    public void GetListNumber(int index)
    {
        inputKey = index + 1;
    }
}
