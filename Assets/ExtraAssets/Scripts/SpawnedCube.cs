using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedCube : MonoBehaviour
{
    private bool isHittedWall;
    private string wallTag = "Wall", cubePickup = "CubePickup", groundTag = "Ground";
    private Transform playerTransform;
    private new Transform transform;
    PlayerControl playerX;
    void Start()
    {
        transform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerX = playerTransform.gameObject.GetComponent<PlayerControl>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!isHittedWall)
        {
            transform.position =new Vector3( playerTransform.position.x, transform.position.y, playerTransform.position.z);          
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(wallTag))
        {
            isHittedWall = true;
            
            StartCoroutine(playerX.ShakeCameraCoroutine());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(cubePickup))
        {
            playerX.PickUpCube(other.gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            playerX.CreateTrail();
        }
    }
}
