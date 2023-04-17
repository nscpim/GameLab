using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Actor
{
    private ControlScheme scheme;
    private Camera mainCamera;
    public int playerInt;
    [HideInInspector] public ScriptableCharacter character;
    private Vector3 moveDirection = Vector3.zero;
    private float timeStamp;
    public bool canSelectCharacter;
    private Timer abilityCooldown;
    public bool canMove;

    [Header("Variables")]
    public CharacterController controller;
    private Vector3 movingDirection = Vector3.zero;
    public LayerMask wallLayerMask;
    public Transform cam;
    private bool isCollidingWithWall = false;
    float turnSmoothVelocity;
    public int currentCheckpoint = 0;
    public Vector3 respawnPosition;

    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform point, cameraPos;
    [SerializeField] private LayerMask grabbable;

    [Header("SwingAbility")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;


    //Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        mainCamera = GetComponentInChildren<Camera>();
        SetViewPortRect(gameObject.name, GameManager.instance.GetAmountOfPlayers());
        abilityCooldown = new Timer();

    }

    // Update is called once per frame
    void Update()
    {
        scheme.Update();
        Timer();
        CharMovement();
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
    public void SetControlScheme(ControlScheme scheme)
    {
        if (scheme != null)
        {
            ClearControlScheme();
        }
        scheme.AssignInput(this);
        this.scheme = scheme;
        
    }
    public void ClearControlScheme()
    {
        scheme = null;
    }

    //Integrate the ScriptableObjects in these methods.
    public void Jump()
    {
        if (controller.isGrounded && canMove)
        {
            movingDirection.y = character.jumpSpeed;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (wallLayerMask == (wallLayerMask | (1 << hit.gameObject.layer)))
        {
            Debug.Log("Collided with a wall!");
            isCollidingWithWall = true;
            character.gravity = 0f;
        }
    }
    void FixedUpdate()
    {
        if (character != null && canMove)
        {
            if (isCollidingWithWall && !controller.isGrounded)
            {

                Debug.Log("Stopped colliding with a wall!");
                character.gravity = 200f;
                isCollidingWithWall = false;
            }
        }
    }

    public void Respawn()
    {
        print("DoTheThing");
        controller.enabled = false;
        transform.position = new Vector3(respawnPosition.x, respawnPosition.y, respawnPosition.z);
        controller.enabled = true;
    }

    public void Movement() 
    {
    
    }
    public void CharMovement()
    {
        if (character != null && canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal" + playerInt);
            float vertical = Input.GetAxisRaw("Vertical" + playerInt);

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


            movingDirection.y -= character.gravity * Time.deltaTime;
            controller.Move(movingDirection * Time.deltaTime);

            if (character.speed < character.maxSpeed)
            {
                character.speed += character.acceleration * Time.deltaTime;
            }

            if (!this.transform.hasChanged)
            {
                character.speed = 100f;
            }
            transform.hasChanged = false;

            //transform.position.x = transform.position.x + speed*Time.deltaTime;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, character.turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * character.speed * Time.deltaTime);
            }
        }
    }

    public void ExecuteAbility()
    {
        if (!abilityCooldown.isActive && canMove)
        {
            switch (character.characterEnum)
            {
                case Character.Test:
                    StartSwing();
                    Debug.Log("Executed Ability, WOW!");
                    break;
                case Character.Test1:
                    Debug.Log("Executed Ability, WOW!");
                    break;
                case Character.Test2:
                    Debug.Log("Executed Ability, WOW!");
                    break;
                case Character.Test3:
                    Debug.Log("Executed Ability, WOW!");
                    break;
                default:
                    break;
            }
            abilityCooldown.SetTimer(character.abilityCooldown);
        }

        else
        {
            Debug.Log(gameObject.name + " Your ability is on cooldown:" + abilityCooldown.TimeLeft());
        }
    }

    public void StartSwing()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, maxSwingDistance, grabbable))
        {
            swingPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(gameObject.transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
            currentGrapplePosition = point.position;
        }
    }

    public void StopSwing()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    public void LateUpdate()
    {
        DrawRope();
    }

    public void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lineRenderer.SetPosition(0, point.position);
        lineRenderer.SetPosition(1, swingPoint);
    }

    public void AirControlToLeft()
    {
        if (joint != null)
        {

        }
    }

    public void AirControlToRight()
    {
        if (joint != null)
        {

        }
    }
    public void AirControlToForward()
    {
        if (joint != null)
        {

        }
    }
    public void ShortenCable()
    {
        if (joint != null)
        {

        }

    }
    public void CableExtender()
    {

        if (joint != null)
        {

        }

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

}
