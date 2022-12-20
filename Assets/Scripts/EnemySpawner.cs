using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] gameObjects;
    //when player enter the room, create 3~6 enemy
    void Start()
    {

    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
    GameObject enemy;
    Debug.Log("enter");
        if(other.transform.name == "maincharacter")
        {
            int randomindex = Random.Range(0, gameObjects.Length);
            Vector3 randomposition = new Vector3(Random.Range(transform.position.x - 13f, transform.position.x - 1f), -1.5f, Random.Range(transform.position.z - 7f, transform.position.z + 8f));

            enemy = Instantiate(gameObjects[randomindex], randomposition, Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            
        }//create enemy
    }//enter the room
}
