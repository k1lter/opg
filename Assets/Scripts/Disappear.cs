using UnityEngine;

public class Disappear : MonoBehaviour
{
    private float timeLeft = 5;
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
            if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Item"))
            {
                Destroy(gameObject);
            }
        }
        else if(gameObject.name == "Rocket(Clone)")
        {
            if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Item"))
            {
                Destroy(gameObject);
            }
        }
    }
}
