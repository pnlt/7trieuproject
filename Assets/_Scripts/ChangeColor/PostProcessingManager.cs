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
    private int frameIndex = 0;
    private int frameFinal;
    private bool activateNightLight;
    private bool inNight;

    private void Start()
    {
        gameManager = GameManager._instance;
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<AutoExposure>(out autoExposure);
        volume.profile.TryGetSettings<Bloom>(out bloom);

        time = gameManager.GetTimeCycle();
        activateNightLight = false;
        inNight = false;
    }

    private void Update()
    {
        DayNightCycle();
    }

    private void DayNightCycle()
    {
        BloomActive(); 
        timeChanging += Time.deltaTime / time;
        frameIndex += 1;
        frameFinal = (int)(((1 - timeChanging) * frameIndex) / timeChanging) + frameIndex;
        deltaTime = (1.8f * swapDirect) / frameFinal;
        autoExposure.minLuminance.value = Mathf.Clamp(autoExposure.minLuminance.value += deltaTime, -0.8f, 1);
      
       
        if (timeChanging >= 1)
        {
            frameIndex = 0;
            swapDirect = -swapDirect;
            activateNightLight = !activateNightLight;
            timeChanging = 0;
            inNight = !inNight;
        }
    }

    private void BloomActive()
    {
        if (inNight)
        {
            bloom.active = true;
            bloom.intensity.value -= .05f;
            if (timeChanging >= 0.15f)
            {
                bloom.active = false;
                bloom.intensity.value = 50;
            }
                
        }
        else
        {
            bloom.active = false;
        }
    }
}
