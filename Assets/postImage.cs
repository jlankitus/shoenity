using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;
using System.Text;

public class postImage : MonoBehaviour {

	/*
	void Start()
	{
		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/file";
		WebRequest myReq = WebRequest.Create(url);
		string credentials = "eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT08xQTFOSThYcXJtIn0.kGDxTfumgmH5AcX322_AljPWYDpqX97fI8VyvpbzSeA";
		CredentialCache mycache = new CredentialCache();
		myReq.Headers["Authorization"] = "Bearer " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

		WebResponse wr = myReq.GetResponse();
		Stream receiveStream = null;
		receiveStream = wr.GetResponseStream();
		StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
		string content = reader.ReadToEnd();
		print(content);
		var json = "[" + content + "]"; // change this to array
		var objects = JArray.Parse(json); // parse as array  
		foreach (JObject o in objects.Children<JObject>())
		{
			foreach (JProperty p in o.Properties())
			{
				string name = p.Name;
				string value = p.Value.ToString();
				Console.Write(name + ": " + value);
			}
		}
		Console.ReadLine();
	}
	*/

	/*
	private Stream Upload(string actionUrl, string paramString, Stream paramFileStream, byte [] paramFileBytes)
	{
		HttpContent stringContent = new StringContent(paramString);
		HttpContent fileStreamContent = new StreamContent(paramFileStream);
		HttpContent bytesContent = new ByteArrayContent(paramFileBytes);
		using (var client = new HttpClient())
		using (var formData = new MultipartFormDataContent())
		{
			formData.Add(stringContent, "param1", "param1");
			formData.Add(fileStreamContent, "file1", "file1");
			formData.Add(bytesContent, "file2", "file2");
			var response = client.PostAsync(actionUrl, formData).Result;
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			return response.Content.ReadAsStreamAsync().Result;
		}
	}
	*/
} 
