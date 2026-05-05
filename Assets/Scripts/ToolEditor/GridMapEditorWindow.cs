using System.Collections.Generic;
using System.IO;
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

    private int currentMapMode = 0;

    private List<string> allLevelName = new List<string>();

    private LevelDataBaseSO levelDataBaseSO;

    private Dictionary<Vector2Int, BrickBase> placedBrickDict = new Dictionary<Vector2Int, BrickBase>();

    private string currentMapName;

    private int currentLevelId;

    private Vector2Int startPosition;

    // Data for brick

    private string brickFolderPath = "Assets/Prefabs/MapBricks";
    private BrickPrefabDataBase brickPrefabDataBase;

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

    public BrickPrefabDataBase BrickPrefabDataBase => brickPrefabDataBase;

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

            if (oldBrick != null && oldBrick.gameObject != null)
            {
                DestroyImmediate(oldBrick.gameObject);
            }


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

    #region SAVE AND LOAD


    public void SaveMap()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Level", "Level_01", "asset", "Choose save location");


        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        //Check if this path have a SO ?

        LevelDataSO levelDataSO = AssetDatabase.LoadAssetAtPath<LevelDataSO>(path);

        if (levelDataSO == null)
        {
            // Create a SO
            levelDataSO = ScriptableObject.CreateInstance<LevelDataSO>();

            AssetDatabase.CreateAsset(levelDataSO, path);
        }

        if (currentMapMode == 0)
        {
            LoadNewLevel();
        }

        levelDataSO.nameLevel = currentMapName;

        levelDataSO.levelId = currentLevelId;

        levelDataSO.mapSize = gridSize;

        levelDataSO.brickSaveDatas.Clear();

        List<BrickSaveData> brickSaveDatas = new List<BrickSaveData>();

        int countStartPosition = 0;
        foreach (var item in placedBrickDict)
        {
            if (item.Value != null)
            {
                if (item.Value.GetBrickState() == BrickState.StartBlock)
                {
                    levelDataSO.startPosition = item.Key;
                }
                brickSaveDatas.Add(new BrickSaveData
                {
                    x = item.Key.x,
                    y = item.Key.y,
                    IdBrick = item.Value.GetBrickId(),
                    eulerRotate = item.Value.GetEulerRotation()
                });

                if (item.Value.GetBrickState() == BrickState.StartBlock)
                {
                    countStartPosition += 1;
                }
            }
        }
        levelDataSO.brickSaveDatas = brickSaveDatas;
        if (countStartPosition == 1)
        {
            

            //Call unity to save the change of old SO or save new SO
            EditorUtility.SetDirty(levelDataSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Auto add level to database
            if (levelDataBaseSO != null && currentMapMode == 0)
            {
                levelDataBaseSO.AddLevelData(levelDataSO);
            }

            EditorUtility.DisplayDialog("Notification", "Your Level was saved successful", "Ok", "Nuh uh");

        }
        else if (countStartPosition > 1)
        {
            EditorUtility.DisplayDialog("Warning", "Your map have more than 1 start position", "Ok", "No");
            return;
        }
        else
        {
            EditorUtility.DisplayDialog("Warning", "Your map have no start position", "Ok", "No");
            return;
        }




    }

    public void LoadCurrentLevel(LevelDataSO levelDataSO)
    {
        ClearMap();
        currentLevelId = levelDataSO.levelId;
        currentMapName = levelDataSO.nameLevel;

        gridSize = levelDataSO.mapSize;

        placedBrickDict.Clear();
        placedBrickDict = new Dictionary<Vector2Int, BrickBase>();

        foreach (BrickSaveData brickSaveData in levelDataSO.brickSaveDatas)
        {
            BrickBase brick = brickPrefabDataBase.GetBrickPrefab(brickSaveData.IdBrick);

            BrickBase brickObject = (BrickBase)PrefabUtility.InstantiatePrefab(brick);
            brickObject.SetEulerRotation(brickSaveData.eulerRotate);
            placedBrickDict.Add(new Vector2Int(brickSaveData.x, brickSaveData.y), brickObject);
        }

        LoadMap();

    }

    public void LoadNewLevel()
    {
        currentLevelId = levelDataBaseSO.GetCountLevel();

        currentMapName = "Level-" + (currentLevelId + 1);


    }
    public void LoadDatabase()
    {
        string[] guids = AssetDatabase.FindAssets("t:LevelDataBaseSO");

        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);

            levelDataBaseSO = AssetDatabase.LoadAssetAtPath<LevelDataBaseSO>(path);

            if (levelDataBaseSO != null)
            {
                allLevelName = levelDataBaseSO.GetAllNameLevel();
            }
        }


    }
    public void LoadBricksFromAsset()


    {
        string[] guids = AssetDatabase.FindAssets("t:BrickPrefabDataBase");

        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);

            brickPrefabDataBase = AssetDatabase.LoadAssetAtPath<BrickPrefabDataBase>(path);

            if (brickPrefabDataBase != null)
            {
                brickNames = brickPrefabDataBase.GetAllNamePrefab().ToArray();
            }
        }
    }

    public void LoadMap()
    {
        
        foreach (var brickData in placedBrickDict)
        {
            Vector2Int gridPos = brickData.Key;

            if(brickData.Value == null)
            {
                continue;
            }
            Vector3 worldPos = GridHelper.ConvertGridToWorldPosition(gridPos.x, gridPos.y, GameConfig.OriginPos);
            int indexPrefab = brickData.Value.GetBrickId();
            Vector3 rotateEuler =brickData.Value.GetEulerRotation();

            brickData.Value.transform.position = worldPos;
            brickData.Value.SetEulerRotation(rotateEuler);
            brickData.Value.transform.SetParent(root.transform);
        }
    }

    #endregion
    #region GUI
    private void OnGUISavedMapMode()
    {
        GUILayout.Label("EDIT OLD MAP");

        EditorGUI.BeginChangeCheck();

        currentLevelId = EditorGUILayout.Popup("Choosing Level: ", currentLevelId, allLevelName.ToArray());

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Làm mới danh sách Level"))
        {
            LoadDatabase();
        }

        EditorGUILayout.Space(15);
        if (EditorGUI.EndChangeCheck())
        {

            LoadCurrentLevel(levelDataBaseSO.GetLevel(currentLevelId));

            EditorGUILayout.Space(10);
        }
        gridSize.x = EditorGUILayout.IntField("Size X:", gridSize.x);
        gridSize.y = EditorGUILayout.IntField("Size Y:", gridSize.y);

        isMapCreated = true;
        SceneView.RepaintAll();

    }

    private void OnGUINewMapMode()
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
        GUILayout.Space(15);

    }
    //UI on inspector

    private void OnGUI()
    {

        // Draw tool bar for choosing tool editor mode
        EditorGUI.BeginChangeCheck();
        currentMapMode = GUILayout.Toolbar(currentMapMode, new string[] { "New Map", "Saved Map" }, GUILayout.Height(30));

        // if user click another tab then change mode
        if (EditorGUI.EndChangeCheck())
        {
            // disable all focus
            GUI.FocusControl(null);
        }
        if (currentMapMode == 0)
        {
            OnGUINewMapMode();
        }
        else
        {
            OnGUISavedMapMode();
        }



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


        EditorGUILayout.Space(15);
        if (GUILayout.Button("Clear Map"))
        {
            if (EditorUtility.DisplayDialog("Warning", "Are you sure want to CLEAR map ?", "Ok", "Nah"))
            {
                ClearMap();
            }

        }

        EditorGUILayout.Space(15);
        if (GUILayout.Button("Save"))
        {
            if (EditorUtility.DisplayDialog("Confirm", "Are you sure want to save this map", "Ok", "No"))
            {
                SaveMap();
            }
        }

    }
    // Handle the preview visual for brick before place
    public void UpdatePreviewObject()
    {
        if (previewBrick != null)
        {
            DestroyImmediate(previewBrick.gameObject);
        }

        if (brickPrefabDataBase != null && brickPrefabDataBase.Count() == 0) return;

        // create prefab
        BrickBase selectedPrefab = brickPrefabDataBase.GetBrickPrefab(selectedBrickIndex);
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

    #endregion

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

    public void CreateRoot()
    {
        if (root == null)
        {
            GameObject gridRoot = GameObject.Find("Root");

            if (gridRoot == null)
            {
                gridRoot = new GameObject("Root");
            }
            SetRoot(gridRoot.transform);
        }
    }
    #region Init
    // Subcribe event to draw on Scene view
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        LoadBricksFromAsset();

        LoadDatabase();

        CreateRoot();

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

    }
    #endregion
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


    private void ClearMap()
    {
        foreach (var brick in placedBrickDict.Values)
        {
            if (brick != null)
            {
                DestroyImmediate(brick.gameObject);
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

