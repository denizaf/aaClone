using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public LevelData[] levels;
    public Text pinsRequiredText;
    public Text levelText;
    public GameObject circle;
    public GameObject pinPrefab;
    public float transitionDuration = 2f;
    
    private string _levelFilePath;
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
        
        _levelFilePath = "Assets/Data/Level_Data.txt";
        LoadCurrentLevel();
    }

    private void Start()
    {
        SetupLevel();
    }

    public int GetCurrentLevelIndex()
    {
        return _currentLevelIndex;
    }
    
    private void LoadCurrentLevel()
    {
        if (File.Exists(_levelFilePath))
        {
            string levelData = File.ReadAllText(_levelFilePath);
            int.TryParse(levelData, out _currentLevelIndex);
        }
        else
        {
            _currentLevelIndex = 0; // Default to level 0 if file does not exist
        }
    }
    
    private void SaveCurrentLevel()
    {
        File.WriteAllText(_levelFilePath, _currentLevelIndex.ToString());
    }

    private void SetupLevel()
    {
        _pinsAttached = 0;
        _currentLevel = levels[_currentLevelIndex];
        levelText.text = ("Level\n" + _currentLevel.levelNumber);
        
        //Initialize the circle based on level data
        var rotatingCircle = circle.GetComponent<RotatingCircle>();
        rotatingCircle.speed = _currentLevel.circleSpeed;
        rotatingCircle.acceleration = _currentLevel.circleAcceleration;
        rotatingCircle.reverseDirection = _currentLevel.reverseDirection;
        rotatingCircle.StartRotation();
        
        PinManager.Instance.Initialize(circle, _currentLevel);

        UpdatePinsRequiredText();
    }
    
    public bool IsGameOver()
    {
        return GameManager.Instance.CheckGameOverCondition();
    }

    public bool IsLevelCompleted()
    {
        return _pinsAttached >= _currentLevel.pinsRequired;
    }
    
    private void ClearPreviousPins()
    {
        foreach (Transform child in circle.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResetColors()
    {
        circle.GetComponent<SpriteRenderer>().color = _originalCircleColor;
        
        foreach (Transform child in circle.transform)
        {
            var pinSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (pinSpriteRenderer != null)
            {
                pinSpriteRenderer.color = Color.black;
            }
        }
    }

    public void PinAttached()
    {
        _pinsAttached++;
        UpdatePinsRequiredText();

        if (IsLevelCompleted())
        {
            GameManager.Instance.DisablePinThrowing();
            StartCoroutine(LevelCompletedRoutine());
        }
    }

    private IEnumerator LevelCompletedRoutine()
    {
        // Disable pin throwing
        GameManager.Instance.DisablePinThrowing();
        
        // Change color of circle and pins to green
        PinManager.Instance.ChangeColorOfCircleAndPins(Color.green);

        // Zoom towards the circle
        Camera.main.GetComponent<CameraController>().ZoomToCircle(circle.transform.position);

        // Pull pins towards the circle
        PinManager.Instance.PullPinsToCircle();
        
        // Wait for the transition to complete
        yield return new WaitForSeconds(transitionDuration);
        
        GameManager.Instance.ChangeState(new LevelCompletedState(GameManager.Instance));
    }
    
    
    public void ResetLevel()
    {
        PinManager.Instance.ResetPins();
        ResetCamera();
        SetupLevel();
    }
    
    private void ResetCamera()
    {
        Camera.main.GetComponent<CameraController>().ResetCamera();
    }
    
    public void UpdatePinsRequiredText()
    {
        pinsRequiredText.text = (_currentLevel.pinsRequired - _pinsAttached).ToString();
    }
    
    public void LoadNextLevel()
    {
        _currentLevelIndex++;
        SaveCurrentLevel();

        if (_currentLevelIndex >= levels.Length)
        {
            _currentLevelIndex = 0; // Loop back to the first level or handle game completion
        }

        ResetLevel();
    }
}
