using UnityEngine;

public class GameConfig
{
    public static LayerMask LAYER_DEFAULT = LayerMask.GetMask("Default");

    public static LayerMask LAYER_WALL = LayerMask.GetMask("Wall");

    public static LayerMask LAYER_BRIDGE = LayerMask.GetMask("Bridge");

    public static LayerMask LAYER_BRICK = LayerMask.GetMask("Brick");

    public static string PLAYER_TAG = "Player";

    public static float MAX_DISTANCE_RAYCAST = 50f;



}
