using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private string wallTag = "Wall",cubePickupTag = "CubePickup", checkpointTag = "Checkpoint", groundTag = "Ground";

    [SerializeField] private float speed;

    private bool isGameStarted;
    private bool isAlive = true;

    new Transform transform;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Camera cameraMain;
    private Transform cameraTransform;
    [SerializeField]private Vector3 cameraOffset = new (3.86f, 9.84f, -11.1f);
    private float shakeDuration = 0.3f;
    private float shakeMagnitude = 0.05f;

    private Vector3 clickPosition;
    private bool isDragging;

    [SerializeField] private GameObject spawnCube;
    [SerializeField] private GameObject trailCube;
    [SerializeField] private GameObject tapToPlayText;
    [SerializeField] private GameObject warpEffect;
    [SerializeField] private GameObject collectCubeText;

    void Start()
    {
        transform = GetComponent<Transform>();
        cameraTransform = cameraMain.GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        CameraControl();
        if (isGameStarted&&isAlive)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);            
        }
        PlayerMovement();
    }
    void StartGame()
    {
        if (isGameStarted) return;
        {
            isGameStarted = true;
            tapToPlayText.SetActive(false);
            warpEffect.SetActive(true);
        }
    }

    void PlayerMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
            isDragging = true;
            StartGame();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            clickPosition = Vector3.zero;
        }
        if (isDragging && isAlive)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float dragDirection = (currentMousePosition.x - clickPosition.x)/100;
            if (dragDirection >= 2.1f)
                dragDirection = 2.1f;
            else if(dragDirection <= -2.1f)
                dragDirection = -2.1f;
            transform.position = new Vector3(dragDirection, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            StartCoroutine(ShakeCameraCoroutine());
            Lose();
        }  
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            CreateTrail();
        }
    }

    void Lose()
    {
        isAlive = false;
        gameManager.ActivateLoseMenu();
        warpEffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(checkpointTag))
        {
            Destroy(other.gameObject);
            gameManager.CreateNewPlatform();
        }
        if (other.CompareTag(cubePickupTag))
        {
            PickUpCube(other.gameObject);
        }
    }

    public void PickUpCube(GameObject cube)
    {
        Destroy(cube);
        AddCube();
    }

    void AddCube()
    {
        Instantiate(collectCubeText, transform.position, Quaternion.identity);
        transform.position = transform.position + new Vector3(0, 1, 0);
        Instantiate(spawnCube, transform.position - new Vector3(0, 1, 0), Quaternion.identity);
    }

    void CameraControl()
    {
        cameraTransform.position = new Vector3(cameraOffset.x, cameraOffset.y, transform.position.z)  + cameraOffset;
    }

    public IEnumerator ShakeCameraCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = cameraTransform.position.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = cameraTransform.position.y + Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.position = new Vector3(x, y, cameraTransform.position.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    public void CreateTrail()
    {
        Instantiate(trailCube, new Vector3(transform.position.x, -0.35f, transform.position.z), Quaternion.identity);
    }
}
