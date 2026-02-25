using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Management")]
    public string gameSceneName = "GameScene";

    [Header("UI Panels")]
    public GameObject optionsPanel;

    [Header("Audio")]
    public Slider volumeSlider;

    private void Start()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void PlayGame()
    {
        Debug.Log("Loading Game Scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}