using UnityEngine;

using Player;

using Core;
using UnityEngine.SceneManagement;
using Audio;
using UnityEngine.Rendering.Universal;

public class PauseUIController : MonoBehaviour
{
    [SerializeField] private GameObject uiFrame;

    [SerializeField] private string startSceneName;
    [SerializeField] private string gameSceneName;

    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject optionsUI;

    public bool Paused {
        get
        {
            return uiFrame.activeSelf;
        }

        set
        {
            uiFrame.SetActive(value);
            mainUI.SetActive(value);
            Game.Instance.IsPaused = value;
            if (value == false)
            {
                creditsUI.SetActive(false);
                optionsUI.SetActive(false);
            }
        }
    }

    private void Start()
    {
        Paused = false;
        PlayerCore.Input.AddToPauseAction(OnPausePressed);
    }

    private void OnDisable()
    {
        PlayerCore.Input.RemoveFromPauseAction(OnPausePressed);
    }

    private void OnPausePressed()
    {
        Debug.Log("Pause Pressed.");
        Paused = !Paused;
    }

    public void OnResume()
    {
        Paused = false;
    }

    public void OnRestart()
    {
        AudioManager.StopAllMusic();
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnBackToStart()
    {
        AudioManager.StopAllMusic();
        SceneManager.LoadScene(startSceneName);
    }

    public void OnCredits()
    {
        mainUI.SetActive(false);
        creditsUI.SetActive(true);
    }
    
    public void OnOptions()
    {
        mainUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void OnBackFromCredits()
    {
        mainUI.SetActive(true);
        creditsUI.SetActive(false);
    }

    public void OnBackFromOptions()
    {
        mainUI.SetActive(true);
        optionsUI.SetActive(false);
    }

    public void OnQuit()
    {
        Game.Quit();
    }
}
