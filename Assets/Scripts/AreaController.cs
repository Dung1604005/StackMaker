using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
public class AreaController : MonoBehaviour
{
    [SerializeField] private AreaContext areaCtx;

    [SerializeField] private GameObject blockPrefab;

    [SerializeField] private GameObject emptyPrefab;

    [SerializeField] private GameObject cornerPrefab;

    [SerializeField] private Vector3 originWorldPos;

    [SerializeField] private Vector2Int startGridPosition;

    [SerializeField] private Vector2Int endGridPosition;

    private Dictionary<Vector2Int, GameObject> blockDictionary = new Dictionary<Vector2Int, GameObject>();

    public Vector2Int StartGridPosition => startGridPosition;

    public Vector2Int EndGridPosition => endGridPosition;

    public AreaContext AreaCtx => areaCtx;

    public void OnEnable()
    {
        EventBus<OnChangeDirect>.Subcribe(SlideInDirection);
    }
    public void OnDisable()
    {
        EventBus<OnChangeDirect>.UnSubcribe(SlideInDirection);
    }

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
        blockDictionary.Clear();
        

        for (int x = 0; x < areaCtx.Height; x++)
        {
            for (int z = 0; z < areaCtx.Width; z++)
            {
                BlockState blockState = areaCtx.Grid[x][z];
                Vector3 worldPos = ConvertGridToWorldPosition(x, z);

                if (blockState == BlockState.Blocked)
                {
                    GameObject a = Instantiate(blockPrefab, worldPos, Quaternion.identity, transform);
                    
                }
                else if (blockState == BlockState.LeftTopCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);
                    a.transform.Rotate(new Vector3(0, 270f, 0));
                    blockDictionary.Add(new Vector2Int(x, z), a);
                }
                else if (blockState == BlockState.RightTopCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);             
                    blockDictionary.Add(new Vector2Int(x, z), a);
                }
                else if (blockState == BlockState.LeftBottomCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);
                    a.transform.Rotate(new Vector3(0, 180f, 0));
                    blockDictionary.Add(new Vector2Int(x, z), a);
                }
                else if (blockState == BlockState.RightBottomCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);
                    a.transform.Rotate(new Vector3(0, 90f, 0));
                    blockDictionary.Add(new Vector2Int(x, z), a);
                }
                else 
                {
                    GameObject a = Instantiate(emptyPrefab, worldPos, Quaternion.identity, transform);
                    blockDictionary.Add(new Vector2Int(x, z), a);
                }
                
            }
        }
    }

    public Vector3 ConvertGridToWorldPosition(int x, int z)
    {
        Vector3 originPos = originWorldPos;
        Vector2 cellSize = new Vector2(1f, 1f);
        Vector3 worldPos = new Vector3(originPos.x - x * cellSize.x - cellSize.x / 2f, 0f, originPos.z - z * cellSize.y - cellSize.y / 2f);

        return worldPos;
    }

    public void CalculateOriginWorldPosition(Vector2Int gridPosition, Vector3 worldPosition)
    {
        Vector2 cellSize = new Vector2(1f, 1f);
        Vector3 worldPos = new Vector3(worldPosition.x+ gridPosition.x*cellSize.x +cellSize.x/2f, 0f,
        worldPosition.z + gridPosition.y*cellSize.y + cellSize.y/2f);
       
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
    public Vector2Int GetNextPositionInGrid(Vector2Int gridPosition, Direct direct)
    {
        Vector2Int nextPos = Vector2Int.zero;
        switch (direct)
        {
            case Direct.Back:
                nextPos = new Vector2Int(gridPosition.x + 1, gridPosition.y);
                break;
            case Direct.Forward:
                nextPos = new Vector2Int(gridPosition.x - 1, gridPosition.y);
                break;
            case Direct.Left:
                nextPos = new Vector2Int(gridPosition.x, gridPosition.y - 1);
                break;
            default:
                nextPos = new Vector2Int(gridPosition.x, gridPosition.y + 1);
                break;

        }
        return nextPos;
    }
    public void SlideInDirection(OnChangeDirect onChangeDirect)
    {
        if(onChangeDirect.areaId != areaCtx.areaId)
        {
            return;
        }
        Vector2Int start = onChangeDirect.playerGridPosition;
        Direct direct = onChangeDirect.direct;
        Vector2Int current = start;


        // Lặp đến khi thấy điểm kết thúc theo hướng direct cố định
        for (int i = 0; i < 100; i++)
        {

            Vector2Int nextPos = GetNextPositionInGrid(current, direct);


            if (!IsGridPositionValid(nextPos))
            {
                break;
            }
            if (areaCtx.Grid[nextPos.x][nextPos.y] == BlockState.Blocked)
            {
                break;
            }
            else
            {
                current = nextPos;
            }
        }
        Debug.Log(current);
        EventBus<OnChangeTargetPositionPlayer>.Raise(new OnChangeTargetPositionPlayer
        {
            GridPosition = current,
            TargetPosition = ConvertGridToWorldPosition(current.x, current.y),
            TargetBlockState = areaCtx.Grid[current.x][current.y]
        });
    }

    void Start()
    {
        // LoadLevel("Level_01_01");
        // GenerateGrid();
        
    }




}
