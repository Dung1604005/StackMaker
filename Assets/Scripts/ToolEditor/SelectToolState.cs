using UnityEditor;
using UnityEngine;

public class SelectToolState : IToolEditorState
{
    private GridMapEditorWindow window;

    public SelectToolState(GridMapEditorWindow _window)
    {
        window = _window;
    }
    public void Enter()
    {

    }

    public void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        GameObject selectedObj = Selection.activeGameObject;
        if (selectedObj == null)
        {
            return;
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
        {
            if (selectedObj.TryGetComponent<BrickBase>(out BrickBase result))
            {
                result.RotateBrick(new Vector3(0, 90f, 0));
            }
        }
    }

    public void OnGUI()
    {
        GameObject selectedObj = Selection.activeGameObject;
        if (selectedObj == null)
        {
            return;
        }
        if (selectedObj.TryGetComponent<BrickBase>(out BrickBase result))
        {
            GUILayout.Label("Brick Type: " + result.GetBrickState().ToString());
        }

    }
    public string GetTabName()
    {
        return "Select";
    }

    public void Exit()
    {

    }
}