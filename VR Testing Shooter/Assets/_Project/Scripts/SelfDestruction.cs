using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    [SerializeField] float _delay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _delay);
    }
}
