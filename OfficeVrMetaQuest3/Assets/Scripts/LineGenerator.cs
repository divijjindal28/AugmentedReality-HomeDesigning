using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    Line activeLine;
    [SerializeField] GameObject pointer;
    bool stopOrSprayCalled = false;
    Color LaserPointerColor;
    [SerializeField]
    private GameObject LineArt;

    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private MeshCollider meshCollider;
    //private InputData _inputData;

    private void Start()
    {
        //_inputData = GetComponent<InputData>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log("Pointer Position 1");
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        //{
        //    Debug.Log("Pointer Position 2");
        //    GameObject newLine = Instantiate(linePrefab);
        //    activeLine = newLine.GetComponent<Line>();
        //}

        //if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        //{
        //    Debug.Log("Pointer Position 3");
        //    activeLine = null;
        //}

        //if (activeLine != null)
        //{
        //    Debug.Log("Pointer Position 4");
        //    activeLine.UpdateLine(pointer.transform.position);
        //}

        //if (_inputData._rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        //{
        //    Debug.Log("ShowSpray : triggerValue " + triggerValue);
        //    if (triggerValue > 0.50)
        //    {
        //        Debug.Log("Pointer Position 1");
        //        ShowSpray();
        //        GameObject newLine = Instantiate(linePrefab);
        //        Debug.Log("Pointer Position 2");
        //        activeLine = newLine.GetComponent<Line>();
        //    }
        //    else {
        //        HideSpray();
        //        activeLine.StartErazing();
        //        activeLine = null;
        //    }
        //}

        //if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool Abutton))
        //{
        //    Debug.Log("ShowSpray : triggerValue " + Abutton);
        //    Debug.Log("Pointer Position 1");
        //    ShowSpray();
        //    GameObject newLine = Instantiate(linePrefab);
        //    Debug.Log("Pointer Position 2");
        //    activeLine = newLine.GetComponent<Line>();
        //}
        //else
        //{
        //    HideSpray();
        //    activeLine.StartErazing();
        //    activeLine = null;
        //}

        //if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        //{
        //    Debug.Log("Pointer Position 1");
        //    ShowSpray();
        //    GameObject newLine = Instantiate(linePrefab);
        //    Debug.Log("Pointer Position 2");
        //    activeLine = newLine.GetComponent<Line>();
        //}

        //if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)) {
        //    HideSpray();
        //    activeLine.StartErazing();
        //    activeLine = null;

        //}
        
        Debug.Log("Pointer Position 3 : " + activeLine);
        Debug.Log("Pointer Position 3 : " + activeLine.gameObject.activeSelf);

        if (activeLine != null)
        {
            
            activeLine.UpdateLine(pointer.transform.position);
            Debug.Log("Pointer Position 9");
            
        }


    }


    public void SprayColor() {
        stopOrSprayCalled = true;
        Debug.Log("Pointer Position 1");
        //ShowSpray();
        GameObject newLine = Instantiate(linePrefab);
        
        Debug.Log("SprayColor : "+ LineArt.transform.childCount);
        //LineArt.transform.position = newLine.transform.position;


        //newLine.transform.position = LineArt.transform.position;
        Debug.Log("Pointer Position 2");
        activeLine = newLine.GetComponent<Line>();
    }

    public void StopSprayColor()
    {
        
        stopOrSprayCalled = true;
        //HideSpray();
        //activeLine.StartErazing();
        //Mesh bakeMesh = new Mesh();

        //// Bake the LineRenderer into the mesh
        //activeLine.GetComponent<LineRenderer>().BakeMesh(bakeMesh);

        //// Assign the baked mesh to the MeshFilter
        //meshFilter.mesh = bakeMesh;
        //LineArt.transform.position = activeLine.transform.position ;
        //Debug.Log("StopSprayColor : "+ activeLine.transform);
        //Debug.Log("StopSprayColor : " + activeLine.transform.position);
        //meshCollider.sharedMesh = bakeMesh;
        activeLine.transform.parent = LineArt.transform;
        activeLine.GetComponent<LineRenderer>().useWorldSpace = false;
        activeLine = null;
        
    }


    //private void ShowSpray()
    //{
    //    LaserPointerColor.a = 0f;
    //    try
    //    {
    //        Debug.Log("ShowSpray : " + Spray.gameObject.activeSelf);
    //        Debug.Log("ShowSpray : " + Spray.GetComponent<ParticleSystem>().particleCount);
    //        Spray.gameObject.SetActive(true);
    //        Spray.GetComponent<ParticleSystem>().Play();
            
    //        Debug.Log("ShowSpray : " + Spray.gameObject.activeSelf);
    //        Debug.Log("ShowSpray : " + Spray.GetComponent<ParticleSystem>().particleCount);
    //    }
    //    catch (Exception e) {
    //        Debug.Log("ShowSpray : "+ e.ToString());
    //    }
        
    //    ChangePointerApha(0.0f);
    //}

    //private void HideSpray()
    //{
    //    LaserPointerColor.a = 1f;
    //    LaserPointer.GetComponent<LineRenderer>().startColor = LaserPointerColor;
    //    Spray.gameObject.SetActive(false);
    //    Spray.GetComponent<ParticleSystem>().Stop();
    //    ChangePointerApha(1.0f);
    //}

    private void ChangePointerApha(float alpha)
    {
        Renderer pointersRenderer = pointer.GetComponent<Renderer>();
        Color pointersColor = pointersRenderer.material.color;
        pointersColor.a = alpha;
        pointersRenderer.material.color = pointersColor;
    }
}
