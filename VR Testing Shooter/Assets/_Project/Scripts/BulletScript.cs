using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private Vector3 hitPoint;
    [SerializeField] private float speed = 10;
    [SerializeField] private float expationTime = 999f;
    [SerializeField] private ParticleSystem deathParticle;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, expationTime);
        GetComponent<Rigidbody>().AddForce((hitPoint - transform.position).normalized * speed);
    }


    public void SetHitPoint( Vector3 direction)
    {
        hitPoint = direction;
    }
    public void SetSpeed(float newValue)
    {
        speed = newValue;
    }

    private void OnDestroy() {
        Instantiate(deathParticle, transform.position, new Quaternion());
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}
