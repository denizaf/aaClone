using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int pinsRequired;
    public Vector3[] initialPinPositions;
    public float circleSpeed;
    public float circleAcceleration;
    public bool reverseDirection;
}
