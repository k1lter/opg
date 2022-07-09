using UnityEngine;
using UnityEngine.UI;

public class GunShoot : MonoBehaviour
{
    private GameObject bullet, enemyBullet;
    private Rigidbody2D _rb_bullet;
    private GameObject barrel;
    [SerializeField] Vector2 shootDirection;
    [SerializeField] int ammo;
    public AudioSource shootSound;
    public AudioSource noAmmo;
    public Text ammo_bar;
    private float timeLeft = 0;
    private SceneChange _pause;

    void Start()
    {
        bullet = Resources.Load("Prefabs/Weapons/flame_test") as GameObject;
        enemyBullet = Resources.Load("Prefabs/Weapons/enemy_flame_test") as GameObject;
        ammo_bar = GameObject.Find("Ammo").GetComponent<Text>();
        ammo = 30;
        ammo_bar.text = "Ammo: " + ammo;
        barrel = transform.Find("Barrel").gameObject;
    }

    void Update()
    {
        _pause = GameObject.Find("SceneChange").GetComponent<SceneChange>();
        if (!_pause.pause)
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
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    Reload();
                    ammo_bar.text = "Ammo: " + ammo;
                }
            }
            else if (gameObject.name == "EnemyGun(Clone)") //Стрельба для противника
            {
                if (GameObject.Find("Player") != null)
                {
                    if (timeLeft < 0 && ammo > 0)
                    {
                        Shoot();
                        ammo -= 1;
                        timeLeft = 0.2f;
                        if (ammo == 0)
                        {
                            Reload();
                        }
                    }
                }
            }
        }
    }

    private void Reload()
    {
        ammo = 30;
    }

    private void Shoot()
    {
        if (gameObject.name == "Gun(Clone)")
        {
            GameObject realBullet = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
            _rb_bullet = realBullet.GetComponent<Rigidbody2D>();
            _rb_bullet.AddForce(barrel.transform.right * 50, ForceMode2D.Impulse);
            shootSound.Play();
        }
        else if(gameObject.name == "EnemyGun(Clone)")
        {
            GameObject realEnemyBullet = Instantiate(enemyBullet, barrel.transform.position, barrel.transform.rotation);
            _rb_bullet = realEnemyBullet.GetComponent<Rigidbody2D>();
            _rb_bullet.AddForce(barrel.transform.right * 50, ForceMode2D.Impulse);
            shootSound.Play();
        }
    }
}
