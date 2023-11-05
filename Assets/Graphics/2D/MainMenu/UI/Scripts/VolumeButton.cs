using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    public Sprite openVolumeImage;
    public Sprite closedVolumeImage;

    private Image buttonImage;
    private bool isOpen = true;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("Image component not found on the button GameObject.");
            return;
        }

        buttonImage.sprite = openVolumeImage;

        GetComponent<Button>().onClick.AddListener(ToggleVolume);
    }

    void ToggleVolume()
    {
        isOpen = !isOpen;
        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;

        // Adjust the system volume
        AudioListener.volume = isOpen ? 1.0f : 0.0f; // 1.0 is 100%, 0.0 is 0%
    }
}
