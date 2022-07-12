using UnityEngine;

public class GunFunctions : MonoBehaviour
{
    [SerializeField] private Vector3 _gunPosOffset;
    private Collider2D active_player;
    private AudioClip gun_pickup_audio;
    private AudioSource audioSource;
    private bool pickGun;
    public GameObject gun;

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

    private void OnTriggerStay2D(Collider2D collision)
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
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Shotgun/Shotgun")) as GameObject;
            gun.name = "Shotgun";
        }
        else if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.sniper)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Sniper/Sniper")) as GameObject;
            gun.name = "Sniper";
        }
        else if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.minigun)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Minigun/Minigun")) as GameObject;
            gun.name = "Minigun";
        }
        else if (GetComponent<Gun_item_stats>().gun_Type == Gun_item_stats.gun_Types.bazuka)
        {
            gun = Instantiate(Resources.Load("Prefabs/Weapons/Bazuka/Bazuka")) as GameObject;
            gun.name = "Bazuka";
        }
        active_player.gameObject.GetComponent<CharStats>().active_gun = gun;
        gun.GetComponent<Gun>().owner_id = active_player.gameObject.GetComponent<CharStats>().id;
        audioSource.PlayOneShot(gun_pickup_audio);
        gun.GetComponent<Gun>().ammo_gun = GetComponent<Gun_item_stats>().ammo_gun;
        gun.GetComponent<Gun>().ammo_inv = GetComponent<Gun_item_stats>().ammo_inv;
        Destroy(gameObject);
    }
}
