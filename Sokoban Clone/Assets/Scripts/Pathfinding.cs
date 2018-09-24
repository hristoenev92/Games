using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Color hoverColor;
    public Color pathColor;
    public Color startColor;

    private PlayerCube playerCube;
    private Renderer rend;

    private Vector3 startPosition;
    private Vector3 endPosition;
    
    private char[,] levelGrid = Game.levelToGenerate;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        playerCube = FindObjectOfType<PlayerCube>();
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    private void OnMouseDown()
    {
        startPosition = playerCube.transform.position;
        endPosition = new Vector3(transform.position.x, 0, transform.position.z);
        PathfindingAlgorithm();
    }
    
    private void PathfindingAlgorithm()
    {
        int startX = (int)startPosition.x;
        int startZ = (int)startPosition.z;
        int endX = (int)endPosition.x;
        int endZ = (int)endPosition.z;

        int[,] marksGrid = new int[levelGrid.GetLength(0), levelGrid.GetLength(1)];
        int marker = 0;
        
        for (int col = 0; col < levelGrid.GetLength(0); col++)
        {
            for (int row = 0; row < levelGrid.GetLength(1); row++)
            {
                // new grid to hold all the marks, -1 means that position is not visited
                // P and R are included for another pathfinding
                if (levelGrid[col,row]=='E' || levelGrid[col,row]=='O' || levelGrid[col, row] == 'P' || levelGrid[col, row] == 'R')
                {
                    marksGrid[col, row] = -1;
                }
            }
        }
        Path(marksGrid, marker, startX, startZ,endX, endZ );
    }

    private void Path(int[,] marksGrid,int marker, int startX, int startZ, int endX, int endZ)
    {
        if (startX == endX && startZ == endZ)
        {
            Debug.Log("Path Found");
            playerCube.PlayerTeleport(new Vector3(endX, 0, endZ), levelGrid);
            return;
        }

        //sets a value to every position that has passed
        marksGrid[startZ, startX] = marker;
        //checks every position if it can be passed on 
        //and recursivly continues until the end position or until there are no more moves
        if (IsValidMove(marksGrid, startX + 1, startZ))
        {
            Path(marksGrid, marker+1, startX + 1, startZ, endX, endZ);
        }
        if (IsValidMove(marksGrid, startX - 1, startZ))
        {
            Path(marksGrid, marker + 1, startX - 1, startZ, endX, endZ);
        }
        if (IsValidMove(marksGrid, startX , startZ + 1))
        {
            Path(marksGrid, marker + 1, startX, startZ + 1, endX, endZ);
        }
        if (IsValidMove(marksGrid, startX , startZ - 1))
        {
            Path(marksGrid, marker + 1, startX, startZ - 1, endX, endZ);
        }
    }
    private bool IsValidMove(int[,] marksGrid, int x, int z)
    {
        if (marksGrid[z,x]==-1)
        {
            return true;
        }
        return false;
    }
}
