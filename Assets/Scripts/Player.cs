using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int coins;
    public bool hasKey;
    public SpriteRenderer sr;
    public LayerMask moveLayerMask;

    void Move (Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, moveLayerMask);

        if (hit.collider == null)
        {
            transform.position += new Vector3(dir.x, dir.y, 0);
            EnemyManager.instance.OnPlayerMove();
            Generation.instance.OnPlayerMove();
        }
    }

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.up);
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.down);
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.left);
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.right);
        }
    }

    public void OnAttackUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.up);
        }
    }

    public void OnAttackDown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.down);
        }
    }

    public void OnAttackLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.left);
        }
    }

    public void OnAttackRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.right);
        }
    }

    void TryAttack (Vector2 dir) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, 1 << 7);
        if (hit.collider != null) {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damageToTake)
    {
        curHp -= damageToTake;
        UI.instance.UpdateHealth(curHp);

        if (curHp > 0)
        {
            StartCoroutine("DamageFlash");
        }
        else
        {
            GameManager.instance.Restart();
        }
    }

    IEnumerator DamageFlash()
    {
        Color defaultColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        sr.color = defaultColor;
    }

    public void AddCoins (int amount)
    {
        coins += amount;
        UI.instance.UpdateCoinText(coins);
    }

    public bool AddHealth (int amount)
    {
        if (curHp + amount <= maxHp)
        {
            curHp += amount;
            UI.instance.UpdateHealth(curHp);
            return true;
        }
        return false;
    }
}
