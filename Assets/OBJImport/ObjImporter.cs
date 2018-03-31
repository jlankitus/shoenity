using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

/////////////////////////////////////////////////////////////////////////////////////////////////////////
// Import obj files!
// This script assumes all faces of the obj are triangles and that the number of verts is under the mesh vert limit of 60000
//
// Usage - Call Import with the path to the obj file
//         ObjectImported event will tell you when an object is imported!
/////////////////////////////////////////////////////////////////////////////////////////////////////////

public class ObjImporter : MonoBehaviour
{
   
    string modelPath = "New_Model";

    public delegate void ObjectImportedEventHandler(GameObject gameObject);
    public event ObjectImportedEventHandler ObjectImported;
    public Material mat;

    private struct meshStruct
    {
        public int numVerts;
        public int numUv;
        public int numNorm;
        public Vector3[] vertices;
        public Vector3[] normals;
        public Vector2[] uv;
        public Vector2[] uv1;
        public Vector2[] uv2;
        public int[] triangles;
        public int[] faceVerts;
        public int[] faceUVs;
        public Vector3[] faceData;
        public string name;
        public string fileName;
    }

    public void Start()
    {
        Import("C:\\Users\\Branden\\Documents\\Point Cloud Surface Reconstruction\\Assets\\Unity-PointCloud\\Texture Stuff\\Test Data\\shoe\\shoe.obj");
    }

    public void Import(string mPath)
    {
        modelPath = mPath;
        // Create mesh
        ObjImporter objImporter = new ObjImporter();
        Mesh importedMesh = objImporter.ImportFile(modelPath);

        // Create empty gameobject
        GameObject emptyGameObject = new GameObject();
        emptyGameObject.transform.position = new Vector3(0, 0, 0);

        // Add mesh to game object
        MeshFilter meshFilter = emptyGameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        meshFilter.sharedMesh = importedMesh;

        // Add mesh renderer
        MeshRenderer meshRenderer = emptyGameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        meshRenderer.material = mat;

        // Tell the boys
        if (ObjectImported != null)
        {
            ObjectImported(emptyGameObject);
        }
    }

    // Use this for initialization
    Mesh ImportFile(string filePath)
    {
        meshStruct newMesh = createMeshStruct(filePath);
        Debug.Log(newMesh.vertices.Length);
        populateMeshStruct(ref newMesh);

        Vector3[] newVerts = new Vector3[newMesh.numUv];
        Vector2[] newUVs = new Vector2[newMesh.numUv];
        Vector3[] newNormals = new Vector3[newMesh.numUv];

        /* The following foreach loops through the facedata and assigns the appropriate vertex, uv, or normal
         * for the appropriate Unity mesh array.
         */
        foreach (Vector3 v in newMesh.faceData)
        {
            newVerts[(int)v.y - 1] = newMesh.vertices[(int)v.x - 1];
            if (v.y >= 1)
                newUVs[(int)v.y - 1] = newMesh.uv[(int)v.y - 1];

            if (v.z >= 1)
                newNormals[(int)v.y - 1] = newMesh.normals[(int)v.z - 1];
        }

        Mesh mesh = new Mesh();
        Debug.Log(newVerts.Length);
        mesh.vertices = newVerts;
        mesh.uv = newUVs;
        mesh.normals = newNormals;

        int max = 0;
        foreach(int index in newMesh.triangles)
        {
            if (index > max)
            {
                max = index;
            }
            if (index < 0 || index > newVerts.Length)
            {
                Debug.Log("UH OH");
            }
        }
        Debug.Log(max);
        mesh.triangles = newMesh.triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();

        return mesh;
    }

    private static meshStruct createMeshStruct(string filename)
    {
        int triangles = 0;
        int vertices = 0;
        int vt = 0;
        int vn = 0;
        int face = 0;
        meshStruct mesh = new meshStruct();
        mesh.fileName = filename;
        StreamReader stream = File.OpenText(filename);
        string entireText = stream.ReadToEnd();
        stream.Close();
        using (StringReader reader = new StringReader(entireText))
        {
            
            string currentText = reader.ReadLine();
            char[] splitIdentifier = { ' ' };
            string[] brokenString;
            while (currentText != null)
            {
                if (!currentText.StartsWith("f ") && !currentText.StartsWith("v ") && !currentText.StartsWith("vt ")
                    && !currentText.StartsWith("vn "))
                {
                    currentText = reader.ReadLine();
                    if (currentText != null)
                    {
                        currentText = currentText.Replace("  ", " ");
                    }
                }
                else
                {
                    currentText = currentText.Trim();                           //Trim the current line
                    brokenString = currentText.Split(splitIdentifier, 50);      //Split the line into an array, separating the original line by blank spaces
                    switch (brokenString[0])
                    {
                        case "v":
                            vertices++;
                            break;
                        case "vt":
                            vt++;
                            break;
                        case "vn":
                            vn++;
                            break;
                        case "f":
                            face = face + brokenString.Length - 1;
                            triangles = triangles + 3 * (brokenString.Length - 2); /*brokenString.Length is 3 or greater since a face must have at least
                                                                                     3 vertices.  For each additional vertice, there is an additional
                                                                                     triangle in the mesh (hence this formula).*/
                            break;
                    }
                    currentText = reader.ReadLine();
                    if (currentText != null)
                    {
                        currentText = currentText.Replace("  ", " ");
                    }
                }
            }
        }

        mesh.numUv = vt;
        mesh.numVerts = vertices;
        mesh.numNorm = vn;
        mesh.triangles = new int[triangles];
        mesh.vertices = new Vector3[vertices];
        mesh.uv = new Vector2[vt];
        mesh.normals = new Vector3[vn];
        mesh.faceData = new Vector3[face];
        return mesh;
    }

    private static void populateMeshStruct(ref meshStruct mesh)
    {
        StreamReader stream = File.OpenText(mesh.fileName);
        string entireText = stream.ReadToEnd();
        stream.Close();
        using (StringReader reader = new StringReader(entireText))
        {
            string currentText = reader.ReadLine();

            char[] splitIdentifier = { ' ' };
            char[] splitIdentifier2 = { '/' };
            string[] brokenString;
            string[] brokenBrokenString;
            int f = 0;
            int f2 = 0;
            int v = 0;
            int vn = 0;
            int vt = 0;
            int vt1 = 0;
            int vt2 = 0;
            while (currentText != null)
            {
                if (!currentText.StartsWith("f ") && !currentText.StartsWith("v ") && !currentText.StartsWith("vt ") &&
                    !currentText.StartsWith("vn ") && !currentText.StartsWith("g ") && !currentText.StartsWith("usemtl ") &&
                    !currentText.StartsWith("mtllib ") && !currentText.StartsWith("vt1 ") && !currentText.StartsWith("vt2 ") &&
                    !currentText.StartsWith("vc ") && !currentText.StartsWith("usemap "))
                {
                    currentText = reader.ReadLine();
                    if (currentText != null)
                    {
                        currentText = currentText.Replace("  ", " ");
                    }
                }
                else
                {
                    currentText = currentText.Trim();
                    brokenString = currentText.Split(splitIdentifier, 50);
                    switch (brokenString[0])
                    {
                        case "g":
                            break;
                        case "usemtl":
                            break;
                        case "usemap":
                            break;
                        case "mtllib":
                            break;
                        case "v":
                            mesh.vertices[v] = new Vector3(System.Convert.ToSingle(brokenString[1]), System.Convert.ToSingle(brokenString[2]),
                                                     System.Convert.ToSingle(brokenString[3]));
                            v++;
                            break;
                        case "vt":
                            mesh.uv[vt] = new Vector2(System.Convert.ToSingle(brokenString[1]), System.Convert.ToSingle(brokenString[2]));
                            vt++;
                            break;
                        case "vt1":
                            mesh.uv[vt1] = new Vector2(System.Convert.ToSingle(brokenString[1]), System.Convert.ToSingle(brokenString[2]));
                            vt1++;
                            break;
                        case "vt2":
                            mesh.uv[vt2] = new Vector2(System.Convert.ToSingle(brokenString[1]), System.Convert.ToSingle(brokenString[2]));
                            vt2++;
                            break;
                        case "vn":
                            mesh.normals[vn] = new Vector3(System.Convert.ToSingle(brokenString[1]), System.Convert.ToSingle(brokenString[2]),
                                                    System.Convert.ToSingle(brokenString[3]));
                            vn++;
                            break;
                        case "vc":
                            break;
                        case "f":

                            int j = 1;
                            List<int> intArray = new List<int>();
                            while (j < brokenString.Length && ("" + brokenString[j]).Length > 0)
                            {
                                Vector3 temp = new Vector3();
                                brokenBrokenString = brokenString[j].Split(splitIdentifier2, 3);    //Separate the face into individual components (vert, uv, normal)
                                temp.x = System.Convert.ToInt32(brokenBrokenString[0]);
                                if (brokenBrokenString.Length > 1)                                  //Some .obj files skip UV and normal
                                {
                                    if (brokenBrokenString[1] != "")                                    //Some .obj files skip the uv and not the normal
                                    {
                                        temp.y = System.Convert.ToInt32(brokenBrokenString[1]);
                                    }
                                    temp.z = System.Convert.ToInt32(brokenBrokenString[2]);
                                }
                                j++;

                                mesh.faceData[f2] = temp;
                                intArray.Add(f2);
                                f2++;
                            }
                            j = 1;
                            // Now assuming all faces are triangles so this loop doesnt really do any thing and if we dont have a triangle im not sure if this will work
                            while (j + 2 < brokenString.Length) 
                            {
                                brokenBrokenString = brokenString[1].Split(splitIdentifier2, 3);
                                mesh.triangles[f] = System.Convert.ToInt32(brokenBrokenString[1]) - 1;
                                f++;
                                brokenBrokenString = brokenString[2].Split(splitIdentifier2, 3);
                                mesh.triangles[f] = System.Convert.ToInt32(brokenBrokenString[1]) - 1; 
                                f++;
                                brokenBrokenString = brokenString[3].Split(splitIdentifier2, 3);
                                mesh.triangles[f] = System.Convert.ToInt32(brokenBrokenString[1]) - 1;
                                f++;

                                j++;
                            }
                            break;
                    }
                    currentText = reader.ReadLine();
                    if (currentText != null)
                    {
                        currentText = currentText.Replace("  ", " ");       //Some .obj files insert double spaces, this removes them.
                    }
                }
            }
        }
    }
}