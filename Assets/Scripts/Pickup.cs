using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PickupType
{
    Coin,
    Health,
    Key
}

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int value = 1;

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (type)
            {
                case PickupType.Coin:
                    collision.GetComponent<Player>().AddCoins(value);
                    break;
                case PickupType.Health:
                    collision.GetComponent<Player>().AddHealth(value);
                    break;
                case PickupType.Key:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
