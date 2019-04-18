using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    // Object refs
    GameObject EnemyRef; // This is the parent object
    GameObject PlayerRef; // this is the parent object
    public GameObject EnemyAnchorRef; // G.O. that anchors enemy in pos with spring joint

    // Texts
    public Text EnemyHealthText;

    // Construction variables
    string EnemyName = "Enemy_Fire";
    string[] PartName = { "Face", "Body", "Arm1", "Arm2", "Leg1", "Leg2"};
    GameObject Face;
    GameObject Body;
    GameObject Arm1;
    GameObject Arm2;
    GameObject Leg1;
    GameObject Leg2;

    List<Vector3[]> EnemiesList = new List<Vector3[]>();
    public Vector3[] EnemyVert;
    public Vector3[] EnemyVert2;
    public Vector3[] EnemyVert3;
    public Vector3[] EnemyVert4;
    

    // Materials
    public Material NonElementalMat;
    public Material AirMat;
    public Material EarthMat;
    public Material FireMat;
    public Material NatureMat;
    public Material WaterMat;
    Material MatToApply;

    Elements.ElementType EnemyType;

    float SeperationMultipler = 0.03f;
    float ScaleMultiplier = 0.2f;
    float Drag = 0.3f;

    // Spawn offset
    Vector3 EnemySpawnOffset = new Vector3(6f, 2f, 0f); // Spawn to the left and up

    void Start()
    {
        // Add enemy vector3 arrays into a list
        EnemiesList.Add(EnemyVert);
        EnemiesList.Add(EnemyVert2);
        EnemiesList.Add(EnemyVert3);
        EnemiesList.Add(EnemyVert4);

        // Set material to match type
        MatToApply = NatureMat;
        BuildEnemy();

        // Give battle manager enemy ref
        gameObject.GetComponent<BattleManager>().SetEnemyRef(EnemyRef);

        // Set anchor
        EnemyAnchorRef.GetComponent<SpringJoint>().connectedBody = EnemyRef.GetComponentInChildren<Rigidbody>();        
    }
    
    internal void BuildEnemy()
    {
        // Parent object
        EnemyRef = new GameObject(EnemyName);

        Vector3[] PartVectorShape = ChooseEnemyShape();

        // Body
        Body = CreateMesh(PartName[1], PartVectorShape);
        
        // Add Enemy script to body object
        Body.AddComponent<Enemy>();

        // Add Enemy tag
        Body.tag = "Enemy";

        // Set Enemy Stats
        int EvolutionCount = 1;
        EnemyType = Elements.ElementType.Nature;

        // Adjust scale multiplier and seperation multiplier to factor in evolution
        if (EvolutionCount > 0)
        {
            ScaleMultiplier *= EvolutionCount;
            SeperationMultipler *= EvolutionCount;
        }
        else // Hasnt evolved yet
        {
            ScaleMultiplier *= 0.8f;
            SeperationMultipler *= 0.8f;
        }
        
        // Define Part seperation offset
        Vector3 baseSize = Body.GetComponent<Renderer>().bounds.size;
        float PartSeperationOffset = (((baseSize.x * baseSize.y) / 2) * SeperationMultipler); // Get average of x and y axis then * by scale multipler

        // Scale Body size down
        Body.transform.localScale *= (2 * ScaleMultiplier);

        // Set parent
        Body.transform.parent = EnemyRef.transform;

        // Add capsule collider to body
        Body.AddComponent<CapsuleCollider>();
        Body.GetComponent<CapsuleCollider>().radius = baseSize.x / 4;
        Body.GetComponent<CapsuleCollider>().height = baseSize.y * 2;

        // Face
        Face = CreateMesh(PartName[0], PartVectorShape);
        // Scale size down
        Face.transform.localScale *= ScaleMultiplier;
        // Add cube collider
        Face.AddComponent<BoxCollider>();
        // Move face up
        Face.transform.position = new Vector3(Face.transform.position.x, Face.transform.position.y + (4 * PartSeperationOffset), Face.transform.position.z);
        // Set parent
        Face.transform.parent = EnemyRef.transform;


        // Arm1
        Arm1 = CreateMesh(PartName[2], PartVectorShape);
        // Scale size down
        Arm1.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move arm left and up
        Arm1.transform.position = new Vector3(Arm1.transform.position.x - (2 * PartSeperationOffset), Arm1.transform.position.y + (2 * PartSeperationOffset), Arm1.transform.position.z);
        // Set parent
        Arm1.transform.parent = EnemyRef.transform;

        // Arm2
        Arm2 = CreateMesh(PartName[3], PartVectorShape);
        // Scale size down
        Arm2.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move arm right and up
        Arm2.transform.position = new Vector3(Arm2.transform.position.x + (2 * PartSeperationOffset), Arm2.transform.position.y + (2 * PartSeperationOffset), Arm2.transform.position.z);
        // Set parent
        Arm2.transform.parent = EnemyRef.transform;

        // Leg1
        Leg1 = CreateMesh(PartName[4], PartVectorShape);
        // Scale size down
        Leg1.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move leg left and down
        Leg1.transform.position = new Vector3(Leg1.transform.position.x - PartSeperationOffset, Leg1.transform.position.y - (2 * PartSeperationOffset), Leg1.transform.position.z);
        // Set parent
        Leg1.transform.parent = EnemyRef.transform;

        // Load Leg2
        Leg2 = CreateMesh(PartName[5], PartVectorShape);
        // Scale size down
        Leg2.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move leg right and down
        Leg2.transform.position = new Vector3(Leg2.transform.position.x + PartSeperationOffset, Leg2.transform.position.y - (2 * PartSeperationOffset), Leg2.transform.position.z);
        // Set parent
        Leg2.transform.parent = EnemyRef.transform;

        // Set up rididbodies and character joints
        SetUpCharacterAsRagdoll(Body, Face, Arm1, Arm2, Leg1, Leg2);

        // Spawn above ground and to the right
        EnemyRef.transform.position += EnemySpawnOffset; 

        // Set player ref
        Body.GetComponent<Enemy>().PlayerRef = PlayerRef;
    }

    GameObject CreateMesh(string meshPartName, Vector3[] newVerts)
    {
        // Convert from 3d to 2d
        Vector2[] newVerts2D = new Vector2[newVerts.Length];

        for (int i = 0; i < newVerts2D.Length; i++)
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

        // Create the mesh
        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.triangles = indices;
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();

        // Setup gameobject with mesh
        GameObject MeshObject = new GameObject(meshPartName, typeof(MeshFilter), typeof(MeshRenderer));
        MeshObject.GetComponent<MeshFilter>().mesh = newMesh;
        MeshObject.GetComponent<MeshRenderer>().material = MatToApply;

        return MeshObject;
    }

    void SetUpCharacterAsRagdoll(GameObject body, GameObject face, GameObject arm1, GameObject arm2, GameObject leg1, GameObject leg2)
    {
        // Set up rigidbodies
        body.AddComponent<Rigidbody>();
        face.AddComponent<Rigidbody>();
        arm1.AddComponent<Rigidbody>();
        arm2.AddComponent<Rigidbody>();
        leg1.AddComponent<Rigidbody>();
        leg2.AddComponent<Rigidbody>();

        // Rigidbody drag
        body.GetComponent<Rigidbody>().drag = Drag;
        face.GetComponent<Rigidbody>().drag = Drag;
        arm1.GetComponent<Rigidbody>().drag = Drag;
        arm2.GetComponent<Rigidbody>().drag = Drag;
        leg1.GetComponent<Rigidbody>().drag = Drag;
        leg2.GetComponent<Rigidbody>().drag = Drag;

        // Set constrains on body
        body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        // Set parts to character layer
        body.layer = 9; face.layer = 9;
        arm1.layer = 9; arm2.layer = 9;
        leg1.layer = 9; leg2.layer = 9;

        // Character joints (All starting from body and going out to other parts)
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();

        CharacterJoint[] CharacterJoints = body.GetComponents<CharacterJoint>();

        // Anchor offsets based on body size
        float bodySizeX = body.GetComponent<Renderer>().bounds.size.x;
        float bodySizeY = body.GetComponent<Renderer>().bounds.size.x;

        // Body to face
        CharacterJoints[0].connectedBody = face.GetComponent<Rigidbody>(); // Connect body to face
        CharacterJoints[0].axis = new Vector3(1, 0, 0); // Set axis
        // Set joint anchor y
        CharacterJoints[0].anchor = new Vector3(0f, bodySizeY / 2, 0f);

        // Body to arm1 (left)
        CharacterJoints[1].connectedBody = arm1.GetComponent<Rigidbody>(); // Connect body to arm1
        CharacterJoints[1].axis = new Vector3(0, 1, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[1].anchor = new Vector3(-bodySizeX / 3, bodySizeY / 3, 0f);

        // Body to arm2 (right)
        CharacterJoints[2].connectedBody = arm2.GetComponent<Rigidbody>(); // Connect body to arm2
        CharacterJoints[2].axis = new Vector3(0, -1, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[2].anchor = new Vector3(bodySizeX / 3, bodySizeY / 3, 0f);

        // Body to leg1 (left)
        CharacterJoints[3].connectedBody = leg1.GetComponent<Rigidbody>(); // Connect body to leg1
        CharacterJoints[3].axis = new Vector3(1, 0, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[3].anchor = new Vector3(-bodySizeX * 0.4f, -bodySizeY * 0.4f, 0f);

        // Body to leg2 (right)
        CharacterJoints[4].connectedBody = leg2.GetComponent<Rigidbody>(); // Connect body to leg2
        CharacterJoints[4].axis = new Vector3(1, 0, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[4].anchor = new Vector3(bodySizeX * 0.4f, -bodySizeY * 0.4f, 0f);

        // Fixed joint from face to body
        face.AddComponent<FixedJoint>();
        face.GetComponent<FixedJoint>().connectedBody = body.GetComponent<Rigidbody>();
    }

    Vector3[] ChooseEnemyShape()
    {
        int rand = Random.Range(0, EnemiesList.Count);

        Vector3[] vectorToReturn = EnemiesList[rand];

        return vectorToReturn;
    }

    public void SetPlayerRef(GameObject playerRef)
    {
        PlayerRef = playerRef;
    }

    public GameObject GetPlayerRef()
    {
        return PlayerRef;
    }

    // Text update methods
    public void UpdateText_EnemyHealth(string currentEnemyHealth)
    {
        EnemyHealthText.text = currentEnemyHealth;
    }
    
}
