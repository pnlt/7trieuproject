using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;
    [SerializeField] Color handleActiveColor;

    private Image backgroundImage, handleImage;
    private Toggle toggle;
    private Vector2 handlePosition;

    private bool isVolumeOpen = true;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        toggle.onValueChanged.AddListener(OnSwitch);

        // Ensure the initial state is correctly set
        OnSwitch(isVolumeOpen);
    }

    void OnSwitch(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition; // no anim

        backgroundImage.color = on ? backgroundActiveColor : Color.white; // Change Color.white to your default color
        handleImage.color = on ? handleActiveColor : Color.white; // Change Color.white to your default color

        isVolumeOpen = on;

        // Update the volume button
        GameObject volumeButton = GameObject.Find("VolumeButton");
        if (volumeButton != null)
        {
            VolumeButton volumeScript = volumeButton.GetComponent<VolumeButton>();
            if (volumeScript != null)
            {
                volumeScript.SetVolumeState(on);
            }
        }
    }

    public void SetVolumeState(bool on)
    {
        toggle.isOn = on;
        OnSwitch(on);
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
