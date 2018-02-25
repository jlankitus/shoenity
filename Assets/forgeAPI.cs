using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class forgeAPI : MonoBehaviour {

	public string sceneName;
	public string format;
	private string token;
	private string photoSceneId;

	private void Start()
	{
		photoSceneId = "J4JsZovr9E2tRspPmf6ihe37CSZJm4nHxvZJhYmvmU4";
		token = "eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxOTU4NzIzMiwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJseW1hcW5icnV5b0ROT0tReEJDOFlZcTd3eWREMzRMVXdqb25YNkVtNGVGcUxNeEE2Tk9xTXUxMmE2Rm5FVWdaIn0.-_aRKjKTSKFevyG5ppitvc1JHtwTOYxz6kTGfTOtnT0";
		StartCoroutine(PostPics());
	}

	IEnumerator newPhotoScene()
	{
		/*
		curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/photoscene' -X 'POST' 
		-H 'Content-Type: application/json' 
		-H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT08xQTFOSThYcXJtIn0.kGDxTfumgmH5AcX322_AljPWYDpqX97fI8VyvpbzSeA' 
		-d 'scenename=brandonshoe2' 
		-d 'format=rcs,obj,rcm,ortho' 
		-d 'metadata_name[0]=orthogsd' 
		-d 'metadata_value[0]=0.1' 
		-d 'metadata_name[1]=targetcs' 
		-d 'metadata_value[1]=UTM84-32N'
		 */

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene";
		WWWForm form = new WWWForm();

        form.AddField("scenename", sceneName);
		form.AddField("format", format);
		form.AddField("metadata_name[0]", "orthogsd");
		form.AddField("metadata_value[0]", "0.1");
		form.AddField("metadata_name[1]", "targetcs");
		form.AddField("metadata_value[1]", "UTM84-32N");

        UnityWebRequest www = UnityWebRequest.Post(url, form);
		www.SetRequestHeader("Content-Type","application/json");
		www.SetRequestHeader("Authorization","Bearer " + token);
        yield return www.Send();
 
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
			print(www.downloadHandler.text);
        } 
	}

	IEnumerator PostPics()
	{
		/*
		curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/file' -X 'POST' 
		-H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT08xQTFOSThYcXJtIn0.kGDxTfumgmH5AcX322_AljPWYDpqX97fI8VyvpbzSeA' 
		-F "photosceneid=J4JsZovr9E2tRspPmf6ihe37CSZJm4nHxvZJhYmvmU4" 
		-F "type=image" 
		-F "file[0]=@/home/jed/Pictures/brandonShoe/1.jpg"
		 */

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/file";
		WWWForm form = new WWWForm();

        form.AddField("photosceneid", photoSceneId);
		form.AddField("type", "image");
		// string urlEncoded = WWW.EscapeURL("@/home/jed/Pictures/brandonShoe/1.jpg");
		byte[] bytes = System.IO.File.ReadAllBytes("@/home/jed/Pictures/brandonShoe/1.jpg");
		form.AddBinaryData("file[0]", bytes, "1.jpg");

        UnityWebRequest www = UnityWebRequest.Post(url, form);
		www.SetRequestHeader("Authorization","Bearer " + token);
        yield return www.Send();
 
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
			print(www.downloadHandler.text);
        } 
	}

	IEnumerator GetToken()
	{
		/*
		curl -v 
		'https://developer.api.autodesk.com/authentication/v1/authenticate' 
		-X 'POST' -H 'Content-Type: application/x-www-form-urlencoded' 
		-d "client_id=yZ812Yfi6wDD1uKu3OaMgU7aa2xUIngT&client_secret=gNJYf66myNB8P4os&grant_type=client_credentials&scope=data:read%20data:write"
		 */

		WWWForm form = new WWWForm();
        form.AddField("client_id", "yZ812Yfi6wDD1uKu3OaMgU7aa2xUIngT");
		form.AddField("client_secret", "gNJYf66myNB8P4os");
		form.AddField("grant_type", "client_credentials");
		form.AddField("scope", "data:read data:write");

        UnityWebRequest www = UnityWebRequest.Post("https://developer.api.autodesk.com/authentication/v1/authenticate", form);
		www.SetRequestHeader("Content-Type","application/x-www-form-urlencoded");
        yield return www.Send();
 
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
			print(www.downloadHandler.text);
        } 
	}

	IEnumerator GetStatus()
	{
		var form = new WWWForm();
		var headers = new Hashtable();
		headers.Add ("Content-Type", "application/json");
		headers.Add("Authorization", "Bearer " + token);

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/aSiOVB0WG9OrHjnITJIiDeHz3syPkAg8NcMKYsU5ZQk/progress";
		WWW www = new WWW(url, null, headers);

		// Post a request to an URL with our custom headers
		yield return www;
		string statusJSON = www.text;

		print (www.text);
		print (www.responseHeaders);
	}
}
