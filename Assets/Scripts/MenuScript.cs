using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private string MainLevel;
    [SerializeField]
    private CanvasGroup menu;
    [SerializeField]
    private CanvasGroup options;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Slider sensivitySlider;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        sensivitySlider.value = PlayerPrefs.GetFloat("Sensivity", 300f);
    }

    public void Play()
    {
        SceneManager.LoadScene(MainLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Option()
    {
        menu.alpha = 0;
        menu.interactable = false;
        menu.blocksRaycasts = false;
        options.alpha = 1;
        options.interactable = true;
        options.blocksRaycasts = true;
    }

    public void LeaveOptions()
    {
        options.alpha = 0;
        options.interactable = false;
        options.blocksRaycasts = false;
        menu.alpha = 1;
        menu.interactable = true;
        menu.blocksRaycasts = true;
    }

    public void ChangeVolume(System.Single value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        AudioListener.volume = value;
    }

    public void ChangeSensivity(System.Single value)
    {
        PlayerPrefs.SetFloat("Sensivity", value);
    }
}
