using UnityEngine;
using UnityEngine.UI;

public class GunShoot : MonoBehaviour
{
    private GameObject bullet;
    private Rigidbody2D _rb_bullet;
    private GameObject barrel;
    [SerializeField] Vector2 shootDirection;
    [SerializeField] int ammo;
    public AudioSource shootSound;
    public AudioSource noAmmo;
    private Text ammo_bar;
    private float timeLeft = 0;

    void Start()
    {
        bullet = Resources.Load("Prefabs/Weapons/flame_test") as GameObject;
        ammo_bar = GameObject.Find("Ammo").GetComponent<Text>();
        ammo = 30;
        ammo_bar.text = "Ammo: " + ammo;
        barrel = transform.Find("Barrel").gameObject;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (gameObject.name == "Gun(Clone)") //Стрельба для игрока
        {
            if (Input.GetKey(KeyCode.Mouse0) && timeLeft < 0 && ammo > 0)
            {
                Shoot();
                ammo -= 1;
                ammo_bar.text = "Ammo: " + ammo;
                timeLeft = 0.2f;
            }
            else if (Input.GetKey(KeyCode.Mouse0) && ammo == 0)
            {
                noAmmo.Play();
            }
        }
        else if(gameObject.name == "EnemyGun(Clone)") //Стрельба для противника
        {
            if (timeLeft < 0 && ammo > 0)
            {
                Shoot();
                ammo -= 1;
                timeLeft = 0.2f;
            }
        }

    }

    private void Shoot()
    {
        GameObject realBullet = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        _rb_bullet = realBullet.GetComponent<Rigidbody2D>();
        _rb_bullet.AddForce(barrel.transform.right * 50, ForceMode2D.Impulse);
        shootSound.Play();
    }
}
