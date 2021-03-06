using UnityEngine;
using UnityEngine.UI;

public class CharStats : MonoBehaviour
{
    private GameObject[] players;
    public int id = -1, max_id = 0;
    public bool speed, jump;
    [SerializeField] int hp, armor;
    private Text hp_bar, armor_bar, ammo_bar, speed_bar, jump_bar;
    public GameObject victory_menu;
    private bool pickUp;
    private Collider2D item;
    private Movement player_stats;
    public AudioClip audioPickUp;
    private AudioSource audioSource;
    public GameObject active_gun;
    private int ammo_gun, ammo_gun_max, ammo_inventory;
    [SerializeField] float timer_jump, timer_speed;
    public float timeLeftJump, timeLeftSpeed;
    public bool two_players = false;
    private SceneChange _pause;

    void Start()
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length > 1)
        {
            two_players = true;
        }
        player_stats = gameObject.GetComponent<Movement>();
        _pause = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        timeLeftJump = timer_jump;
        timeLeftSpeed = timer_speed;
        players = GameObject.FindGameObjectsWithTag("Player");
        if (two_players)
        {
            if (gameObject == players[0])
            {
                hp_bar = GameObject.Find("Health_0").GetComponent<Text>();
                armor_bar = GameObject.Find("Armor_0").GetComponent<Text>();
                ammo_bar = GameObject.Find("Ammo_0").GetComponent<Text>();
                jump_bar = GameObject.Find("Jump_0").GetComponent<Text>();
                speed_bar = GameObject.Find("Speed_0").GetComponent<Text>();
            }
            else
            {
                hp_bar = GameObject.Find("Health_1").GetComponent<Text>();
                armor_bar = GameObject.Find("Armor_1").GetComponent<Text>();
                ammo_bar = GameObject.Find("Ammo_1").GetComponent<Text>();
                jump_bar = GameObject.Find("Jump_1").GetComponent<Text>();
                speed_bar = GameObject.Find("Speed_1").GetComponent<Text>();
            }
        }
        else
        {
            hp_bar = GameObject.Find("Health_0").GetComponent<Text>();
            armor_bar = GameObject.Find("Armor_0").GetComponent<Text>();
            ammo_bar = GameObject.Find("Ammo_0").GetComponent<Text>();
            jump_bar = GameObject.Find("Jump_0").GetComponent<Text>();
            speed_bar = GameObject.Find("Speed_0").GetComponent<Text>();
        }
        audioPickUp = Resources.Load("Sounds/Action/PickUp") as AudioClip;
        Give_Id_Player(players);
        audioSource = GetComponent<AudioSource>();
        hp = 10;
        armor = 0;
        hp_bar.text = "Health: " + hp;
        armor_bar.text = "Armor: " + armor;
        pickUp = false;
    }

    private void Update()
    {
        if (jump)
        {
            timeLeftJump -= Time.deltaTime;
            jump_bar.text = "Jump: " + (int)timeLeftJump;
            if (timeLeftJump < 0)
            {
                jump = false;
                timeLeftJump = timer_jump;
            }
        }
        if (speed)
        {
            timeLeftSpeed -= Time.deltaTime;
            speed_bar.text = "Speed: " + (int)timeLeftSpeed;
            if (timeLeftSpeed < 0)
            {
                speed = false;
                timeLeftSpeed = timer_speed;
            }
        }
        if (FindActiveGun() != null)
        {
            ammo_gun = FindActiveGun().GetComponent<Gun>().ammo_gun;
            ammo_gun_max = FindActiveGun().GetComponent<Gun>().ammo_gun_max;
            ammo_inventory = FindActiveGun().GetComponent<Gun>().ammo_inv;
            ammo_bar.text = "Ammo: " + ammo_gun + "/" + ammo_gun_max + ". Ammo left: " + ammo_inventory;
        }
        else
        {
            ammo_bar.text = "Ammo: ?";
        }
        armor_bar.text = "Armor: " + armor;
        if (speed)
        {
            player_stats._speed = 8;
        }
        else
        {
            player_stats._speed = 5;
        }
        if(jump)
        {
            player_stats._jumpForce = 8;
        }
        else
        {
            player_stats._jumpForce = 5;
        }

        if (hp == 0)
        {
            if (players.Length != 1)
            {
                _pause.pause = true;
                victory_menu.SetActive(true);
                if (gameObject != players[0])
                {
                    victory_menu.transform.Find("Victory_text").GetComponent<Text>().text = "Player 1 Win!";
                }
                else
                {
                    victory_menu.transform.Find("Victory_text").GetComponent<Text>().text = "Player 2 Win!";
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && pickUp == true && gameObject == players[0])
        {
            if (item.gameObject.name == "Armor_item(Clone)")
            {
                ArmorPickUp(item);
            }
            else if(item.gameObject.name == "Hp_item(Clone)")
            {
                HpPickap(item);
            }
            else if (item.gameObject.name == "Buster_Speed(Clone)")
            {
                SpeedBustPickUp(item);
            }
            else if (item.gameObject.name == "Buster_Jump(Clone)")
            {
                JumpBustPickUp(item);
            }
        }
        else if(Input.GetKeyDown(KeyCode.End) && pickUp == true && gameObject == players[1])
        {
            if (item.gameObject.name == "Armor_item(Clone)")
            {
                ArmorPickUp(item);
            }
            else if (item.gameObject.name == "Hp_item(Clone)")
            {
                HpPickap(item);
            }
            else if (item.gameObject.name == "Buster_Speed(Clone)")
            {
                SpeedBustPickUp(item);
            }
            else if (item.gameObject.name == "Buster_Jump(Clone)")
            {
                JumpBustPickUp(item);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.name == "Gun_flame(Clone)" && collision.gameObject.GetComponent<Disappear>().owner_id != id && hp > 0)
            {
                if (armor == 0)
                {
                    hp -= 10;
                    hp = Mathf.Clamp(hp, 0, 100);
                    hp_bar.text = "Health: " + hp;
                }
                else if (armor > 0)
                {
                    armor -= 10;
                    armor = Mathf.Clamp(armor, 0, 100);
                    armor_bar.text = "Armor: " + armor;
                }
            }
            else if(collision.gameObject.name == "Rocket(Clone)" && hp > 0)
            {
                if (armor == 0)
                {
                    hp -= 80;
                    hp = Mathf.Clamp(hp, 0, 100);
                    hp_bar.text = "Health: " + hp;
                }
                else if (armor > 0)
                {
                    armor -= 80;
                    armor = Mathf.Clamp(armor, 0, 100);
                    armor_bar.text = "Armor: " + armor;
                }
            }
            else if (collision.gameObject.name == "Armor_item(Clone)")
            {
                pickUp = true;
                item = collision;
            }
            else if (collision.gameObject.name == "Hp_item(Clone)")
            {
                pickUp = true;
                item = collision;
            }
            else if (collision.gameObject.name == "Buster_Speed(Clone)")
            {
                pickUp = true;
                item = collision;
            }
            else if (collision.gameObject.name == "Buster_Jump(Clone)")
            {
                pickUp = true;
                item = collision;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Armor_item(Clone)")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Hp_item(Clone)")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Buster_Speed(Clone)")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Buster_Jump(Clone)")
        {
            pickUp = false;
        }
        else if (collision.gameObject.name == "Buster_Shoot(Clone)")
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
        timeLeftSpeed = timer_speed;
        speed = true;
        audioSource.PlayOneShot(audioPickUp);
    }
    private void JumpBustPickUp(Collider2D item)
    {
        Destroy(item.gameObject);
        timeLeftJump = timer_jump;
        jump = true;
        audioSource.PlayOneShot(audioPickUp);
    }

    private void Give_Id_Player(GameObject[] objects)
    {
        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].GetComponent<CharStats>().id == -1)
            {
                objects[i].GetComponent<CharStats>().id = max_id;
                max_id += 1;
            }
        }
    }

    public GameObject FindById(GameObject[] objects, int id)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<CharStats>().id == id)
            {
                return objects[i];
            }
        }
        return null;
    }

    private GameObject FindActiveGun()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Gun");
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<Gun>().owner_id == id)
            {
                return objects[i];
            }
        }
        return null;
    }
}
