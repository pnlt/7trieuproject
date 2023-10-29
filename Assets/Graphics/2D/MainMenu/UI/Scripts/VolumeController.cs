using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Assign your UI slider to this field in the Inspector
    public Button volumeButton; // Assign your UI button to this field in the Inspector

    private bool isSliderVisible = false;
    private bool isSliderInteracted = false;

    private Coroutine hideSliderCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        HideSlider();

        // Attach the ToggleSliderVisibility method to the button click event
        if (volumeButton != null)
        {
            volumeButton.onClick.AddListener(ToggleSliderVisibility);
        }

        // Attach the SliderInteract method to the slider's interactable state change event
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SliderInteract);
        }
    }

    // Toggle the visibility of the slider
    void ToggleSliderVisibility()
    {
        if (isSliderVisible && !isSliderInteracted)
        {
            // Stop any existing coroutine
            if (hideSliderCoroutine != null)
            {
                StopCoroutine(hideSliderCoroutine);
            }

            // Start a new coroutine to hide the slider with a delay
            hideSliderCoroutine = StartCoroutine(HideSliderWithDelay());
        }
        else
        {
            ShowSlider();
        }

        // Reset the slider interaction flag
        isSliderInteracted = false;
    }

    void ShowSlider()
    {
        volumeSlider.gameObject.SetActive(true);
        isSliderVisible = true;
    }

    IEnumerator HideSliderWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        hideSliderCoroutine = null;
        HideSlider();
    }

    void HideSlider()
    {
        volumeSlider.gameObject.SetActive(false);
        isSliderVisible = false;
    }

    // Handle slider interaction
    void SliderInteract(float value)
    {
        isSliderInteracted = true;
    }
}
