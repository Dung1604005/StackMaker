using UnityEditor;
using UnityEngine;

public class GridMapEditorWindow : EditorWindow
{
    //Create menu item on Unity

    public static void ShowWindow()
    {
        GetWindow<GridMapEditorWindow>("Map Designer");
    }
}
