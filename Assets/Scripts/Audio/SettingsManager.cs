using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("===================Settings=================")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject settingPanel;

    private Animator _animator;
    private bool isPanelOpen = false;

    private void Awake()
    {
        _animator = settingPanel.GetComponent<Animator>();
        _animator.enabled = false;
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
    }

    private void Update()
    {
        // Toggle settings panel when 'P' is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleSettingsPanel();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetAmbientVolume()
    {
        float volume = ambientSlider.value;
        myMixer.SetFloat("Ambient", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ambientVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume()
    {
        Debug.Log("Loading volume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        ambientSlider.value = PlayerPrefs.GetFloat("ambientVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ToggleSettingsPanel()
    {
        if (isPanelOpen)
        {
            CloseSettingsPanel();
        }
        else
        {
            OpenSettingsPanel();
        }
    }

    public void OpenSettingsPanel()
    {
        settingPanel.SetActive(true);
        _animator.enabled = true;
        _animator.Play("Open_setting_UI_anim");
        isPanelOpen = true;
    }

    public void CloseSettingsPanel()
    {
        _animator.Play("Close_Setting_ui_anim");
        StartCoroutine(DeactivatePanelAfterAnimation());
    }

    private IEnumerator DeactivatePanelAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(_animator.GetCurrentAnimatorStateInfo(0).length);
        settingPanel.SetActive(false);
        _animator.enabled = false;
        isPanelOpen = false;
    }

    public void Resume()
    {
        CloseSettingsPanel();
    }
}
