using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFunctions : MonoBehaviour
{
    [SerializeField] private Vector3 _gunPosOffset;
    public GameObject gun;

    void Start()
    {
        _gunPosOffset = new(0.04f, -0.27f, -1);
    }

    void Update()
    {
        if (gun != null)
        {
            gun.transform.position = transform.position + _gunPosOffset;
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
}
