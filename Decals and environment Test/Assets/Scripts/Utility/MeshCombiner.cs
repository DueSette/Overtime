using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> meshesToCombine;

    [SerializeField]
    private string outputFileName;

    public void CombineMesh()
    {
        Debug.Log("Combining " + meshesToCombine.Count + " Meshes");

        //Getting Info
        meshesToCombine = GetChildrenWithModels(meshesToCombine);
        List<Tuple<Mesh, List<Material>>> modelsInfo = GetMeshAndMaterialInformation(GetChildrenWithModels(meshesToCombine));
        List<Material> uniqueMaterials = GetUniqueMaterials(modelsInfo);

        // Creating Combined Mesh
        List<Mesh> submeshes = CreateSubMeshes(modelsInfo);
        Mesh finalMesh = CombineSubMeshes(submeshes);

#if UNITY_EDITOR
        // Save Combined Mesh
        SaveMeshToAssets(finalMesh, outputFileName + "Full");
#endif

        // Creating Gameobject into the scene
        MakeGameobject(finalMesh, uniqueMaterials);
    }


    /*
    ====================================================================================================
    Getting Mesh & Material Information
    ====================================================================================================
    */
    private List<GameObject> GetChildrenWithModels(List<GameObject> objectsToCheck)
    {
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < objectsToCheck.Count; i++)
        {
            MeshRenderer[] childMRs = objectsToCheck[i].GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mr in childMRs)
            {
                GameObject childWithModel = mr.gameObject;
                children.Add(childWithModel);
            }
        }

        return children;
    }

    private List<Tuple<Mesh, List<Material>>> GetMeshAndMaterialInformation(List<GameObject> objectsWithModelsToCombine)
    {
        List<Tuple<Mesh, List<Material>>> collectedInfo = new List<Tuple<Mesh, List<Material>>>();

        for (int i = 0; i < objectsWithModelsToCombine.Count; i++)
        {
            // Getting Mesh
            Mesh mesh = objectsWithModelsToCombine[i].GetComponent<MeshFilter>().sharedMesh;

            // Getting Materials
            List<Material> materials = objectsWithModelsToCombine[i].GetComponent<MeshRenderer>().sharedMaterials.ToList();

            // Saving Info
            Tuple<Mesh, List<Material>> newInfo = new Tuple<Mesh, List<Material>>(mesh, materials);
            collectedInfo.Add(newInfo);
        }

        return collectedInfo;
    }

    private List<Material> GetUniqueMaterials(List<Tuple<Mesh, List<Material>>> modelsInfo)
    {
        // Getting Number Of Different Materials That Make Up The Seperate Meshes
        List<Material> meshMaterials = new List<Material>();
        for (int i = 0; i < modelsInfo.Count; i++)
        {
            for (int j = 0; j < modelsInfo[i].Item2.Count; j++)
            {
                if (!meshMaterials.Contains(modelsInfo[i].Item2[j]))
                {
                    meshMaterials.Add(modelsInfo[i].Item2[j]);
                }
            }
        }
        return meshMaterials;
    }


    /*
    ====================================================================================================
    Generating Model
    ====================================================================================================
    */
    private List<Mesh> CreateSubMeshes(List<Tuple<Mesh, List<Material>>> modelsInfo)
    {
        List<Mesh> createdSubmeshes = new List<Mesh>();
        List<Material> meshMaterials = GetUniqueMaterials(modelsInfo);

        // Creating a submesh for each material
        for (int i = 0; i < meshMaterials.Count; i++)
        {
            Mesh newSubmesh = new Mesh();
            List<CombineInstance> combineInstance = new List<CombineInstance>();

            for (int j = 0; j < modelsInfo.Count; j++) // foreach model to combine
            {
                for (int k = 0; k < modelsInfo[j].Item2.Count; k++) // check each submesh in the model
                {
                    if (modelsInfo[j].Item2[k] == meshMaterials[i]) // check that it uses the material
                    {
                        // Copying Submesh Across
                        Mesh subMesh = new Mesh();
                        subMesh.vertices = modelsInfo[j].Item1.vertices;
                        subMesh.triangles = modelsInfo[j].Item1.GetTriangles(k);
                        subMesh.normals = modelsInfo[j].Item1.normals;
                        subMesh.uv = modelsInfo[j].Item1.uv;
                        subMesh.uv2 = modelsInfo[j].Item1.uv2;

                        UnwrapParam unwrapParams = new UnwrapParam
                        {
                            hardAngle = 60,
                            packMargin = 16,
                            angleError = 1,
                            areaError = 1
                        };
                        Unwrapping.GenerateSecondaryUVSet(subMesh, unwrapParams);

                        CombineInstance newInstance = new CombineInstance();
                        newInstance.mesh = subMesh;
                        newInstance.transform = meshesToCombine[j].transform.localToWorldMatrix;
                        meshesToCombine[j].gameObject.SetActive(false);

                        combineInstance.Add(newInstance);
                    }
                }
            }

            newSubmesh.CombineMeshes(combineInstance.ToArray());
            createdSubmeshes.Add(newSubmesh);
        }


        Debug.Log("Created " + createdSubmeshes.Count + " Submeshes");
        return createdSubmeshes;
    }


    private Mesh CombineSubMeshes(List<Mesh> submeshesToCombine)
    {
        Mesh combinedMesh = new Mesh();
        CombineInstance[] combineInstance = new CombineInstance[submeshesToCombine.Count];

        // Combining submeshes into a single asset
        for (int i = 0; i < submeshesToCombine.Count; i++)
        {
            combineInstance[i].mesh = submeshesToCombine[i];
            combineInstance[i].transform = this.transform.localToWorldMatrix;
        }
        combinedMesh.CombineMeshes(combineInstance, false);

        // Generating Lightmap UV Settings
        Unwrapping.GenerateSecondaryUVSet(combinedMesh);
        
        return combinedMesh;
    }

    private void MakeGameobject(Mesh newMesh, List<Material> meshMaterials)
    {
        MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
        mf.sharedMesh = newMesh;


        MeshRenderer mr = this.gameObject.AddComponent<MeshRenderer>();
        mr.sharedMaterials = meshMaterials.ToArray();
    }


    /*
    ====================================================================================================
    Saving Asset
    ====================================================================================================
    */
    private void SaveMeshToAssets(Mesh meshToSave, string fileName)
    {
        // Saving To Assets
        string filePath = "Assets/";
        filePath += fileName;
        filePath += "_Mesh.asset";
        AssetDatabase.CreateAsset(meshToSave, filePath);
    }
}