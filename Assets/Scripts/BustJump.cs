using UnityEngine;

public class BustJump : MonoBehaviour
{
    [SerializeField] float bust_force;
    private bool in_zone = false;
    private Collider2D player;

    void Update()
    {
        if(in_zone && player.gameObject.GetComponent<Movement>()._isGrounded)
        {
            player.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bust_force, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            in_zone = true;
            player = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        in_zone = false;
    }
}
