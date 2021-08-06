﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;

public class Runner : MonoBehaviour
{
    [Header(" Components ")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider collider;
    [SerializeField] private Renderer renderer;

  

    [Header(" Target Settings ")]
    private bool targeted;

    [Header(" Detection ")]
    [SerializeField] private LayerMask obstaclesLayer;
    

    // Start is called before the first frame update
    void Start()
    {
        animator.speed = 1.5f;
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!collider.enabled)
            return;

        DetectObstacles();
       
    }

    private void DetectObstacles()
    {
        if (Physics.OverlapSphere(transform.position, 0.1f, obstaclesLayer).Length > 0)
            Explode();
    }
    
    public void IsFighting()
    {
      
        animator.SetBool("isFighting",true);
    }
    public void StopFighting()
    {
        animator.SetBool("isFighting", false);
        animator.SetBool("isRunning", true);
    }
    public void IsDancing()
    {
        animator.SetBool("isDancing", true);
        animator.SetBool("isRunning", false);
    }
    private IEnumerator running()
    {
      
        float delay = Random.Range(0f, 0.3f);
        yield return new WaitForSeconds(delay);
        animator.SetInteger("State", 1);

    }

    public void StopRunning()
    {
        animator.SetInteger("State", 0);
        
    }

    public void SetAsTarget()
    {
        targeted = true;
    }

    public bool IsTargeted()
    {
        return targeted;
    }

    public void Explode()
    {
        collider.enabled = false;
        renderer.enabled = false;

        if (transform.parent != null && transform.parent.childCount <= 1)
            UIManager.setGameoverDelegate?.Invoke();

        transform.parent = null;
       

        gameObject.SetActive(false);
        Audio_Manager.instance.play("Runner_Die");
    }
}
