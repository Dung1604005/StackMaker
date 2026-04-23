using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
public class AreaController : MonoBehaviour
{
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private AreaContext areaCtx;
    [SerializeField] private Vector3 originWorldPos;

    [SerializeField] private Vector2Int startGridPosition;

    [SerializeField] private Vector2Int endGridPosition;

    public Vector2Int StartGridPosition => startGridPosition;

    public Vector2Int EndGridPosition => endGridPosition;

    public AreaContext AreaCtx => areaCtx;

    public Vector3 OriginWorldPos => originWorldPos;

    [ContextMenu("Load Data")]

    public void LoadLevel(string path)
    {
        

        //Load file lên bằng Resources
        TextAsset mapFile = Resources.Load<TextAsset>(path);
        if (mapFile != null)
        {
            // Lấy text trong map file
            string jsonText = mapFile.text;

            // Đọc data
            areaCtx = JsonConvert.DeserializeObject<AreaContext>(mapFile.text);
            startGridPosition = new Vector2Int(areaCtx.StartPosition[0], areaCtx.StartPosition[1]);
            endGridPosition = new Vector2Int(areaCtx.EndPosition[0], areaCtx.EndPosition[1]);

        }
        else
        {
            Debug.LogError("Không tìm thấy file tại đường dẫn: Resources/" + path);
        }

    }

    [ContextMenu("GENERATE MAP")]

    public void GenerateGrid()
    {
  
        for (int x = 0; x < areaCtx.Height; x++)
        {
            for (int z = 0; z < areaCtx.Width; z++)
            {
                // Handle spawn brick and rotate if it is corner
                BlockState blockState = areaCtx.Grid[x][z];
                Vector3 worldPos = ConvertGridToWorldPosition(x, z, originWorldPos);
                int indexPrefab = 0;
                Vector3 rotateEuler = Vector3.zero;

                if (blockState == BlockState.Blocked)
                {
                    indexPrefab = GameConfig.ID_PREFAB_BLOCK;
                    
                }
                else if (blockState == BlockState.LeftTopCorner || blockState == BlockState.RightTopCorner ||
                blockState == BlockState.LeftBottomCorner ||blockState == BlockState.RightBottomCorner )
                {
                    indexPrefab = GameConfig.ID_PREFAB_CORNER_BRICK;
                    if(blockState == BlockState.LeftTopCorner)
                    {
                        rotateEuler = new Vector3(0, 270f, 0);    
                    }
                    else if(blockState == BlockState.LeftBottomCorner)
                    {
                        rotateEuler = new Vector3(0, 180f, 0);
                    }
                    else if(blockState == BlockState.RightBottomCorner)
                    {
                        rotateEuler = new Vector3(0, 90f, 0);
                    }           
                }
                else 
                {
                    indexPrefab = GameConfig.ID_PREFAB_BRICK;
                }
                
                BrickBase brickBase = gridSystem.GetBrickPrefab(indexPrefab);

                BrickBase ob = PoolManager.Instance.Spawn<BrickBase>(brickBase, worldPos, Quaternion.identity);
                ob.transform.SetParent(this.transform);
                ob.SetInfo(blockState,rotateEuler);
                

                
            }
        }
    }

    public Vector3 ConvertGridToWorldPosition(int x, int z, Vector3 originPos)
    {
        
        Vector2 cellSize = new Vector2(1f, 1f);
        Vector3 worldPos = new Vector3(originPos.x - x * cellSize.x - cellSize.x / 2f, 0f, originPos.z - z * cellSize.y - cellSize.y / 2f);

        return worldPos;
    }

    public void CalculateOriginWorldPosition(Vector2Int gridPosition, Vector3 worldPosition)
    {
        
        Vector3 worldPos = new Vector3(worldPosition.x+ gridPosition.x*GameConfig.CellSize.x +GameConfig.CellSize.x/2f, 0f,
        worldPosition.z + gridPosition.y*GameConfig.CellSize.y + GameConfig.CellSize.y/2f);
       
        originWorldPos = worldPos;
    }

    public bool IsGridPositionValid(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.y < 0 || gridPosition.x >= areaCtx.Height || gridPosition.y >= areaCtx.Width)
        {
            return false;
        }
        return true;
    }

    




}
