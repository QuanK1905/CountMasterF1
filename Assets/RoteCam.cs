using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoteCam : MonoBehaviour
{
    public Transform runnerParent;
    

    // Update is called once per frame
    void Update()
    {
        Vector3 targetRunner = runnerParent.position + Vector3.forward;
        targetRunner.x = 0;
        transform.position = targetRunner;
    }
}
