using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header(" Detection ")]
    [SerializeField] private LayerMask runnersLayer;
    [SerializeField] private float detectionDistance;
    private Runner targetRunner;

    [Header(" Movement ")]
    [SerializeField] private float moveSpeed;

    [Header(" Animation ")]
    [SerializeField] private Animator animator;



    public int strength;
    public float health;
    public float maxHealth = 100;
    
    public GameObject healthBarUI;
    public Slider slider;
    void Start()
    {
       // animator.SetInteger("State", 0);
        slider.value = CalculateHealth();

    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();
        if(health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if(health <=0)
        {
            Explode();
            healthBarUI.SetActive(false);
            FindObjectOfType<SquadController>().RunStatus();
        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }


      //  if (targetRunner == null)
            FindTargetRunner();
       
       
    }

    public void IsFighting()
    {
        animator.SetBool("isFighting", true);
    }
    float CalculateHealth()
    {
        return health / maxHealth;
    }
    private void FindTargetRunner()
    {
        Collider[] detectedRunners = Physics.OverlapSphere(transform.position, detectionDistance, runnersLayer);

        if (detectedRunners.Length <= 0) return;




        StartMoving();
    }
    private void StartMoving()
    {
        animator.SetInteger("State", 1);
        transform.parent = null;
    }


    private void Explode()
    {
        transform.gameObject.SetActive(false);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }


}
