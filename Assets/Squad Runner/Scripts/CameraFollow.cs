using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Transform RotateCam;
    public float rotateSpeed;
    Vector3 offset;
    bool fightBoss = false;
    private void Start()
    {
        offset = transform.position - player.position;
    }

    public void ChangeView()
    {
        fightBoss = !fightBoss;
        
    }
    void Update()
    {
        if (fightBoss)
        {
            if( RotateCam.rotation.y >= -0.45 )
                
             {

                RotateCam.Rotate(0, rotateSpeed * Time.deltaTime, 0);

            }
            if( Vector3.Distance(transform.position,RotateCam.position) > 19f)
                transform.position = Vector3.MoveTowards(transform.position, RotateCam.transform.position, 5 * Time.deltaTime);
        }
        
        else
        {
            Vector3 targetPos = player.position + offset;
            targetPos.x = Mathf.Clamp(targetPos.x, -1.7f, 1.7f);

            transform.position = targetPos;
        }
    }
}
