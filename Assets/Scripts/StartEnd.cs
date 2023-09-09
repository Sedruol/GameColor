using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartEnd : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Button btnExit;
    [SerializeField] private bool start = false;
    [SerializeField] private bool end = false;
    [SerializeField] private string PlayerPrefUnlocked = "unlocked";
    [SerializeField] private Transform lvlsButtonsParent;
    [SerializeField] private Animator transitionAnimator;
    [HideInInspector] private int lastLevel;
    public IEnumerator SceneLoad()
    {
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void Start()
    {
        //SaveManager.SaveLevelData(1);
        //PlayerPrefs.SetInt(PlayerPrefUnlocked, 1);
        //PlayerPrefs.SetInt("level", 1);
        if (start)
        {
            for (int x = 1; x < lvlsButtonsParent.childCount; x++)
            {
                lvlsButtonsParent.GetChild(x).GetComponent<Button>().enabled = false;
                lvlsButtonsParent.GetChild(x).GetComponent<Image>().color = new Color(1f, 1f, 1f, 50f/255f);
            }
            Data data = SaveManager.LoadLevelData();
            if (data != null)
            {
                lastLevel = data.levelsUnlocked;
                if (lastLevel > PlayerPrefs.GetInt(PlayerPrefUnlocked, 1))
                    PlayerPrefs.SetInt(PlayerPrefUnlocked, lastLevel);
                Debug.Log(PlayerPrefUnlocked + PlayerPrefs.GetInt(PlayerPrefUnlocked, 1));
                for (int y = 1; y < PlayerPrefs.GetInt(PlayerPrefUnlocked, 1); y++)
                {
                    lvlsButtonsParent.GetChild(y).GetComponent<Button>().enabled = true;
                    lvlsButtonsParent.GetChild(y).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            button.onClick.AddListener(() => StartGame());
            btnExit.onClick.AddListener(() => CloseGame());
        }
        else if (end) button.onClick.AddListener(() => CloseGame());
    }

    public void SelectLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
        Debug.Log("Level: " + PlayerPrefs.GetInt("level", 1));
    }
    private void StartGame()
    {
        StartCoroutine(SceneLoad());
    }
    private void CloseGame()
    {
        Application.Quit();
    }
}