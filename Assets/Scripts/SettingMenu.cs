using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Mute Buttons")]
    [SerializeField] private Button toggleMusicButton;
    [SerializeField] private Button toggleSFXButton;

    [Header("Sprites")]
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite unmuteSprite;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private const float minVolume = 0.0001f;
    private const float muteDb = -80f;

    void Awake()
    {
        // Setup slider listeners
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Load mixer values and convert to slider-friendly 0.0001–1 range
        if (audioMixer.GetFloat("MusicVolume", out float musicDb))
        {
            musicVolume = Mathf.Pow(10f, musicDb / 20f);
            SetSliderSilently(musicSlider, musicVolume);
            toggleMusicButton.GetComponent<Image>().sprite = (musicDb <= muteDb) ? muteSprite : unmuteSprite;
        }

        if (audioMixer.GetFloat("SFXVolume", out float sfxDb))
        {
            sfxVolume = Mathf.Pow(10f, sfxDb / 20f);
            SetSliderSilently(sfxSlider, sfxVolume);
            toggleSFXButton.GetComponent<Image>().sprite = (sfxDb <= muteDb) ? muteSprite : unmuteSprite;
        }
    }

    public void ToggleMusic()
    {
        if (audioMixer.GetFloat("MusicVolume", out float currentDb))
        {
            if (currentDb <= muteDb)
            {
                // Unmute
                SetMusicVolume(musicVolume);
                SetSliderSilently(musicSlider, musicVolume);
                toggleMusicButton.GetComponent<Image>().sprite = unmuteSprite;
            }
            else
            {
                // Mute
                musicVolume = musicSlider.value; // Save current volume
                audioMixer.SetFloat("MusicVolume", muteDb);
                toggleMusicButton.GetComponent<Image>().sprite = muteSprite;
            }
        }
    }

    public void ToggleSFX()
    {
        if (audioMixer.GetFloat("SFXVolume", out float currentDb))
        {
            if (currentDb <= muteDb)
            {
                // Unmute
                SetSFXVolume(sfxVolume);
                SetSliderSilently(sfxSlider, sfxVolume);
                toggleSFXButton.GetComponent<Image>().sprite = unmuteSprite;
            }
            else
            {
                // Mute
                sfxVolume = sfxSlider.value; // Save current volume
                audioMixer.SetFloat("SFXVolume", muteDb);
                toggleSFXButton.GetComponent<Image>().sprite = muteSprite;
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, minVolume, 1f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);

        if (volume > minVolume)
            musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, minVolume, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);

        if (volume > minVolume)
            sfxVolume = volume;
    }

    private void SetSliderSilently(Slider slider, float value)
    {
        // Temporarily detach listeners to avoid recursive SetXVolume() calls
        slider.onValueChanged.RemoveAllListeners();
        slider.value = value;

        // Reattach appropriate listener
        if (slider == musicSlider)
            slider.onValueChanged.AddListener(SetMusicVolume);
        else if (slider == sfxSlider)
            slider.onValueChanged.AddListener(SetSFXVolume);
    }
}
