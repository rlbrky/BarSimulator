using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    [Header("Objects")] 
    public Transform holdPoint;
    
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private Transform orientation;

    [Header("CameraManagement")]
    //[SerializeField] private CamSwitchMan cameraManager;
    [SerializeField] private KeyCode camKey;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 movementDir;
    private Rigidbody rb;
    [Header("Anims")]
    //[SerializeField] private Animator _animator;
    public bool isWalking;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        Inputs();
        SpeedControl();

        //if (Input.GetKeyDown(camKey))
          //  cameraManager.SwitchCamera();

        //Handle drag
        rb.drag = groundDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //Calc movement direction to always walk the way you are looking.
        movementDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(movementDir.normalized * moveSpeed * 10f, ForceMode.Force);

        if (rb.velocity != Vector3.zero)
        {
            isWalking = true;
            //_animator.SetBool("isWalking", true);
        }
        else
        {
            isWalking = false;
            //_animator.SetBool("isWalking", false);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    public void StopMovement()
    {
        //_animator.SetBool("isWalking", false);
        instance.enabled = false;
    }

    public void OpenPhone()
    {
        //_animator.SetBool("isPhoneOpen", true);
        //cameraManager.SwitchToPhoneCamera();
    }

    public void ClosePhone()
    {
        //_animator.SetBool("isPhoneOpen", false);
    }
}
