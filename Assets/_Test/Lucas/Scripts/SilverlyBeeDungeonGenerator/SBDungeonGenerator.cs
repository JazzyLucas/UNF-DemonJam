using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.AI;
using UnityEngine;
using Random = UnityEngine.Random;

public class SBDungeonGenerator : MonoBehaviour
{
    [SerializeField] private DungeonGenerationConfigurationSO DungeonGenerationConfigurationSo;

    void Start()
    {
        DoMazeGenerator();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DoMazeGenerator();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            InitializeNavMeshes();
        }
    }

    private void LateUpdate()
    {
        TryInitializationOfDungeon();
    }

    private void GenerateDungeon()
    {
        uint tempRoomCount = 0;
        for (int i = 0; i < DungeonGenerationConfigurationSo.size.x; i++)
        {
            for (int j = 0; j < DungeonGenerationConfigurationSo.size.y; j++)
            {
                Cell currentCell = DungeonGenerationConfigurationSo.board[(i + j * DungeonGenerationConfigurationSo.size.x)];
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < DungeonGenerationConfigurationSo.rooms.Length; k++)
                    {
                        int p = DungeonGenerationConfigurationSo.rooms[k].ProbabilityOfSpawning(i, j);

                        if(p == 2)
                        {
                            randomRoom = k;
                            break;
                        } else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    if(randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }


                    var newRoom = Instantiate(DungeonGenerationConfigurationSo.rooms[randomRoom].room, new Vector3(i * DungeonGenerationConfigurationSo.offset.x, 0, -j * DungeonGenerationConfigurationSo.offset.y), Quaternion.identity, transform).GetComponent<SBRoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + i + "-" + j;
                    tempRoomCount++;
                    DungeonGenerationConfigurationSo.objectPool.Add(newRoom.gameObject);
                }
            }
        }

        DungeonGenerationConfigurationSo.roomCount = tempRoomCount;
    }

    private void DoMazeGenerator()
    {
        // Initializations
        DungeonGenerationConfigurationSo.roomsReady = 0;
        if (DungeonGenerationConfigurationSo.objectPool != null)
        {
            foreach (var obj in DungeonGenerationConfigurationSo.objectPool)
            {
                Destroy(obj);
            }
            DungeonGenerationConfigurationSo.objectPool.Clear();
        }
        DungeonGenerationConfigurationSo.objectPool ??= new List<GameObject>();
        DungeonGenerationConfigurationSo.board = new List<Cell>();

        for (int i = 0; i < DungeonGenerationConfigurationSo.size.x; i++)
        {
            for (int j = 0; j < DungeonGenerationConfigurationSo.size.y; j++)
            {
                DungeonGenerationConfigurationSo.board.Add(new Cell());
            }
        }

        int currentCell = DungeonGenerationConfigurationSo.startPos;

        Stack<int> path = new Stack<int>();

        // Limit
        int k = 0;

        while (k<1000)
        {
            k++;

            DungeonGenerationConfigurationSo.board[currentCell].visited = true;

            if(currentCell == DungeonGenerationConfigurationSo.board.Count - 1)
            {
                break;
            }

            //Check the cell's neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        DungeonGenerationConfigurationSo.board[currentCell].status[2] = true;
                        currentCell = newCell;
                        DungeonGenerationConfigurationSo.board[currentCell].status[3] = true;
                    }
                    else
                    {
                        DungeonGenerationConfigurationSo.board[currentCell].status[1] = true;
                        currentCell = newCell;
                        DungeonGenerationConfigurationSo.board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        DungeonGenerationConfigurationSo.board[currentCell].status[3] = true;
                        currentCell = newCell;
                        DungeonGenerationConfigurationSo.board[currentCell].status[2] = true;
                    }
                    else
                    {
                        DungeonGenerationConfigurationSo.board[currentCell].status[0] = true;
                        currentCell = newCell;
                        DungeonGenerationConfigurationSo.board[currentCell].status[1] = true;
                    }
                }

            }

        }
        GenerateDungeon();
    }

    public void TryInitializationOfDungeon()
    {
        Debug.Log("Checking if all rooms are ready.");
        Debug.Log("Rooms Ready:" + DungeonGenerationConfigurationSo.roomsReady + " / " + DungeonGenerationConfigurationSo.roomCount);
        if (DungeonGenerationConfigurationSo.roomsReady >= DungeonGenerationConfigurationSo.roomCount)
        {
            Debug.Log("All rooms are ready!");
            InitializeNavMeshes();
            DungeonGenerationConfigurationSo.roomsReady = 0;
        }
    }
    
    private void InitializeNavMeshes()
    {
        Debug.Log("Initializing NavMeshes!");
        var surfaces =  FindObjectsOfType<NavMeshSurface>();
        foreach (var s in surfaces)
        {
            s.BuildNavMesh();
        }
    }

    private List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up neighbor
        if (cell - DungeonGenerationConfigurationSo.size.x >= 0 && !DungeonGenerationConfigurationSo.board[(cell-DungeonGenerationConfigurationSo.size.x)].visited)
        {
            neighbors.Add((cell - DungeonGenerationConfigurationSo.size.x));
        }

        //check down neighbor
        if (cell + DungeonGenerationConfigurationSo.size.x < DungeonGenerationConfigurationSo.board.Count && !DungeonGenerationConfigurationSo.board[(cell + DungeonGenerationConfigurationSo.size.x)].visited)
        {
            neighbors.Add((cell + DungeonGenerationConfigurationSo.size.x));
        }

        //check right neighbor
        if ((cell+1) % DungeonGenerationConfigurationSo.size.x != 0 && !DungeonGenerationConfigurationSo.board[(cell +1)].visited)
        {
            neighbors.Add((cell +1));
        }

        //check left neighbor
        if (cell % DungeonGenerationConfigurationSo.size.x != 0 && !DungeonGenerationConfigurationSo.board[(cell - 1)].visited)
        {
            neighbors.Add((cell -1));
        }

        return neighbors;
    }
}
