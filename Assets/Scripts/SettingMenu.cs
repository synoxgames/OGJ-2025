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

    [Header("Buttons")]
    [SerializeField] private Button toggleMusicButton;
    [SerializeField] private Button toggleSFXButton;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    [Header("Sprites")]
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite unmuteSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;
    [Header("Difficulty Settings")]
    [SerializeField] public SelectedArt selectedArtRef;
    // Difficulty threshold defaults
    [SerializeField] private float easyUpperBound = 300; // this is the upper bound for the accuracy threshold for easy, higher is easier
    [SerializeField] private float easyLowerBound = 160; // this is the lower bound, lower is harder
    [SerializeField] private float mediumUpperBound = 110;
    [SerializeField] private float mediumLowerBound = 80;
    [SerializeField] private float hardUpperBound = 90;
    [SerializeField] private float hardLowerBound = 50f;
    [SerializeField] private float easyDisplayTime = 5f; // time art is displayed for easy
    [SerializeField] private float mediumDisplayTime = 4f; 
    [SerializeField] private float hardDisplayTime = 3f;
    [SerializeField] private float easyMoneyMultiplier = 0.5f; // multiplier for the money earned from this art per accuracy point
    [SerializeField] private float mediumMoneyMultiplier = 0.25f;
    [SerializeField] private float hardMoneyMultiplier = 0.1f;
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

    public void SelectEasy()
    {
        // Set button states
        easyButton.GetComponent<Image>().sprite = selectedSprite;
        mediumButton.GetComponent<Image>().sprite = unselectedSprite;
        hardButton.GetComponent<Image>().sprite = unselectedSprite;

        // Update selected art thresholds
        selectedArtRef.upperBound = easyUpperBound;
        selectedArtRef.lowerBound = easyLowerBound;

        selectedArtRef.displayTime = easyDisplayTime;
        selectedArtRef.moneyMultiplier = easyMoneyMultiplier;

        Debug.Log($"Selected easy difficulty: Upper = {selectedArtRef.upperBound}, Lower = {selectedArtRef.lowerBound}");
    }

    public void SelectMedium()
    {
        // Set button states
        easyButton.GetComponent<Image>().sprite = unselectedSprite;
        mediumButton.GetComponent<Image>().sprite = selectedSprite;
        hardButton.GetComponent<Image>().sprite = unselectedSprite;

        // Update selected art thresholds
        selectedArtRef.upperBound = mediumUpperBound;
        selectedArtRef.lowerBound = mediumLowerBound;

        selectedArtRef.displayTime = mediumDisplayTime;
        selectedArtRef.moneyMultiplier = mediumMoneyMultiplier;

        Debug.Log($"Selected medium difficulty: Upper = {selectedArtRef.upperBound}, Lower = {selectedArtRef.lowerBound}");
    }

    public void SelectHard()
    {
        // Set button states
        easyButton.GetComponent<Image>().sprite = unselectedSprite;
        mediumButton.GetComponent<Image>().sprite = unselectedSprite;
        hardButton.GetComponent<Image>().sprite = selectedSprite;

        // Update selected art thresholds
        selectedArtRef.upperBound = hardUpperBound;
        selectedArtRef.lowerBound = hardLowerBound;

        selectedArtRef.displayTime = hardDisplayTime;
        selectedArtRef.moneyMultiplier = hardMoneyMultiplier;

        Debug.Log($"Selected hard difficulty: Upper = {selectedArtRef.upperBound}, Lower = {selectedArtRef.lowerBound}");
    }
}
