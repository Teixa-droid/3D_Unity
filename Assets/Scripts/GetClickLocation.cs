using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickLocation : MonoBehaviour
{
    public string earthTag;
    public string POITag;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Debug.Log(GetPointUnderCursor());
    }

    Vector3 GetPointUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        Vector3 returnPosition = Vector3.zero;

        if(Physics.Raycast(ray,out hit))
        {
            if (hit.collider.tag == earthTag)
            {
                Debug.Log("Click na terra");
                
            }

            if (hit.collider.tag == POITag)
            {
                
                Debug.Log("Click no POI");
               
            }
        }

        return returnPosition;
    }

  

    public static Vector2 CartesianToSpherical(float x, float y, float z)
    {

        float radius = Mathf.Sqrt((x * x) + (z * z));
        float latitude = Mathf.Atan(y/radius)*180/Mathf.PI;
        
       
        float longitude = Mathf.Atan2(x,z) * (180 / Mathf.PI);

        longitude = Mathf.Tan(y /x) * (180 / Mathf.PI);

        longitude = Mathf.Tan(y / x);



        return  new Vector2(0,0);
    }



    public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart)
    {
        float a = radius * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = radius * Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);
    }

    Vector3 getSphericalCoordinates(Vector3 cartesian)
    {
        float r = Mathf.Sqrt(
            Mathf.Pow(cartesian.x, 2) +
            Mathf.Pow(cartesian.y, 2) +
            Mathf.Pow(cartesian.z, 2)
        );

        float phi = Mathf.Atan2(cartesian.z / cartesian.x, cartesian.x);
        float theta = Mathf.Acos(cartesian.y / r);

        return new Vector3(r, phi, theta);
    }



}
