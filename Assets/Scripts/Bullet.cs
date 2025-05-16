using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0,100)] public float penetrationChance = 20;

    public bool isShell = false;
    public GameObject shotgunShellFragment;
    public Vector2 shellAmountRange;

    public bool isShellFragment;
    public Collider originalShellHit;

    private void OnTriggerEnter(Collider other)
    {
        if ((isShellFragment && other == originalShellHit) || other.CompareTag("Bullet")) { return; }
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Zombie")) { return; }

        float damage = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        other.gameObject.GetComponent<Health>().TakeDamage(damage);

        bool keepBullet = Random.Range(0, 100) <= penetrationChance;
        if (!keepBullet)
        {
            DestroyBullet();
            return;
        } else if (isShell)
        {
            int numFragments = Mathf.RoundToInt(Random.Range(shellAmountRange.x, shellAmountRange.y));

            for (int i = 0; i < numFragments; i++)
            {
                GameObject fragment = Instantiate(shotgunShellFragment, transform.position, Quaternion.identity);
                Bullet fragmentBullet = fragment.GetComponent<Bullet>();
                fragmentBullet.isShellFragment = true;
                fragmentBullet.originalShellHit = other;

                Rigidbody fragRb = fragment.GetComponent<Rigidbody>();
                if (fragRb != null)
                {
                    Vector3 spreadDirection = (transform.forward + Random.insideUnitSphere * 0.4f).normalized;
                    fragRb.velocity = spreadDirection * Random.Range(30f, 45f);
                }
            }
        }
    }

    private void DestroyBullet()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        BoxCollider col = GetComponent<BoxCollider>();
        TrailRenderer trail = GetComponent<TrailRenderer>();
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        col.enabled = false;
        mesh.enabled = false;

        float lifetime = trail.time;
        Destroy(gameObject, lifetime);
    }
}
