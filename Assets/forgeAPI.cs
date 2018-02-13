using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Poll
{
	public string Usage;
	public string Resource;
	public string Photoscene;
}
	
public class Photoscene
{
	public string photosceneid;
	public string progressmsg;
	public string progress;
}

public class forgeAPI : MonoBehaviour {

	void Start()
	{
		StartCoroutine(GetStatus());
	}

	IEnumerator PostPics()
	{
		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/file";
		WWWForm form = new WWWForm();
		form.AddField("myField", "myData");

		UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", form);
		yield return www.Send();

		if(www.isNetworkError) 
		{
			Debug.Log(www.error);
		}
		else 
		{
			Debug.Log("Form upload complete!");
		}
	}

	IEnumerator GetStatus()
	{
		var form = new WWWForm();
		var headers = new Hashtable();
		headers.Add ("Content-Type", "application/json");
		headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNjIxNjE0Miwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJHbTl1Z3BlREFkUm5LaHFMTkd6QnRKNXg5dFUyME92YWF6QkxIbkViR0lrS1RVQkFncTlkVHZ5OWtaYllDRFlCIn0.7FMuHEBY0bhDYG46G7EgAmaM6hcoRalXXkMkXctWS6U");

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/aSiOVB0WG9OrHjnITJIiDeHz3syPkAg8NcMKYsU5ZQk/progress";
		WWW www = new WWW(url, null, headers);

		// Post a request to an URL with our custom headers
		yield return www;
		string statusJSON = www.text;

		/*
		Poll myObject = new Poll();
		myObject = JsonUtility.FromJson<Poll>(statusJSON);
		Photoscene photoscene = new Photoscene ();
		photoscene = JsonUtility.FromJson<Photoscene>(myObject.Photoscene);
		// Photoscene ps = myObject.Photoscene;
		*/

		print (www.text);
		print (www.responseHeaders);

		// print (myObject.Usage);
		// print (photoscene.progress);

		/*
		WWWForm form = new WWWForm();
		form.AddField ("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNjA1MzUwNywic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJMcXNlcGhpY1BRWXRrZVBOOTcwekhhNFN1bFBVUHBwQkdHMnN4RVg2aXhwYm9URHRHVjBweFhxbmxSdzdjbHBPIn0.Dy6-TVFoh-0YPE6XW6mibaRlljlcyvYNKcFfFXJOpGQ");

		using (var w = UnityWebRequest.Get (form)) 
		{
			yield return w.SendWebRequest();
			if (w.isNetworkError || w.isHttpError) 
			{
				print(w.error);
				print(w.downloadHandler.text);
			}
			else 
			{
				print(w.downloadHandler.text);
			}
		}
		*/

		/*
		using (UnityWebRequest www = UnityWebRequest.Get("https://en.wikipedia.org/wiki/Jeffrey_Epstein"))
		{
			yield return www.Send();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				// Show results as text
				Debug.Log(www.downloadHandler.text);

				// Or retrieve results as binary data
				byte[] results = www.downloadHandler.data;
			}
		}
		*/
	}
}
