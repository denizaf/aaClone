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
        
        // Calculate and place initial pins based on angles
        float circleRadius = circle.GetComponent<CircleCollider2D>().radius;
        foreach (float angle in _currentLevel.initialPinAngles)
        {
            float radian = angle * Mathf.Deg2Rad; // Convert degrees to radians
            Vector3 pinPosition = new Vector3(
                circle.transform.position.x + circleRadius * Mathf.Cos(radian),
                circle.transform.position.y + circleRadius * Mathf.Sin(radian),
                0
            );
            var initialPin = Instantiate(pinPrefab, pinPosition, Quaternion.identity, circle.transform).GetComponent<Pin>();
            initialPin.speed = 0;
            initialPin.isInitialPin = true;  // Mark this pin as an initial pin
            initialPin.PlacePin(pinPosition);  // Place the pin without moving
        }

        UpdatePinsRequiredText();
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

        if (_pinsAttached >= _currentLevel.pinsRequired)
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
        ChangeColorOfCircleAndPins(Color.green);

        // Zoom towards the circle
        Camera.main.GetComponent<CameraController>().ZoomToCircle(circle.transform.position);

        // Pull pins towards the circle
        PullPinsToCircle();
        
        // Wait for the transition to complete
        yield return new WaitForSeconds(transitionDuration);

        // Proceed to the next level
        _currentLevelIndex++;
        SaveCurrentLevel();
        
        if (_currentLevelIndex >= levels.Length)
        {
            _currentLevelIndex = 0; // Loop back to the first level or handle game completion
        }
        SetupLevel();
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
    
    public void ResetLevel()
    {
        ClearPreviousPins();
        ResetColors();
        ResetCamera();
        SetupLevel();
    }
    
    private void ResetCamera()
    {
        Camera.main.GetComponent<CameraController>().ResetCamera();
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
}
