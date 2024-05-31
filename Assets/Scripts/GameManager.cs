using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //public GameObject pinPrefab;
    //public Transform pinSpawnPoint;
    public float restartDelay = 1.5f;
    public float cameraShakeDuration = 0.5f;
    public float cameraShakeMagnitude = 0.1f;
    
    private bool _isGameOver = false;
    private bool _canThrowPin = false;
    private GameStateController _stateController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _stateController = new GameStateController();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(new MenuState(this));
    }
    
    private void Update()
    {
        _stateController.Update();
    }
    
    public void ChangeState(IGameState newState)
    {
        _stateController.ChangeState(newState);
    }

    public void PinLanded()
    {
        _canThrowPin = true;
    }

    public void DisablePinThrowing()
    {
        _canThrowPin = false;
    }

    public void EnablePinThrowing()
    {
        _canThrowPin = true;
    }
    
    public bool CanThrowPin()
    {
        return _canThrowPin;
    }

    public void GameOver()
    {
        //Display game over UI
        //Handle game over Logic
        Debug.Log("Game over.");
        DisplayGameOverUI();
        Invoke("RestartLevel", restartDelay);
    }

    public void CollisionOccurred(Vector2 collisionPoint)
    {
        if (_isGameOver) return;
        _isGameOver = true;
        
        GameObject.FindGameObjectWithTag("Circle").GetComponent<RotatingCircle>().StopRotation();
        Camera.main.GetComponent<CameraController>().SetRestartDelay(restartDelay);
        Camera.main.GetComponent<CameraController>().ZoomToCollision(collisionPoint);
        Camera.main.GetComponent<CameraController>().ShakeCamera(cameraShakeDuration, cameraShakeMagnitude);
    }
    
    public void RestartLevel()
    {
        _isGameOver = false;
        _canThrowPin = true;
        LevelManager.Instance.ResetLevel();
        ChangeState(new PlayingState(this));
    }
    
    public bool CheckGameOverCondition()
    {
        return _isGameOver;
    }

    public bool CheckLevelCompletedCondition()
    {
        return LevelManager.Instance.IsLevelCompleted();
    }

    public bool CheckRestartCondition()
    {
        return UIManager.Instance.IsRestartButtonPressed();
    }

    public bool CheckNextLevelCondition()
    {
        return UIManager.Instance.IsNextLevelButtonPressed();
    }
    
    public bool CheckBackToMenuCondition()
    {
        return UIManager.Instance.IsMenuButtonPressed();
    }

    public void DisplayGameOverUI()
    {
        UIManager.Instance.ShowGameOverPanel();
    }

    public void HideGameOverUI()
    {
        UIManager.Instance.HideGameOverPanel();
    }

    public void DisplayLevelCompletedUI()
    {
        UIManager.Instance.ShowLevelCompletedPanel();
    }

    public void HideLevelCompletedUI()
    {
        UIManager.Instance.HideLevelCompletedPanel();
    }

    public void LoadNextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
        ChangeState(new PlayingState(this));
    }
    
    public void SavePlayerStats()
    {
        PlayerPrefs.SetInt("LastPlayedLevel", LevelManager.Instance.GetCurrentLevelIndex());
        /*PlayerPrefs.SetInt("PinsShot", pinsShot);
        PlayerPrefs.SetInt("PinsLanded", pinsLanded);
        PlayerPrefs.SetInt("LevelsPassed", levelsPassed);
        PlayerPrefs.SetInt("LevelsFailed", levelsFailed);
        PlayerPrefs.Save();*/
    }

    private void OnApplicationQuit()
    {
        SavePlayerStats();
    }

    public void GoToStartPage()
    {
        ChangeState(new MenuState(this));
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}
