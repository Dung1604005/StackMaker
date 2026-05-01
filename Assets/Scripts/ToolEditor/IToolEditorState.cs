using UnityEditor;

public interface IToolEditorState
{
    public void Enter();

    //This func to catch the event on scene
    void OnSceneGUI(SceneView sceneView);

    //This func to draw UI on inspector
    void OnGUI();

    public string GetTabName();

    public void Exit();
}