using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject pinPrefab;
    public Transform pinSpawnPoint;
    public float restartDelay = 1.5f;
    public float cameraShakeDuration = 0.5f;
    public float cameraShakeMagnitude = 0.1f;
    
    private bool _isGameOver = false;
    private bool _canThrowPin = false;

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
        _canThrowPin = false;
    }

    private void Update()
    {
        if (_isGameOver || !_canThrowPin) return;
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(pinPrefab, pinSpawnPoint.position, Quaternion.identity);
            _canThrowPin = false;
        }
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

    public void GameOver()
    {
        //Display game over UI
        //Handle game over Logic
        Debug.Log("Game over.");
        
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
    
    private void RestartLevel()
    {
        _isGameOver = false;
        _canThrowPin = true;
        LevelManager.Instance.ResetLevel();
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

}
