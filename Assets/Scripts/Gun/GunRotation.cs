using UnityEngine;


//????? ??? ???? ?????? ????? ?????????, ?? ??? ????? 
public class GunRotation : MonoBehaviour
{
    private SceneChange _pause;
    private enum Side
    {
        Left = -1,
        Right = 1
    }

    //????????????? ?????
    [SerializeField] private SpriteRenderer _gunSprite;
    private Vector2 _one;
    private Vector2 _two;
    private Vector3 gun_offset = new(0, -0.25f, -1);
    private Transform _transform;
    private GameObject owner;
    private GameObject[] players;
    public bool gun_turn_left = false;

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _one = Vector2.right;
        _transform = GetComponent<Transform>();
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<CharStats>().id == GetComponent<Gun>().owner_id)
            {
                owner = players[i];
            }
        }
    }

    private void Update()
    {
        _pause = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        transform.position = owner.transform.position + gun_offset;
        if (!_pause.pause)
        {
            if (!owner.GetComponent<CharStats>().two_players)
            {
                float z = GetRotate();
                _transform.rotation = Quaternion.Euler(0, 0, z);
                flipSprite();
            }
            else
            {
                if(owner.GetComponent<Movement>()._direction.x > 0)
                {
                    gun_turn_left = false;
                }
                else if(owner.GetComponent<Movement>()._direction.x < 0)
                {
                    gun_turn_left = true;
                }
                if (gun_turn_left)
                {
                    float z = 180;
                    _transform.rotation = Quaternion.Euler(0, 0, z);
                    flipSprite();
                }
                else
                {
                    float z = 0;
                    _transform.rotation = Quaternion.Euler(0, 0, z);
                    flipSprite();
                }
            }
        }
    }

    private void flipSprite()
    {
        _gunSprite.flipY = transform.rotation.z < 0.7f && transform.rotation.z  > -0.7f ? false : true;
    }

    private float GetRotate() //??????? ??????? ???? ????? ?????????? ? ????????
    {
        _two = _camera.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
        float scalarResult = _one.x * _two.x + _one.y * _two.y;
        float absResult = _one.magnitude * _two.magnitude;
        float divition = scalarResult / absResult;
        return Mathf.Acos(divition) * Mathf.Rad2Deg * (int)getSide();
    }

    private void OnDrawGizmos() //??????? ?????? ????? ??? ????????
    {
        if(_transform != null)
        {
            Gizmos.DrawLine(_transform.position, _one * 10);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_transform.position, _camera.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private Side getSide() //????? ????? ????????? ??? ? ????????? ??????
    {
        Side side = Side.Right;
        if(_two.y <= _one.y)
        {
            side = Side.Left;
        }
        return side;
    }
}
