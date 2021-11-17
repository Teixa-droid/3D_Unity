using System.Collections.Generic;

[System.Serializable]
public class AdminCodes1
{
    public string ISO3166_2;
}

[System.Serializable]
public class Geoname
{
    public string adminCode1;
    public string lng;
    public int geonameId;
    public string toponymName;
    public string countryId;
    public string fcl;
    public int population;
    public string countryCode;
    public string name;
    public string fclName;
    public AdminCodes1 adminCodes1;
    public string countryName;
    public string fcodeName;
    public string adminName1;
    public string lat;
    public string fcode;
}

[System.Serializable]
public class Geonames
{
    public int totalResultsCount;
    public List<Geoname> geonames;
}