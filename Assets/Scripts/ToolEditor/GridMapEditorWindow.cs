using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class GridMapEditorWindow : EditorWindow
{
    // --- CẤU HÌNH ---
    private Vector2Int gridSize = new Vector2Int(1, 1);

    private Vector3 originPosition = Vector3.zero;
    
    private bool isMapCreated = false;

    // Data for brick

    private string brickFolderPath = "Assets/Prefabs/MapBricks";
    private BrickBase[] availableBrick;

    private string[] brickNames;
    private int selectedBrickIndex = 0;

    // Cache data

    private List<Pair<Vector2Int, BlockState>> mapBricks;

    private Dictionary<Vector2Int , bool> placedBrickDict;

    private Transform root;
    

    //Create menu item on Unity
    [MenuItem("Tools/Map Designer")]
    public static void ShowWindow()
    {
        GetWindow<GridMapEditorWindow>("Map Designer");
    }


    //UI on inspector

    private void OnGUI()
    {


        GUILayout.Label("EDIT SIZE OF MAP (Square Size)");

        gridSize.x = EditorGUILayout.IntField("Size X:", gridSize.x);
        gridSize.y = EditorGUILayout.IntField("Size Y:", gridSize.y);

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Create Map"))
        {
            //Draw map again
            isMapCreated = true;
            SceneView.RepaintAll();
        }

        EditorGUILayout.Space(10);

        GUILayout.Label("CHỌN BRICK");

        if(brickNames != null && brickNames.Length > 0)
        {
            selectedBrickIndex = EditorGUILayout.Popup("Choosing block: ", selectedBrickIndex, brickNames);
        }

        if (GUILayout.Button("Làm mới danh sách Gạch"))
        {
            LoadBricksFromAsset();
        }

    }

    private void LoadBricksFromAsset()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] {brickFolderPath});
        List<BrickBase> validBricks = new List<BrickBase>();

        foreach(string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefabAsset != null)
            {
                BrickBase brickComponent = prefabAsset.GetComponent<BrickBase>();
                if (brickComponent != null) validBricks.Add(brickComponent);
            }
        }
        availableBrick = validBricks.ToArray();
        brickNames = new string[availableBrick.Length];
        for(int i = 0; i < availableBrick.Length; i++)
        {
            brickNames[i] = availableBrick[i].gameObject.name;
        }
    }

    private void CreateMap()
    {
        Handles.color = new Color(0f, 1f, 1f, 0.8f);
        //Draw Grid map by draw x line horizontal and y line vertical

        for(int row = 0; row <= gridSize.x; row++)
        {
            Handles.DrawLine(new Vector3(-row, 0f, 0f), new Vector3(-row, 0f, -gridSize.y));
        }

        for(int collumn = 0; collumn <= gridSize.y; collumn++)
        {
            Handles.DrawLine(new Vector3(0, 0f, -collumn), new Vector3(-gridSize.x, 0f, -collumn));
        }
    }

    // Subcribe event to draw on Scene view
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    // Nơi bạn xử lý thao tác click chuột và vẽ lưới trên Scene 3D
    private void OnSceneGUI(SceneView sceneView)
    {
        //Draw map
        if (isMapCreated)
        {
            CreateMap();
        } 

             
    }


}

public struct Pair<T1, T2>
{
    public T1 First;
    public T2 Second;

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}
