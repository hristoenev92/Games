using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject floorCube;
    public GameObject wallCube;
    public GameObject movableBox;
    public PlayerCube playerCube;
    public GameObject endPiont;
    public Camera mainCamera;
    public GameObject winMenu;
    
    public static List<char[,]> levelsList = new List<char[,]>
    { 
        Levels.level1Grid,
        Levels.level2Grid,
        Levels.level3Grid
    };

    private static int currentLevel;
    public static char[,] levelToGenerate;
    private int endPositionsCount;
    
    public void Start()
    {
        currentLevel = Levels.levelSelector;
        ArrayCopy();
        GenerateBoard();
        playerCube = FindObjectOfType<PlayerCube>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            playerCube.PlayerMove(GetInput(), levelToGenerate);
            if (Win())
            {
                winMenu.SetActive(true);
                enabled = false;
                Destroy();
                if (currentLevel< levelsList.Count-1)
                {
                    Levels.levelSelector++;
                    Start();
                }
                else
                {
                    Levels.levelSelector = 0;
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

    private void GenerateBoard()
    {
        endPositionsCount = 0;
        // col - z, row - x
        for (int col = 0; col < levelToGenerate.GetLength(0); col++)
        {
            for (int row = 0; row < levelToGenerate.GetLength(1); row++)
            {
                switch (levelToGenerate[col, row])
                {
                    case 'W':
                        {
                            Instantiate(wallCube, new Vector3(row, 0, col), Quaternion.identity, transform);
                            Instantiate(wallCube, new Vector3(row, -1, col), Quaternion.identity, transform);
                            break;
                        }
                    case 'P':
                        {
                            Instantiate(floorCube, new Vector3(row, -1, col), Quaternion.identity, transform);
                            Instantiate(playerCube, new Vector3(row, 0, col), Quaternion.identity, transform);
                            break;
                        }
                    case 'O':
                        {
                            Instantiate(floorCube, new Vector3(row, -1, col), Quaternion.identity, transform);
                            break;
                        }
                    case 'E':
                        {
                            Instantiate(endPiont, new Vector3(row, -1, col), Quaternion.identity, transform);
                            endPositionsCount++;
                            break;
                        }
                    case 'B':
                        {
                            Instantiate(floorCube, new Vector3(row, -1, col), Quaternion.identity, transform);
                            Instantiate(movableBox, new Vector3(row, 0, col), Quaternion.identity, transform);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            mainCamera.transform.position = new Vector3(levelToGenerate.GetLength(1) / 2, 13, -3.5f);
        }
    }

    private Vector3 GetInput()
    {
        Vector3 direction = new Vector3(0, 0, 0);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction.x = 1;
            direction.z = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction.x = -1;
            direction.z = 0;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction.x = 0;
            direction.z = 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction.x = 0;
            direction.z = -1;
        }

        return direction;
    }
    private bool Win()
    {
        int filledEndPositions = 0;
        for (int col = 0; col < levelToGenerate.GetLength(0); col++)
        {
            for (int row = 0; row < levelToGenerate.GetLength(1); row++)
            {
                if (levelToGenerate[col, row] == 'F')
                {
                    filledEndPositions++;
                }
            }
        }
        if (endPositionsCount==filledEndPositions)
        {
            Debug.Log("You Win!!");
            return true;
        }
        return false;
    }
    private void Destroy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
    private void ArrayCopy()
    {
        //arrays are objects and mutable, this is to copy the original array not just make a reference to it
        levelToGenerate = new char[levelsList[currentLevel].GetLength(0), levelsList[currentLevel].GetLength(1)];
        for (int col = 0; col < levelsList[currentLevel].GetLength(0); col++)
        {
            for (int row = 0; row < levelsList[currentLevel].GetLength(1); row++)
            {
                levelToGenerate[col, row] = levelsList[currentLevel][col, row];
            }
        }
    }
}
