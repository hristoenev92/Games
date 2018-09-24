using UnityEngine;

public class PlayerCube : MonoBehaviour
{    
    public void PlayerMove(Vector3 direction, char[,] grid)
    {
        if (!IsBlocked(direction, grid))
        {
            transform.position += direction;
            UpdateGrid(grid, direction);
        }
        
    }
    public void PlayerTeleport(Vector3 teleportPosition, char[,] grid)
    {
        UpdateGridAfterTeleport(grid, teleportPosition);
        transform.position = teleportPosition;
    }

    private bool IsBlocked(Vector3 direction, char[,] grid)
    {
        Vector3 newPosition = transform.position + direction;
        GameObject[] allWalls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in allWalls)
        {
            if (wall.transform.position == newPosition)
            {
                return true;
            }
        }

        GameObject[] allMovableBoxes = GameObject.FindGameObjectsWithTag("MovableBox");
        foreach (var movableBox in allMovableBoxes)
        {
            if (movableBox.transform.position == newPosition)
            {
                MovableBox box = movableBox.GetComponent<MovableBox>();
                if (box.BoxMove(direction, grid))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void UpdateGrid(char[,] grid, Vector3 direction)
    {
        int pastX = (int)transform.position.x-(int)direction.x;
        int pastZ = (int)transform.position.z-(int)direction.z;
        int currentX = (int)transform.position.x;
        int currentZ = (int)transform.position.z;
        
        switch (grid[pastZ,pastX])
        {
            case 'P':
                {
                    grid[pastZ, pastX] = 'O';
                    break;
                }
            case 'R':
                {
                    grid[pastZ, pastX] = 'E';
                    break;
                }
        }
        switch (grid[currentZ, currentX])
        {
            case 'O':
                {
                    grid[currentZ, currentX] = 'P';
                    break;
                }
            case 'E':
                {
                    grid[currentZ, currentX] = 'R';
                    break;
                }
        }
    }
    private void UpdateGridAfterTeleport(char[,] grid, Vector3 teleportPosition)
    {
        int pastX = (int)transform.position.x;
        int pastZ = (int)transform.position.z;
        int currentX = (int)teleportPosition.x;
        int currentZ = (int)teleportPosition.z;

        switch (grid[pastZ, pastX])
        {
            case 'P':
                {
                    grid[pastZ, pastX] = 'O';
                    break;
                }
            case 'R':
                {
                    grid[pastZ, pastX] = 'E';
                    break;
                }
        }
        switch (grid[currentZ, currentX])
        {
            case 'O':
                {
                    grid[currentZ, currentX] = 'P';
                    break;
                }
            case 'E':
                {
                    grid[currentZ, currentX] = 'R';
                    break;
                }
        }
    }
}
