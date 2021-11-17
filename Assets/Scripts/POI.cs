using UnityEngine;

public class POI
{
    private float latitude;
    private float longitude;
    private string description;
    private Color color;

    public POI(float latitude, float longitude, string description)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.description = description;
    }

    public POI(float latitude, float longitude, string description, Color color)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.description = description;
        this.color = color;
    }

    public float Latitude
    {
        get
        {
            return this.latitude;
        }

        set
        {
            latitude = value;
        }
    }

    public float Longitude
    {
        get
        {
            return longitude;
        }

        set
        {
            longitude = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public Color ColorValue
    {
        get
        {
            return color;
        }

        set
        {
            color = value;
        }
    }
}
