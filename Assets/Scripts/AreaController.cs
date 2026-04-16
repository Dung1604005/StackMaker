using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json;
public class AreaController : MonoBehaviour
{
    [SerializeField] private AreaContext areaCtx;

    [SerializeField] private GameObject blockPrefab;

    [SerializeField] private GameObject emptyPrefab;

    [SerializeField] private GameObject cornerPrefab;

    [SerializeField] private Vector3 originWorldPos;
    void Start()
    {
        
    }

    [ContextMenu("Load Data")]

    public void LoadLevel()
    {
        string path = "Level_01"; 

        //Load file lên bằng Resources
        TextAsset mapFile = Resources.Load<TextAsset>(path);
        if (mapFile != null)
        {
            // Lấy text trong map file
            string jsonText = mapFile.text; 

            // Đọc data
            areaCtx = JsonConvert.DeserializeObject<AreaContext>(mapFile.text);
            
        }
        else
        {
            Debug.LogError("Không tìm thấy file tại đường dẫn: Resources/" + path);
        }
        
    }

    [ContextMenu("GENERATE MAP")]

    public void GenerateGrid()
    {
        Vector3 originPos = originWorldPos;

        Vector2 cellSize=  new Vector2(1f, 1f);
        areaCtx.TotalStack = areaCtx.Height*areaCtx.Width;

        for(int x = 0; x < areaCtx.Height; x++)
        {
            for(int z = 0; z < areaCtx.Width; z++)
            {
                BlockState blockState = areaCtx.Grid[x][z];
                Vector3 worldPos = new Vector3(originPos.x - x * cellSize.x - cellSize.x / 2f, 0f, originPos.z - z * cellSize.y - cellSize.y / 2f);
                if(blockState == BlockState.Blocked)
                {
                    GameObject a = Instantiate(blockPrefab, worldPos, Quaternion.identity, transform);
                    a.name = $"Block[{x}][{z}]";
                    areaCtx.TotalStack -= 1;
                }
                else if(blockState  == BlockState.LeftTopCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);
                     a.transform.Rotate(new Vector3(0, 270f, 0));
                    a.name = $"Block[{x}][{z}]";
                }
                else if(blockState == BlockState.RightTopCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);
                    a.name = $"Block[{x}][{z}]";
                }
                else if(blockState == BlockState.LeftBottomCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);
                     a.transform.Rotate(new Vector3(0, 180f, 0));
                    a.name = $"Block[{x}][{z}]";
                }
                else if(blockState == BlockState.RightBottomCorner)
                {
                    GameObject a = Instantiate(cornerPrefab, worldPos, Quaternion.identity, transform);
                    a.transform.Rotate(new Vector3(0, 90f, 0));
                    a.name = $"Block[{x}][{z}]";
                }
                else
                {
                    GameObject a = Instantiate(emptyPrefab, worldPos, Quaternion.identity, transform);
                    a.name = $"Block[{x}][{z}]";
                }
            }
        }
    }

    

    
}
