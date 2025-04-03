using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] private GameObject placeableObjPrefab;
    [SerializeField] private GameObject previewObjPrefab;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask placementSurfaceLayerMask;

    [Header("Raycast Parameters")]
    [SerializeField] private float objectDistanceFromPlayer;
    [SerializeField] private float raycastStartVerticalOffset;
    [SerializeField] private float raycastDistance;
    
    [Header("Preview Material")]
    [SerializeField] private Material previewMaterial;
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;

    private GameObject _previewObj = null;
    private Vector3 _currentPlacementPosition = Vector3.zero;
    private bool _inPlacementMode = false;
    private bool _validPreviewState = false;
    
    private void Update()
    {
        UpdateInput();

        if (_inPlacementMode)
        {
            UpdateCurrentPlacementPosition();
            
            if(CanPlaceObject())
                SetValidPreviewState();
            else
                SetInvalidPreviewState();
        }
    }

    private void UpdateCurrentPlacementPosition()
    {
        Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized; 
        
        Vector3 startPos = playerCamera.transform.position + (cameraForward * objectDistanceFromPlayer);
        startPos.y += raycastStartVerticalOffset;
        
        RaycastHit hit;
        if (Physics.Raycast(startPos, Vector3.down, out hit, raycastDistance, placementSurfaceLayerMask))
        {
            _currentPlacementPosition = hit.point;
        }
        
        Quaternion rotation = Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y, 0);
        _previewObj.transform.position = _currentPlacementPosition;
        _previewObj.transform.rotation = rotation;
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnterPlacementMode();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ExitPlacementMode();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        if (!_inPlacementMode || !_validPreviewState) return;
        
        Quaternion rotation = Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y, 0);
        Instantiate(placeableObjPrefab, _currentPlacementPosition, rotation, transform);
        
        ExitPlacementMode();
    }

    #region Preview Checks
    private bool CanPlaceObject()
    {
        if (_previewObj == null) return false;

        return _previewObj.GetComponentInChildren<PreviewObjectChecker>().IsValid;
    }

    private void SetValidPreviewState()
    {
        previewMaterial.color = validColor;
        _validPreviewState = true;
    }

    private void SetInvalidPreviewState()
    {
        previewMaterial.color = invalidColor;
        _validPreviewState = false;
    }
    #endregion
    
    private void EnterPlacementMode()
    {
        if(_inPlacementMode) return;
        
        Quaternion rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
        _previewObj = Instantiate(previewObjPrefab, _currentPlacementPosition, rotation, transform);
        _inPlacementMode = true;
    }

    private void ExitPlacementMode()
    {
        Destroy(_previewObj);
        _previewObj = null;
        _inPlacementMode = false;
    }
}
