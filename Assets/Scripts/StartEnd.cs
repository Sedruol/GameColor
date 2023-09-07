using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartEnd : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private bool start = false;
    [SerializeField] private bool end = false;
    private void Start()
    {
        if (start) button.onClick.AddListener(() => StartGame());
        else if (end) button.onClick.AddListener(() => CloseGame());
    }
    public void SelectLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
        Debug.Log("Level: " + PlayerPrefs.GetInt("level", 1));
    }
    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void CloseGame()
    {
        Application.Quit();
    }
}