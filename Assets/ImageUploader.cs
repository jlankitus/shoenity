using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageUploader : MonoBehaviour {

	WebCamTexture webCamTexture;
	int count = 0;

	void Start() 
	{
		webCamTexture = new WebCamTexture();
		GetComponent<Renderer>().material.mainTexture = webCamTexture;
		webCamTexture.Play();
	}

	void Update() {
		if (Input.GetMouseButtonDown (0))
		{
			count++;
			StartCoroutine (TakePhoto ());
		}
	}

	IEnumerator TakePhoto()
	{

		// NOTE - you almost certainly have to do this here:

		yield return new WaitForEndOfFrame(); 

		// it's a rare case where the Unity doco is pretty clear,
		// http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
		// be sure to scroll down to the SECOND long example on that doco page 

		Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
		photo.SetPixels(webCamTexture.GetPixels());
		photo.Apply();

		//Encode to a PNG
		byte[] bytes = photo.EncodeToPNG();

		print (count);
		string your_path = Application.dataPath + "/StreamingAssets/"; // Application.persistentDataPath;
		print (your_path);
		//Write out the PNG. Of course you have to substitute your_path for something sensible
		File.WriteAllBytes(your_path + count + ".png", bytes);
	}

	IEnumerator UploadFileCo(string localFileName, string uploadURL)
	{
		WWW localFile = new WWW("file:///" + localFileName);
		yield return localFile;
		if (localFile.error == null)
			Debug.Log("Loaded file successfully");
		else
		{
			Debug.Log("Open file error: "+localFile.error);
			yield break; // stop the coroutine here
		}
		WWWForm postForm = new WWWForm();
		// version 1
		//postForm.AddBinaryData("theFile",localFile.bytes);
		// version 2
		postForm.AddBinaryData("theFile",localFile.bytes,localFileName,"text/plain");
		WWW upload = new WWW(uploadURL,postForm);        
		yield return upload;
		if (upload.error == null)
			Debug.Log("upload done :" + upload.text);
		else
			Debug.Log("Error during upload: " + upload.error);
	}
	void UploadFile(string localFileName, string uploadURL)
	{
		StartCoroutine(UploadFileCo(localFileName, uploadURL));
	}
}
