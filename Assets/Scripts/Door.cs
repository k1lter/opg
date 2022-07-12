using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    public bool opened = false;
    public bool near_door = false;
    [SerializeField] float timer;
    private float timeLeft = 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && near_door)
        {
            opened = true;
            _animator.SetBool("Opened", opened);
        }
        if(opened)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft<0)
            {
                opened = false;
                _animator.SetBool("Opened", opened);
                timeLeft = timer;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            near_door = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            near_door = false;
        }
    }
}
