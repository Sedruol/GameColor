using System.Collections;
using System.Collections.Generic;
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
    private int activeSceneIndex;
    private string currentColor;
    private SpriteRenderer srPlayer;

    // Start is called before the first frame update
    void Start()
    {
        srPlayer = GetComponent<SpriteRenderer>();
        rbPlayer = GetComponent<Rigidbody2D>();
        rbPlayer.gravityScale = gravity;
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        if (collision.gameObject.CompareTag("ColorChanger"))
        {
            ChangeColor();
            Destroy(collision.gameObject);
            return;
        }
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            gameObject.SetActive(false);
            Instantiate(particles, transform.position, Quaternion.identity);
            Invoke("LoadNextScene", restartTime);
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

    private void ChangeColor()
    {//despues puedo pensar en alguna forma para que no repita el color y en alguna otra forma de obtener los tags
        int number = Random.Range(0, 4);
        switch (number)
        {
            case 0:
                srPlayer.color = colors[0];
                currentColor = "Orange";
                break;
            case 1:
                srPlayer.color = colors[1];
                currentColor = "Violet";
                break;
            case 2:
                srPlayer.color = colors[2];
                currentColor = "Cyan";
                break;
            case 3:
                srPlayer.color = colors[3];
                currentColor = "Pink";
                break;
        }

    }
}