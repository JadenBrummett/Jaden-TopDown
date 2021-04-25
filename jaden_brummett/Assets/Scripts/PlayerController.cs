using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody2D myRB;
    private AudioSource speaker;
    public AudioClip shootSoundEffect;
    public AudioClip punchSoundEffect;

    public float speed = 10;
    public float bulletlifespan = 1;
    public float bulletSpeed = 15;
    public int playerHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        speaker = GetComponent<AudioSource>();
        playerHealth = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            transform.SetPositionAndRotation(new Vector2(), new Quaternion());
            playerHealth = 3;
        }

        Vector2 velocity = myRB.velocity;

        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        velocity.y = Input.GetAxisRaw("Vertical") * speed;

        myRB.velocity = velocity;

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameObject b = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y + 1), transform.rotation);
            b.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);

            speaker.clip = shootSoundEffect;
            speaker.Play();

            Destroy(b,bulletlifespan);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameObject b = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y - 1), transform.rotation);
            b.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);

            speaker.clip = shootSoundEffect;
            speaker.Play();

            Destroy(b,bulletlifespan);
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject b = Instantiate(bullet, new Vector2(transform.position.x - 1, transform.position.y), transform.rotation);
            b.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);

            speaker.clip = shootSoundEffect;
            speaker.Play();

            Destroy(b,bulletlifespan);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameObject b = Instantiate(bullet, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
            b.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);

            speaker.clip = shootSoundEffect;
            speaker.Play();

            Destroy(b,bulletlifespan);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "enemySprite")
        {
            // This also means playerHealth = playerHealth - 1;
            speaker.clip = punchSoundEffect;
            speaker.Play();

            playerHealth--;
        }

        else if((collision.gameObject.name == "pickup") && (playerHealth < 3))
        {
            // This also means playerHealth = playerHealth + 1;
            playerHealth++;
            collision.gameObject.SetActive(false);
        }
    }
}
