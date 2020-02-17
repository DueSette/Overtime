using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))] // Ensures there is a mesh filter component on object this is attatched to 
public class HillGenerator : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 40;
    public float random;

    public float xPosition;

    public float timer = 3f;


    // Start is called before the first frame update
    void Start()
    {
        xPosition = transform.position.x;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;     
    }



    private void Update()
    {
        UpdateMesh();
        CreateShape();

        timer += Time.deltaTime;

        if (timer >= 5)
        {
            xSize = xSize + 10;
            //zSize = zSize + 10;
            timer = 0;

        }

    }


    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z <= zSize; z++) // Loops through all Vertices on Z axis
        {



            for (int x = 0; x <= xSize; x++) // Loops through all Vertices on Z axis
            {

                float y = Mathf.PerlinNoise(x * .1f, z * .1f) * .3f;

                if(i > xSize * 10 && i < xSize * 30 || i == xSize * xSize || i  < xSize)
                {
                    y = 0;
                }
                else
                {
                    y = Mathf.PerlinNoise(x * .3f, z * .3f) * 3f;
                }

                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;


        for (int z = 0; z < zSize; z++)
        {

            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }

        

    }


    void UpdateMesh()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 1);

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); // Stops strange light effects from PCG triangles.
    }



   /* private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }*/


    /* vertices = new Vector3[]
        {
            new Vector3 (0,0,0),
            new Vector3 (0,0,1), // Offsets by 1 on the Z axis
            new Vector3 (1,0,0), // Offsets by 1 on the X axis
            new Vector3 (1,0,1)
        };

        triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2
        };*/
}
