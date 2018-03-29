using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class accessToken
{
	public string access_token;

	public static accessToken CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<accessToken>(jsonString);
    }
}

// {"Usage":"0.82937216758728","Resource":"\/photoscene","Photoscene":{"photosceneid":"WIErSs15wKRyl3AOyOuablfrecNksc6jLjnKV7mE4Q0"}}

public class Photoscene
{
	public string photosceneid;
}
/*
public class RootObject
{
	public Photoscene Photoscene;
}*/

[System.Serializable]
public class RootObj
{
    public Photoscene Photoscene;
}

public class forgeAPI : MonoBehaviour {

	public Text sceneText;
	private string sceneName;
	public string format;

	private string access_token;
	private string photoSceneId;
	private bool status;

	public GameObject emptyPrefabWithMeshRenderer;
	
	public void createScene()
	{
		status = false;
		StartCoroutine(GetToken());
	}

	IEnumerator GetToken()
	{
		sceneName = sceneText.text;
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
            Debug.Log("Token complete!");
			print(www.downloadHandler.text);

			string json = www.downloadHandler.text;

			// accessToken at = new accessToken();
			access_token = accessToken.CreateFromJSON(json).access_token;
			StartCoroutine(newPhotoScene());
			// StartCoroutine(Download());
        } 
	}

	IEnumerator newPhotoScene()
	{
		/*
		curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/photoscene' -X 'POST' 
		-H 'Content-Type: application/json' 
		-H 'Authorization: Bearer eyJhbGciO
iJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT08xQTFOSThYcXJtIn0.kGDxTfumgmH5AcX322_AljPWYDpqX97fI8VyvpbzSeA' 
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
		form.AddField("metadata_name[0]", "obj");
		form.AddField("metadata_value[0]", "0.1");
		form.AddField("metadata_name[1]", "targetcs");
		form.AddField("metadata_value[1]", "UTM84-32N");

        UnityWebRequest www = UnityWebRequest.Post(url, form);
		www.SetRequestHeader("Content-Type","application/json");
		www.SetRequestHeader("Authorization","Bearer " + access_token);
        yield return www.Send();
 
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Photoscene complete!");
			// photoSceneId = SceneID.CreateFromJSON(www.downloadHandler.text).photosceneid;
			// print(photoSceneId);
			photoSceneId = parsePhotoSceneID(www.downloadHandler.text);
			StartCoroutine(PostPics());
        } 
	}

	string parsePhotoSceneID(string json)
	{
		string[] split = json.Split(':');
		string ID = split[4];
		ID = ID.Remove(0,1);
		ID = ID.Remove((ID.Length - 1),1);
		ID = ID.Remove((ID.Length - 1),1);
		ID = ID.Remove((ID.Length - 1),1);
		return ID;

	}

	IEnumerator PostPics()
	{
		/*
		curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/file' -X 'POST' 
		-H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT08xQTFOSThYcXJtIn0.kGDxTfumgmH5AcX322_AljPWYDpqX97fI8VyvpbzSeA' 
		-F "photosceneid=J4JsZovr9E2tRspPmf6ihe37CSZJm4nHxvZJhYStartCoroutine(newPhotoScene());mvmU4" 
		-F "type=image" 
		-F "file[0]=@/home/jed/Pictures/brandonShoe/1.jpg"
		 */

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/file";
		WWWForm form = new WWWForm();

        form.AddField("photosceneid", photoSceneId);
		form.AddField("type", "image");
		// string urlEncoded = WWW.EscapeURL("@/home/jed/Pictures/brandonShoe/1.jpg");
		string [] fileEntries = Directory.GetFiles("Assets/StreamingAssets/peanuts/");
		int count = 1;
		foreach(string file in fileEntries)
		{
			if(!file.Contains(".meta"))
			{
				byte[] bytes = System.IO.File.ReadAllBytes(file);
				form.AddBinaryData("file[0]", bytes, count.ToString() + ".jpg");

				UnityWebRequest www = UnityWebRequest.Post(url, form);
				www.SetRequestHeader("Authorization","Bearer " + access_token);
				yield return www.Send();
		
				if(www.isError) {
					Debug.Log(www.error);
				}
				else {
					Debug.Log("photo upload complete!");
					print(www.downloadHandler.text);
				} 
				count++;
			}		
		}
		StartCoroutine(Process());
	}	

	IEnumerator Process()
	{
		/*d
		Process photoscene

		curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/VLtjyTqPTCWq36NyLx2jdHFO6wlaqKnlqWGgFFOuHDg' 
		-X 'POST' 
		-H 'Content-Type: application/json' d
		-H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT
		 */

		var form = new WWWForm();
		// var headers = new Hashtable();
		// headers.Add ("Content-Type", "application/json");
		// headers.Add("Authorization", "Bearer " + access_token);

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/" + photoSceneId;
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		www.SetRequestHeader("Content-Type","application/json");
		www.SetRequestHeader("Authorization","Bearer " + access_token);

		// Post a request to an URL with our custom headers
		yield return www.Send();
		string statusJSON = www.downloadHandler.text;
		print (www.downloadHandler.text);
		print("PROCESSING");
		
		StartCoroutine(GetStatus());
	}

	IEnumerator GetStatus()
	{
		while(status != true)
		{
			var form = new WWWForm();
			var headers = new Hashtable();
			headers.Add ("Content-Type", "application/json");
			headers.Add("Authorization", "Bearer " + access_token);

			string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/" + photoSceneId + "/progress";
			WWW www = new WWW(url, null, headers);

			// Post a request to an URL with our custom headers
			yield return www;
			string statusJSON = www.text;
			string[] split = statusJSON.Split(':');
			string last = split[split.Length - 1];
			last = last.Remove(0,1);
			last = last.Remove((last.Length - 1),1);
			last = last.Remove((last.Length - 1),1);
			last = last.Remove((last.Length - 1),1);

			if(last == "100")
			{
				StartCoroutine(Download());
			}
			else
			{
				print (www.text);
				print (www.responseHeaders);
				yield return new WaitForSeconds(3f);
			}
			
		}
	}

	 IEnumerator ImportObject (string url) {
 
        WWW www = new WWW(url);
        yield return www;
 
        if (string.IsNullOrEmpty(www.error)) {
            Debug.Log("Download Error");
        } else {
            string write_path = Application.dataPath + "download.obj";
 
            System.IO.File.WriteAllBytes(write_path, www.bytes);
 
            Debug.Log("Wrote to path");
        }
 
        GameObject spawnedPrefab;
		FastObjImport importTool = new FastObjImport();
		Mesh peanuts = importTool.ImportFile("download.obj");
		spawnedPrefab = Instantiate(emptyPrefabWithMeshRenderer);
		spawnedPrefab.GetComponent<MeshFilter>().mesh = peanuts;
        spawnedPrefab.transform.position = new Vector3(0, 0, 0);
		print("done");
		/*
        Mesh importedMesh = objImporter.ImportFile(Application.dataPath + "/Objects/" + modelName);
 
        spawnedPrefab = Instantiate(emptyPrefabWithMeshRenderer);
        spawnedPrefab.transform.position = new Vector3(0, 0, 0);
        spawnedPrefab.GetComponent<MeshFilter>().mesh = importedMesh;
		*/
    }

	IEnumerator Download()
	{
		var form = new WWWForm();
		var headers = new Hashtable();
		headers.Add ("Content-Type", "application/json");
		headers.Add("Authorization", "Bearer " + access_token);

		string photoSceneId = "P0PpN1lNqjbTfwAMCAX1wOJfYobDmfXCwy9xDqPeT6A";
		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/" + photoSceneId;
		WWW www = new WWW(url, null, headers);

		// Post a request to an URL with our custom headers
		yield return www;

		print (www.text);
		print (www.responseHeaders);

		string statusJSON = www.text;
		string[] split = statusJSON.Split(':');

		string last1 = split[7];
		last1 = last1.Remove(0,1);

		string last2 = split[8];
		
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);

		string result = (last1 + ':' + last2);
		print(result);
		string downloadURL = Regex.Unescape(result);
		print(downloadURL);

		/*
		using (WWW download = new WWW(downloadURL))
        {
            yield return download;

			File.WriteAllBytes("download.obj", download.bytes);
			FastObjImport importTool = new FastObjImport();
			Mesh peanuts = importTool.ImportFile("download.obj");
			print(peanuts.name);
			GameObject.Instantiate(peanuts, gameObject.transform);	
			print("done");		
        }
		*/
		

		/*
			var form = new WWWForm();
			var headers = new Hashtable();
			headers.Add ("Content-Type", "application/json");
			headers.Add("Authorization", "Bearer " + access_token);

			string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/" + photoSceneId + "/progress";
			WWW www = new WWW(url, null, headers);

			// Post a request to an URL with our custom headers
			yield return www;
		 */
	}
}
