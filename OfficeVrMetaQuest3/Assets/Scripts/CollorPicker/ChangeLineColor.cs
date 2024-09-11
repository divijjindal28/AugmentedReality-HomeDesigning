

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLineColor : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Color color;
    
    private GradientColorKey[] colorKeys;
    private Color[] colors;
    private float[] colorKeyPositions;
    private float defaultLineRendererVal = 0.01442909f;
    private void Start()
    {
        colors = new Color[] {
            new Color(color.r, color.g, color.b, 1f),
            new Color(color.r, color.g, color.b, 1f),
            new Color(color.r, color.g, color.b, 0f),
        };

        colorKeyPositions = new float[] { 0f, 0.5f, 1f };
        colorKeys = new GradientColorKey[colors.Length];
    }

    private void Update()
    {
        float startWidth = lineRenderer.startWidth;
        float endWidth = lineRenderer.endWidth;

        Debug.Log("ChangeLineWidth Start Width: " + startWidth);
        Debug.Log("ChangeLineWidth End Width: " + endWidth);
    }

    // Start is called before the first frame update
    public void ChangeColor(Color color)
    {
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;

        for (int i = 0; i < Mathf.Min(colors.Length, colorKeyPositions.Length); i++)
        {
            colorKeys[i] = (new GradientColorKey(colors[i], colorKeyPositions[i]));
        }
        gradient.SetKeys(colorKeys, new GradientAlphaKey[0]);

        

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

    }


    public void ChangeLineWidth()
    {
        try {
            Debug.Log("ChangeLineWidth2 Started ");
            float width = slider.value * defaultLineRendererVal;
            Debug.Log("ChangeLineWidth2 : " + width);
            AnimationCurve widthCurve = lineRenderer.widthCurve;
            Debug.Log("ChangeLineWidth2 : 1");
            float[] newWidths = { width, width, 0 };
            Debug.Log("ChangeLineWidth2 : 2");
            for (int i = 0; i < newWidths.Length; i++)
            {
                Debug.Log("ChangeLineWidth2 : 3");
                Keyframe key = widthCurve[i];
                key.value = newWidths[i];
                widthCurve.MoveKey(i, key); // Update the key with the new width value
            }
            Debug.Log("ChangeLineWidth2 : 4");
            lineRenderer.widthCurve = widthCurve;
            Debug.Log("ChangeLineWidth2 Ended: ");
        } catch (Exception e) {
            Debug.Log("ChangeLineWidth2 Error : "+ e);
        }
        
    }

}

