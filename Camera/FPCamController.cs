using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPCamController : MonoBehaviour
{
    public static FPCamController instance { get; private set; }


    [SerializeField] private Sprite crosshairImage;
    [SerializeField] private Color crosshairColor = Color.white;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private Transform orientation;
    [Header("Model")]
    [SerializeField] private GameObject _playerModel;


    private Image crosshairObject;

    private float xRot;
    private float yRot;
    public bool isMenuActive = false;

    #region Mouse Smoothing Variables
    private float xAccumulator;
    private float yAccumulator;
    
    private const float snappiness = 10f; //Larger value = less filtering, more responsiveness.

    [Header("Mouse Smoothing")]
    [SerializeField] private bool enableMouseSmoothing = false;
    #endregion

    #region Head Bob
    [Header("Head Bob")]
    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);


    // Internal Variables
    private bool isHeadBobEnabledOnStart;
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        crosshairObject = GetComponentInChildren<Image>();
        crosshairObject.sprite = crosshairImage;
        crosshairObject.color = crosshairColor;
    }

    private void Start()
    {
        isHeadBobEnabledOnStart = enableHeadBob;
        jointOriginalPos = joint.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isMenuActive)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            if(enableMouseSmoothing)
            {
                xAccumulator = Mathf.Lerp(xAccumulator, mouseX, snappiness * Time.deltaTime);
                yAccumulator = Mathf.Lerp(yAccumulator, mouseY, snappiness * Time.deltaTime);

                yRot += xAccumulator;
                xRot -= yAccumulator;
            }
            else
            {
                yRot += mouseX;
                xRot -= mouseY;
            }
            
            xRot = Mathf.Clamp(xRot, -90f, 60f);

            _playerModel.transform.rotation = Quaternion.Euler(0, yRot, 0);
        
            transform.rotation = Quaternion.Euler(xRot, yRot, 0);
            orientation.rotation = Quaternion.Euler(0, yRot, 0);
        }

        if (enableHeadBob)
        {
            HeadBob();
        }
    }

    public void Activate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Deactivate()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void HeadBob()
    {
        if (PlayerController.instance.isWalking)
        {
            // Calculates HeadBob speed during walking
            timer += Time.deltaTime * bobSpeed;
            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            // Resets when player stops moving
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }

    public void HeadBobCheck()
    {
        if(isHeadBobEnabledOnStart)
        {
            enableHeadBob = true;
        }
        else
        {
            enableHeadBob = false;
        }
    }
}