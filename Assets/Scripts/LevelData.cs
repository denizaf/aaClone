using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int pinsRequired;
    public float[] initialPinAngles;
    public float circleSpeed;
    public float circleAcceleration;
    public bool reverseDirection;
}
