using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public LevelData[] levels;
    public Text pinsRequiredText;
    public GameObject circle;
    public GameObject pinPrefab;
    
    private int _currentLevelIndex = 0;
    private int _pinsAttached = 0;
    private LevelData _currentLevel;

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
        SetupLevel();
    }

    private void SetupLevel()
    {
        _pinsAttached = 0;
        _currentLevel = levels[_currentLevelIndex];

        //Initialize the circle based on level data
        var rotatingCircle = circle.GetComponent<RotatingCircle>();
        rotatingCircle.speed = _currentLevel.circleSpeed;
        rotatingCircle.acceleration = _currentLevel.circleAcceleration;
        rotatingCircle.reverseDirection = _currentLevel.reverseDirection;

        for (int i = 0; i < _currentLevel.initialPinPositions.Length; i++)
        {
            Instantiate(pinPrefab, _currentLevel.initialPinPositions[i], Quaternion.identity,
                circle.transform);
        }
        
        UpdatePinsRequiredText();
    }

    public void PinAttached()
    {
        _pinsAttached++;
        UpdatePinsRequiredText();

        /*if (_pinsAttached >= _currentLevel.pinsRequired)
        {
            LevelCompleted();
        }*/
    }
    
    private void UpdatePinsRequiredText()
    {
        pinsRequiredText.text = (_currentLevel.pinsRequired - _pinsAttached).ToString();
    }

    private void LevelCompleted()
    {
        _currentLevelIndex++;
        if (_currentLevelIndex < levels.Length)
        {
            SetupLevel();
        }
        SetupLevel();
    }
}
