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
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
