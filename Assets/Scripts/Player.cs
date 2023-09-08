using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbPlayer;
    [SerializeField] private float forceVelocity;
    [SerializeField] private float gravity;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Color[] colors = new Color[4];
    [SerializeField] private float restartTime = 1f;
    [SerializeField] private Transform circle;
    [SerializeField] private GameObject changeColor;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private GameObject star;
    [SerializeField] private Animator transitionAnimator;
    private string[] tags = new string[4];
    private int activeSceneIndex;
    private string currentColor;
    private int lastNumberColor = -1;
    private SpriteRenderer srPlayer;

    private void InitialConfiguration()
    {
        srPlayer = GetComponent<SpriteRenderer>();
        rbPlayer = GetComponent<Rigidbody2D>();
        rbPlayer.gravityScale = gravity;
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < tags.Length; i++)
            tags[i] = circle.GetChild(i).tag;
    }
    public IEnumerator SceneLoad(int add)
    {
        yield return new WaitForSeconds(restartTime / 2);
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(restartTime);
        SceneManager.LoadScene(activeSceneIndex + add);
    }
    private void ChangeColor()
    {
        int number = Random.Range(0, 4);
        while (number == lastNumberColor)
            number = Random.Range(0, 4);
        switch (number)
        {
            case 0:
                srPlayer.color = colors[0];
                currentColor = tags[0];
                break;
            case 1:
                srPlayer.color = colors[1];
                currentColor = tags[1];
                break;
            case 2:
                srPlayer.color = colors[2];
                currentColor = tags[2];
                break;
            case 3:
                srPlayer.color = colors[3];
                currentColor = tags[3];
                break;
        }
        lastNumberColor = number;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitialConfiguration();
        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            rbPlayer.velocity = Vector2.zero;
            rbPlayer.AddForce(new Vector2(0, forceVelocity));
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                rbPlayer.velocity = Vector2.zero;
                rbPlayer.AddForce(new Vector2(0, forceVelocity));
            }
        }
#endif
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(changeColor.tag))
        {
            ChangeColor();
            Destroy(collision.gameObject);
            return;
        }
        if (collision.gameObject.CompareTag(finishLine.tag))
        {
            //gameObject.SetActive(false);
            DisablePlayer();
            Instantiate(particles, transform.position, Quaternion.identity);
            if (PlayerPrefs.GetInt("level", 1) <= 9)
            {
                PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1) + 1);
                SaveManager.SaveLevelData(PlayerPrefs.GetInt("level", 1));
                StartCoroutine(SceneLoad(0));
                //Invoke("RestartScene", restartTime);
            }
            else StartCoroutine(SceneLoad(1));
            //Invoke("LoadNextScene", restartTime);
            return;
        }
        if (collision.gameObject.CompareTag(star.tag))
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            return;
        }
        if (!collision.gameObject.CompareTag(currentColor))
        {
            //gameObject.SetActive(false);
            DisablePlayer();
            Instantiate(particles, transform.position, Quaternion.identity);
            StartCoroutine(SceneLoad(0));
            //Invoke("RestartScene", restartTime);
        }
    }
    private  void DisablePlayer()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
    private void LoadNextScene()
    {
        SceneManager.LoadScene(activeSceneIndex + 1);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(activeSceneIndex);
    }
}