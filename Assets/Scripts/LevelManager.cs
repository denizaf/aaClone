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
    public float transitionDuration = 2f;
    
    private int _currentLevelIndex = 0;
    private int _pinsAttached = 0;
    private LevelData _currentLevel;
    private Color _originalCircleColor = Color.white;

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

        foreach (var position in _currentLevel.initialPinPositions)
        {
            var initialPin = Instantiate(pinPrefab, position, Quaternion.identity, circle.transform);
            initialPin.GetComponent<Pin>().isInitialPin = true;
        }
        
        UpdatePinsRequiredText();
    }

    public void PinAttached()
    {
        _pinsAttached++;
        UpdatePinsRequiredText();

        if (_pinsAttached >= _currentLevel.pinsRequired)
        {
            StartCoroutine(LevelCompletedRoutine());
        }
    }

    private IEnumerator LevelCompletedRoutine()
    {
        // Change color of circle and pins to green
        ChangeColorOfCircleAndPins(Color.green);

        // Zoom towards the circle
        Camera.main.GetComponent<CameraController>().ZoomToCircle(circle.transform.position);

        // Pull pins towards the circle
        PullPinsToCircle();
        
        // Wait for the transition to complete
        yield return new WaitForSeconds(transitionDuration);

        // Proceed to the next level
        _currentLevelIndex++;
        if (_currentLevelIndex < levels.Length)
        {
            SetupLevel();
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }
    
    private void ChangeColorOfCircleAndPins(Color color)
    {
        // Change color of the circle
        circle.GetComponent<SpriteRenderer>().color = color;

        // Change color of all attached pins
        foreach (Transform child in circle.transform)
        {
            var pinSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (pinSpriteRenderer != null)
            {
                pinSpriteRenderer.color = color;
            }
        }
    }
    
    private void PullPinsToCircle()
    {
        foreach (Transform child in circle.transform)
        {
            StartCoroutine(MovePinToCenter(child));
        }
    }
    
    private IEnumerator MovePinToCenter(Transform pin)
    {
        Vector3 startPosition = pin.position;
        Vector3 endPosition = circle.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            pin.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pin.position = endPosition;
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
