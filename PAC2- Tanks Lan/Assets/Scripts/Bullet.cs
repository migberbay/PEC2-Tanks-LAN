using UnityEngine;

public class Bullet : MonoBehaviour {

    private void Start()
    {
        // Add velocity to the bullet
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 6;
    }

    void OnCollisionEnter(Collision collision) {
        GameObject hit = collision.gameObject;
        if(hit.gameObject.name != "Bullet(Clone)"){ 
            Health health = hit.GetComponent<Health>();
            if (health != null){
                health.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }
}
