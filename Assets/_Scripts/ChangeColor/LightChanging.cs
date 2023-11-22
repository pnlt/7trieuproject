using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanging : MonoBehaviour
{
    [SerializeField] private Color[] colorArray;
    [SerializeField] private Light skyColor;
    private float time;
    private GameManager gameManager;
    private float timeTransition;

    private int firstColor;
    private int secondColor;
    //public Light pointLight;

    private void Start()
    {
        gameManager = GameManager._instance;
        skyColor = GetComponent<Light>();
        time = gameManager.GetTimeCycle();
        firstColor = 0;
        secondColor = 1;
    }

    private void Update()
    {
        Transition();
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
