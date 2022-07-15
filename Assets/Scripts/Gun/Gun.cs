using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum ammo_Types //Вид патронов
    {
        pistol_ammo = 0,
        shotgun_ammo = 1,
        sniper_ammo = 2,
        minigun_ammo = 3,
        bazuka_rocket = 4
    }

    public enum gun_Types //Вид оружия
    {
        pistol = 0,
        shotgun = 1,
        sniper = 2,
        minigun = 3,
        bazuka = 4
    }

    private SceneChange _pause;

    //Звук
    private AudioSource audioSource;
    private AudioClip shoot_audio, no_ammo_audio, reload_audio;


    //Переменные которые настраивают оружие
    [SerializeField] gun_Types gun_Type; //ID вида оружия
    [SerializeField] int amount_of_ammo_inv; //Количество патронов в инвентаре
    [SerializeField] int amount_of_ammo_gun; //Количество патронов в магазине
    [SerializeField] float firing_frequency; //Частота стрельбы
    [SerializeField] float damage; //Урон выстрела
    [SerializeField] ammo_Types ammo_Type; //ID вида патронов

    public int owner_id;
    public int ammo_gun, ammo_inv, ammo_gun_max;
    private float timeLeft = 0;
    private GameObject barrel;
    private GameObject bullet;
    private GameObject gun_item;
    private Rigidbody2D _rb_bullet;
    private CharStats Find;
    private GameObject[] players;


    void Start()
    {
        ammo_gun_max = amount_of_ammo_gun;
        players = GameObject.FindGameObjectsWithTag("Player");
        Find = GameObject.FindGameObjectWithTag("Player").GetComponent<CharStats>();
        audioSource = GetComponent<AudioSource>();
        no_ammo_audio = Resources.Load("Sounds/Weapons/Pistol/NoAmmo") as AudioClip;

        //**********Выбор текстуры снаряда в зависимости от вида патрона
        if (ammo_Type != ammo_Types.bazuka_rocket)
        {
            bullet = Resources.Load("Prefabs/Weapons/Gun_flame") as GameObject;
        }
        else
        {
            bullet = Resources.Load("Prefabs/Weapons/Rocket") as GameObject; 
        }
        //***********


        //*************Выбор переменных для вида оружия !!!Готовы не все!!!
        if(ammo_Type == ammo_Types.pistol_ammo)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Pistol/Shoot_Pistol") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Pistol/Reload") as AudioClip;
        }
        else if (ammo_Type == ammo_Types.minigun_ammo)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Minigun/Shoot_Minigun") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Pistol/Reload") as AudioClip;
        }
        else if(ammo_Type == ammo_Types.shotgun_ammo)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Shotgun/Shoot_Shotgun") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Shotgun/Reload_Shotgun") as AudioClip;
        }
        else if (ammo_Type == ammo_Types.sniper_ammo)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Sniper/Shoot_Sniper") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Pistol/Reload") as AudioClip;
        }
        else if (ammo_Type == ammo_Types.bazuka_rocket)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Bazuka/Shoot_Bazuka") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Bazuka/Reload_Bazuka") as AudioClip;
        }
        //*****************************


        barrel = gameObject.transform.Find("Barrel").gameObject; //Дуло откуда летит снаряд

    }

    void Update()
    {
        _pause = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        if (!_pause.pause)
        {
            timeLeft -= Time.deltaTime;
            if (!Find.two_players)
            {
                if (Input.GetKey(KeyCode.Mouse0) && ammo_gun > 0)
                {
                    if (timeLeft < 0)
                    {
                        Shoot();
                        timeLeft = firing_frequency;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Mouse0) && ammo_gun == 0)
                {
                    audioSource.PlayOneShot(no_ammo_audio);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Reload();
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    DropGun();
                }
            }
            else
            {
                if(Find.FindById(players,owner_id) == players[0])
                {
                    if (Input.GetKey(KeyCode.S) && ammo_gun > 0)
                    {
                        if (timeLeft < 0)
                        {
                            Shoot();
                            timeLeft = firing_frequency;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.S) && ammo_gun == 0)
                    {
                        audioSource.PlayOneShot(no_ammo_audio);
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        Reload();
                    }
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        DropGun();
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.DownArrow) && ammo_gun > 0)
                    {
                        if (timeLeft < 0)
                        {
                            Shoot();
                            timeLeft = firing_frequency;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow) && ammo_gun == 0)
                    {
                        audioSource.PlayOneShot(no_ammo_audio);
                    }
                    if (Input.GetKeyDown(KeyCode.RightControl))
                    {
                        Reload();
                    }
                    if (Input.GetKeyDown(KeyCode.RightShift))
                    {
                        DropGun();
                    }
                }
            }
        }
    }

    private void Shoot()
    {
        switch(ammo_Type)
        {
            case ammo_Types.pistol_ammo:
                ShootPistol();
                break;
            case ammo_Types.minigun_ammo:
                ShootMinigun();
                break;
            case ammo_Types.sniper_ammo:
                ShootSniper();
                break;
            case ammo_Types.shotgun_ammo:
                ShootShotgun();
                break;
            case ammo_Types.bazuka_rocket:
                ShootBazuka();
                break;
        }
    }

    private void Reload()
    {
        int ammo_need = amount_of_ammo_gun - ammo_gun;
        if (ammo_inv >= ammo_need)
        {
            ammo_gun += ammo_need;
            ammo_inv -= ammo_need;
        }
        else
        {
            ammo_gun += ammo_inv;
            ammo_inv = 0;
        }
        audioSource.PlayOneShot(reload_audio);
    }

    private void ShootPistol()
    {
        GameObject realBullet = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        _rb_bullet = realBullet.GetComponent<Rigidbody2D>();
        _rb_bullet.AddForce(barrel.transform.right * 35, ForceMode2D.Impulse);
        audioSource.PlayOneShot(shoot_audio);
        ammo_gun -= 1;
    }
    private void ShootMinigun()
    {
        GameObject realBullet = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        _rb_bullet = realBullet.GetComponent<Rigidbody2D>();
        _rb_bullet.AddForce(barrel.transform.right * 45, ForceMode2D.Impulse);
        audioSource.PlayOneShot(shoot_audio);
        ammo_gun -= 1;
    }

    private void ShootSniper()
    {
        GameObject realBullet = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        _rb_bullet = realBullet.GetComponent<Rigidbody2D>();
        _rb_bullet.AddForce(barrel.transform.right * 60, ForceMode2D.Impulse);
        audioSource.PlayOneShot(shoot_audio);
        ammo_gun -= 1;
    }

    private void ShootShotgun()
    {
        Vector3 up_bullet = new Vector2(0, 1.5f);
        GameObject realBullet_0 = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        GameObject realBullet_1 = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        GameObject realBullet_2 = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        Rigidbody2D _rb_bullet_0 = realBullet_0.GetComponent<Rigidbody2D>();
        Rigidbody2D _rb_bullet_1 = realBullet_1.GetComponent<Rigidbody2D>();
        Rigidbody2D _rb_bullet_2 = realBullet_2.GetComponent<Rigidbody2D>();
        _rb_bullet_0.AddForce(barrel.transform.right * 35, ForceMode2D.Impulse);
        _rb_bullet_1.AddForce(barrel.transform.right * 35 + up_bullet, ForceMode2D.Impulse);
        _rb_bullet_2.AddForce(barrel.transform.right * 35 + (up_bullet * -1), ForceMode2D.Impulse);
        audioSource.PlayOneShot(shoot_audio);
        ammo_gun -= 1;
    }

    private void ShootBazuka()
    {
        GameObject realBullet = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        _rb_bullet = realBullet.GetComponent<Rigidbody2D>();
        _rb_bullet.AddForce(barrel.transform.right * 25, ForceMode2D.Impulse);
        audioSource.PlayOneShot(shoot_audio);
        ammo_gun -= 1;
    }

    private void DropGun()
    {
        int side;
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

        string new_name;
        new_name = "Pistol_item";

        if (gun_Type == gun_Types.pistol)
        {
            gun_item = Resources.Load("Prefabs/Weapons/Items/Pistol_item") as GameObject;
        }
        else if (gun_Type == gun_Types.shotgun)
        {
            gun_item = Resources.Load("Prefabs/Weapons/Items/Shotgun_item") as GameObject;
            new_name = "Shotgun_item";
        }
        else if (gun_Type == gun_Types.minigun)
        {
            gun_item = Resources.Load("Prefabs/Weapons/Items/Minigun_item") as GameObject;
            new_name = "Minigun_item";
        }
        else if (gun_Type == gun_Types.sniper)
        {
            gun_item = Resources.Load("Prefabs/Weapons/Items/Sniper_item") as GameObject;
            new_name = "Sniper_item";
        }
        else if (gun_Type == gun_Types.bazuka)
        {
            gun_item = Resources.Load("Prefabs/Weapons/Items/Bazuka_item") as GameObject;
            new_name = "Bazuka_item";
        }
        if(ammo_gun != 0 || ammo_inv != 0)
        {
            GameObject _gun_item = Instantiate(gun_item, gameObject.transform.position + dropOffset, Quaternion.identity);
            _gun_item.name = new_name;
            _gun_item.GetComponent<Gun_item_stats>().ammo_gun = ammo_gun;
            _gun_item.GetComponent<Gun_item_stats>().ammo_inv = ammo_inv;
            Rigidbody2D _girb = _gun_item.GetComponent<Rigidbody2D>();
            _girb.AddForce(dropVector.normalized * 2, ForceMode2D.Impulse);

            Find.FindById(players, owner_id).GetComponent<CharStats>().active_gun = null;
        }

        Destroy(gameObject);
    }
}
