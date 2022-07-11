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

    public int gun_id;
    public int owner_id;
    public int ammo_gun, ammo_inv;
    private float timeLeft = 0;
    private GameObject barrel;
    private GameObject bullet;
    private GameObject gun_item;
    private Rigidbody2D _rb_bullet;


    void Start()
    {
        owner_id = 0; //Надо вычислять автоматически!!!
        ammo_gun = amount_of_ammo_gun;
        ammo_inv = amount_of_ammo_inv;
        audioSource = GetComponent<AudioSource>();
        no_ammo_audio = Resources.Load("Sounds/Weapons/Pistol/NoAmmo") as AudioClip;

        //**********Выбор текстуры снаряда в зависимости от вида патрона
        if (ammo_Type != ammo_Types.bazuka_rocket)
        {
            bullet = Resources.Load("Prefabs/Weapons/flame_test") as GameObject;
        }
        else
        {
            //Тут ракета. Ее надо сделать
            bullet = Resources.Load("Prefabs/Weapons/flame_test") as GameObject; 
        }
        //***********


        //*************Выбор переменных для вида оружия !!!Готовы не все!!!
        if(ammo_Type == ammo_Types.pistol_ammo || ammo_Type == ammo_Types.minigun_ammo)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Pistol/Shoot_Pistol") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Pistol/Reload") as AudioClip;
        }
        else if(ammo_Type == ammo_Types.shotgun_ammo)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Shotgun/Shoot") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Pistol/Reload") as AudioClip;
        }
        else if (ammo_Type == ammo_Types.sniper_ammo)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Sniper/Shoot_Sniper") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Pistol/Reload") as AudioClip;
        }
        else if (ammo_Type == ammo_Types.bazuka_rocket)
        {
            shoot_audio = Resources.Load("Sounds/Weapons/Bazuka/Shoot") as AudioClip;
            reload_audio = Resources.Load("Sounds/Weapons/Pistol/Reload") as AudioClip;
        }
        //*****************************


        barrel = gameObject.transform.Find("Barrel").gameObject; //Дуло откуда летит снаряд

    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && ammo_gun > 0)
        {
            if (timeLeft < 0)
            {
                Shoot();
                timeLeft = firing_frequency;
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && ammo_gun == 0)
        {
            audioSource.PlayOneShot(no_ammo_audio);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            DropGun();
        }

    }

    private void Shoot()
    {
        switch(ammo_Type)
        {
            case ammo_Types.pistol_ammo:
                ShootPistol();
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
            gun_item = Resources.Load("Prefabs/Weapons/Pistol/Pistol_item") as GameObject;
        }
        else if (gun_Type == gun_Types.shotgun)
        {
            gun_item = Resources.Load("Prefabs/Weapons/Pistol/Shotgun_item") as GameObject;
            new_name = "Shotgun_item";
        }

        GameObject _gun_item = Instantiate(gun_item, gameObject.transform.position + dropOffset, Quaternion.identity);
        _gun_item.name = new_name;
        Rigidbody2D _girb = _gun_item.GetComponent<Rigidbody2D>();
        _girb.AddForce(dropVector.normalized * 2, ForceMode2D.Impulse);

        GameObject.FindGameObjectWithTag("Player").GetComponent<CharStats>().active_gun = null;

        Destroy(gameObject);
    }
}
