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
            return;
        }

        
        isOpen = PlayerPrefs.GetInt("VolumeState", 1) == 1;

        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;

        GetComponent<Button>().onClick.AddListener(togglevolume);
    }

    void togglevolume()
    {
        isOpen = !isOpen;
        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;


        PlayerPrefs.SetInt("VolumeState", isOpen ? 1 : 0);


    }

    public void setvolumestate(bool on)
    {
        isOpen = on;
        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;
        PlayerPrefs.SetInt("VolumeState", isOpen ? 1 : 0);

    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("VolumeState", 1);
    }

}
