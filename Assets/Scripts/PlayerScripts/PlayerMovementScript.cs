using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;

    [Range(1f, 20f)]
    public float speed = 12f;
    float currentSpeed;

    [Range(-30f, -5f)]
    public float gravity = -19.62f;

    [Range(1f, 10f)]
    public float jumpHeight = 3f;

    public float level = 1;

    public Transform groundCheck;

    [Range(0.1f, 1f)]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask waterMask;

    Vector3 velocity;
    bool isGrounded;
    bool inWater;
    bool wasInWater;

    public static PlayerMovementScript instance;

    bool running = false;

    float hp = 100;
    float pain;
    float stamina = 100f;

    bool bleeding = false;

    public LayerMask layerMask;
    private MapGenerator mapGenerator;

    public GameObject playerCamera;
    public GameObject pauseMenu;

    public UIData uiData;

    private void Start()
    {
        uiData.inventory.SetActive(false);
        uiData.inventory.transform.GetChild(2).gameObject.SetActive(true);

        pauseMenu.SetActive(false);

        mapGenerator = GameObject.Find("/Chunks").GetComponent<MapGenerator>();

        StartCoroutine(Spawn());

        instance = this;
    }

    IEnumerator Spawn()
    {
        yield return new WaitUntil(() => mapGenerator.meshGenerated == true);
        transform.position = new Vector3(0, DetectGroundHeight(transform.position), 0);

        if(GameManager.instance != null)
        {
            yield return new WaitUntil(() => GameManager.instance.doneLoading);
        }

        playerCamera.SetActive(true);
    }

    float DetectGroundHeight(Vector3 position)
    {
        position.y = 500;
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(position, Vector3.down);

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            return hit.point.y;
        }

        return 0;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        inWater = Physics.CheckSphere(gameObject.transform.GetChild(0).position, groundDistance, waterMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (inWater)
        {
            currentSpeed = speed / 2f;
        }
        else if (currentSpeed != speed && !running)
        {
            currentSpeed = speed;
        }

        if(inWater != wasInWater)
        {
            FogCustomizing.WaterSettings(inWater);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * 2 / ((inWater) ? 2 : 1);
            running = true;

            if (stamina > 0)
            {
                stamina -= 0.1f;
            }
            else
            {
                pain += 0.01f;
            }
        }
        else
        {
            running = false;

            if (stamina < 100)
            {
                stamina += 0.05f;
            }
        }

        wasInWater = inWater;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Jump") && isGrounded && Time.timeScale != 0.5f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        } else if (Input.GetKeyUp(KeyCode.Escape) && Time.timeScale == 0 || Input.GetKeyUp(KeyCode.Escape) && Time.timeScale == 1)
        {
            Cursor.lockState = (Time.timeScale == 0) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = Time.timeScale != 0;

            Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
            pauseMenu.SetActive((Time.timeScale == 0) ? true : false);
        } else if (Input.GetKeyUp(KeyCode.Tab) && Time.timeScale == 0.1f || Input.GetKeyUp(KeyCode.Tab) && Time.timeScale == 1)
        {
            const float timeScale = 0.1f;

            Inventory(Time.timeScale != timeScale);
            Cursor.lockState = (Time.timeScale == timeScale) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = Time.timeScale != 0;

            Time.timeScale = (Time.timeScale == timeScale) ? 1 : timeScale;
        } else if (Input.GetKeyUp(KeyCode.BackQuote) && Time.timeScale == 0.5f || Input.GetKeyUp(KeyCode.BackQuote) && Time.timeScale == 1 || Time.timeScale == 0.5f && Input.GetKeyUp(KeyCode.Escape))
        {
            uiData.console.SetActive(!uiData.console.activeSelf);
            uiData.console.GetComponent<InputField>().ActivateInputField();

            Time.timeScale = uiData.console.activeSelf ? 0.5f : 1;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (!uiData.console.activeSelf)
        {
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        if (transform.position.y < 1)
        {
            transform.position = new Vector3(transform.position.x, DetectGroundHeight(transform.position), transform.position.z);
        }
    }

    private void Inventory(bool enabled)
    {
        if (enabled)
        {
            uiData.inventory.SetActive(true);
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            uiData.inventory.SetActive(false);
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }
}