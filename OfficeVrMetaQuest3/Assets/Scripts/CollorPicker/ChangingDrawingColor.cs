using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingDrawingColor : MonoBehaviour
{
    [SerializeField] public ParticleSystem sprayParticleSystem;
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public Color color;
    private GradientColorKey[] colorKeys;
    private Color[] colors;
    private float[] colorKeyPositions;
    private void Start()
    {
        colors = new Color[] {
            new Color(color.r, color.g, color.b, 1f),
            new Color(color.r, color.g, color.b, 1f),
            new Color(color.r, color.g, color.b, 0f),
        };

        colorKeyPositions = new float[] {0f, 0.5f, 1f };
        colorKeys = new GradientColorKey[colors.Length];
    }
    // Start is called before the first frame update
    public void ChangeColor()
    {
        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Blend;

        for (int i = 0; i < Mathf.Min(colors.Length, colorKeyPositions.Length); i++)
        {
            colorKeys[i] = (new GradientColorKey(colors[i], colorKeyPositions[i]));
        }
        gradient.SetKeys(colorKeys, new GradientAlphaKey[0]);

        var colorOverLifetimeModule = sprayParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(gradient);

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

    }

}
