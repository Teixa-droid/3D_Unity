using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TileMap
{
    public Texture2D Texture { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int Zoom { get; set; }
}

public class Map2DManager : MonoBehaviour
{
    public enum SCHEME { normal_day, satellite_day }
    public enum SIZE { Medium, Large }
    public enum FORMAT { png, jpg }

    public string baseURL = "https://2.base.maps.ls.hereapi.com";
    public string path = "maptile/2.1";
    public string resource = "maptile";
    public string version = "newest";

    public SCHEME scheme = SCHEME.normal_day;
    public SIZE size = SIZE.Medium;
    public FORMAT format = FORMAT.jpg;

    private string apiKey = "nq7qNlYSz46w2UU51lxuLKqlSiFnAqOkK5yi8y22Wvw";
    private string URL;

    public double latitude = 41.342904;
    public double longitude = -8.751174;
    public int zoom = 16;

    public int column = 31199;
    public int row = 24506;

    public GameObject panel2D;
    public RawImage RawImageCenter;

    public List<TileMap> ListGridView = new List<TileMap>();

    private int? colAdjust = 0;
    private int? rowAdjust = 0;

    public void Load2DMap(int col, int row)
    {
        colAdjust = col;
        rowAdjust = row;
        Load2DMap();
    }
    public void Load2DMap(double lat, double lon)
    {
        latitude = lat;
        longitude = lon;
        Load2DMap();
    }
    public void Load2DMap()
    {
        ConvertLatLongToRowCol();

     

        Debug.Log("column: " + column);
        Debug.Log("row: " + row);

        URL = baseURL + "/" + path + "/" + resource + "/" + version + "/" + "normal.day" + "/" + zoom + "/" + (column + colAdjust.Value) + "/" + (row + rowAdjust.Value) + "/" + (size == SIZE.Medium ? "256" : "512") + "/" + "png8" + "?apiKey=" + apiKey;
        Debug.Log(URL);
        StartCoroutine(LoadTiles());
    }

    IEnumerator LoadTiles()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D textureCenter = ((DownloadHandlerTexture)www.downloadHandler).texture;

            RawImageCenter.texture = textureCenter;

            ListGridView.Add(new TileMap
            {
                Column = column,
                Row = row,
                Texture = textureCenter,
                Zoom = zoom
            });

            int centerRow = row;
            int centerCol = column;

            var newRow = 0;
            var newCol = 0;

            var newXRawImg = 0;
            var newYRawImg = 0;

            int numCol = 5;
            int numrow = 5;

            for (int x = 0; x < numCol; x++) 
            {
                for (int y = 0; y < numrow; y++) 
                {
                    if (x == 0)
                        newCol = column - 2;
                    if (x == 1)
                        newCol = column - 1;
                    if (x == 2)
                        newCol = column;
                    if (x == 3)
                        newCol = column + 1;
                    if (x == 4)
                        newCol = column + 2;

                    if (y == 0)
                        newRow = row + 2;
                    if (y == 1)
                        newRow = row + 1;
                    if (y == 2)
                        newRow = row;
                    if (y == 3)
                        newRow = row - 1;
                    if (y == 4)
                        newRow = row - 2;

                    if (colAdjust.HasValue)
                        newCol = newCol + colAdjust.Value;

                    if (rowAdjust.HasValue)
                        newRow = newRow + rowAdjust.Value;

                    newXRawImg = x;
                    newYRawImg = y;

                 

                    if (newRow == centerRow && newCol == centerCol)
                        continue;

               
                    TileMap tile = ListGridView.FirstOrDefault(f => f.Column == newCol && f.Row == newRow && f.Zoom == zoom);
                    if (tile != null)
                    {
                        var rawImage = GameObject.Find($"RawImage[{newXRawImg},{newYRawImg}]");
                        if (rawImage != null)
                            rawImage.GetComponent<RawImage>().texture = tile.Texture;
                    }
                    else
                    {
                        URL = baseURL + "/" + path + "/" + resource + "/" + version + "/" + "normal.day" + "/" + zoom + "/" + newCol + "/" + newRow + "/" + (size == SIZE.Medium ? "256" : "512") + "/" + "png8" + "?apiKey=" + apiKey;

                        UnityWebRequest wwwV2 = UnityWebRequestTexture.GetTexture(URL);
                        yield return wwwV2.SendWebRequest();

                        if (wwwV2.isNetworkError || wwwV2.isHttpError)
                        {
                            Debug.Log(wwwV2.error);
                        }
                        else
                        {
                            Texture2D textureTile = ((DownloadHandlerTexture)wwwV2.downloadHandler).texture;

                            var rawImage = GameObject.Find($"RawImage[{newXRawImg},{newYRawImg}]");
                            if (rawImage != null)
                                rawImage.GetComponent<RawImage>().texture = textureTile;
                            
                            ListGridView.Add(new TileMap
                            {
                                Column = newCol,
                                Row = newRow,
                                Texture = textureTile,
                                Zoom = zoom
                            });
                        }
                    }                    
                }
            }
        }
    }

    

    private void ConvertLatLongToRowCol()
    {
        double latRad = latitude * Math.PI / 180;
        double n = Math.Pow(2, zoom);

        column = Convert.ToInt32(Math.Truncate(n * ((longitude + 180) / 360)));
        row = Convert.ToInt32(Math.Truncate((n * (1 - (Math.Log(Math.Tan(latRad) + 1 / Math.Cos(latRad)) / Math.PI)) / 2)));
    }
}
