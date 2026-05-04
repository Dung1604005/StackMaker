using UnityEditor;
using UnityEngine;

public class PaintToolState : IToolEditorState
{

    private GridMapEditorWindow window;

    private Vector3 offsetBrickPos = new Vector3(0f, -1f, 0f);



    public PaintToolState(GridMapEditorWindow _gridMapEditorWindow)
    {
        window = _gridMapEditorWindow;
    }
    public void Enter()
    {

    }

    public void OnSceneGUI(SceneView sceneView)
    {
        //Make select default of unity disable
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;

        // Create a plane with infinity size but have y =0 (same with grid)
        // We check the intersection of plane and ray, if true => return distance

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {

            Vector3 hitPoint = ray.GetPoint(enter);

            Vector2Int gridPos = GridHelper.ConvertWorldPositionToGridPosition(hitPoint, GameConfig.OriginPos);

            //If this pos is valid then create preview brick and highlight
            if (GridHelper.IsGridPositionValid(gridPos, window.GridSize.x, window.GridSize.y))
            {
                if (window.PreviewBrick != null)
                {
                    window.DrawHighLightCell(-gridPos.x, -gridPos.y, Color.green);
                    window.PreviewBrick.gameObject.SetActive(true);

                    window.PreviewBrick.gameObject.transform.position = GridHelper.ConvertGridToWorldPosition(gridPos.x, gridPos.y, GameConfig.OriginPos) + offsetBrickPos;
                    //If user is holding mouse or click then place brick
                    if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0)
                    {

                        PlaceBrick(gridPos);
                    }

                    if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
                    {
                        
                        window.PreviewBrick.RotateBrick(new Vector3(0, 90f, 0));
                        
                    }
                }
            }
            else
            {
                if (window.PreviewBrick != null && window.PreviewBrick.gameObject.activeSelf)
                {
                    window.PreviewBrick.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (window.PreviewBrick != null && window.PreviewBrick.gameObject.activeSelf)
            {
                window.PreviewBrick.gameObject.SetActive(false);
            }
        }



    }
    //Handle the logic place a brick
    private void PlaceBrick(Vector2Int gridPos)
    {
        // Check there is any brick is choosed ?
        if (window.AvailableBrick != null && window.AvailableBrick.Length == 0) return;
        if (window.SelectedBrickIndex < 0 || window.SelectedBrickIndex >= window.AvailableBrick.Length) return;


        BrickBase selectedBrick = window.AvailableBrick[window.SelectedBrickIndex];

        // Check if this pos is empty ?
        if (window.PlacedBrickDict.ContainsKey(gridPos))
        {
            if (window.PlacedBrickDict[gridPos] == null ||window.PlacedBrickDict[gridPos].gameObject == null )
            {
                window.RemoveBrick(gridPos);
            }
            else
            {
                // If this position have brick but different type then remove it to let the new brick place in
                if (selectedBrick.GetBrickState() != window.PlacedBrickDict[gridPos].GetBrickState())
                {
                    window.RemoveBrick(gridPos);
                }
                else
                {
                    // If this position have same type of brick then do nothing
                    return;
                }
            }


        }


        // Create a root container all brick
        if (window.Root == null)
        {
            GameObject gridRoot = GameObject.Find("Root");

            if (gridRoot == null)
            {
                gridRoot = new GameObject("Root");
            }
            window.SetRoot(gridRoot.transform);
        }

        //Create brick

        BrickBase brickObject = (BrickBase)PrefabUtility.InstantiatePrefab(selectedBrick);

        brickObject.transform.position = GridHelper.ConvertGridToWorldPosition(gridPos.x, gridPos.y, GameConfig.OriginPos) + offsetBrickPos;

        brickObject.RotateBrick(window.PreviewBrick.GetEulerRotation());

        brickObject.transform.SetParent(window.Root, false);

        //Use undo to ctrl z to remove change

        Undo.RegisterCreatedObjectUndo(brickObject.gameObject, "Brick Paint");

        window.AddBrick(gridPos, brickObject);
    }

    public void OnGUI()
    {
        GUILayout.Label("CHỌN BRICK");


        if (window.BrickNames != null && window.BrickNames.Length > 0)
        {
            //Check if user choose another brick ?
            // If true then update preview brick

            EditorGUI.BeginChangeCheck();

            int selectedBrickIndex = EditorGUILayout.Popup("Choosing block: ", window.SelectedBrickIndex, window.BrickNames);

            if (EditorGUI.EndChangeCheck())
            {
                window.SetSelectedBrick(selectedBrickIndex);
                window.UpdatePreviewObject();
            }
        }


        EditorGUILayout.Space(10);

        if (GUILayout.Button("Làm mới danh sách Gạch"))
        {
            window.LoadBricksFromAsset();
        }
    }

    public string GetTabName()
    {
        return "Paint";
    }

    public void Exit()
    {

    }
}