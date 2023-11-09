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


        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;

        GetComponent<Button>().onClick.AddListener(togglevolume);
    }

    void togglevolume()
    {
        isOpen = !isOpen;
        buttonImage.sprite = isOpen ? openVolumeImage : closedVolumeImage;


        //audiolistener.volume = isopen ? 1.0f : 0.0f;


    }

    //public void setvolumestate(bool on)
    //{
    //    isopen = on;
    //    buttonimage.sprite = isopen ? openvolumeimage : closedvolumeimage;


    //    audiolistener.volume = isopen ? 1.0f : 0.0f; 
    //}
}
