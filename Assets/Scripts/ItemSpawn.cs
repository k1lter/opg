using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] bool only_supplies;
    [SerializeField] bool only_guns;
    [SerializeField] float timer;

    public Object[] possible_items;
    private float timeLeft = 0;

    void Start()
    {
        if(only_supplies && only_guns)
        {
            Debug.LogWarning("Выбраны сразу 2 категории у Спавна. Переключение на режим оружия");
            only_supplies = false;
        }

        if(only_guns)
        {
            possible_items = Resources.LoadAll("Prefabs/Weapons/Items");
        }
        else if(only_supplies)
        {
            possible_items = Resources.LoadAll("Prefabs/Items");
        }
        else
        {
            Debug.LogWarning("Не выбраны настройки спавна");
        }
    }

    void Update()
    {
        if (possible_items.Length > 0)
        {
            if (transform.childCount == 0)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    SpawnItem();
                    timeLeft = timer;
                }
            }
        }
    }

    private void SpawnItem()
    {
        GameObject spawned_item;
        spawned_item = Instantiate((GameObject)possible_items[Random.Range(0, possible_items.Length - 1)], transform);
        spawned_item.transform.position = gameObject.transform.position;
    }
}
