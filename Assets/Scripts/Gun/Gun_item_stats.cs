using UnityEngine;

public class Gun_item_stats : MonoBehaviour
{
    public int ammo_gun;
    public int ammo_inv;
    public enum gun_Types //Вид оружия
    {
        pistol = 0,
        shotgun = 1,
        sniper = 2,
        minigun = 3,
        bazuka = 4
    }

    public gun_Types gun_Type;
}
