using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyLifeCycle : MonoBehaviour
{
    [SerializeField] private float _lifeCycle;

    private float lifeTime;
    private ObjectPoolingWithProbability pool;

    // Start is called before the first frame update
    void Start()
    {
        pool = transform.parent.GetComponent<ObjectPoolingWithProbability>();
    }

    private void Update()
    {
        if (gameObject.activeSelf == false)
            lifeTime = 0;

        lifeTime += Time.deltaTime;
        if (lifeTime >= _lifeCycle)
        {
            pool.ReturnObject(gameObject);
            lifeTime = 0;
        }     
    }

}
