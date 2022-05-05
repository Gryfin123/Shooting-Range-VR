using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    
    [SerializeField] private float bulletDamage = 1f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootpoint;
    
    public void Shoot()
    {
        RaycastHit hit;
        Quaternion fireRotation = Quaternion.LookRotation(gameObject.transform.forward, gameObject.transform.up);
        

        if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hit, Mathf.Infinity))
        {
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
}
