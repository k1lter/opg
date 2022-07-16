using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public float _speed;
    [SerializeField] public float _jumpForce;
    [SerializeField] private Vector3 _groundCheckOffset;
    private Rigidbody2D _rb;
    public Vector3 _direction;
    private bool _isMoving;
    public bool _isGrounded;
    private CharacterAnimations _animations;
    [SerializeField] private SpriteRenderer _characterSprite;
    private SceneChange _pause;
    private AudioClip audio_jump;
    private AudioSource audioSource;
    private GameObject[] players;


    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
        _animations = GetComponentInChildren<CharacterAnimations>();
        audioSource = GetComponent<AudioSource>();
        audio_jump = Resources.Load("Sounds/Movement/Jump") as AudioClip;
    }

    void Update()
    {
        _pause = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        if (!_pause.pause)
        {
            CheckGround();
            if (players.Length == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_isGrounded)
                    {
                        Jump();
                        _animations.Jump();
                    }
                }
                Move();
            }
            else
            {
                Move2players();
            }
        }
    }

    private void CheckGround()
    {
        float rayLength = 0.3f;
        Vector3 rayStartPosition = transform.position + _groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, Vector2.down, rayLength);
        if(hit.collider != null)
        {
            _isGrounded = hit.collider.CompareTag("Ground") ? true : false;
        }
        else
        {
            _isGrounded = false;
        }
    }    

    private bool IsFlying()
    {
        if(_rb.velocity.y < 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private void Move()
    {
        _direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        transform.position += _direction * _speed * Time.deltaTime;
        _isMoving = _direction.x != 0 ? true : false;
        if (_isMoving)
        {
            _characterSprite.flipX = _direction.x > 0 ? false : true;
        }
        _animations.IsMoving = _isMoving;
        _animations.IsFlying = IsFlying();
    }

    private void Move2players()
    {
        _direction = new Vector2(0, 0);
        if (gameObject == players[0])
        {
            if(Input.GetKey(KeyCode.A))
            {
                transform.position = transform.position + Vector3.left * _speed * Time.deltaTime;
                _direction = Vector2.left;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                transform.position = transform.position + Vector3.right * _speed * Time.deltaTime;
                _direction = Vector2.right;
            }
            if(Input.GetKeyDown(KeyCode.W))
            {
                if (_isGrounded)
                {
                    Jump();
                    _animations.Jump();
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position = transform.position + Vector3.left * _speed * Time.deltaTime;
                _direction = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position = transform.position + Vector3.right * _speed * Time.deltaTime;
                _direction = Vector2.right;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_isGrounded)
                {
                    Jump();
                    _animations.Jump();
                }
            }
        }
        _isMoving = _direction.x != 0 ? true : false;
        if (_isMoving)
        {
            _characterSprite.flipX = _direction.x > 0 ? false : true;
        }
        _animations.IsMoving = _isMoving;
        _animations.IsFlying = IsFlying();
    }

    private void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _isGrounded = false;
        audioSource.PlayOneShot(audio_jump);
    }
}
