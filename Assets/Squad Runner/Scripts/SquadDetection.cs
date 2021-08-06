using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;

public class SquadDetection : MonoBehaviour
{
    [Header(" Managers ")]
    [SerializeField] private SquadFormation squadFormation;
    
    [SerializeField] private Runner runner;
    [SerializeField] private Transform runnersParent;

    [Header(" Settings ")]
    [SerializeField] private LayerMask doorLayer;
    [SerializeField] private LayerMask finishLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask bossLayer;
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.IsGame())
        {
            DetectDoors();
            DetectFinishLine();
            DetectionObstacles();
            
            if(checkBoss == false) DetectionBoss();
          if(FrezePos)
            {
                Vector3 pos = transform.position;
                pos.x = 0;
                transform.position = pos;
            }
        }
    }
    int i;
    bool checkBoss= false;
    bool FrezePos = false;
   
    private void DetectionBoss()
    {

        Vector3 distance = transform.position - 1*Vector3.back;
        Collider[] detectBoss = Physics.OverlapSphere(distance, 0.5f, bossLayer);
           if (detectBoss.Length <= 0) return;

        Collider colliderBoss = detectBoss[0];
        Boss boss = colliderBoss.GetComponent<Boss>();
        FrezePos = true;
        int damage = boss.strength;
        checkBoss = true;

        boss.IsFighting();
       

        FindObjectOfType<SquadController>().RunStatus(); 
        for ( i = 0; i < runnersParent.childCount; i++)
        {
            runner = runnersParent.GetChild(i).GetComponent<Runner>();
            runner.IsFighting();
            
        }
        squadFormation.ResetPos();
        squadFormation.HideText();
        FindObjectOfType<CameraFollow>().ChangeView();

        int ok = 5;

        int runnerCount = runnersParent.childCount;
        StartCoroutine(hitBoss());



        IEnumerator hitBoss()
        {
            for (int i = 0; i < 5; ++i)
            {
                yield return new WaitForSeconds(1.2f);
                boss.health -= 20;
                squadFormation.DelRunners(4);
                ok--;
                if (ok == 0)

                    for ( i = 0; i < runnersParent.childCount; i++)
                    {
                        runner = runnersParent.GetChild(i).GetComponent<Runner>();
                        runner.StopFighting();

                    }

            }

        }


        
      


    }
    
    private void DetectDoors()
    {
        Collider[] detectedDoors = Physics.OverlapSphere(transform.position, squadFormation.GetSquadRadius(), doorLayer);

        if (detectedDoors.Length <= 0) return;

        Collider collidedDoorCollider = detectedDoors[0];
        Door collidedDoor = collidedDoorCollider.GetComponentInParent<Door>();

        int runnersAmountToAdd = collidedDoor.GetRunnersAmountToAdd(collidedDoorCollider, transform.childCount);
        squadFormation.AddRunners(runnersAmountToAdd);
        
    }

    private void DetectFinishLine()
    {
        if (Physics.OverlapSphere(transform.position, 1, finishLayer).Length > 0)
        {
            FindObjectOfType<FinishLine>().PlayConfettiParticles();
            for (int i = 0; i < runnersParent.childCount; i++)
            {
                runner = runnersParent.GetChild(i).GetComponent<Runner>();
                runner.IsDancing();
            }
            SetLevelComplete();
           
        }

    }

    private void DetectionObstacles()
    {
        if (Physics.OverlapSphere(transform.position, 0.5f, obstacleLayer).Length > 0)
        {
            runner.Explode();

        }
    }
   

    private void SetLevelComplete()
    {
        UIManager.setLevelCompleteDelegate?.Invoke(3);
        
    }


    


}
