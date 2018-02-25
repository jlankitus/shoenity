using CI.HttpClient;
using UnityEngine;
using UnityEngine.UI;

public class ExampleSceneManagerController : MonoBehaviour
{
    public Text LeftText;
    public Text RightText;
    public Slider ProgressSlider;

    /*
	private System.IO.Stream Upload(string actionUrl, string paramString, Stream paramFileStream, byte [] paramFileBytes)
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
	}*/

    public void Upload()
    {
		/*
        HttpClient client = new HttpClient();

        byte[] buffer = new byte[1000000];
        new System.Random().NextBytes(buffer);

        ByteArrayContent content = new ByteArrayContent(buffer, "application/bytes");

        ProgressSlider.value = 0;

        client.Post(new System.Uri("http://httpbin.org/post"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {           
        }, (u) =>
        {
            LeftText.text = "Upload: " +  u.PercentageComplete.ToString() + "%";
            ProgressSlider.value = u.PercentageComplete;
        });
		*/

		/*

		HttpClient client = new HttpClient();

		MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();

		byte[] buffer = new byte[1000000];
		new System.Random().NextBytes(buffer);

		ByteArrayContent content = new ByteArrayContent(buffer, "application/bytes");

		multipartFormDataContent.Add(content, "MyFile", "MyFile.txt");
		multipartFormDataContent.Add ("Authorization", );

		client.Post(new Uri(url), multipartFormDataContent, HttpCompletionOption.AllResponseContent, (r) =>
		{
		}, (u) =>
		{
		});
		*/
    }

    public void Download()
    {
        HttpClient client = new HttpClient();

        ProgressSlider.value = 100;

        client.GetByteArray(new System.Uri("http://download.thinkbroadband.com/5MB.zip"), HttpCompletionOption.StreamResponseContent, (r) =>
        {
            RightText.text = "Download: " + r.PercentageComplete.ToString() + "%";
            ProgressSlider.value = 100 - r.PercentageComplete;
        });
    }

    public void UploadDownload()
    {
        HttpClient client = new HttpClient();

        byte[] buffer = new byte[1000000];
        new System.Random().NextBytes(buffer);

        ByteArrayContent content = new ByteArrayContent(buffer, "application/bytes");

        ProgressSlider.value = 0;

        client.Post(new System.Uri("http://httpbin.org/post"), content, HttpCompletionOption.StreamResponseContent, (r) =>
        {
            RightText.text = "Download: " + r.PercentageComplete.ToString() + "%";
            ProgressSlider.value = 100 - r.PercentageComplete;
        }, (u) =>
        {
            LeftText.text = "Upload: " + u.PercentageComplete.ToString() + "%";
            ProgressSlider.value = u.PercentageComplete;
        });
    }

    public void Delete()
    {
        HttpClient client = new HttpClient();
        client.Delete(new System.Uri("http://httpbin.org/delete"), HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Get()
    {
        HttpClient client = new HttpClient();
        client.GetByteArray(new System.Uri("http://httpbin.org/get"), HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Patch()
    {
        HttpClient client = new HttpClient();

        StringContent content = new StringContent("Hello World");

        client.Patch(new System.Uri("http://httpbin.org/patch"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Post()
    {
        HttpClient client = new HttpClient();

        StringContent content = new StringContent("Hello World");

        client.Post(new System.Uri("http://httpbin.org/post"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Put()
    {
        HttpClient client = new HttpClient();

        StringContent content = new StringContent("Hello World");

        client.Put(new System.Uri("http://httpbin.org/put"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }
}