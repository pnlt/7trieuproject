using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector3 initialPos;
    private void Start()
    {
        initialPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, initialPos.y, transform.localPosition.z);
    }
}
