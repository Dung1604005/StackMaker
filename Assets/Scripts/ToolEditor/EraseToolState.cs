using UnityEditor;
using UnityEngine;

public class EraseToolState: IToolEditorState
{
     private GridMapEditorWindow window;

     private Vector3 offsetBrickPos = new Vector3(0f, -1f, 0f);

    public EraseToolState(GridMapEditorWindow _window)
    {
        window = _window;
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
            if (GridHelper.IsGridPositionValid(gridPos, window.GridSize.x, window.GridSize.y) )
            {
                if (window.PreviewBrick != null)
                {
                    window.DrawHighLightCell(-gridPos.x, -gridPos.y, Color.red);
                    //If user is holding mouse or click then erase brick
                    if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0)
                    {
                        
                        EraseBrick(gridPos);
                    }
                }
            }            
        }
    }

    public void EraseBrick(Vector2Int gridPos)
    {
         // Check there is any brick is choosed ?
       // Check there is any brick is choosed ?
        if (window.BrickPrefabDataBase != null && window.BrickPrefabDataBase.Count() == 0) return;
        if (window.SelectedBrickIndex < 0 || window.SelectedBrickIndex >= window.BrickPrefabDataBase.Count()) return;

        // Check if this pos is empty ?
        if(window.PlacedBrickDict.ContainsKey(gridPos) )
        {
            if(window.PlacedBrickDict[gridPos] == null)
            {
                return;
            }
            else
            {
                window.RemoveBrick(gridPos);
            }
            

        }
    }

    public void OnGUI()
    {
        
    }

    public string GetTabName()
    {
        return "Erase";
    }

    public void Exit()
    {
        
    }
}