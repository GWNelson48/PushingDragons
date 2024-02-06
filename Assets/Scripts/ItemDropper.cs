using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] GameObject bombItem;
    [SerializeField] GameObject powerUpItem;
    [SerializeField] GameObject healthItem;

    [Tooltip("1 = Bomb, 2 = Power Up, 3 = Health")]
    [SerializeField] int itemToDrop = 3;

    public void DropItem()
    {
        switch (itemToDrop)
        {
            case 3:
                Instantiate(healthItem, transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(powerUpItem, transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(bombItem, transform.position, Quaternion.identity);;
                break;
            default:
                Instantiate(healthItem, transform.position, Quaternion.identity);
                break;
        }
    }
}
