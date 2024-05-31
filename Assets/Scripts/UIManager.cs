using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletedPanel;
    public GameObject statsPanel;
    public GameObject levelsPanel;
    
    //start panel
    public Button soundButton;
    public Button playButton;
    public Button levelsButton;
    public Button levelsBackButton;
    public Button statsButton;
    public Button statsBackButton;
    public Text currentLevelText;
    
    //Game Over Panel
    public Button restartButton;
    public Button menuButton;
    
    //Level Completed Panel
    public Button nextLevelButton;
    public Button menuButton2;
    
    private bool _playButtonPressed;
    private bool _nextLevelButtonPressed;
    private bool _restartButtonPressed;
    private bool _menuButtonPressed;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //start panel
        soundButton.onClick.AddListener(ToggleSound);
        playButton.onClick.AddListener(OnPlayButtonPressed);
        levelsButton.onClick.AddListener(ShowLevelsPanel);
        levelsBackButton.onClick.AddListener(HideLevelsPanel);
        statsButton.onClick.AddListener(ShowStatsPanel);
        statsBackButton.onClick.AddListener(HideStatsPanel);
        
        //game over panel
        restartButton.onClick.AddListener(OnRestartButtonPressed);
        menuButton.onClick.AddListener(OnMenuButtonPressed);
        
        //level completed panel
        nextLevelButton.onClick.AddListener(OnNextLevelButtonPressed);
        menuButton2.onClick.AddListener(OnMenuButtonPressed);
    }
    
    public void UpdateLevelButton(int level)
    {
        currentLevelText.text = "Level\n" + level;
    }

    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        levelCompletedPanel.SetActive(false);
        statsPanel.SetActive(false);
        levelsPanel.SetActive(false);
    }

    public void HideStartPanel()
    {
        startPanel.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowLevelCompletedPanel()
    {
        levelCompletedPanel.SetActive(true);
    }

    public void HideLevelCompletedPanel()
    {
        levelCompletedPanel.SetActive(false);
    }

    public void ShowStatsPanel()
    {
        statsPanel.SetActive(true);
    }

    public void HideStatsPanel()
    {
        statsPanel.SetActive(false);
    }
    
    private void ToggleSound()
    {
        //SoundManager.Instance.ToggleSound();
        UpdateSoundButton();
    }

    private void UpdateSoundButton()
    {
        soundButton.GetComponentInChildren<Text>().text = SoundManager.Instance.IsSoundOn() ? "Sound On" : "Sound Off";
    }
    
    public void ShowLevelsPanel()
    {
        levelsPanel.SetActive(true);
    }

    public void HideLevelsPanel()
    {
        levelsPanel.SetActive(false);
    }

    public void OnPlayButtonPressed()
    {
        _playButtonPressed = true;
    }

    public void OnRestartButtonPressed()
    {
        _restartButtonPressed = true;
    }

    public void OnNextLevelButtonPressed()
    {
        _nextLevelButtonPressed = true;
    }

    public void OnMenuButtonPressed()
    {
        _menuButtonPressed = true;
    }
    
    public bool IsNextLevelButtonPressed()
    {
        if (_nextLevelButtonPressed)
        {
            _nextLevelButtonPressed = false;
            return true;
        }
        return false;
    }
    
    public bool IsRestartButtonPressed()
    {
        if (_restartButtonPressed)
        {
            _restartButtonPressed = false;
            return true;
        }
        return false;
    }

    public bool IsPlayButtonPressed()
    {
        if (_playButtonPressed)
        {
            _playButtonPressed = false;
            return true;
        }
        return false;
    }

    public bool IsMenuButtonPressed()
    {
        if (_menuButtonPressed)
        {
            _menuButtonPressed = false;
            return true;
        }
        return false;
    }
}
