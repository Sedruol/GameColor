using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private List<LevelData> listLevelData = new List<LevelData>();
    private void Awake()
    {
        Instantiate(listLevelData[PlayerPrefs.GetInt("level", 1) - 1].GetLevel);
    }
}