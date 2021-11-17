using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel2DManager : MonoBehaviour
{
    public float dragSpeed = 5;
    private bool isDragging = false;

    public Vector2 totalImages;
    public GameObject tileRawImage;
    public Transform panel2D;

    private Vector2 rawSize;

    private List<Vector3> InitialPosition;

    void Start()
    {
        
        InitialPosition = new List<Vector3>();
        foreach (Transform image in transform)
        {
            InitialPosition.Add(image.position);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
            isDragging = false;
                  
        if (isDragging)
        {
            foreach (Transform image in transform)
            {
                var translation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) ;
    
                image.Translate(translation * dragSpeed);
            }                
        }
    }

    public void ResetTilesPosition()
    {
        var count = 0;
        foreach (Transform image in transform)
        {
            image.transform.position = InitialPosition[count];
            count++;
        }
    }
}
