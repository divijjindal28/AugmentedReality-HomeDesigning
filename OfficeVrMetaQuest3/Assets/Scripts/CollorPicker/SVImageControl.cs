using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SVImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField]
    private Image pickerImage;
    private RawImage SVimage;
    private ColorPickerControl CC;
    private RectTransform rectTransform, pickerTransform;

    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    private void Awake()
    {
        SVimage = GetComponent<RawImage>();
        CC = FindObjectOfType<ColorPickerControl>();
        rectTransform = GetComponent<RectTransform>();
        pickerTransform = pickerImage.GetComponent<RectTransform>();
        pickerTransform.localPosition = new Vector2(-(rectTransform.sizeDelta.x * 0.5f),-(rectTransform.sizeDelta.y * 0.5f));
        Debug.Log("SVImageControl: " + pickerTransform.localPosition);
    }

    void UpdateColor(PointerEventData eventData) {
        Vector3 pos = rectTransform.InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition);
        Vector3 initialPos = pos;
        Debug.Log("SVImageControl ABCDEFGHIJKL: " + pos);
        float deltaX = rectTransform.sizeDelta.x * 0.5f;
        float deltaY = rectTransform.sizeDelta.y * 0.5f;

        if (pos.x < -deltaX) {
            pos.x = -deltaX;
        }

        if (pos.x > deltaX)
        {
            pos.x = deltaX;
        }

        if (pos.y < -deltaY)
        {
            pos.y = -deltaY;
        }

        if (pos.y > deltaY)
        {
            pos.y = deltaY;
        }

        float x = pos.x + deltaX;
        float y = pos.y + deltaY;

        float xNorm = x / rectTransform.sizeDelta.x;
        float yNorm = y / rectTransform.sizeDelta.y;

        pos.z = 0;
        initialPos.z = 0;

        Vector3 finalVector = pos - initialPos;
        Debug.Log("SVImageControl pos : " + pos);
        Debug.Log("SVImageControl initialPos : " + initialPos);
        Debug.Log("SVImageControl Final : " + finalVector);
        pickerTransform.localPosition = pos;
        //Debug.Log("SVImageControl Final : " + pos);
        pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNorm);
        CC.SetSV(xNorm, yNorm); 
    }

}
