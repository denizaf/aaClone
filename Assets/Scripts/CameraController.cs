using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float moveSpeed = 2f;
    public float targetOrthographicSize = 3f;
    private Vector3 _targetPosition;
    private Vector3 _originalPosition;
    private float _originalOrthographicSize;
    private bool _isZooming = false;
    private float _restartDelay = 1.5f;

    private void Start()
    {
        _originalPosition = transform.position;
        _originalOrthographicSize = Camera.main.orthographicSize;
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_isZooming)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, moveSpeed * Time.deltaTime);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f && Mathf.Abs(Camera.main.orthographicSize - targetOrthographicSize) < 0.1f)
            {
                _isZooming = false;
                // Call GameOver after a delay
                Invoke("CallGameOver", _restartDelay);
            }
        }
    }

    public void SetRestartDelay(float delay)
    {
        _restartDelay = delay;
    }
    
    public void ZoomToCollision(Vector3 collisionPoint)
    {
        _targetPosition = new Vector3(collisionPoint.x, collisionPoint.y, transform.position.z);
        _isZooming = true;
    }
    
    public void ZoomToCircle(Vector3 circlePosition)
    {
        _targetPosition = new Vector3(circlePosition.x, circlePosition.y, transform.position.z);
        _isZooming = true;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(ShakeCameraNow(duration, magnitude));
    }

    private IEnumerator ShakeCameraNow(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }
    
    private void CallGameOver()
    {
        GameManager.Instance.GameOver();
    }
    
    public void ResetCamera()
    {
        transform.position = _originalPosition;
        Camera.main.orthographicSize = _originalOrthographicSize;
    }
}
