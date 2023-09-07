using UnityEngine;
[CreateAssetMenu(fileName = "New Level Data", menuName = "ScriptableObject/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private GameObject level;
    public GameObject GetLevel { get { return level; } }
}