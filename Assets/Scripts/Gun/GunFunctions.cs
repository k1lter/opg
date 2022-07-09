using UnityEngine;
using UnityEngine.UI;

public class GunFunctions : MonoBehaviour
{
    [SerializeField] private Vector3 _gunPosOffset;
    public GameObject gun;
    private Text ammo_bar;
    private GameObject gun_item;
    private SceneChange _pause;

    void Start()
    {
        _gunPosOffset = new(0.04f, -0.27f, -1);
        gun_item = Resources.Load("Prefabs/Weapons/Gun_item") as GameObject;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gun") && gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Gun")) as GameObject;
        }
        else if (collision.gameObject.CompareTag("Gun") && gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            gun = Instantiate(Resources.Load("Prefabs/Weapons/EnemyGun")) as GameObject;
        }
    }
    private void DropGun()
    {
        Vector3 err = new Vector3(2, 0, 0);
        Destroy(GameObject.Find("Gun(Clone)"));
        GameObject _gun_item = Instantiate(gun_item, gameObject.transform.position + err, Quaternion.identity);
        Rigidbody2D _girb = _gun_item.GetComponent<Rigidbody2D>();
        _girb.AddForce(gameObject.transform.right * 2, ForceMode2D.Impulse);
        ammo_bar = GameObject.Find("Ammo").GetComponent<Text>();
        ammo_bar.text = "Ammo: ?";
        gun = null;
    }
}
