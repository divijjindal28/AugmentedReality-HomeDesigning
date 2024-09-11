using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ColorPickerControl : MonoBehaviour
{
    public float currentHue, currentSat, currentVal;

    [SerializeField]
    private RawImage hueImage, setValImage, outputImage;

    [SerializeField]
    private Slider hueSlider;
     
    private Texture2D hueTexture, svTexture, outputTexture;

    [SerializeField]
    MeshRenderer changeThis;

    [SerializeField]
    ChangeLineColor changeLineColor;

    private void Start()
    {
        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();
        UpdateOutputImage();
    }
    private void CreateHueImage() {
        
        hueTexture = new Texture2D(16, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int y = 0; y < hueTexture.height; y++)
        {
            for (int x = 0; x < hueTexture.width; x++)
            {
                hueTexture.SetPixel(x, y, Color.HSVToRGB(
                    (float)y / hueTexture.height,
                    1,
                    1
                    ));
            }
        }

        hueTexture.Apply();
        currentHue = 0;
        hueImage.texture = hueTexture;

    }

    private void CreateSVImage() {
        svTexture = new Texture2D(16, 16);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SetValTexture";

        for (int y = 0; y < svTexture.height; y++) {
            for (int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, y, Color.HSVToRGB(
                    currentHue,
                    (float)x / svTexture.width,
                    (float)y / svTexture.height
                    ));
            }
        }

        svTexture.Apply();
        currentSat = 0;
        currentVal = 0;
        setValImage.texture = svTexture;
    }

    private void CreateOutputImage() {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            hueTexture.SetPixel(0, i, currentColor);
        }

        hueTexture.Apply();
        outputImage.texture = outputTexture;
    }

    private void UpdateOutputImage() {
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++) {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();
        changeThis.GetComponent<MeshRenderer>().material.color = currentColor;
        changeLineColor.ChangeColor(currentColor);
    }

    public void SetSV(float S, float V) {
        currentSat = S;
        currentVal = V;
        UpdateOutputImage();
    }

    public void UpdateSVImage() {



        currentHue = hueSlider.value;
        for (int y = 0; y < svTexture.height; y++) {
            for (int x = 0; x < svTexture.width; x++) {
                svTexture.SetPixel(x, y, Color.HSVToRGB(
                    currentHue,
                    (float)x/ svTexture.width,
                    (float)y / svTexture.height
                    ));
            }
        }
        svTexture.Apply();
        UpdateOutputImage();

    }

}
