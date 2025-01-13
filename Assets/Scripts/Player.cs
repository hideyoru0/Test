using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class Player : MonoBehaviour
{

    Vector2 inputVector;

    float inputX;
    float inputY;

    [SerializeField] Rigidbody rb;
    [SerializeField] Transform raySp;
    
    float speed = 120.0f;
    public float rayLength = 0.5f;
    public bool isGround = false;
    public bool isJumping = false;
    public Transform spawnPoint;
    public GameObject coin;
    public GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 컨트롤러의 InputVector2를 사용하여 direction에 대입
        Vector3 direction = new Vector3(inputX, rb.velocity.y, inputY);
        // Vector3의 속도를 newVelocity로 생성
        Vector3 newVelocity = direction * speed * Time.deltaTime;
        rb.velocity = newVelocity;

        // 지면에 붙어있으면 점프하도록 한다.
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            isJumping = false;
            //StartCoroutine(JumpCoroutine());
            JumpNoCoroutine();
        }

        DetectLand();
    }

    void JumpNoCoroutine()
    {
        rb.AddForce(Vector3.up * 50.0f, ForceMode.Force);
    }

    IEnumerator JumpCoroutine()
    {
        if (isJumping) yield return null;
        rb.AddForce(Vector3.up * 50.0f, ForceMode.Force);
        yield return null;
    }

    void DetectLand()
    {
        RaycastHit hit;
        Debug.DrawRay(raySp.position + Vector3.up * 0.1f, Vector3.down * 1.2f, Color.red);
        if (Physics.Raycast(raySp.position + Vector3.up * 0.1f, Vector3.down, out hit, rayLength))
        {

            if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
            {
                isGround = true;
            }
            //Debug.Log($"{isGround} changed");
        }
        else
        {
            isGround = false;
            //Debug.Log($"{isGround} unchanged");
        }

        if (isGround && isJumping)
        {

        }
    }

    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();

        inputX = inputVector.x;
        inputY = inputVector.y;

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Door" && Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log("K");
            //Instantiate(coin, spawnPoint.position, Quaternion.identity);
            gameManager.SpawnCoin();
        }
    }
}
