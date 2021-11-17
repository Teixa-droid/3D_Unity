using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class POIInfo : MonoBehaviour
{
    public TextMeshPro textField;
    
    public void SetText(string POIText)
    {
        textField.text = POIText;
    }
}

