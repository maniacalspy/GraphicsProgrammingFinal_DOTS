﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour
{
    Mesh mesh;


    enum MeshShapes { triangle, square, quad, loop};

    [SerializeField]
    MeshShapes thisShape;


    Vector3[] verticies;
    int[] triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (thisShape)
        {
            case (MeshShapes.triangle):
                MakeTriangleMeshData();
                break;
            case (MeshShapes.square):
                MakeSquareMeshData(1);
                break;
            case (MeshShapes.quad):
                MakeQuadMeshData();
                break;
            case (MeshShapes.loop):
                MakeLoopMeshData();
                break;
            default:
                MakeTriangleMeshData();
                break;
        }
        
        CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeTriangleMeshData()
    {
        verticies = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0)};
        triangles = new int[] { 0, 1, 2 };
    }
    public void MakeSquareMeshData(float xextents)
    {
        verticies = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, xextents), new Vector3(xextents, 0, 0), new Vector3(xextents,0,xextents)};
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    public void MakeQuadMeshData()
    {
        verticies = new Vector3[] { new Vector3(0,0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(1,0,1), //bottom face
                                    new Vector3(0,0,0), new Vector3(0,1,0), new Vector3 (1,0,0), new Vector3 (1,1,0), //front face
                                    new Vector3(0,1,0), new Vector3(0,1,1), new Vector3(1,1,0), new Vector3(1,1,1), //top face
                                    new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(0,1,1), new Vector3(1,1,1), //back face
                                    new Vector3(0,0,0), new Vector3(0,0,1), new Vector3(0,1,0), new Vector3(0,1,1), //left face
                                    new Vector3(1,0,0), new Vector3(1,1,0), new Vector3(1,0,1), new Vector3(1,1,1) //right face
        };
                                    
        triangles = new int[] { 0, 1, 2, 2,1,3,
                                4, 5, 6, 6, 5, 7,
                                8, 9, 10, 10, 9, 11,
                                12, 13, 14, 14, 13, 15,
                                16, 17, 18, 18, 17, 19,
                                20, 21, 22, 22, 21, 23
        };
    }

    public void MakeLoopMeshData()
    {
        List<Vector3> NewVerticies = new List<Vector3>();
        List<int> newTriangles = new List<int>();
        for (int i = 1; i < 6; i++)
        {
            NewVerticies.Add(new Vector3(i, i, i));
            NewVerticies.Add(new Vector3(i, i+1, i+2));
            NewVerticies.Add(new Vector3(i+2, i+1, i));
            newTriangles.Add(3*(i-1));
            newTriangles.Add(3 * (i-1) + 1);
            newTriangles.Add(3 * (i-1) + 2);
        }

        verticies = NewVerticies.ToArray();
        triangles = newTriangles.ToArray();
    }

    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;


        mesh.RecalculateNormals();
    }

}
