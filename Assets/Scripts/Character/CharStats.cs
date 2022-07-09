using UnityEngine;
using UnityEngine.UI;

public class CharStats : MonoBehaviour
{
    public bool speed, armor, jump;
    [SerializeField] int hp;
    private Text hp_bar;

    void Start()
    {
        hp_bar = GameObject.Find("health").GetComponent<Text>();
        hp = 100;
        hp_bar.text = "Health: " + hp;
    }

    private void Update()
    {
        if(hp == 0)
        {
            Debug.Log("Blin, ya umer(");
            Destroy(gameObject);
            if(gameObject.name == "Enemy")
            {
                Destroy(GameObject.Find("EnemyGun(Clone)")); //Пусть пока так будет, но надо исправить
            }
            else if (gameObject.name == "Player")
            {
                Destroy(GameObject.Find("Gun(Clone)")); //Пусть пока так будет, но надо исправить
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "Player")
        {
            if (collision.gameObject.name == "enemy_flame_test(Clone)" && hp > 0)
            {
                hp -= 10;
                hp_bar.text = "Health: " + hp;
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
}
