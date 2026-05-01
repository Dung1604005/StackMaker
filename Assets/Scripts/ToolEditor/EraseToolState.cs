using UnityEditor;
using UnityEngine;

public class EraseToolState: IToolEditorState
{
     private GridMapEditorWindow window;

    public EraseToolState(GridMapEditorWindow _window)
    {
        window = _window;
    }
    public void Enter()
    {
        
    }

    public void OnSceneGUI(SceneView sceneView)
    {
        


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