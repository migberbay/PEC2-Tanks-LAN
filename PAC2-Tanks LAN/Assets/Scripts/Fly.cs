using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject shellExplosion;
    public float shotSpeed;

    public float explosionRadius;
    public float explosionForce;
    public float MaxDamage;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shotSpeed, ForceMode.Impulse);
        Destroy(this.gameObject, 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("boom?");
        Explode();
        Destroy(this.gameObject);
    }

    void Explode()
    {
        GameObject explosion = Instantiate(shellExplosion, transform.position, transform.rotation) as GameObject;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(var c in colliders)
        {
            //Debug.Log("Collider found in explosion radius: " + c.gameObject.name);
            if (c.gameObject.tag == "Player" || c.gameObject.tag == "Enemy")
            {
                Rigidbody rb = c.GetComponent<Rigidbody>();
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                float damage = damageCalc(rb.position);
                Debug.Log("damage done: " + damage.ToString());
                TankLife hp =  c.gameObject.GetComponent<TankLife>();
                hp.TakeDamage(damage);
            }
        }

        Destroy(explosion, 1.5f);
    }

    float damageCalc(Vector3 targetPos)
    {
        float relativeDistance = explosionRadius - Vector3.Distance(transform.position, targetPos);
        return Mathf.Max(0f, relativeDistance * MaxDamage);
    }
}
