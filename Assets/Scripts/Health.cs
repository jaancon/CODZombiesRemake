using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Health : MonoBehaviour
{
    public enum objectType
    {
        Player,
        Enemy
    }

    public objectType type;
    public float health;
    public float maxHealth = 100f;
    public int zombie_Monetary_Worth;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            EndOfLife();
        }
    }

    public void Heal(float amount)
    {
        if (type != objectType.Player) { return; }
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    void EndOfLife()
    {
        switch (type)
        {
            case objectType.Player:
                //end game
                break;
            case objectType.Enemy:
                Vector3 velocity = gameObject.GetComponent<NavMeshAgent>().velocity;
                Money.scriptInstance.GainMoney(zombie_Monetary_Worth);
                gameObject.GetComponent<Animator>().SetBool("Dead", true);
                gameObject.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                gameObject.GetComponent<CapsuleCollider>().enabled = false;

                Zombie zombieScript = gameObject.GetComponent<Zombie>();
                zombieScript.isDead = true;
                foreach (var collider in zombieScript.ragdollColliders)
                {
                    collider.enabled = true;
                }
                foreach (var rb in zombieScript.ragdollRigidbodies)
                {
                    rb.isKinematic = false;
                    rb.velocity = velocity;
                }
                Destroy(this.gameObject, 5f);
                break;
        }
    }
}
