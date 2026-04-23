using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridMapEditorWindow : EditorWindow
{
    // --- CẤU HÌNH ---
    private float gridSize = 1f;
    private int selectedBlockIndex = 0;

    private Dictionary<Vector3Int, GameObject> mapBlocks;
    
    // Giả lập danh sách các block. Sau này ta sẽ thay bằng danh sách Prefab thật.
    private string[] blockNames = { "Đất (Dirt)", "Đá (Stone)", "Gỗ (Wood)" };

    //Create menu item on Unity
    [MenuItem("Tools/Map Designer")]
    public static void ShowWindow()
    {
        GetWindow<GridMapEditorWindow>("Map Designer");
    }


    //UI on inspector

    private void OnGUI()
    {


        GUILayout.Label("EDIT SIZE OF A ZONE (Square Size)");

        gridSize = EditorGUILayout.Slider("Size:", gridSize, 7f, 30f);
        
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
        // Code xử lý Raycast chuột và vẽ Object mờ ở đây...

        
    }
}
