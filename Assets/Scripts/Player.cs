using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetMouseButtonDown(0))
        {
            rbPlayer.velocity = Vector2.zero;
            rbPlayer.AddForce(new Vector2(0, forceVelocity));
        }
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
            gameObject.SetActive(false);
            Instantiate(particles, transform.position, Quaternion.identity);
            if (PlayerPrefs.GetInt("level", 1) <= 9)
            {
                PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1) + 1);
                Invoke("RestartScene", restartTime);
            }
            else
                Invoke("LoadNextScene", restartTime);
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
            gameObject.SetActive(false);
            Instantiate(particles, transform.position, Quaternion.identity);
            Invoke("RestartScene", restartTime);
        }
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