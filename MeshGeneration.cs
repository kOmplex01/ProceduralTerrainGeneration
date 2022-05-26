using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshGeneration : MonoBehaviour
{

    public ComputeShader shader;
    //mesh details
    private int mapResolution = 256;
    [Range(0, 10)]
    public int octaves;
    [Range(0.1f, 30)]
    public float persistance;
    [Range(0.1f, 2)]
    public float lacunarity;
    [Range(0, 200)]
    public float depth;
    [Range(0.1f, 10)]
    public float scale;
    [Range(-10f, 10f)]
    public float offsetX;
    [Range(-10f, 10f)]
    public float offsetZ;


    //kernal handles for shaders
    int noiseHandle;
    int TrisHandle;
    int textureHandle;

    ComputeBuffer heights;
    ComputeBuffer heightMapBuffer;
    ComputeBuffer trisBuffer;

    Vector2[] heightValues;
    Vector3[] heightMap;
    int[] trisMap;

    MeshRenderer _renderer;
    RenderTexture texture;

    Mesh mesh;


    void Start()
    {
        FirstStep();
    }

    public void FirstStep()
    {
        //enabling the texture, and setting the dimensions of the mesh/map
        texture = new RenderTexture(mapResolution, mapResolution, 1);
        texture.enableRandomWrite = true;
        texture.Create();

        _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = true;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //defining diiferet buffers
        heights = new ComputeBuffer((mapResolution + 1) * (mapResolution + 1), sizeof(float) * 2);
        heightValues = new Vector2[(mapResolution + 1) * (mapResolution + 1)];
        heightMap = new Vector3[(mapResolution + 1) * (mapResolution + 1)];
        heightMapBuffer = new ComputeBuffer((mapResolution + 1) * (mapResolution + 1), sizeof(float) * 3);
        trisBuffer = new ComputeBuffer(mapResolution * mapResolution * 6, sizeof(int));
        trisMap = new int[mapResolution * mapResolution * 6];

        InitShader(); //initialising shader
    }

    void InitShader()
    {
        //setting the kernal handles
        noiseHandle = shader.FindKernel("Height");
        TrisHandle = shader.FindKernel("Triangle");
        textureHandle = shader.FindKernel("Texture");
        CreateGround(); //return a 2D array with different points of x and z


        InitValues(); //initialising shader values

        heights.SetData(heightValues); //setting array(returned by CreateGround Method) to buffer

        //setting some more shader values
        shader.SetBuffer(noiseHandle, "Heights", heights);
        shader.SetBuffer(noiseHandle, "HeightMap", heightMapBuffer);
        shader.SetBuffer(textureHandle, "Heights", heights);
        shader.SetBuffer(TrisHandle, "TrisMap", trisBuffer);

        //calling the different shader pragmas
        Dispatch(noiseHandle, (mapResolution + 1)); //making height map using perlin noise
        Dispatch(TrisHandle, mapResolution); //making triangles and vertices array
        Dispatch(textureHandle, 16, 16); //used for texture, not implemented yet

        //getting data from the shader
        heightMapBuffer.GetData(heightMap);
        trisBuffer.GetData(trisMap);

        //releasing the memory of the buffer
        heightMapBuffer.Release();
        heights.Release();
        trisBuffer.Release();


        //creating the mesh by the data collected
        CreateMesh();

    }

    void Dispatch(int handle, int count)
    {
        shader.Dispatch(handle, count, 1, 1);
    }

    void Dispatch(int handle, int xCount, int yCount)
    {
        shader.Dispatch(handle, xCount, yCount, 1);
    }

    void InitValues()
    {
        shader.SetInt("mapResolution", mapResolution);

        shader.SetInt("octaves", octaves);
        shader.SetFloat("persistance", persistance);
        shader.SetFloat("lacunarity", lacunarity);

        shader.SetFloat("offsetX", offsetX * 100f);
        shader.SetFloat("offsetZ", offsetZ * 100f);
        shader.SetFloat("depth", depth);
        shader.SetFloat("scale", scale);


        shader.SetTexture(textureHandle, "Result", texture);
        _renderer.sharedMaterial.mainTexture = texture;

    }

    void CreateGround()
    {
        for (int i = 0, x = 0; x <= mapResolution; x++)
        {
            for (int z = 0; z <= mapResolution; z++)
            {

                heightValues[i] = new Vector2(x, z);
                i++;
            }
        }
    }

    void CreateMesh()
    {
        mesh.Clear();

        mesh.vertices = heightMap;
        mesh.triangles = trisMap;


        mesh.RecalculateNormals();

    }
}
