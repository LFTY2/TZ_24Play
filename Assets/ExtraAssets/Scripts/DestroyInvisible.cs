using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInvisible : MonoBehaviour
{
    private new Transform transform;
    private Transform playerTransform;

    void Start()
    {
        transform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z > transform.position.z + 60f)
        {
            Destroy(gameObject);
        }
    }
}

