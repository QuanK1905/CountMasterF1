
using UnityEngine;
using TMPro;

public class SquadFormation : MonoBehaviour
{
    [Header(" Components ")]
    [SerializeField] private TextMeshPro squadAmountText;
    [SerializeField] private Transform bubble;
    [Header(" Formation Settings ")]
    [Range(0f, 1f)]
    [SerializeField] private float radiusFactor;
    [Range(0f, 1f)]
    [SerializeField] private float angleFactor;

    [Header(" Settings ")]
    [SerializeField] private Runner runnerPrefab;

    


    public void HideText()
    {
        bubble.gameObject.SetActive(false);
    }
    bool resetPosition = true;
    public void ResetPos()
    {
        resetPosition = !resetPosition;
    }
    private void FixedUpdate()
    {
        if (resetPosition)
            FermatSpiralPlacement();
    }
    void Update()
    {
       
        squadAmountText.text = transform.childCount.ToString();
    }

    private void FermatSpiralPlacement()
    {
        float goldenAngle = 137.5f * angleFactor;  

        for (int i = 0; i < transform.childCount; i++)
        {
            float x = radiusFactor * Mathf.Sqrt(i+1) * Mathf.Cos(Mathf.Deg2Rad * goldenAngle * (i+1));
            float z = radiusFactor * Mathf.Sqrt(i+1) * Mathf.Sin(Mathf.Deg2Rad * goldenAngle * (i+1));

            Vector3 runnerLocalPosition = new Vector3(x, 0, z);
            transform.GetChild(i).localPosition = Vector3.Lerp(transform.GetChild(i).localPosition, runnerLocalPosition, 0.1f);
        }
    }

    public float GetSquadRadius()
    {
        return radiusFactor * Mathf.Sqrt(transform.childCount);
    }

    public void AddRunners(int amount)
    {
        Audio_Manager.instance.play("Play");
        for (int i = 0; i < amount; i++)
        {
            Runner runnerInstance = Instantiate(runnerPrefab, transform);
            //  runnerInstance.SendMessage("running");
            runnerInstance.GetComponent<Runner>().StartRunning();
            runnerInstance.name = "Runner_" + runnerInstance.transform.GetSiblingIndex();
        }
    }
  
    public void DelRunners(int amount)
    {
        if (amount > transform.childCount) amount = transform.childCount;
        for (int i = 0; i < amount; i++)
           
          {
            float a = Random.Range(0, transform.childCount);
            Audio_Manager.instance.play("Runner_Die");
            Transform runnerDel = transform.GetChild((int)a);
            Runner runner = runnerDel.GetComponent<Runner>();
            runner.Explode();
           
        }
    }
}
