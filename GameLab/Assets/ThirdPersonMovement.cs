using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody controller;
    public float speed = 30f;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity = 0.1f;
    public float jumpSpeed = 20.0f;
    //public float gravity = 10.0f;
    private Vector3 movingDirection = Vector3.zero;
    public float maxSpeed = 70f;
    // public float acceleration = 5;
    public LayerMask wallLayerMask;
    public float launchSpeed;
    public GameObject prefabToSpawn;
    public int playerInt;
    public ControlScheme scheme;
    public Quaternion respawnRotation;


    public float moveSpeed = 50f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    private Vector3 movement;
    public bool isMoving;
    public bool isJumping;
    public bool isGrounded;


    private Timer spawnTimer;
    private GameObject objectSpawnedIn;

    public Camera mainCamera;
    [HideInInspector] public ScriptableCharacter character;
    private float timeStamp;
    public bool canSelectCharacter;
    private Timer abilityCooldown;
    private Timer secondAbilityTimer;
    public bool canMove;
    private bool canPause;
    private Timer pauseTimer;


    //public bool isGrounded;
    public float raycastDistance = 1.5f;

    [Header("Checkpoints")]
    public int currentCheckpoint = 0;
    public Vector3 respawnPosition;

    private bool isCollidingWithWall = false;
    void Start()
    {
        isJumping = false;
        DontDestroyOnLoad(this);
        prefabToSpawn.SetActive(false);
        if (CineMachineHandler.instance != null)
        {
            mainCamera = CineMachineHandler.instance.brainCams[playerInt];
            SetViewPortRect(gameObject.name, GameManager.instance.GetAmountOfPlayers());
        }
        else
        {
            Invoke("TryCineMachineSetup", 0.5f);
        }
        abilityCooldown = new Timer();

        respawnPosition = GameManager.instance.spawnPoints[0].transform.position;
        respawnRotation = GameManager.instance.spawnPoints[0].transform.rotation;
        secondAbilityTimer = new Timer();
        spawnTimer = new Timer();
    }

    public void TryCineMachineSetup()
    {
        try
        {
            Debug.Log("Main Camera");
            mainCamera = CineMachineHandler.instance.brainCams[playerInt - 1];
            SetViewPortRect(gameObject.name, GameManager.instance.GetAmountOfPlayers());
            UpdateLayers();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }


    }


    public void SetViewPortRect(string _name, int amount)
    {
        //X , Y, Width and Height
        //Widh and height are always the same in 2 or 4 players
        //X position of screen on the X axis, starting from the bottom left corner
        //Y Position of screen on the Y axis, starting from the bottom
        switch (amount)
        {
            case 2:
                switch (_name)
                {
                    case "Player: 1":
                        mainCamera.rect = new Rect(0, 0.5f, 1f, 0.5f);
                        break;
                    case "Player: 2":
                        mainCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
                        break;
                    default:
                        Debug.LogError("Players not correctly initalized!");
                        break;
                }
                break;
            case 3:
                switch (_name)
                {
                    case "Player: 1":
                        mainCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 2":
                        mainCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 3":
                        mainCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
                        break;
                    default:
                        Debug.LogError("Players not correctly initalized!");
                        break;
                }
                break;
            case 4:
                switch (_name)
                {
                    case "Player: 1":
                        mainCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 2":
                        mainCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 3":
                        mainCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                        break;
                    case "Player: 4":
                        mainCamera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                        break;
                    default:
                        Debug.LogError("Players not correctly initalized!");
                        break;
                }
                break;
            default:
                break;
        }
    }

    public void UpdateLayers()
    {

        switch (playerInt)
        {
            case 1:
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P1Cam");
                break;
            case 2:
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P2Cam");
                break;
            case 3:
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P3Cam");
                break;
            case 4:
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P4Cam");
                break;
            default:
                break;
        }
    }


    public void UpdateMesh()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        filter.mesh = character.characterMesh;
    }

    public void Timer()
    {
        if (abilityCooldown.isActive && abilityCooldown.TimerDone())
        {
            abilityCooldown.StopTimer();
        }


        if (secondAbilityTimer.isActive && secondAbilityTimer.TimerDone())
        {
            secondAbilityTimer.StopTimer();
        }


        if (spawnTimer.isActive && spawnTimer.TimerDone())
        {
            spawnTimer.StopTimer();
            Destroy(objectSpawnedIn);
        }

    }


    public void ExecuteAbility()
    {
        if (!abilityCooldown.isActive && canMove)
        {
            switch (character.characterEnum)
            {
                case Character.Test:
                    SpawnPrefab();
                    Debug.Log("Ability goes yeet");
                    break;
                case Character.Test1:

                    break;
                case Character.Test2:

                    break;
                case Character.Test3:

                    break;
                default:
                    break;
            }
            abilityCooldown.SetTimer(character.abilityCooldown);
        }

        else
        {
            //Debug.Log(gameObject.name + " Your ability is on cooldown:" + abilityCooldown.TimeLeft());
        }
    }

    public void ExecuteSecondAbility()
    {
        if (!secondAbilityTimer.isActive && canMove)
        {
            switch (character.characterEnum)
            {
                case Character.Test:
                    Debug.Log("Second ability sjjeeesh");
                    break;
                case Character.Test1:

                    break;
                case Character.Test2:

                    break;
                case Character.Test3:

                    break;
                default:
                    break;
            }
            secondAbilityTimer.SetTimer(character.abilityCooldown);
        }

        else
        {
            //Debug.Log(gameObject.name + " Your ability is on cooldown:" + abilityCooldown.TimeLeft());
        }
    }

    public void SetControlScheme(ControlScheme scheme)
    {
        if (scheme != null)
        {
            ClearControlScheme();
        }
        scheme.AssignInput(this);
        this.scheme = scheme;

    }


    public void SelectedCharacter(Character selection)
    {
        character = GameManager.instance.characters[(int)selection];
        //UpdateMesh();
        //+1 because arrays start at 0, but we need the actual number of players here.
        if (GameManager.instance.order <= GameManager.instance.amountOfPlayers)
        {
            GameManager.instance.SelectedPlayer();
        }
    }
    public void ClearControlScheme()
    {
        scheme = null;
    }



    public void Respawn()
    {
        Vector3 oldPos = transform.position;
        Vector3 newPos = respawnPosition;

        transform.position = new Vector3(respawnPosition.x, respawnPosition.y, respawnPosition.z);
        transform.rotation = respawnRotation;
        CineMachineHandler.instance.cameras[playerInt].PreviousStateIsValid = false;
        CineMachineHandler.instance.cameras[playerInt].OnTargetObjectWarped(transform, oldPos - newPos);

    }



    public void Jump()
    {
        if (isGrounded && canMove)
        {
            isJumping = true;
        }
    }

    void Update()
    {
        scheme.Update();
        Timer();
        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal" + playerInt);
            float verticalInput = Input.GetAxis("Vertical" + playerInt);
            Vector3 movement = transform.forward * verticalInput * moveSpeed;
          
            
            controller.velocity = movement;

            float rotationSpeed = 3f;
            if (horizontalInput != 0)
            {
                Quaternion rotation = Quaternion.Euler(0f, horizontalInput * rotationSpeed, 0f);
                controller.MoveRotation(controller.rotation * rotation);
            }
        }



        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnPrefab();
            Debug.Log("Wow");
        }
    }
    void FixedUpdate()
    {
        CheckGrounded();

        if (isJumping)
        {
            Jumping();
        }

        ApplyGravity();


        isJumping = false;

        if (isCollidingWithWall && !isGrounded)
        {

            Debug.Log("Stopped colliding with a wall!");
            //gravity = 200f;
            isCollidingWithWall = false;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 5f, LayerMask.GetMask("Ground"));
    }
   

    private void Jumping()
    {
        controller.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        isJumping = false;
    }

    private void ApplyGravity()
    {
        controller.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }


    public void Pause()
    {
        GameManager.Pause();
        Debug.Log("Paused");
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (wallLayerMask == (wallLayerMask | (1 << hit.gameObject.layer)))
        {

            Debug.Log("Collided with a wall!");
            isCollidingWithWall = true;
           // gravity = 0f;
        }

    }



    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Slow Area"))
        {
            speed = 50f;
            Debug.Log("Player entered the trigger!");
        }

        if (other.CompareTag("Fast Area"))
        {
            speed = 300f;
            Debug.Log("Player entered the trigger!");
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jump Pad"))
        {
            movingDirection.y = launchSpeed;
            Debug.Log("Player entered the jump pad trigger!");
        }
    }

    void SpawnPrefab()
    {
        // Instantiate the prefab at the current object's position
        GameObject newPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        newPrefab.SetActive(true);
        objectSpawnedIn = newPrefab;
        spawnTimer.SetTimer(5f);
    }







}
