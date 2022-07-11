using UnityEngine;

public class GunFunctions : MonoBehaviour
{
    private Collider2D active_player;
    [SerializeField] private Vector3 _gunPosOffset;
    public GameObject gun;
    private bool pickGun;
    private AudioClip gun_pickup_audio;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gun_pickup_audio = Resources.Load("Sounds/Action/PickUp") as AudioClip;
        pickGun = false;
    }

    void Update()
    {
        if(pickGun == true && Input.GetKeyDown(KeyCode.E))
        {
            PickGun();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Item"))
        {
            if (collision.gameObject.GetComponent<CharStats>().active_gun == null)
            {
                active_player = collision;
                pickGun = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            active_player = null;
            pickGun = false;
        }
    }

    private void PickGun()
    {
        if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.pistol)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Pistol/Pistol")) as GameObject;
            gun.name = "Pistol";
        }
        else if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.shotgun)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Pistol/Pistol")) as GameObject;
        }
        else if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.sniper)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Pistol/Pistol")) as GameObject;
        }
        else if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.minigun)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Pistol/Pistol")) as GameObject;
        }
        else if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.bazuka)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Pistol/Pistol")) as GameObject;
        }
        active_player.gameObject.GetComponent<CharStats>().active_gun = gun;
        audioSource.PlayOneShot(gun_pickup_audio);
        Destroy(gameObject);
    }
}
