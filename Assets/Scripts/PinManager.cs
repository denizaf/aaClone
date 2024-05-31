using System.Collections;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public static PinManager Instance { get; private set; }

    public GameObject pinPrefab;
    public Transform pinSpawnPoint;
    public float transitionDuration = 2f;
    public float pinSpeed = 20f;

    private GameObject _circle;
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

    public void Initialize(GameObject circle, LevelData currentLevel)
    {
        _circle = circle;
        _currentLevel = currentLevel;
        _originalCircleColor = _circle.GetComponent<SpriteRenderer>().color;

        SetupInitialPins();
    }

    private void SetupInitialPins()
    {
        float circleRadius = _circle.GetComponent<CircleCollider2D>().radius;
        foreach (float angle in _currentLevel.initialPinAngles)
        {
            float radian = angle * Mathf.Deg2Rad;
            Vector3 pinPosition = new Vector3(
                _circle.transform.position.x + circleRadius * Mathf.Cos(radian),
                _circle.transform.position.y + circleRadius * Mathf.Sin(radian),
                0
            );
            var initialPin = Instantiate(pinPrefab, pinPosition, Quaternion.identity, _circle.transform).GetComponent<Pin>();
            initialPin.speed = 0;
            initialPin.isInitialPin = true;
            initialPin.PlacePin(pinPosition);
        }
    }

    public void AttachPin()
    {
        _pinsAttached++;
        LevelManager.Instance.UpdatePinsRequiredText();

        if (_pinsAttached >= _currentLevel.pinsRequired)
        {
            GameManager.Instance.DisablePinThrowing();
            StartCoroutine(LevelCompletedRoutine());
        }
    }

    private IEnumerator LevelCompletedRoutine()
    {
        GameManager.Instance.DisablePinThrowing();

        ChangeColorOfCircleAndPins(Color.green);

        Camera.main.GetComponent<CameraController>().ZoomToCircle(_circle.transform.position);

        PullPinsToCircle();

        yield return new WaitForSeconds(transitionDuration);

        GameManager.Instance.ChangeState(new LevelCompletedState(GameManager.Instance));
    }

    public void ChangeColorOfCircleAndPins(Color color)
    {
        _circle.GetComponent<SpriteRenderer>().color = color;

        foreach (Transform child in _circle.transform)
        {
            var pinSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (pinSpriteRenderer != null)
            {
                pinSpriteRenderer.color = color;
            }
        }
    }

    public void ResetPins()
    {
        ClearPreviousPins();
        ResetColors();
    }

    private void ClearPreviousPins()
    {
        foreach (Transform child in _circle.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResetColors()
    {
        _circle.GetComponent<SpriteRenderer>().color = _originalCircleColor;

        foreach (Transform child in _circle.transform)
        {
            var pinSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (pinSpriteRenderer != null)
            {
                pinSpriteRenderer.color = Color.black;
            }
        }
    }

    public void PullPinsToCircle()
    {
        foreach (Transform child in _circle.transform)
        {
            StartCoroutine(MovePinToCenter(child));
        }
    }

    private IEnumerator MovePinToCenter(Transform pin)
    {
        if (pin == null) yield break; // Check if the pin is null before starting
        
        Vector3 startPosition = pin.position;
        Vector3 endPosition = _circle.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            if (pin == null) yield break; // Check if the pin is null inside the loop
            
            pin.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (pin != null) // Final check before setting the position
        {
            pin.position = endPosition;
        }
    }

    public void ThrowPin()
    {
        if (GameManager.Instance.IsGameOver() || !GameManager.Instance.CanThrowPin()) return;

        Instantiate(pinPrefab, pinSpawnPoint.position, Quaternion.identity);
        GameManager.Instance.DisablePinThrowing();
    }
}
