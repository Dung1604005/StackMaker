using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridMapEditorWindow : EditorWindow
{
    // Data general
    private Vector2Int gridSize = new Vector2Int(1, 1);

    private Vector3 originPosition = Vector3.zero;

    private bool isMapCreated = false;

    private IToolEditorState[] allToolStates;
    private string[] tabNames;

    //Data for map

    private Dictionary<Vector2Int, BrickBase> placedBrickDict = new Dictionary<Vector2Int, BrickBase>();

    private string currentMapName;

    private int currentLevelId;

    private Vector2Int startPosition;

    // Data for brick

    private string brickFolderPath = "Assets/Prefabs/MapBricks";
    private BrickBase[] availableBrick;

    private string[] brickNames;
    private int selectedBrickIndex = 0;

    private BrickBase previewBrick;

    // Cache data

    private IToolEditorState currentToolState;

    private int currentTabIndex = 0;

    private Transform root;

    #region Getter

    public Vector2Int GridSize => gridSize;


    public BrickBase PreviewBrick => previewBrick;

    public BrickBase[] AvailableBrick => availableBrick;

    public int SelectedBrickIndex => selectedBrickIndex;

    public string[] BrickNames => brickNames;

    public Dictionary<Vector2Int, BrickBase> PlacedBrickDict => placedBrickDict;

    public Transform Root => root;
    #endregion

    #region SETTER
    public void SetRoot(Transform _root)
    {
        root = _root;
    }

    public void SetSelectedBrick(int index)
    {
        selectedBrickIndex = index;
    }

    public void AddBrick(Vector2Int gridPos, BrickBase brick)
    {
        //This pos have brick
        if (placedBrickDict.ContainsKey(gridPos) && placedBrickDict[gridPos] != null)
        {
            return;
        }

        placedBrickDict.Add(gridPos, brick);
    }
    public void RemoveBrick(Vector2Int gridPos)
    {
        if (placedBrickDict.ContainsKey(gridPos))
        {
            BrickBase oldBrick = placedBrickDict[gridPos];

            Undo.DestroyObjectImmediate(oldBrick.gameObject);

            placedBrickDict.Remove(gridPos);
        }

    }

    #endregion

    public void ChangeState(IToolEditorState newState)
    {
        //Exit old state to change to new state
        if (currentToolState != null)
        {
            currentToolState.Exit();
        }


        currentToolState = newState;

        if (currentToolState != null)
        {
            currentToolState.Enter();
        }

    }


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



        // Draw tool bar for choosing tool editor mode
        EditorGUI.BeginChangeCheck();
        currentTabIndex = GUILayout.Toolbar(currentTabIndex, tabNames, GUILayout.Height(30));

        // if user click another tab then change state
        if (EditorGUI.EndChangeCheck())
        {
            // disable all focus
            GUI.FocusControl(null);
            ChangeState(allToolStates[currentTabIndex]);
        }

        GUILayout.Space(15);

        //Draw UI for tool mode
        if (currentToolState != null)
        {
            currentToolState.OnGUI();
        }


        EditorGUILayout.Space(10);
        if (GUILayout.Button("Clear Map"))
        {
            ClearMap();
        }

    }

    public void LoadBricksFromAsset()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { brickFolderPath });
        List<BrickBase> validBricks = new List<BrickBase>();

        foreach (string guid in guids)
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
        for (int i = 0; i < availableBrick.Length; i++)
        {
            brickNames[i] = availableBrick[i].gameObject.name;
        }
    }

    private void CreateMap()
    {
        Handles.color = Color.skyBlue;
        //Draw Grid map by draw x line horizontal and y line vertical

        for (int row = 0; row <= gridSize.x; row++)
        {
            Handles.DrawLine(new Vector3(-row, 0.1f, 0f), new Vector3(-row, 0.1f, -gridSize.y));
        }

        for (int collumn = 0; collumn <= gridSize.y; collumn++)
        {
            Handles.DrawLine(new Vector3(0, 0.1f, -collumn), new Vector3(-gridSize.x, 0.1f, -collumn));
        }
    }

    // Subcribe event to draw on Scene view
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        LoadBricksFromAsset();

        UpdatePreviewObject();
        //Init all ToolState
        allToolStates = new IToolEditorState[]
        {
            new SelectToolState(this),
            new PaintToolState(this),
            new EraseToolState(this)

        };

        //Init tab name for each tool state

        tabNames = new string[allToolStates.Length];
        for (int i = 0; i < tabNames.Length; i++)
        {
            tabNames[i] = allToolStates[i].GetTabName();
        }

        //Init first tool state
        currentTabIndex = 0;
        ChangeState(allToolStates[currentTabIndex]);
    }


    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        if (previewBrick != null)
        {
            DestroyImmediate(previewBrick.gameObject);
        }
        ClearMap();
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        //Draw map
        if (isMapCreated)
        {
            CreateMap();
        }

        currentToolState.OnSceneGUI(sceneView);



        sceneView.Repaint();


    }

    // Handle the preview visual for brick before place
    public void UpdatePreviewObject()
    {
        if (previewBrick != null)
        {
            DestroyImmediate(previewBrick.gameObject);
        }

        if (availableBrick != null && availableBrick.Length == 0) return;

        // create prefab
        BrickBase selectedPrefab = availableBrick[selectedBrickIndex];
        previewBrick = Instantiate(selectedPrefab);

        //Make previewBrick invisible from Hierarchy and dont be saved in file 

        previewBrick.gameObject.hideFlags = HideFlags.HideAndDontSave;

        //Turn off all collider of gameobject and its child
        Collider collider = previewBrick.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        Collider[] colliders = previewBrick.GetComponentsInChildren<Collider>();
        foreach (var col in colliders) col.enabled = false;


    }

    public void DrawHighLightCell(float x, float z, Color color)
    {
        Handles.color = color;
        Vector3 p1 = new Vector3(x, 0.2f, z);
        Vector3 p2 = new Vector3(x - 1, 0.2f, z);
        Vector3 p3 = new Vector3(x - 1, 0.2f, z - 1);
        Vector3 p4 = new Vector3(x, 0.2f, z - 1);
        Handles.DrawLines(new Vector3[] { p1, p2, p2, p3, p3, p4, p4, p1 });
    }

    public void LoadDataAllMap()
    {
        

    }

    public void SaveMap()
    {
        LevelData levelData = new LevelData();

        levelData.name = currentMapName;

        levelData.levelId = currentLevelId;

        levelData.mapSize = gridSize;


    }


    private void ClearMap()
    {
        foreach (var brick in placedBrickDict.Values)
        {
            if (brick != null)
            {
                Undo.DestroyObjectImmediate(brick.gameObject);
            }
        }
        placedBrickDict.Clear();
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

