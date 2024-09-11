using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneControllor : MonoBehaviour {
    private ARAnchorManager _anchorManager;
    private List<ARAnchor> _anchor = new List<ARAnchor>();
    [SerializeField]
    private InputActionReference _toggleBrushColor;

    [SerializeField]
    private InputActionReference _togglePlanesActions;

    [SerializeField]
    private InputActionReference _rightActivateAction;

    [SerializeField]
    private InputActionReference _leftActivateAction;

    [SerializeField]
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor leftRayInteractor;

    [SerializeField]
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor rightDirectInteractor;

    [SerializeField]
    private GameObject cube;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private LineGenerator lineGenerator;

    [SerializeField]
    private GameObject colorChangePanel;


    // Start is called before the first frame update

    private ARPlaneManager _planeManager;
    private ARBoundingBoxManager _aRBoundingBoxManager;
    private int visualizationMode = 2;
    private bool _isVisible = true;
    private int _numPlanesAddedOccoured = 0;
    private bool changeColor = false;

    void Start()
    {
        _anchorManager = GetComponent<ARAnchorManager>();
        if (_anchorManager is null) {
            Debug.Log("Anchor not logged");
        }
        _aRBoundingBoxManager = GetComponent<ARBoundingBoxManager>();
        if (_aRBoundingBoxManager is null)
        {
            Debug.Log("Anchor not logged");
        }
        _planeManager = GetComponent<ARPlaneManager>();
        _toggleBrushColor.action.performed += ColorChange;
        _togglePlanesActions.action.performed += Action_performed;
        _aRBoundingBoxManager.trackablesChanged.AddListener(OnBoxesChanged);
        _planeManager.trackablesChanged.AddListener(_planeManager_planesChanged);

        _rightActivateAction.action.started += OnRightActivationAction;
        _rightActivateAction.action.canceled += OnRightDectivationAction;
        //_rightActivateAction.action.performed += OnRightActivationAction;

        _leftActivateAction.action.performed += OnLeftActivationAction;
        _anchorManager.trackablesChanged.AddListener(OnAnchorChanged);
        rightDirectInteractor.selectEntered.AddListener(OnGrab);
        rightDirectInteractor.selectExited.AddListener(OnRelease);
        //SetVisualization(visualizationMode);
    }

    private void ColorChange(InputAction.CallbackContext obj)
    {
        changeColor = !changeColor;
        colorChangePanel.SetActive(changeColor);
    }

    //private void OnDestroy()
    //{
    //    rightDirectInteractor.selectEntered.RemoveListener(OnGrab);
    //    rightDirectInteractor.selectExited.RemoveListener(OnRelease);
    //}



    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Object grabbed");
        ARAnchor aRAnchor = args.interactableObject.transform.gameObject.GetComponent<ARAnchor>();
        Destroy(aRAnchor);
        // Add your custom logic for when the object is grabbed
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        //Debug.Log("Object released");
        //if (args.interactableObject.transform.GetComponent<ARAnchor>() == null)
        //{
        //    ARAnchor anchor = args.interactableObject.transform.gameObject.AddComponent<ARAnchor>();
        //    if (anchor != null)
        //    {
        //        _anchor.Add(anchor);
        //    }
        //}
        // Add your custom logic for when the object is released
    }

    private void OnAnchorChanged(ARTrackablesChangedEventArgs<ARAnchor> args)
    {   foreach (var removedAnchor in args.removed) {
            if (removedAnchor.Value != null) {
                _anchor.Remove(removedAnchor.Value);
                Destroy(removedAnchor.Value.gameObject);
            }
            
        }
        //foreach (var updatedAnchor in obj.updated)
        //{
        //    _anchor.Remove(updatedAnchor);
            
        //}
        throw new System.NotImplementedException();
    }

    private void OnLeftActivationAction(InputAction.CallbackContext obj)
    {
        Debug.Log("OnLeftActivationAction called 0");
        checkIfRayHitsCollider();
        throw new System.NotImplementedException();
    }

    private void checkIfRayHitsCollider() {
        Debug.Log("OnLeftActivationAction called 1");
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Debug.Log("OnLeftActivationAction called 2");
            Debug.Log("RAYCAST HIT THE OBJECT : " + hit.transform.name);
            Debug.Log("OnLeftActivationAction called 3");
            Quaternion rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            Debug.Log("OnLeftActivationAction called 4");
            Pose pose = new Pose(hit.point, rotation);
            Debug.Log("OnLeftActivationAction called 5 : "+pose);
            CreateAnchorAsync(pose);
            Debug.Log("OnLeftActivationAction called 6");
        }
        else {
            Debug.Log("RAYCAST HIT THE OBJECT : NONE");
        }
    }

    private async void CreateAnchorAsync(Pose pose)
    {
        Debug.Log("OnLeftActivationAction called 7");
        var result = await _anchorManager.TryAddAnchorAsync(pose);
        Debug.Log("OnLeftActivationAction called 8");
        if (result.status.IsSuccess()) {
            Debug.Log("OnLeftActivationAction called 9");
            ARAnchor anchor = result.value as ARAnchor;
            Debug.Log("OnLeftActivationAction called 10 : "+ anchor);
            try
            {
                _anchor.Add(anchor);
            }
            catch (Exception e) {
                Debug.Log("OnLeftActivationAction called error : "+ e.ToString());
            }
            
            Debug.Log("OnLeftActivationAction called 11");
            GameObject instance = Instantiate(prefab, anchor.pose.position, anchor.pose.rotation);
            Debug.Log("OnLeftActivationAction called 12");
            instance.transform.SetParent(anchor.transform);
            Debug.Log("OnLeftActivationAction called 13");
        }
    }

    private void OnRightActivationAction(InputAction.CallbackContext obj)
    {
        lineGenerator.SprayColor();
        //spawnGrababbleCube();
    }

    private void OnRightDectivationAction(InputAction.CallbackContext obj)
    {
        lineGenerator.StopSprayColor();
        //spawnGrababbleCube();
    }

    private void spawnGrababbleCube() {
        Vector3 spawnPosition;

        foreach (var plane in _planeManager.trackables) {
            if (plane.classifications.HasFlag(PlaneClassifications.Floor)) {
                spawnPosition = plane.transform.position;
                spawnPosition.y += 0.3f;
                Instantiate(cube, spawnPosition, Quaternion.identity);
            }
        }
    }

    private void _planeManager_planesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        if (args.added.Count > 0) {
            _numPlanesAddedOccoured++;
            foreach (var plane in _planeManager.trackables) {
                PrintPlaneLabels(plane);
            }
            SetVisualization(visualizationMode);
        }

    }

    private void OnBoxesChanged(ARTrackablesChangedEventArgs<ARBoundingBox> args)
    {
        if (args.added.Count > 0)
        {
            foreach (var plane in _aRBoundingBoxManager.trackables)
            {
                //PrintPlaneLabels(plane);
            }
            SetVisualization(visualizationMode);
        }
    }

    private void PrintPlaneLabels(ARPlane plane) {
        string label = plane.classifications.ToString();
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

    public void TogglePlanesAndBoundingBoxes() {
        Debug.Log("XRI-Action-Performed : Action Perfomed 1");
        visualizationMode = (visualizationMode + 1) % 3;
        Debug.Log("XRI-Action-Performed : Action Perfomed 2");
        SetVisualization(visualizationMode);
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        
        Debug.Log("XRI-Action-Performed : Action Perfomed 1");
        visualizationMode = (visualizationMode + 1) % 3;
        Debug.Log("XRI-Action-Performed : Action Perfomed 2");
        SetVisualization(visualizationMode);
        //_isVisible = !_isVisible;
        //float fillAlpha = _isVisible ? 0.3f : 0f;
        //float lineAlpha = _isVisible ? 1.0f : 0f;
        //foreach (var plane in _planeManager.trackables)
        //{
        //    SetPlaneAlpha(plane, fillAlpha, lineAlpha);
        //}
    }

    private void SetVisualization(int mode) {
        Debug.Log("XRI-Action-Performed : Action Perfomed 3 : Mode : "+mode);
        switch (mode){
            case 1:
                Debug.Log("XRI-Action-Performed : Action Perfomed 4");
                SetPlanesVisibility(true);
                SetBoundingBoxesVisibility(false);
                break;
            case 2:
                Debug.Log("XRI-Action-Performed : Action Perfomed 5");
                SetPlanesVisibility(false);
                SetBoundingBoxesVisibility(true);
                break;
            default:
                Debug.Log("XRI-Action-Performed : Action Perfomed 6");
                SetPlanesVisibility(false);
                SetBoundingBoxesVisibility(false);
                break;
        }
    }

    private void SetPlanesVisibility(bool isVisible) {
        float fillAlpha = isVisible ? 0.3f : 0f;
        float lineAlpha = isVisible ? 1.0f : 0f;
        foreach (var plane in _planeManager.trackables)
        {
            SetTrackableAlpha(plane, fillAlpha, lineAlpha);
        }
    }

    private void SetBoundingBoxesVisibility(bool isVisible)
    {
        float fillAlpha = isVisible ? 0.3f : 0f;
        float lineAlpha = isVisible ? 1.0f : 0f;
        foreach (var box in _aRBoundingBoxManager.trackables)
        {
            Debug.Log("Bounding box : "+ box);
            SetTrackableAlpha(box, fillAlpha, lineAlpha);
        }
    }

    private void SetTrackableAlpha(ARTrackable trackable, float fillAlpha, float LineAlpha) {
        var meshRenderer = trackable.GetComponentInChildren<MeshRenderer>();
        var lineRenderer = trackable.GetComponentInChildren<LineRenderer>();

        if (meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = fillAlpha;
            meshRenderer.material.color = color;
        }

        if (lineRenderer != null)
        {
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;

            startColor.a = LineAlpha;
            endColor.a = LineAlpha;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }

    private void OnDestroy()
    {
        _rightActivateAction.action.performed -= OnRightActivationAction;
        _togglePlanesActions.action.performed -= Action_performed;
        _planeManager.trackablesChanged.RemoveListener(_planeManager_planesChanged);
        _leftActivateAction.action.performed -= OnLeftActivationAction;
        _anchorManager.trackablesChanged.RemoveListener(OnAnchorChanged);
        rightDirectInteractor.selectEntered.RemoveListener(OnGrab);
        rightDirectInteractor.selectExited.RemoveListener(OnRelease);
        _aRBoundingBoxManager.trackablesChanged.RemoveListener(OnBoxesChanged);
        _toggleBrushColor.action.performed -= ColorChange;
    }
}
