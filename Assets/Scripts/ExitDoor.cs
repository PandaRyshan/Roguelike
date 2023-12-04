using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (collision.GetComponent<Player>().hasKey) {
                collision.GetComponent<Player>().hasKey = false;
                Debug.Log("Go To Next Level");
            }
        }
    }
}
