using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Map2DManager map2DManager;
    public POIManager poiManager;

    public GameObject earth3d;
    public GameObject panel2d;

    public Text txtButtonToggleMapFormat;

    public InputField latUI;
    public InputField longUI;

    public Button btnZoomIn;
    public Button btnZoomOut;
   

    public bool is3D;

    void Start()
    {    
        is3D = true;
    }

    public void ToggleMapFormat()
    {
        if (is3D)
        {
            is3D = false;
            earth3d.SetActive(false);
            panel2d.SetActive(true);

            btnZoomIn.gameObject.SetActive(true);
            btnZoomOut.gameObject.SetActive(true);
            

            txtButtonToggleMapFormat.text = "3D";

            Load2DMapByInput();
        }
        else
        {
            is3D = true;
            earth3d.SetActive(true);
            panel2d.SetActive(false);

            btnZoomIn.gameObject.SetActive(false);
            btnZoomOut.gameObject.SetActive(false);
            

            txtButtonToggleMapFormat.text = "2D";
        }
    }

    public void SearchButton()
    {
        if (is3D)
        {
            poiManager.LoadPOIs();
        }
        else
        {
            Load2DMapByInput();
        }
    }

    private void Load2DMapByInput()
    {
        if (!string.IsNullOrEmpty(latUI.text) && !string.IsNullOrEmpty(longUI.text))
        {
            double lat = double.Parse(latUI.text.Replace(".", ","));
            double lon = double.Parse(longUI.text.Replace(".", ","));

            map2DManager.Load2DMap(lat, lon);
        }
    }

    public void ZoomIn()
    {
        map2DManager.zoom = map2DManager.zoom + 1;
        map2DManager.Load2DMap();
    }
    public void ZoomOut()
    {
        map2DManager.zoom = map2DManager.zoom - 1;
        map2DManager.Load2DMap();
    }

    public bool MapFormatIs3D()
    {
        return is3D;
    }

    public void SelectPoiLocation()
    {
        Debug.Log("SelectPoiLocation");
    }

    private int colRef = 0;
    private int rowRef = 0;


}
