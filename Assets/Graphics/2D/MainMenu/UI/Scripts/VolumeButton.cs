using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    public Sprite openVolumeImage;
    public Sprite closedVolumeImage;

    private Image buttonImage;
    private bool isOpen = true;

    // Use PlayerPrefs to store volume state
    private const string VolumePrefsKey = "IsVolumeOpen";

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            return;
        }

        // Retrieve the volume state from PlayerPrefs
        isOpen = PlayerPrefs.GetInt(VolumePrefsKey, 1) == 1;

        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;

        GetComponent<Button>().onClick.AddListener(ToggleVolume);
    }

    void ToggleVolume()
    {
        isOpen = !isOpen;
        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;

        // Adjust the system volume
        AudioListener.volume = isOpen ? 1.0f : 0.0f; // 1.0 is 100%, 0.0 is 0%

        // Save the volume state to PlayerPrefs
        PlayerPrefs.SetInt(VolumePrefsKey, isOpen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetVolumeState(bool on)
    {
        isOpen = on;
        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;

        // Adjust the system volume
        AudioListener.volume = isOpen ? 1.0f : 0.0f; // 1.0 is 100%, 0.0 is 0%
    }
}
