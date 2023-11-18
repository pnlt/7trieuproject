using UnityEngine;
using UnityEngine.UI;

public class VolumeEffectButton : MonoBehaviour
{
    public Sprite openVolumeEffectImage;
    public Sprite closedVolumeEffectImage;

    private Image buttonImage;
    private bool isOpen = true;


    void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            return;
        }


        isOpen = PlayerPrefs.GetInt("EffectVolumeState", 1) == 1;

        buttonImage.sprite = isOpen ? openVolumeEffectImage : closedVolumeEffectImage;

        


        GetComponent<Button>().onClick.AddListener(togglevolume);
    }

    void togglevolume()
    {
        isOpen = !isOpen;
        buttonImage.sprite = isOpen ? openVolumeEffectImage : closedVolumeEffectImage;


        PlayerPrefs.SetInt("EffectVolumeState", isOpen ? 1 : 0);


    }

    public void setvolumestate(bool on)
    {
        isOpen = on;
        buttonImage.sprite = isOpen ? openVolumeEffectImage : closedVolumeEffectImage;
        PlayerPrefs.SetInt("EffectVolumeState", isOpen ? 1 : 0);

    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("EffectVolumeState", 1);
    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetInt("EffectVolumeState", isOpen ? 1 : 0);
        }
    }
}
