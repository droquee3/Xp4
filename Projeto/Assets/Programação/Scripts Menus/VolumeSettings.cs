using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    public void ChangeVolume()
    {
        AudioListener.volume = musicSlider.value;

    }
}
