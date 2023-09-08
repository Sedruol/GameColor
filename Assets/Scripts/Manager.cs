using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private Button home;
    [SerializeField] private List<LevelData> listLevelData = new List<LevelData>();
    [SerializeField] private Animator transitionAnimator;
    public IEnumerator SceneLoad()
    {
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    private void Awake()
    {
        Instantiate(listLevelData[PlayerPrefs.GetInt("level", 1) - 1].GetLevel);
    }
    private void Start()
    {
        home.onClick.AddListener(() => GoHome());
    }
    private void GoHome()
    {
        StartCoroutine(SceneLoad());
    }
}