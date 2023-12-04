using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public int health;
    public int damage;
    public float attackChance = 0.5f;
    public GameObject deathDropPrefab;
    public SpriteRenderer spriteRenderer;
    public LayerMask moveLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void TakeDamage (int damageToTake) {
        health -= damageToTake;
        if (health <= 0) {
            if (deathDropPrefab != null) {
                Instantiate(deathDropPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        StartCoroutine(DamageFlash());

        if (Random.value > attackChance) {
            player.TakeDamage(damage);
        }
    }

    IEnumerator DamageFlash () {
        Color defaultColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = defaultColor;
    }

    public void Move()
    {
        if (Random.value < 0.5f) return;

        Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
        foreach (Vector3 dir in directions)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Player"));
            if (hit2D.collider != null && hit2D.collider.gameObject.CompareTag("Player"))
            {
                TryAttack(dir);
                return;
            }
        }

        Vector3 moveDir = Vector3.zero;
        bool canMove = false;

        while (canMove == false) {
            moveDir = GetRandomDirection();
            RaycastHit2D hitEnemy = Physics2D.Raycast(transform.position + moveDir, Vector2.zero, 0f, LayerMask.GetMask("Enemy"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 1f, moveLayerMask);
            if (hit.collider == null && hitEnemy.collider == null) {
                canMove = true;
            }
        }

        transform.position += moveDir;
    }

    void TryAttack (Vector2 dir) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Player"));
        if (hit.collider != null) {
            player.TakeDamage(1);
        }
    }

    Vector3 GetRandomDirection () {
        int range = Random.Range(0, 4);
        if (range == 0) {
            return Vector3.up;
        } else if (range == 1) {
            return Vector3.down;
        } else if (range == 2) {
            return Vector3.left;
        } else if (range == 3) {
            return Vector3.right;
        }
        return Vector3.zero;
    }

}
