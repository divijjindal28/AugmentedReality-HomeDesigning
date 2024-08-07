using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneControllor : MonoBehaviour {
    private ARAnchorManager _anchorManager;
    private List<ARAnchor> _anchor = null;
    [SerializeField]
    private InputActionReference _togglePlanesActions;

    [SerializeField]
    private InputActionReference _rightActivateAction;

    [SerializeField]
    private InputActionReference _leftActivateAction;

    [SerializeField]
    private XRRayInteractor leftRayInteractor;

    [SerializeField]
    private XRDirectInteractor rightDirectInteractor;

    [SerializeField]
    private GameObject cube;

    [SerializeField]
    private GameObject prefab;
    // Start is called before the first frame update

    private ARPlaneManager _planeManager;
    private bool _isVisible = true;
    private int _numPlanesAddedOccoured = 0;
    void Start()
    {
        _anchorManager = GetComponent<ARAnchorManager>();
        if (_anchorManager is null) {
            Debug.Log("Anchor not logged");
        }
        _planeManager = GetComponent<ARPlaneManager>();
        _togglePlanesActions.action.performed += Action_performed;
        _planeManager.planesChanged += _planeManager_planesChanged;
        _rightActivateAction.action.performed += OnRightActivationAction;
        _leftActivateAction.action.performed += OnLeftActivationAction;
        _anchorManager.anchorsChanged += OnAnchorChanged;
        rightDirectInteractor.selectEntered.AddListener(OnGrab);
        rightDirectInteractor.selectExited.AddListener(OnRelease);
    }

    //private void OnDestroy()
    //{
    //    rightDirectInteractor.selectEntered.RemoveListener(OnGrab);
    //    rightDirectInteractor.selectExited.RemoveListener(OnRelease);
    //}

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Object grabbed");
        ARAnchor aRAnchor = args.interactable.gameObject.GetComponent<ARAnchor>();
        Destroy(aRAnchor);
        // Add your custom logic for when the object is grabbed
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Object released");
        if (args.interactable.GetComponent<ARAnchor>() == null)
        {
            ARAnchor anchor = args.interactable.gameObject.AddComponent<ARAnchor>();
            if (anchor != null)
            {
                _anchor.Add(anchor);
            }
        }
        // Add your custom logic for when the object is released
    }

    private void OnAnchorChanged(ARAnchorsChangedEventArgs obj)
    {   foreach (var removedAnchor in obj.removed) {
            _anchor.Remove(removedAnchor);
            Destroy(removedAnchor.gameObject);
        }
        //foreach (var updatedAnchor in obj.updated)
        //{
        //    _anchor.Remove(updatedAnchor);
            
        //}
        throw new System.NotImplementedException();
    }

    private void OnLeftActivationAction(InputAction.CallbackContext obj)
    {
        checkIfRayHitsCollider();
        throw new System.NotImplementedException();
    }

    private void checkIfRayHitsCollider() {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Debug.Log("RAYCAST HIT THE OBJECT : " + hit.transform.name);
            Quaternion rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            GameObject instance = Instantiate(prefab, hit.point, rotation);

            if (instance.GetComponent<ARAnchor>() == null) {
                ARAnchor anchor = instance.AddComponent<ARAnchor>();
                if (anchor != null) {
                    _anchor.Add(anchor);
                }
            }
        }
        else {
            Debug.Log("RAYCAST HIT THE OBJECT : NONE");
        }
    }

    private void OnRightActivationAction(InputAction.CallbackContext obj)
    {
        spawnGrababbleCube();
    }

    private void spawnGrababbleCube() {
        Vector3 spawnPosition;

        foreach (var plane in _planeManager.trackables) {
            if (plane.classification == PlaneClassification.Floor) {
                spawnPosition = plane.transform.position;
                spawnPosition.y += 0.3f;
                Instantiate(cube, spawnPosition, Quaternion.identity);
            }
        }
    }

    private void _planeManager_planesChanged(ARPlanesChangedEventArgs obj)
    {
        if (obj.added.Count > 0) {
            _numPlanesAddedOccoured++;
            foreach (var plane in _planeManager.trackables) {
                PrintPlaneLabels(plane);
            }
        }


    }

    private void PrintPlaneLabels(ARPlane plane) {
        string label = plane.classification.ToString();
        string log = $"Plane ID :{plane.trackableId} label: {label}";
        Debug.Log(log);
    }

    private void SetPlaneAlpha(ARPlane plane, float fillAlpha, float lineAlpha) {
        var meshRenderer = plane.GetComponentInChildren<MeshRenderer>();
        var lineRenderer = plane.GetComponentInChildren<LineRenderer>();

        if (meshRenderer != null) {
            Color color = meshRenderer.material.color;
            color.a = fillAlpha;
            meshRenderer.material.color = color;
        }

        if (lineRenderer != null) {
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;

            startColor.a = lineAlpha;
            endColor.a = lineAlpha;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        _isVisible = !_isVisible;
        float fillAlpha = _isVisible ? 0.3f : 0f;
        float lineAlpha = _isVisible ? 1.0f : 0f;
        foreach (var plane in _planeManager.trackables)
        {
            SetPlaneAlpha(plane, fillAlpha, lineAlpha);
        }
    }

    private void OnDestroy()
    {
        _rightActivateAction.action.performed -= OnRightActivationAction;
        _togglePlanesActions.action.performed -= Action_performed;
        _planeManager.planesChanged -= _planeManager_planesChanged;
        _leftActivateAction.action.performed -= OnLeftActivationAction;
        _anchorManager.anchorsChanged -= OnAnchorChanged;
        rightDirectInteractor.selectEntered.RemoveListener(OnGrab);
        rightDirectInteractor.selectExited.RemoveListener(OnRelease);
    }
}
