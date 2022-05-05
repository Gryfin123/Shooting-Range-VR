using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public delegate void Notify();
    public event Notify ShotFired = delegate {};
    public event Notify ShotFiredSuccessfully = delegate {};

    [SerializeField] private float bulletDamage = 1f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootpoint;
    
    private void Start() {
        //StartCoroutine(JustShoot()); // Testing only    
    }

    public void Shoot()
    {
        ShotFired.Invoke();
        RaycastHit hit;
        Quaternion fireRotation = Quaternion.LookRotation(gameObject.transform.forward, gameObject.transform.up);
        
        if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hit, Mathf.Infinity))
        {
            ShotFiredSuccessfully.Invoke();
            GameObject tempBullet = Instantiate(projectile, shootpoint.transform.position, fireRotation);
            
            BulletScript moveComponent = tempBullet.GetComponent<BulletScript>();

            if (moveComponent != null)
            {
                moveComponent.SetHitPoint(hit.point);
            }
            else
            {
                Debug.Log("Created bullet doesn't have move component");
                Destroy(tempBullet);
            }
        }
    }

    private IEnumerator JustShoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            Shoot();
        }
    }
}
