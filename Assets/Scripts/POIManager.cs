using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class POIManager : MonoBehaviour
{
    public GameObject POIIcon;
    public GameObject earth;
    public float earthRadius = 6375;
    public GameObject lookAtCamera;

    // Geonames
    public string serviceAddress = "http://api.geonames.org/searchJSON";
    private string countryCode;
    public int startRow = 0;
    public int maxRows = 100;
    public string username;
    public InputField inputSearch;
    public InputField txtCountry;
    public InputField txtCity;
    public InputField txtStreet;
    public InputField txtPoi;
    public InputField txtLat;
    public InputField txtLong;
    public Geonames data;
    private string completeQuery;



    public InputField latUI;
    public InputField longUI;

    public delegate void LoadingComplete();
    public static event LoadingComplete POIsLoaded;
    public static event LoadingComplete StopRotation;


    private Dictionary<string, string> countries;
    public string countriesFileName = "CountriesGeonames";

    public UIManager uiManager;

  
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (lookAtCamera != null && earth.transform.childCount > 0)
            foreach (Transform POIIcon in earth.transform)
            {
                POIIcon.transform.LookAt(lookAtCamera.transform.position, lookAtCamera.transform.up);
            }
    }

    public void LoadPOIs()
    {
        
        StopRotation();

        startRow = 0;

        var country = txtCountry.text;
        var city = encodeUTF8(txtCity.text);
        var street = encodeUTF8(txtStreet.text);
        var pointOfInterest = encodeUTF8(txtPoi.text);

      
        if(!string.IsNullOrEmpty(country))
            countryCode = checkCountryCode(country.ToUpper());


        if (txtLat.text != "" && txtLong.text != "")
        {
            var lat = float.Parse(txtLat.text.Replace('.', ','));
            var longt = float.Parse(txtLong.text.Replace('.', ','));

            if (uiManager.MapFormatIs3D())
            {
                DestroyGeonames();
                InstanciateGeonames("", lat, longt);
                POIsLoaded();
            }        
           
        }
        else if (countryCode != "" && (pointOfInterest != "" || city != "" || street != ""))
        {
            completeQuery = $@"{serviceAddress}?maxRows={maxRows}&startRow={startRow}&username={username}&country={countryCode}{(pointOfInterest != "" ? "&q=" + pointOfInterest : "")}{(street != "" ? "&name=" + street : "")}{(city != "" ? "&name=" + city : "")}";

            StartCoroutine(GetData(completeQuery));
        }
        else
            Debug.LogError("Fill the inputs"); 

    }

    private string checkCountryCode(string country)
    {
        string fileContents = Resources.Load<TextAsset>(countriesFileName).text;

        countries = new Dictionary<string, string>();

        string[] countriesInfo = fileContents.Split('\n');

        for (int i = 0; i < countriesInfo.Length; i++)
        {
            string[] data = countriesInfo[i].Split(';');

            string countryCode = data[0].ToUpper();
            string countryName = data[1].Replace("\r", "").ToUpper();

            countries.Add(countryName, countryCode);
        }

        if (!countries.ContainsKey(country))
        {
            Debug.LogError("Country Code not found");
            return null;
        }
        else
        {
            string value = null;
            countries.TryGetValue(country, out value);
            return value;

        }
    }


    IEnumerator GetData(string query)
    {

        DestroyGeonames();


        Debug.Log(query);

        do
        {
            using (UnityWebRequest request = UnityWebRequest.Get(query))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    try
                    {
                        data = JsonUtility.FromJson<Geonames>(request.downloadHandler.text);
                        if (!uiManager.MapFormatIs3D())
                            break;
                        foreach (var item in data.geonames)
                        {
                            string name = item.name;
                            float latitude = float.Parse(item.lat.Replace('.', ','));
                            float longitude = float.Parse(item.lng.Replace('.', ','));
                            InstanciateGeonames(name, latitude, longitude);
                            startRow++;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.Message);
                        throw;
                    }
                }
            }

        } while (startRow < data.totalResultsCount);

        if (uiManager.MapFormatIs3D())
            POIsLoaded();
    }

    public void InstanciateGeonames(string name, float latitude, float longitude)
    {

        Vector3 GEOPosition = SphericalToCartesian(earthRadius, latitude, longitude);
        GameObject thisPOI = Instantiate(POIIcon);

        thisPOI.transform.position = GEOPosition;
        thisPOI.transform.rotation = Quaternion.identity;
        thisPOI.transform.SetParent(earth.transform);
        thisPOI.name = name;
        thisPOI.GetComponent<POIInfo>().SetText(thisPOI.name);
    }

    public void DestroyGeonames()
    {

        var earthToDestroy = earth.GetComponent<Transform>();

        for (int i = 0; i < earthToDestroy.childCount; i++)
        {
            var child = earthToDestroy.GetChild(i).gameObject;
            Destroy(child.gameObject, 0);
        }

    }

    private Vector3 SphericalToCartesian(float radius, float latitude, float longitude)
    {
        latitude *= Mathf.Deg2Rad;
        longitude *= Mathf.Deg2Rad;

        latitude -= Mathf.Deg2Rad * 90;

        float x = radius * Mathf.Sin(latitude) * Mathf.Cos(longitude);
        float y = radius * Mathf.Cos(latitude);
        float z = radius * Mathf.Sin(latitude) * Mathf.Sin(longitude);

        return new Vector3(x, y, z);
    }



    private string encodeUTF8(string value)
    {
        if(value != "")
        {
            return value.Replace(" ", "%20");
        }
        return value;
    }

}