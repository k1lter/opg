using UnityEngine;
using UnityEngine.UI;

public class GunFunctions : MonoBehaviour
{
    [SerializeField] private Vector3 _gunPosOffset;
    public GameObject gun;
    private Text ammo_bar;
    private GameObject gun_item;
    private SceneChange _pause;
    private bool pickGun;
    private Collider2D destroyGun;
    public AudioClip gunPickUpSound;
    private AudioSource audioSource;

    void Start()
    {
        _gunPosOffset = new(0.04f, -0.27f, -1);
        gun_item = Resources.Load("Prefabs/Weapons/Gun_item") as GameObject;
        audioSource = GetComponent<AudioSource>();
        pickGun = false;
    }

    void Update()
    {
        _pause = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        if (!_pause.pause)
        {
            if (gun != null)
            {
                gun.transform.position = transform.position + _gunPosOffset;
                if (Input.GetKeyDown(KeyCode.G))
                {
                    DropGun();
                }
            }
        }
        if(pickGun == true && Input.GetKeyDown(KeyCode.E))
        {
            PickGun(destroyGun);
        }
    }

    private void PickGun(Collider2D collision)
    {
        if (gun == null)
        {
            Destroy(collision.gameObject);
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Gun")) as GameObject;
            audioSource.PlayOneShot(gunPickUpSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gun") && gameObject.CompareTag("Player"))
        {
            pickGun = true;
            destroyGun = collision;
        }
        else if (collision.gameObject.CompareTag("Gun") && gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            gun = Instantiate(Resources.Load("Prefabs/Weapons/EnemyGun")) as GameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gun") && gameObject.CompareTag("Player"))
        {
            pickGun = false;
        }
    }
    private void DropGun()
    {
        int side = 0;
        Vector2 dropVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
        if (dropVector.x < 0)
        {
            side = -1;
        }
        else
        {
            side = 1;
        }
        Vector3 dropOffset = new(1 * side, 0, 0);

        Destroy(GameObject.Find("Gun(Clone)"));
        GameObject _gun_item = Instantiate(gun_item, gameObject.transform.position + dropOffset, Quaternion.identity);
        Rigidbody2D _girb = _gun_item.GetComponent<Rigidbody2D>();
        _girb.AddForce(dropVector.normalized * 2, ForceMode2D.Impulse);
        ammo_bar = GameObject.Find("Ammo").GetComponent<Text>();
        ammo_bar.text = "Ammo: ?";
        gun = null;
    }
}
