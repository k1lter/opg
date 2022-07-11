using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public float _speed;
    [SerializeField] public float _jumpForce;
    [SerializeField] private Vector3 _groundCheckOffset;
    private Rigidbody2D _rb;
    private Vector3 _direction;
    private bool _isMoving;
    public bool _isGrounded;
    private CharacterAnimations _animations;
    [SerializeField] private SpriteRenderer _characterSprite;
    private SceneChange _pause;
    public AudioClip audio_jump;
    private AudioSource audioSource;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animations = GetComponentInChildren<CharacterAnimations>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        _pause = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        if (!_pause.pause)
        {
            Move();
            CheckGround();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_isGrounded)
                {
                    Jump();
                    _animations.Jump();
                }
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
        //_rb.AddForce(_direction * _speed, ForceMode2D.Force);
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
        audioSource.PlayOneShot(audio_jump);
    }
}
