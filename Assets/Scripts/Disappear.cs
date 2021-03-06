using UnityEngine;

public class Disappear : MonoBehaviour
{
    [SerializeField] float timeLeft;
    public int owner_id = -1;
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "Gun_flame(Clone)")
        {
            if (!collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Item"))
            {
                if(collision.gameObject.CompareTag("Player"))
                {
                    if(collision.gameObject.GetComponent<CharStats>().id != owner_id)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else if(gameObject.name == "Rocket(Clone)")
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (!collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Item"))
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (collision.gameObject.GetComponent<CharStats>().id != owner_id)
                    {
                        AudioClip explosion_audio = Resources.Load("Sounds/Weapons/Bazuka/Explosion") as AudioClip;
                        audioSource.PlayOneShot(explosion_audio);
                        transform.Find("Rocket_flame").gameObject.SetActive(true);
                        transform.Find("Rocket_bullet").gameObject.SetActive(false);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        GetComponent<Rigidbody2D>().simulated = false;
                    }
                }
                else
                {
                    AudioClip explosion_audio = Resources.Load("Sounds/Weapons/Bazuka/Explosion") as AudioClip;
                    audioSource.PlayOneShot(explosion_audio);
                    transform.Find("Rocket_flame").gameObject.SetActive(true);
                    transform.Find("Rocket_bullet").gameObject.SetActive(false);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
    }
}
