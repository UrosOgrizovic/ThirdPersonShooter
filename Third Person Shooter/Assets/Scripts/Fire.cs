using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 centre = new Vector3(0.5f, 0.5f, 0);
            Ray _rayStart = Camera.main.ViewportPointToRay(centre);
            RaycastHit hit;
            if (Physics.Raycast(_rayStart, out hit)) {
                Debug.Log("Hit: " + hit.collider.name);
                HealthSystem enemyHealth = hit.collider.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(50);
                }
            }
        }
    }
}
