using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    private PostProcessVolume volume;
    private AutoExposure autoExposure;
    private Bloom bloom;
    private GameManager gameManager;

    private float time;
    private float timeChanging;
    private float deltaTime;
    private int swapDirect = 1;
    private int frameExposureIndex = 0;
    private int frameBloomIndex = 0;
    private int frameFinal;
    private bool inNight;
    private float timeBloom;
    private float rate = .25f;

    private void Start()
    {
        gameManager = GameManager._instance;
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<AutoExposure>(out autoExposure);
        volume.profile.TryGetSettings<Bloom>(out bloom);

        time = gameManager.GetTimeCycle();
        inNight = false;
    }

    private void Update()
    {
        DayNightCycle();
    }

    private void DayNightCycle()
    {
        timeChanging += Time.deltaTime / time;
        deltaTime = CalculateRateTime(ref deltaTime, ref frameExposureIndex, .8f); 
        autoExposure.minLuminance.value = Mathf.Clamp(autoExposure.minLuminance.value += deltaTime, -0.8f, 0f);

        timeBloom = CalculateRateTime(ref timeBloom, ref frameBloomIndex, 35);
        if (!inNight && timeChanging >= .7f)
            rate = 2f;
        else if (inNight)
            rate = 1.5f;
        bloom.intensity.value = Mathf.Clamp(bloom.intensity.value += timeBloom * rate, 0, 35);
        
       
        if (timeChanging >= 1)
        {
            rate = .25f;
            frameExposureIndex = 0;
            frameBloomIndex = 0;
            swapDirect = -swapDirect;
            timeChanging = 0;
            inNight = !inNight;
        }
    }
    
    private float CalculateRateTime(ref float deltaTime, ref int frameIndex, float value)
    {
        frameIndex += 1;
        frameFinal = (int)(((1 - timeChanging) * frameIndex) / timeChanging) + frameIndex;
        deltaTime = (value * swapDirect) / frameFinal;
        return deltaTime;
    }

}
