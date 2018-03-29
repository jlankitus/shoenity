using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProductInfo
{
    public string title;
}

/*
[System.Serializable]
public class RootObj
{
    public ProductInfo product_info;
}
*/

// string jsontest = "{\"product_info\":{\"title\": \"Product Name\"}}";
public class testJSON : MonoBehaviour {

	void Start () 
	{
        // RootObj m = JsonUtility.FromJson<RootObj> ("{\"product_info\":{\"title\": \"Product Name\"}}");]
        // print (m.product_info.title);

    }
}
