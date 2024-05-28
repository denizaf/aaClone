using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private bool _isSoundOn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleSound()
    {
        _isSoundOn = !_isSoundOn;
        AudioListener.volume = _isSoundOn ? 1 : 0;
    }

    public bool IsSoundOn()
    {
        return _isSoundOn;
    }
}
