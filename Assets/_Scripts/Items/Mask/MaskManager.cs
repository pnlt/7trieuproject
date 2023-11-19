using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    public List<Mask_Script> masksPool;

    private void OnEnable()
    {
        masksPool = new List<Mask_Script>();
    }

    public void AddMask(Mask_Script mask)
    {
        masksPool.Add(mask);
    }
    
}
