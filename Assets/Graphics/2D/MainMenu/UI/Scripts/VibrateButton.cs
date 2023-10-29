using UnityEngine;
using UnityEngine.EventSystems;

public class VibrateButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool vibrateEnabled = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if the Vibrate button is clicked
        if (vibrateEnabled)
        {
            // Call the method to trigger vibration
            Vibrate();
        }
    }

    // Add this method for vibration
    private void Vibrate()
    {
        // Add your vibration logic here
        Handheld.Vibrate();
    }
}
