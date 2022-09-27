using System.Collections.Generic;
using UnityEngine;

public class UpdateResizeWorld : MonoBehaviour
{
    public List<GameObject> WorldWalls;
    List<Vector3> WorldWallsPosition;
    public List<GameObject> SpawnPoints;
    List<Vector3> SpawnPointsPosition;
    public GameObject DropBar;
    float screenW;
    float screenH;
    int preWay;
    float preDecreasingRate;
    float preTBNumber;
    int currentWay;    
    float decreasingRate;
    float TBNumber;
    
    void OnEnable()
    {
        screenW = Screen.width;
        screenH = Screen.height;
        WorldWallsPosition = new List<Vector3>();
        SpawnPointsPosition = new List<Vector3>();
        preWay = 0;
        currentWay = 0;
        preDecreasingRate = 1f;
        decreasingRate = 1f;
        preTBNumber = 0f;
        TBNumber = 0f;
        SaveWorldWallsAndSpawnPointsPosition();
        MoveWallsAndSpawnPoints();       
    }

    void Update()
    {
        if (screenW != Screen.width || screenH != Screen.height)
        {
            screenW = Screen.width;
            screenH = Screen.height;
            MoveWallsAndSpawnPoints();
            if (preWay != 0 || currentWay != 0)
            {
                MoveDropBarGameObjects();
            }          
            Debug.Log("화면 크기 변화로 월드 게임오브젝트 위치 변화");
        }
    }

    void SaveWorldWallsAndSpawnPointsPosition()
    {
        for (int i = 0; i < WorldWalls.Count; i++)
        {
            Vector3 postionTmp = WorldWalls[i].transform.position;
            WorldWallsPosition.Add(postionTmp);
        }

        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            Vector3 postionTmp = SpawnPoints[i].transform.position;
            SpawnPointsPosition.Add(postionTmp);
        }
    }

    void MoveWallsAndSpawnPoints()
    {
        float setR = (float) 1/2;
        float defaultR = (float) 600/1024;
        float screenR = (float) screenW/screenH;              
        float cameraDepth = Camera.main.orthographicSize;

        preWay = currentWay;
        currentWay = 0;

        if (decreasingRate <= 1f)
        {
            preDecreasingRate = decreasingRate;
        }
        else
        {
            preDecreasingRate = 1f;
        }
        decreasingRate = (float) 1;  

        preTBNumber = TBNumber;
        TBNumber = 0;
        float LRNumber = 0;

        if (defaultR >= screenR && setR < screenR)
        {
            WorldWalls[0].transform.position = new Vector3(WorldWallsPosition[0].x, WorldWallsPosition[0].y, WorldWallsPosition[0].z);
            WorldWalls[1].transform.position = new Vector3(WorldWallsPosition[1].x, WorldWallsPosition[1].y, WorldWallsPosition[1].z);

            LRNumber = (defaultR - screenR)*cameraDepth;
            Debug.Log("LRNumber: " + LRNumber);
            WorldWalls[2].transform.position = new Vector3(WorldWallsPosition[2].x + LRNumber, WorldWallsPosition[2].y, WorldWallsPosition[2].z);
            WorldWalls[3].transform.position = new Vector3(WorldWallsPosition[3].x - LRNumber, WorldWallsPosition[3].y, WorldWallsPosition[3].z);

            currentWay = 1;
        }
        else if (setR >= screenR)
        {
            TBNumber = cameraDepth*(1-2*screenR);
            Debug.Log("TBNumber: " + TBNumber);
            WorldWalls[0].transform.position = new Vector3(WorldWallsPosition[0].x, WorldWallsPosition[0].y - TBNumber, WorldWallsPosition[0].z);
            WorldWalls[1].transform.position = new Vector3(WorldWallsPosition[1].x, WorldWallsPosition[1].y + TBNumber, WorldWallsPosition[1].z);

            LRNumber = (defaultR - screenR)*cameraDepth;
            Debug.Log("LRNumber: " + LRNumber);
            WorldWalls[2].transform.position = new Vector3(WorldWallsPosition[2].x + LRNumber, WorldWallsPosition[2].y, WorldWallsPosition[2].z);
            WorldWalls[3].transform.position = new Vector3(WorldWallsPosition[3].x - LRNumber, WorldWallsPosition[3].y, WorldWallsPosition[3].z);

            currentWay = 2;
        }
        else
        {
            WorldWalls[0].transform.position = new Vector3(WorldWallsPosition[0].x, WorldWallsPosition[0].y, WorldWallsPosition[0].z);
            WorldWalls[1].transform.position = new Vector3(WorldWallsPosition[1].x, WorldWallsPosition[1].y, WorldWallsPosition[1].z);
            WorldWalls[2].transform.position = new Vector3(WorldWallsPosition[2].x, WorldWallsPosition[2].y, WorldWallsPosition[2].z);
            WorldWalls[3].transform.position = new Vector3(WorldWallsPosition[3].x, WorldWallsPosition[3].y, WorldWallsPosition[3].z);

            currentWay = 0;
        }

        decreasingRate = screenR/defaultR;        
        Debug.Log("decreasingRate: " + decreasingRate);
        if (defaultR >= screenR && setR < screenR)
        {
            SpawnPoints[0].transform.position = new Vector3(SpawnPointsPosition[0].x*decreasingRate, SpawnPointsPosition[0].y, SpawnPointsPosition[0].z);
            SpawnPoints[1].transform.position = new Vector3(SpawnPointsPosition[1].x*decreasingRate, SpawnPointsPosition[1].y, SpawnPointsPosition[1].z);
            SpawnPoints[2].transform.position = new Vector3(SpawnPointsPosition[2].x*decreasingRate, SpawnPointsPosition[2].y, SpawnPointsPosition[2].z);
            SpawnPoints[3].transform.position = new Vector3(SpawnPointsPosition[3].x*decreasingRate, SpawnPointsPosition[3].y, SpawnPointsPosition[3].z);
            SpawnPoints[4].transform.position = new Vector3(SpawnPointsPosition[4].x*decreasingRate, SpawnPointsPosition[4].y, SpawnPointsPosition[4].z);
            SpawnPoints[5].transform.position = new Vector3(SpawnPointsPosition[5].x*decreasingRate, SpawnPointsPosition[5].y, SpawnPointsPosition[5].z);
        }
        else if (setR >= screenR)
        {
            SpawnPoints[0].transform.position = new Vector3(SpawnPointsPosition[0].x*decreasingRate, SpawnPointsPosition[0].y - TBNumber, SpawnPointsPosition[0].z);
            SpawnPoints[1].transform.position = new Vector3(SpawnPointsPosition[1].x*decreasingRate, SpawnPointsPosition[1].y - TBNumber, SpawnPointsPosition[1].z);
            SpawnPoints[2].transform.position = new Vector3(SpawnPointsPosition[2].x*decreasingRate, SpawnPointsPosition[2].y - TBNumber, SpawnPointsPosition[2].z);
            SpawnPoints[3].transform.position = new Vector3(SpawnPointsPosition[3].x*decreasingRate, SpawnPointsPosition[3].y - TBNumber, SpawnPointsPosition[3].z);
            SpawnPoints[4].transform.position = new Vector3(SpawnPointsPosition[4].x*decreasingRate, SpawnPointsPosition[4].y - TBNumber, SpawnPointsPosition[4].z);
            SpawnPoints[5].transform.position = new Vector3(SpawnPointsPosition[5].x*decreasingRate, SpawnPointsPosition[5].y - TBNumber, SpawnPointsPosition[5].z);
        }
        else
        {
            SpawnPoints[0].transform.position = new Vector3(SpawnPointsPosition[0].x, SpawnPointsPosition[0].y, SpawnPointsPosition[0].z);
            SpawnPoints[1].transform.position = new Vector3(SpawnPointsPosition[1].x, SpawnPointsPosition[1].y, SpawnPointsPosition[1].z);
            SpawnPoints[2].transform.position = new Vector3(SpawnPointsPosition[2].x, SpawnPointsPosition[2].y, SpawnPointsPosition[2].z);
            SpawnPoints[3].transform.position = new Vector3(SpawnPointsPosition[3].x, SpawnPointsPosition[3].y, SpawnPointsPosition[3].z);
            SpawnPoints[4].transform.position = new Vector3(SpawnPointsPosition[4].x, SpawnPointsPosition[4].y, SpawnPointsPosition[4].z);
            SpawnPoints[5].transform.position = new Vector3(SpawnPointsPosition[5].x, SpawnPointsPosition[5].y, SpawnPointsPosition[5].z);
        }        
    }

    void MoveDropBarGameObjects()
    {
        List<Vector3> DropBarGameObjectsOriginalPosition = new List<Vector3>();
        DropBarGameObjectsOriginalPosition = SetDropBarGameObjectsOriginalPosition();        
        List<Vector3> DropBarGameObjectsPosition = new List<Vector3>();
        for (int i = 0; i < DropBarGameObjectsOriginalPosition.Count; i++)
        {
            Vector3 positionTmp = DropBarGameObjectsOriginalPosition[i];
            if (currentWay == 1)
            {
                Vector3 position = new Vector3(positionTmp.x*decreasingRate, positionTmp.y, positionTmp.z);
                DropBarGameObjectsPosition.Add(position);
            }
            else if (currentWay == 2)
            {
                Vector3 position = new Vector3(positionTmp.x*decreasingRate, positionTmp.y - TBNumber, positionTmp.z);
                DropBarGameObjectsPosition.Add(position);
            }
            else
            {
                Vector3 position = new Vector3(positionTmp.x, positionTmp.y, positionTmp.z);
                DropBarGameObjectsPosition.Add(position);
            }
        }

        List<GameObject> DropBarGameObjects = new List<GameObject>();
        for (int i = 0; i < DropBar.transform.childCount; i++)
        {
            DropBarGameObjects.Add(DropBar.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < DropBarGameObjects.Count; i++)
        {
            DropBarGameObjects[i].transform.position = new Vector3(DropBarGameObjectsPosition[i].x, DropBarGameObjectsPosition[i].y, DropBarGameObjectsPosition[i].z);
        }

    }

    List<Vector3> SetDropBarGameObjectsOriginalPosition()
    {
        List<GameObject> DropBarGameObjects = new List<GameObject>();
        for (int i = 0; i < DropBar.transform.childCount; i++)
        {
            DropBarGameObjects.Add(DropBar.transform.GetChild(i).gameObject);
        }
        List<Vector3> DropBarGameObjectsPosition = new List<Vector3>();
        for (int i = 0; i < DropBarGameObjects.Count; i++)
        {
            Vector3 positionTmp = DropBarGameObjects[i].transform.position;
            if (preWay == 1)
            {
                Vector3 position = new Vector3(positionTmp.x/preDecreasingRate, positionTmp.y, positionTmp.z);
                DropBarGameObjectsPosition.Add(position);
            }
            else if (preWay == 2)
            {
                Vector3 position = new Vector3(positionTmp.x/preDecreasingRate, positionTmp.y + preTBNumber, positionTmp.z);
                DropBarGameObjectsPosition.Add(position);
            }
            else
            {
                Vector3 position = new Vector3(positionTmp.x, positionTmp.y, positionTmp.z);
                DropBarGameObjectsPosition.Add(position);
            }
        }

        return DropBarGameObjectsPosition;
    }
}