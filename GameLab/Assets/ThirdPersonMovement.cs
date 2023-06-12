using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ThirdPersonMovement : MonoBehaviour
{


    [Header("Main Variables")]
    public CharacterController controller;
    public int playerInt;
    public ControlScheme scheme;
    private Animator _anim;
    public Camera mainCamera;
    [HideInInspector] public ScriptableCharacter character;
    public bool canSelectCharacter;
    public float raycastDistance = 1.5f;

    [Header("Movement")]
    public float speed = 30f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public float jumpSpeed = 20.0f;
    public float gravity = 130f;
    public float maxSpeed = 70f;
    public float acceleration = 5;
    public float launchSpeed;
    private Vector3 movingDirection = Vector3.zero;
    public LayerMask wallLayerMask;
    public bool canJump;
    public bool canMove;
    public bool isGrounded;
    public bool attached;
    private bool isCollidingWithWall = false;

    [Header("Abilities")]
    public GameObject prefabToSpawn;
    public GameObject secondAbility;
    private GameObject objectSpawnedIn;
    private GameObject secondObjectSpawnedIn;

    [Header("Timers")]
    private Timer spawnTimer;
    private Timer secondSpawnTimer;
    private Timer cutOffTime;
    private Timer abilityCooldown;
    private Timer secondAbilityTimer;
    private Timer respawnTimer;


    [Header("Checkpoints")]
    public int currentCheckpoint = 0;
    public Vector3 respawnPosition;
    public Quaternion respawnRotation;
    public bool respawning;



    /// <summary>
    /// Initialize variables and setup the cameras based on how many players are in the game
    /// Start is being called once in the ingame scene.
    /// </summary>
    void Start()
    {
        respawnTimer = new Timer();
        secondSpawnTimer = new Timer();
        cutOffTime = new Timer();
        DontDestroyOnLoad(this);
        controller = GetComponent<CharacterController>();
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

    /// <summary>
    /// Method for setting up the cameras if the first one failed, this is purely a safety net if somehow the code is executed in another order.
    /// This wont affect performance because its a call thats being done once.
    /// </summary>
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

    /// <summary>
    /// Setting up the cameras based on how many players.
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="amount"></param>
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

    /// <summary>
    /// Update of the layers, so everyone sees the correct things in the scene and not each others cameras etc.
    /// </summary>
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

    /// <summary>
    /// After character selection, update the character model and setup the correct ability gameobjects.
    /// </summary>
    public void UpdateMesh()
    {
        //Change character prefab
        Destroy(gameObject.transform.GetChild(0).gameObject);
        Destroy(gameObject.GetComponent<Animator>());
        GameObject characterPrefab = character.characterPrefab.gameObject;
        GameObject prefabSpawnable = GameObject.Instantiate(characterPrefab);
        Destroy(prefabSpawnable.GetComponent<ThirdPersonMovement>());
        Destroy(prefabSpawnable.GetComponent<CharacterController>());
        Destroy(prefabSpawnable.GetComponent<ThirdPersonDash>());
        prefabSpawnable.transform.SetParent(gameObject.transform, false);
        prefabSpawnable.transform.position = prefabSpawnable.transform.parent.transform.position;
        prefabSpawnable.transform.localScale = new Vector3(1, 1, 1);
        _anim = prefabSpawnable.GetComponent<Animator>();
        prefabToSpawn = character.Ability1;
        secondAbility = character.Ability2;

    }

    /// <summary>
    /// Returns the cooldown of the speed ability
    /// </summary>
    /// <returns></returns>
    public int ReturnAbilityCooldown() 
    {
        return Mathf.RoundToInt(abilityCooldown.TimeLeft());
    }


    /// <summary>
    /// return the cooldown of the slow ability
    /// </summary>
    /// <returns></returns>
    public int ReturnSecondAbilityCooldown() 
    {
        return Mathf.RoundToInt(secondAbilityTimer.TimeLeft());
    }

    /// <summary>
    /// Code executed by timers
    /// </summary>
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

        if (secondSpawnTimer.isActive && secondSpawnTimer.TimerDone())
        {
            secondSpawnTimer.StopTimer();
            Destroy(secondObjectSpawnedIn);
        }

        if (cutOffTime.isActive && cutOffTime.TimerDone())
        {
            cutOffTime.StopTimer();
            ChangeCameras();
        }

        if (respawnTimer.isActive && respawnTimer.TimerDone())
        {
            respawnTimer.StopTimer();
            this.canMove = true;
            gameObject.GetComponent<ThirdPersonDash>().canDash = true;
            respawning = false;
        }
    }

    /// <summary>
    /// Executed the first ability through the input manager
    /// </summary>
    public void ExecuteAbility()
    {
        if (!abilityCooldown.isActive && canMove)
        {
            switch (character.characterEnum)
            {
                case Character.Test:
                    SpawnPrefab();
                    break;
                case Character.Test1:
                    SpawnPrefab();
                    break;
                case Character.Test2:
                    SpawnPrefab();
                    break;
                case Character.Test3:
                    SpawnPrefab();
                    break;
                default:
                    break;
            }
            abilityCooldown.SetTimer(character.abilityCooldown);
            GameManager.GetManager<AudioManager>().PlaySound("speedup", false, transform.position, false, transform.gameObject);
        }
        else
        {
            Debug.Log(gameObject.name + " Your first ability is on cooldown: " + abilityCooldown.TimeLeft());
        }
    }
    /// <summary>
    /// Executed the second ability through the input manager
    /// </summary>
    public void ExecuteSecondAbility()
    {
        if (!secondAbilityTimer.isActive && canMove)
        {
            switch (character.characterEnum)
            {
                case Character.Test:
                    SpawnSecondPrefab();
                    break;
                case Character.Test1:
                    SpawnSecondPrefab();
                    break;
                case Character.Test2:
                    SpawnSecondPrefab();
                    break;
                case Character.Test3:
                    SpawnSecondPrefab();
                    break;
                default:
                    break;
            }
            secondAbilityTimer.SetTimer(character.abilityCooldown);
            GameManager.GetManager<AudioManager>().PlaySound("slowdown", false, transform.position, false, transform.gameObject);
        }
        else
        {
            Debug.Log(gameObject.name + " Your second ability is on cooldown: " + abilityCooldown.TimeLeft());
        }
    }


    /// <summary>
    /// Sets the controlscheme for the player
    /// </summary>
    /// <param name="scheme"></param>
    public void SetControlScheme(ControlScheme scheme)
    {
        if (scheme != null)
        {
            ClearControlScheme();
        }
        scheme.AssignInput(this);
        this.scheme = scheme;
    }

    /// <summary>
    /// When character is selected, sets the correct character and scriptable character object
    /// </summary>
    /// <param name="selection"></param>
    public void SelectedCharacter(Character selection)
    {
        character = GameManager.instance.characters[(int)selection];
        UpdateMesh();
        //+1 because arrays start at 0, but we need the actual number of players here.
        if (GameManager.instance.order <= GameManager.instance.amountOfPlayers)
        {
            GameManager.instance.SelectedPlayer();
        }
      
        //Playing sound based on which character has been selected
        switch (character.characterName)
        {
            case "Etienne":
                GameManager.GetManager<AudioManager>().PlaySound("etienne", false, transform.position, false, transform.gameObject);
                break;
            case "Flor":
                GameManager.GetManager<AudioManager>().PlaySound("flor", false, transform.position, false, transform.gameObject);
                break;
            case "Sahin":
                GameManager.GetManager<AudioManager>().PlaySound("sahin", false, transform.position, false, transform.gameObject);
                break;
            case "Milena":
                GameManager.GetManager<AudioManager>().PlaySound("milena", false, transform.position, false, transform.gameObject);
                break;
            default:
                break;
        }
        CineMachineHandler.instance.simpleCameras[playerInt - 1].gameObject.SetActive(true);
        CineMachineHandler.instance.cameras[playerInt - 1].gameObject.SetActive(false);
    }

    /// <summary>
    /// if somehow the scheme is not null it will clear the control scheme at the start
    /// </summary>
    public void ClearControlScheme()
    {
        scheme = null;
    }

    /// <summary>
    /// All mechanics in place for when the player respawns
    /// </summary>
    public void Respawn()
    {
        _anim.SetFloat("speedMovement", 0f);
        Vector3 old = transform.position;
        Vector3 newPos;
        respawning = true;
        this.canMove = false;
        gameObject.GetComponent<ThirdPersonDash>().canDash = false;
        respawnTimer.SetTimer(3f);
        cutOffTime.SetTimer(2.5f);
        GameManager.GetManager<AudioManager>().PlaySound("respawn", false, transform.position, false, transform.gameObject);
        speed = 30f;
        controller.enabled = false;
        print("Test Respawn");
        transform.position = new Vector3(respawnPosition.x, respawnPosition.y, respawnPosition.z);
        newPos = transform.position;
        transform.rotation = respawnRotation;
        controller.enabled = true;
        CineMachineHandler.instance.simpleCameras[playerInt - 1].OnTargetObjectWarped(gameObject.transform, old - newPos);
        CineMachineHandler.instance.cameras[playerInt - 1].OnTargetObjectWarped(gameObject.transform, old - newPos);
        CineMachineHandler.instance.simpleCameras[playerInt - 1].gameObject.SetActive(true);
        CineMachineHandler.instance.cameras[playerInt - 1].gameObject.SetActive(false);
    }

    /// <summary>
    /// Method for switching cameras
    /// being used when respawning
    /// </summary>
    public void ChangeCameras()
    {
        CineMachineHandler.instance.simpleCameras[playerInt - 1].gameObject.SetActive(false);
        CineMachineHandler.instance.cameras[playerInt - 1].gameObject.SetActive(true);
    }


    /// <summary>
    /// Method for when the player jumps
    /// </summary>
    public void Jump()
    {
        if (isGrounded && canMove && canJump)
        {
            movingDirection.y = 80f;
            gravity = 200f;
            GameManager.GetManager<AudioManager>().PlaySound("jump", false, transform.position, false, transform.gameObject);
        }
        if (!isGrounded)
        {
            gravity = 200f;
           //movingDirection.y -= gravity * Time.deltaTime;
            canJump = false;
        }
        else
        {
            canJump = true;
            gravity = 130f;
        }
    }

    /// <summary>
    /// Gets called whenever the player finished the map.
    /// </summary>
    public void ReachedEnd() 
    {
        CineMachineHandler.instance.cameras[playerInt - 1].gameObject.SetActive(true);
        CineMachineHandler.instance.simpleCameras[playerInt - 1].gameObject.SetActive(false);
        CineMachineHandler.instance.cameras[playerInt - 1].m_XAxis.Value = CineMachineHandler.instance.simpleCameras[playerInt - 1].m_XAxis.Value + 0.5f * Time.deltaTime;
        speed = 30f;
        this.canMove = false;
        gameObject.GetComponent<ThirdPersonDash>().canDash = false;

        gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().Play("Offensive Idle", 1);
    }

    /// <summary>
    /// Update gameloop
    /// </summary>
    void Update()
    {
        gameObject.transform.GetChild(0).rotation = gameObject.transform.rotation;
        scheme.Update();
        Timer();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, raycastDistance, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
            canJump = true;
        }
        else
        {
            isGrounded = false;
            canJump = false;
        }

        if (GameManager.instance.currentMatchCountdown >= 9 || respawning)
        {
            movingDirection.y -= gravity * Time.deltaTime;
            movingDirection.y = Mathf.Clamp(movingDirection.y, -gravity, float.MaxValue);
            controller.Move(movingDirection * Time.deltaTime);
        }

        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal" + playerInt) * 0.4f;
            float vertical = Input.GetAxisRaw("Vertical" + playerInt);
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            movingDirection.y -= gravity * Time.deltaTime;
            movingDirection.y = Mathf.Clamp(movingDirection.y, -gravity, float.MaxValue);
            controller.Move(movingDirection * Time.deltaTime);

            if (speed <= maxSpeed)
            {
                speed += acceleration * Time.deltaTime;
            }
            if (speed >= maxSpeed)
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


            //// Carla wrote this 
            //_anim.SetFloat("speedMovement", 0f);

            if (speed == 30f)
            {
                _anim.SetFloat("speedMovement", 0f);
            }
            if (speed > 30f && speed < maxSpeed)
            {
                _anim.SetFloat("speedMovement", 0.3f);
            }
            if (speed >= maxSpeed)
            {
                _anim.SetFloat("speedMovement", 1.0f);
            }

            //transform.position.x = transform.position.x + speed*Time.deltaTime;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
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

    /// <summary>
    /// Called when the game should be paused
    /// </summary>
    public void Pause()
    {
        GameManager.Pause();
        Debug.Log("Paused");
    }

    /// <summary>
    /// When the player collides with wall objects it will stick to them
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (wallLayerMask == (wallLayerMask | (1 << hit.gameObject.layer)))
        {
            Debug.Log("Collided with a wall!");
            isCollidingWithWall = true;
            gravity = 0f;
        }

        if (hit.gameObject.CompareTag("Platform"))
        {
            attached = true;
            this.transform.parent = hit.transform;

        }
    }

    /// <summary>
    /// Fixed update loop for physics
    /// </summary>
    void FixedUpdate()
    {

        if (isCollidingWithWall && !controller.isGrounded)
        {

            Debug.Log("Stopped colliding with a wall!");
            gravity = 130f;
            isCollidingWithWall = false;
            canJump = true;
            isGrounded = true;
        }
        if (attached && !controller.isGrounded)
        {
            attached = false;
            this.transform.parent = null;
        }
    }

    /// <summary>
    /// Trigger Methods for the abilitys
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Slow Area") && other.GetComponent<CheckPlayer>().GrabPlayerInt() != playerInt)
        {
            speed = 50f;
            Debug.LogWarning("Player: " + playerInt + " entered the trigger!  slow down, prefab had this int: " + other.GetComponent<CheckPlayer>().GrabPlayerInt());
        }

        if (other.CompareTag("Fast Area") && other.GetComponent<CheckPlayer>().GrabPlayerInt() == playerInt)
        {
            speed = 300f;
            Debug.LogWarning("Player: "+ playerInt + " entered the trigger! Speed Up, prefab had this int: " + other.GetComponent<CheckPlayer>().GrabPlayerInt());
        }
    }
    //Reset and rework this


   
    /// <summary>
    /// Used when the player jumps or gets off from a moving platform
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            this.transform.parent = null;
        }
    }


    /// <summary>
    /// Enter trigger for when the player steps on the jumppad
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jump Pad"))
        {
            movingDirection.y = launchSpeed;
            GameManager.GetManager<AudioManager>().PlaySound("jumppad", false, Vector3.zero, false, null);
            Debug.Log("Player entered the jump pad trigger!");
        }
    }

    /// <summary>
    /// Spawns first ability prefab
    /// </summary>
    void SpawnPrefab()
    {
        // Instantiate the prefab at the current object's position
        GameObject newPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        newPrefab.AddComponent<CheckPlayer>();
        newPrefab.GetComponent<CheckPlayer>().SetPlayerInt(playerInt);
        newPrefab.SetActive(true);
        objectSpawnedIn = newPrefab;
        spawnTimer.SetTimer(5f);
    }

    /// <summary>
    /// spawns second ability prefab
    /// </summary>
    void SpawnSecondPrefab()
    {
        // Instantiate the prefab at the current object's position
        GameObject newPrefab = Instantiate(secondAbility, transform.TransformPoint(-Vector3.forward * 1), Quaternion.identity);
        newPrefab.AddComponent<CheckPlayer>();
        newPrefab.GetComponent<CheckPlayer>().SetPlayerInt(playerInt);
        newPrefab.SetActive(true);
        secondObjectSpawnedIn = newPrefab;
        secondSpawnTimer.SetTimer(5f);
    }
}
