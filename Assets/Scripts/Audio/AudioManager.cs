using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer myAudioMixer;
    public Slider masterSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
        }
    }

    public void SetMasterVolume()
    {
        if (masterSlider == null)
        {
            return;
        }
        float volume = masterSlider.value;
        myAudioMixer.SetFloat("MasterVolumeParameter", Mathf.Log10(volume) * 20); // Convert linear slider value to logarithmic scale
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
        if (masterSlider != null)
        {
            masterSlider.value = savedVolume;
        }

        SetMasterVolume();
    }
}
