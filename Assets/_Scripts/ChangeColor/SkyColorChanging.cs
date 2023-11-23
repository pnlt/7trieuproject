using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyColorChanging : MonoBehaviour
{
    [SerializeField] private Color[] colorArray;
    [SerializeField] private SpriteRenderer skyColor;
    [SerializeField] private float time;
    private float timeTransition;

    private int firstColor;
    private int secondColor;

    private void Start()
    {
        skyColor = GetComponent<SpriteRenderer>();
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
