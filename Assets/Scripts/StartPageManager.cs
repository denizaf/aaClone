using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartPageManager : MonoBehaviour
{
    public Button soundButton;
    public Button continueButton;
    public Button levelsButton;
    public Button levelsBackButton;
    public Button statsButton;
    public Button statsBackButton;
    
    public GameObject startPanel;
    public GameObject statsPanel;
    public GameObject levelsPanel;
    
    public Text pinsShotText;
    public Text pinsLandedText;
    public Text levelsPassedText;
    public Text levelsFailedText;

    private int _lastPlayedLevel;
    private int _pinsShot;
    private int _pinsLanded;
    private int _levelsPassed;
    private int _levelsFailed;

    private void Start()
    {
        soundButton.onClick.AddListener(ToggleSound);
        continueButton.onClick.AddListener(ContinueGame);
        levelsButton.onClick.AddListener(ShowLevels);
        levelsBackButton.onClick.AddListener(HideLevels);
        statsButton.onClick.AddListener(ShowStats);
        statsBackButton.onClick.AddListener(HideStats);

        LoadPlayerStats();
        UpdateStatsUI();
    }

    private void ToggleSound()
    {
        SoundManager.Instance.ToggleSound();
        UpdateSoundButton();
    }

    private void UpdateSoundButton()
    {
        soundButton.GetComponentInChildren<Text>().text = SoundManager.Instance.IsSoundOn() ? "Sound On" : "Sound Off";
    }

    private void ContinueGame()
    {
        startPanel.gameObject.SetActive(false);
        GameManager.Instance.EnablePinThrowing();
    }

    private void ShowLevels()
    {
        levelsPanel.gameObject.SetActive(true);
    }
    
    private void HideLevels()
    {
        levelsPanel.gameObject.SetActive(false);
    }

    private void ShowStats()
    {
        statsPanel.gameObject.SetActive(true);
    }
    
    private void HideStats()
    {
        statsPanel.gameObject.SetActive(false);
    }

    private void LoadPlayerStats()
    {
        // Load player stats from PlayerPrefs or a file
        _lastPlayedLevel = PlayerPrefs.GetInt("LastPlayedLevel", 1);
        _pinsShot = PlayerPrefs.GetInt("PinsShot", 0);
        _pinsLanded = PlayerPrefs.GetInt("PinsLanded", 0);
        _levelsPassed = PlayerPrefs.GetInt("LevelsPassed", 0);
        _levelsFailed = PlayerPrefs.GetInt("LevelsFailed", 0);
    }

    private void UpdateStatsUI()
    {
        pinsShotText.text = "Pins Shot: " + _pinsShot;
        pinsLandedText.text = "Pins Landed: " + _pinsLanded;
        levelsPassedText.text = "Levels Passed: " + _levelsPassed;
        levelsFailedText.text = "Levels Failed: " + _levelsFailed;
    }
}