using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pallet_random_script : MonoBehaviour
{
    public GameObject[] difficults;
    public bool isDefault;
    private float reducePercent = 0.5f;

    private void OnDisable()
    {
        isDefault = false;
        if (!isDefault)
        {
            int random_value = Random.Range(0, difficults.Length);


            if (random_value == 0)
            {
                if (Random.value < reducePercent)
                {
                    random_value = Random.Range(1, difficults.Length);
                }
            }

            difficults[random_value].gameObject.SetActive(true);
        }
    }
}





