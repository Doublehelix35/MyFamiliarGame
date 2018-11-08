using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour {

    Vector3[] VectorArrayHead = new Vector3[1000];
    private Vector3 FirstTouchPos;   // First touch position
    private Vector3 LastTouchPos;   // Last touch position

    private LineRenderer lineRend;
    int LinePointCount = 1;
    public float MinDistanceBetweenPoints = 0.2f;
    public float LineWidth = 0.2f;
    public Material LineMat;

    GameObject ActiveMeshObject;
    public Material FaceMaterial;

    public Toggle ResetToggle;

    public Vector3[] testVert;


    void Start()
    {
        //ActiveMeshObject = CreateMesh("Face", testVert);

        lineRend = gameObject.AddComponent<LineRenderer>();
        lineRend.material = LineMat;
        lineRend.widthMultiplier = LineWidth;
        lineRend.positionCount = LinePointCount; // Set line size
        lineRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

	void Update ()
    {
        if(Input.touchCount >= 1) // user is touching the screen with a touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) // check for the first touch
            {
                // Update touch positions
                FirstTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
                LastTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

                // Start of line
                lineRend.positionCount = LinePointCount; // Set line size
                lineRend.SetPosition(0, new Vector3(FirstTouchPos.x, FirstTouchPos.y, 0f)); // set pos of new line segment
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                // Check distance from last point
                float dist = Vector3.Distance(LastTouchPos, Camera.main.ScreenToWorldPoint(touch.position));

                if (Mathf.Abs(dist) >= MinDistanceBetweenPoints)
                {
                    // Update touch position
                    LastTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

                    // Next point of line
                    LinePointCount++;
                    lineRend.positionCount = LinePointCount; // Increase line size
                    lineRend.SetPosition(LinePointCount - 1, new Vector3(LastTouchPos.x, LastTouchPos.y, 0f)); // set pos of new line segment
                    
                }
                
            }
            else if (touch.phase == TouchPhase.Ended) // check if the finger is removed from the screen
            {
                // End of line
                lineRend.positionCount = LinePointCount; // Set line size
                lineRend.SetPosition(LinePointCount - 1, new Vector3(FirstTouchPos.x, FirstTouchPos.y, 0f));
                LinePointCount = 1;

                // Convert into mesh
                Vector3[] LineVerts = new Vector3[lineRend.positionCount];
                lineRend.GetPositions(LineVerts);
                ActiveMeshObject = CreateMesh("Face", LineVerts);

                // Destroy and reset
                Destroy(ActiveMeshObject, 3f);
                ActiveMeshObject = new GameObject();
            }
        }
    }

    public void ClearLine(bool ClearIt)
    {
        if (ClearIt)
        {
            LinePointCount = 1;
            lineRend.positionCount = LinePointCount;
            ResetToggle.isOn = false;
        }
        
    }

    GameObject CreateMesh(string meshName, Vector3[] newVerts)
    {
        // Convert from 3d to 2d
        Vector2[] newVerts2D = new Vector2[newVerts.Length];
                
        for(int i = 0; i < newVerts2D.Length; i++)
        {
            newVerts2D[i] = new Vector2(newVerts[i].x, newVerts[i].y);
        }        

        Triangulator tr = new Triangulator(newVerts2D);
        int[] indices = tr.Triangulate();

        // Create vector 3 vertices
        Vector3[] vertices = new Vector3[newVerts2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(newVerts2D[i].x, newVerts2D[i].y, 0f);
        }

        Mesh newMesh = new Mesh();

        newMesh.vertices = vertices;
        newMesh.triangles = indices;
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();

        GameObject MeshObject = new GameObject(meshName, typeof(MeshFilter), typeof(MeshRenderer));

        MeshObject.GetComponent<MeshFilter>().mesh = newMesh;
        MeshObject.GetComponent<MeshRenderer>().material = FaceMaterial;

        return MeshObject;

        // Fan techinque

        //Vector3[] vertices = newVerts;
        //Vector2[] uv = new Vector2[newVerts.Length];
        //int[] triangles = new int[(newVerts.Length - 2) * 3];

        //Debug.Log(triangles.Length);

        //for(int i = 0; i < newVerts.Length; i++)
        //{
        //    uv[i] = newVerts[i];
        //}

        //if(triangles.Length <= 3)
        //{
        //    triangles[0] = 0;
        //    triangles[1] = 1;
        //    triangles[2] = 2;
        //}
        //else
        //{
        //    for (int i = 0; i < triangles.Length / 3; i++)
        //    {                
        //        triangles[i * 3 + 0] = 0;
        //        triangles[i * 3 + 1] = i + 1;
        //        triangles[i * 3 + 2] = i + 2;
        //    }            
        //}


        //Mesh newMesh = new Mesh();

        //newMesh.vertices = vertices;
        //newMesh.uv = uv;
        //newMesh.triangles = triangles;

        //GameObject MeshObject = new GameObject(meshName, typeof(MeshFilter), typeof(MeshRenderer));

        //MeshObject.GetComponent<MeshFilter>().mesh = newMesh;
        //MeshObject.GetComponent<MeshRenderer>().material = FaceMaterial;

        //return MeshObject;
    }
}
