using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LuckyBoxMotion : MonoBehaviour
{
    [SerializeField] private float speedMotion;
    [SerializeField] private float timeThreshold;

    private Vector3 direction;
    private float initialThreshold;

    // Start is called before the first frame update
    private void Start()
    {
        direction = Vector3.up;
        initialThreshold = timeThreshold;
    }

    // Update is called once per frame
    private void Update()
    {
        BoxMotion();
        
    }

    private void BoxMotion()
    {
        timeThreshold -= Time.deltaTime;
        transform.position += direction * speedMotion * Time.deltaTime;
        if (timeThreshold <= 0)
        {
            direction = -direction;
            timeThreshold = initialThreshold;
        }
    }
}
