using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    int platformSpawned = 0;
    [SerializeField] GameObject[] platformPaterns;
    [SerializeField]private GameObject loseMenu;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            CreateNewPlatform();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewPlatform()
    {
        Instantiate(platformPaterns[Random.Range(0, 4)], new Vector3(0, 0, platformSpawned * 30), Quaternion.identity);
        platformSpawned++;
    }

    public void ActivateLoseMenu()
    {
        loseMenu.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
