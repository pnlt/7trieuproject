using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _lifeCycle;

    private float lifeTime;
    private ObjectPooling pool;

    // Start is called before the first frame update
    void Start()
    {
        pool = transform.parent.GetComponent<ObjectPooling>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.activeSelf == false)
            lifeTime = 0;

        lifeTime += Time.deltaTime;
        if (lifeTime > _lifeCycle)
        {
            pool.ReturnObject(gameObject);   
            lifeTime = 0;
        }
    }
}
