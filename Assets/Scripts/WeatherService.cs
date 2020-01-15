using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class Data{
	public string sunrise;
	public string sunset;
	public string solar_noon;
	public string day_length;
	public string civil_twilight_begin;
	public string civil_twilight_end;
	string nautical_twilight_begin;
	string nautical_twilight_end;
	string astronomical_twilight_begin;
	string astronomical_twilight_end;
}
[System.Serializable]
public class SunSetData{
	public Data results;
	public string status;
	
}
public class GetInfoHandler : MonoBehaviour {
	public Text sunSetInfo;
	public Text dateTime;
	public Text weatherForecast;
	// Use this for initialization

	public void GetSunSetInfo(){
		StartCoroutine("SunSetInfo");
	
	}

	IEnumerator SunSetInfo(){

		WWW www = new WWW("https://api.sunrise-sunset.org/json?lat=41.7201600&lng=1.98&date=today");
		yield return www;

        JObject obj = JObject.Parse(www.text);
        string sunset = (string)obj["results"]["sunset"];
        string sunrise = (string)obj["results"]["sunrise"];
        Debug.Log("sunset: " + sunset);
        Debug.Log("sunrise: " + sunrise);

        sunSetInfo.GetComponent<Text>().text = www.text.ToString();
			
		SunSetData myData;
		myData = JsonUtility.FromJson<SunSetData>(www.text);

	char stopAt = ':';
	int charLocation = myData.results.sunset.IndexOf(stopAt, 0,myData.results.sunset.Length);
	string hour = myData.results.sunset.Substring(0,charLocation);
	string substring = myData.results.sunset.Substring(charLocation+1);
        Debug.Log("SunSet: " + myData.results.sunset);
        charLocation = substring.IndexOf(stopAt, 0,substring.Length);
	string minutes = substring.Substring(0,charLocation);
	substring = myData.results.sunset.Substring(charLocation+1);

	int minutesI = int.Parse(minutes);
	int hourI 	= int.Parse(hour);

	stopAt ='M';
	charLocation = substring.IndexOf(stopAt,0,substring.Length);
	char p = 'P';
	if (substring[charLocation-1]==p)
			hourI = hourI+12;

	sunSetInfo.GetComponent<Text>().text = hourI+"-"+minutesI;

	}

	public void ClockTimeInfo(){

		int hourNow = System.DateTime.Now.Hour;
		int minutesNow = System.DateTime.Now.Minute;
		
		dateTime.GetComponent<Text>().text = hourNow+"--"+minutesNow;

	}

	public void GetWeatherforecast(){
		StartCoroutine("Weatherforecast");
	}


//https://github.com/SaladLab/Json.Net.Unity3D/releases
	IEnumerator Weatherforecast(){

	WWW www = new WWW("http://api.apixu.com/v1/forecast.json?key=9302f9d8d4e04d33820192441181712&q=Castelldefels");
		yield return www;

		JObject obj = JObject.Parse(www.text);
		string temp = (string)obj["current"]["temp_c"];
		weatherForecast.GetComponent<Text>().text = temp;
	}


}
