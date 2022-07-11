using UnityEngine;
using UnityEngine.UI;

public class CharStats : MonoBehaviour
{
    public int id;
    public bool speed, jump, shoot;
    [SerializeField] int hp, armor;
    private Text hp_bar, armor_bar;
    private bool pickUp;
    private Collider2D item;
    private Movement player_stats;
    private GunShoot player_gun;
    public AudioClip audioPickUp;
    private AudioSource audioSource;
    public GameObject active_gun;

    void Start()
    {
        id = 0;
        hp_bar = GameObject.Find("health").GetComponent<Text>();
        armor_bar = GameObject.Find("Armor").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        player_stats = gameObject.GetComponent<Movement>();
        hp = 10;
        armor = 0;
        hp_bar.text = "Health: " + hp;
        armor_bar.text = "Armor: " + armor;
        pickUp = false;
    }

    private void Update()
    {
        armor_bar.text = "Armor: " + armor;
        if(speed)
        {
            player_stats._speed = 8;
        }
        if(jump)
        {
            player_stats._jumpForce = 8;
        }
        if(shoot) //Не работает
        {
            if (GameObject.Find("Gun(Clone)") != null)
            {
                player_gun = GameObject.Find("Gun(Clone)").GetComponent<GunShoot>();
                player_gun.timer = 0.25f;
                shoot = false;
            }
        }
        if (hp == 0)
        {
            Debug.Log("Blin, ya umer(");
            Destroy(gameObject);
            Destroy(active_gun); //Пусть пока так будет, но надо исправить
        }
        if (Input.GetKeyDown(KeyCode.E) && pickUp == true)
        {
            if (item.gameObject.name == "Armor_item")
            {
                ArmorPickUp(item);
            }
            else if(item.gameObject.name == "Hp_item")
            {
                HpPickap(item);
            }
            else if (item.gameObject.name == "Buster_Speed")
            {
                SpeedBustPickUp(item);
            }
            else if (item.gameObject.name == "Buster_Jump")
            {
                JumpBustPickUp(item);
            }
            else if (item.gameObject.name == "Buster_Shoot")
            {
                ShootBustPickUp(item);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "Player")
        {
            if (collision.gameObject.name == "enemy_flame_test(Clone)" && hp > 0)
            {
                if (armor == 0)
                {
                    hp -= 10;
                    hp_bar.text = "Health: " + hp;
                }
                else if(armor > 0)
                {
                    armor -= 10;
                    armor = Mathf.Clamp(armor, 0, 100);
                    armor_bar.text = "Armor: " + armor;
                }
            }
            else if (collision.gameObject.name == "Armor_item")
            {
                pickUp = true;
                item = collision;
            }
            else if (collision.gameObject.name == "Hp_item")
            {
                pickUp = true;
                item = collision;
            }
            else if (collision.gameObject.name == "Buster_Speed")
            {
                pickUp = true;
                item = collision;
            }
            else if (collision.gameObject.name == "Buster_Jump")
            {
                pickUp = true;
                item = collision;
            }
            else if (collision.gameObject.name == "Buster_Shoot")
            {
                pickUp = true;
                item = collision;
            }
        }
        else if(gameObject.name == "Enemy")
        {
            if (collision.gameObject.name == "flame_test(Clone)" && hp > 0)
            {
                hp -= 10;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Armor_item")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Hp_item")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Buster_Speed")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Buster_Jump")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Buster_Shoot")
        {
            pickUp = false;
        }
    }

    private void ArmorPickUp(Collider2D item)
    {
        Destroy(item.gameObject);
        armor = 100;
        armor_bar.text = "Armor: " + armor;
        audioSource.PlayOneShot(audioPickUp);
    }

    private void HpPickap(Collider2D item)
    {
        Destroy(item.gameObject);
        hp = 100;
        hp_bar.text = "Health: " + hp;
        audioSource.PlayOneShot(audioPickUp);
    }

    private void SpeedBustPickUp(Collider2D item)
    {
        Destroy(item.gameObject);
        speed = true;
        audioSource.PlayOneShot(audioPickUp);
    }
    private void JumpBustPickUp(Collider2D item)
    {
        Destroy(item.gameObject);
        jump = true;
        audioSource.PlayOneShot(audioPickUp);
    }

    private void ShootBustPickUp(Collider2D item)
    {
        Destroy(item.gameObject);
        shoot = true;
        audioSource.PlayOneShot(audioPickUp);
    }
}
