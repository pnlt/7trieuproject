using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReturn : MonoBehaviour
{

    [SerializeField] private float _lifeCycle;

    private float lifeTime;
    private ObjectPoolingWithProbability pool;

    // Start is called before the first frame update
    private void Start()
    {
        pool = transform.parent.GetComponent<ObjectPoolingWithProbability>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.activeSelf == false)
            lifeTime = 0;
    }

    public void LifeCoolDown()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= _lifeCycle)
        {
            pool.ReturnObject(gameObject);
            lifeTime = 0;
        }
    }
}
