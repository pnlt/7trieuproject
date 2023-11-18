using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_Values : MonoBehaviour
{
    [SerializeField] private Transform[] X_Pos;
    public List<Transform> GetX_Values()
    {
        List<Transform> listX_Values = new List<Transform>();
        foreach (Transform t in X_Pos)
        {
            listX_Values.Add(t);
        }

        return listX_Values;
    }
}
