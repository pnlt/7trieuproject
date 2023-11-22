using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanging : MonoBehaviour
{
    [SerializeField] private Color[] colorArray;
    [SerializeField] private Light skyColor;
    [SerializeField] private float time;
    private float timeTransition;

    private int firstColor;
    private int secondColor;
    //public Light pointLight;

    private void Start()
    {
        skyColor = GetComponent<Light>();
        firstColor = 0;
        secondColor = 1;
    }

    private void Update()
    {
        Transition();
        //Debug.Log(colorArray.Length);
//        if (skyColor.color.r < colorArray[secondColor].r && skyColor.color.g < colorArray[secondColor].g && skyColor.color.b < colorArray[secondColor].b)
//{
//    pointLight.enabled = true;
//}
//else
//{
//    pointLight.enabled = false;
//}
    }

    private void Transition()
    {
        timeTransition += Time.deltaTime / time;
        skyColor.color = Color.Lerp(colorArray[firstColor], colorArray[secondColor], timeTransition);
        if (timeTransition >= 1)
        {
            timeTransition = 0;
            SwapIdx(ref firstColor, ref secondColor);
        }
    }

    private void SwapIdx(ref int firstColor, ref int secondColor)
    {
        int temp;
        temp = firstColor;
        firstColor = secondColor;
        secondColor = temp;
    }
}
