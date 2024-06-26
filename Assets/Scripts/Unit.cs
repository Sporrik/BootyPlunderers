using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxHealth, currentHealth;
    public int movement, moveSpeed;
    public int damage;

    public int collectedTreasure;
    public GameObject heldObject;

    public bool isOnBad = false;
    public bool hasAttacked = false;

    public Vector3 targetPosition, attackTarget;
    public Hex targetHex;
    public Hex currentHex;
    public Hex previousHex;
    public Hex attackHex;

    private void Awake()
    {
        //Set stats
        maxHealth = 100;
        currentHealth = maxHealth;

        moveSpeed = 3;
        movement = moveSpeed;

        currentHex = transform.position.ToHex();
        previousHex = currentHex;
    }

    public IEnumerator MoveUnit()
    {
        targetHex = targetPosition.ToHex();

        for (int remaining = movement; remaining > 0; remaining--)
        {
            currentHex = transform.position.ToHex();

            if (isOnBad)
            {
                isOnBad = false;

                targetHex = previousHex;
                currentHex = previousHex;

                transform.position = previousHex.ToWorld();
                movement++;

                GetComponent<Node>().ApplyTransform();
                StopCoroutine(MoveUnit());
            }

            var direction = targetHex - currentHex;

            if (direction.q == 0 && direction.r == 0)
            {
                remaining = 0;
                goto EndMovement;
            }
            if (direction.q != 0)
            {
                direction.q /= Mathf.Abs(direction.q);
            }
            if (direction.r != 0)
            {
                direction.r /= Mathf.Abs(direction.r);
            }

            previousHex = currentHex;
            currentHex += direction;

            transform.position = currentHex.ToWorld();
            movement--;

        EndMovement:
            GetComponent<Node>().ApplyTransform();
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "P1_Crew" || collision.tag == "P2_Crew"|| collision.tag == "Border")
        {
            isOnBad = true;
        }

        if (collision.tag == "Treasure")
        {
            if (heldObject == null)
            {
                Debug.Log("Player collides with treasure!");
                var treasure = collision.gameObject;
                collision.enabled = false;
                treasure.transform.localScale = new Vector2(0.5f, 0.5f);
                treasure.transform.SetParent(transform);
                treasure.transform.position += Vector3.up * 0.5f;
                heldObject = treasure;
            }
        }

        if (collision.tag == "ClaimPoint")
        {
            if (heldObject != null)
            {
                ++collectedTreasure;
                Debug.Log("Player collides with Claim Point!");

                Destroy(heldObject);
                heldObject = null;
            }
        }
    }

    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            heldObject.GetComponent<Collider2D>().enabled = true;
            return true;
        }
        else return false;
    }

    public void Reset()
    {
        movement = moveSpeed;
        hasAttacked = false;
    }
}