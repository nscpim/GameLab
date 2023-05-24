using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 30f;
    public Transform cam; 
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float jumpSpeed = 20.0f;
    public float gravity = 5f;
    private Vector3 movingDirection = Vector3.zero;
    public float maxSpeed = 70f;
    public float acceleration = 5;
    public LayerMask wallLayerMask;
    public float launchSpeed;
    public GameObject prefabToSpawn;
    public int playerInt;
    public ControlScheme scheme;
    bool canJump = true;

    private Camera mainCamera;
    [HideInInspector] public ScriptableCharacter character;
    private float timeStamp;
    public bool canSelectCharacter;
    private Timer abilityCooldown;
    private Timer secondAbilityTimer;
    public bool canMove;


    public bool isGrounded;
    public float raycastDistance = 1.5f;

    [Header("Checkpoints")]
    public int currentCheckpoint = 0;
    public Vector3 respawnPosition;

    private bool isCollidingWithWall = false;
    void Start()
    {
        DontDestroyOnLoad(this);
        controller = GetComponent<CharacterController>();
        prefabToSpawn.SetActive(false);
        mainCamera = GetComponentInChildren<Camera>();
        SetViewPortRect(gameObject.name, GameManager.instance.GetAmountOfPlayers());
        abilityCooldown = new Timer();
        UpdateLayers();
        respawnPosition = GameManager.instance.spawnPoints[0].transform.position;
        secondAbilityTimer = new Timer();
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
                cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P1Cam");
                break;
            case 2:
                cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P2Cam");
                break;
            case 3:
                cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P3Cam");
                break;
            case 4:
                cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "UI", "Water", "Grabbable", "Wall", "Ground", "Post", "P4Cam");
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
    }


    public void ExecuteAbility()
    {
        if (!abilityCooldown.isActive && canMove)
        {
            switch (character.characterEnum)
            {
                case Character.Test:
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

        controller.enabled = false;
        transform.position = new Vector3(respawnPosition.x, respawnPosition.y, respawnPosition.z);
        controller.enabled = true;
    }

   

    public void Jump()
    {
        if (isGrounded && canMove && canJump)
        {
            movingDirection.y = jumpSpeed;
        }
        
        if (!isGrounded) 
        {
            gravity = 165f;
            movingDirection.y -= gravity * Time.deltaTime;
            canJump = false;
        }
        else
        {
            canJump = true;
        }
        
    }

    void Update()
    {
        scheme.Update();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, raycastDistance, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal" + playerInt) * 0.5f;
            float vertical = Input.GetAxisRaw("Vertical" + playerInt);
            Vector3 direction = new Vector3(-horizontal, 0f, vertical).normalized;

            movingDirection.y -= gravity * Time.deltaTime;
            controller.Move(movingDirection * Time.deltaTime);

            if (speed <= maxSpeed)
            {
                speed += acceleration * Time.deltaTime;
            }
            if (speed > maxSpeed)
            {
                Debug.Log("Gets here");
                speed = maxSpeed;
            }

            if (!transform.hasChanged)
            {
                speed = 30f;
                Debug.Log("Reset Speed");
            }
            transform.hasChanged = false;

            //transform.position.x = transform.position.x + speed*Time.deltaTime;



            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }

       

        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnPrefab();
            Debug.Log("Wow");
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (wallLayerMask == (wallLayerMask | (1 << hit.gameObject.layer)))
        {
            
            Debug.Log("Collided with a wall!");
            isCollidingWithWall = true;
            gravity = 0f;


        }

    }

    void FixedUpdate()
    {
        
        if (isCollidingWithWall && !controller.isGrounded)
        {
            
            Debug.Log("Stopped colliding with a wall!");
            gravity = 200f;
            isCollidingWithWall = false;
            canJump = true;
            isGrounded = true;


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
    }







}
