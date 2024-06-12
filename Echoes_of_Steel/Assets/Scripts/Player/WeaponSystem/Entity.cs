using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float health;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value; 
            Debug.Log(health);

            if(health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        Health = startingHealth;
    }
}
