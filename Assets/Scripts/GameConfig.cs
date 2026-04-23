
using UnityEngine;

public class GameConfig
{
    public static LayerMask LAYER_DEFAULT = LayerMask.GetMask("Default");

    public static LayerMask LAYER_WALL = LayerMask.GetMask("Wall");

    public static LayerMask LAYER_BRIDGE = LayerMask.GetMask("Bridge");

    public static LayerMask LAYER_BRICK = LayerMask.GetMask("Brick");

    public const string PLAYER_TAG = "Player";

    public const string STACK_TAG = "stack";

    public const float MAX_DISTANCE_RAYCAST = 50f;

    public const int ID_PREFAB_BLOCK = 0;
    public const int ID_PREFAB_BRICK = 1;

    public const int ID_PREFAB_CORNER_BRICK = 2;

    public const int ID_PREFAB_BRIDGE = 3;

    public static Vector3 CellSize = new Vector3(1f, 1f,1f);



}
