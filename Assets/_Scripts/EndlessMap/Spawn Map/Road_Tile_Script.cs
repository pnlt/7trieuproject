using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Tile_Script : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
        Player = GameObject.Find("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.z > transform.position.z + 48f) {
            this.gameObject.SetActive(false);
            GameObject road_tile = ObjectPoolingRoad.instance.GetPooledObject();
            if (road_tile != null)
            {
                road_tile.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 58f * 4);
                road_tile.SetActive(true);
            }
        }
    }
}
