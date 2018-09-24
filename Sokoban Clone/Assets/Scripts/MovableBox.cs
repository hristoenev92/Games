using UnityEngine;

public class MovableBox : MonoBehaviour {

    public Material onEndPoint;
    public Material onEmpty;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public bool BoxMove(Vector3 direction, char[,] grid)
    {
        if (!IsBlocked(direction))
        {
            transform.position += direction;
            UpdateGrid(grid, direction);
            return true;
        }
        return false;
    }

    private bool IsBlocked(Vector3 direction)
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
            if(movableBox.transform.position == newPosition)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateGrid(char[,] grid, Vector3 direction)
    {
        int pastX = (int)transform.position.x - (int)direction.x;
        int pastZ = (int)transform.position.z - (int)direction.z;
        int currentX = (int)transform.position.x;
        int currentZ = (int)transform.position.z;
        
        switch (grid[pastZ, pastX])
        {
            case 'B':
                {
                    grid[pastZ, pastX] = 'O';
                    break;
                }
            case 'F':
                {
                    grid[pastZ, pastX] = 'E';
                    rend.sharedMaterial = onEmpty;
                    break;
                }
        }
        switch (grid[currentZ, currentX])
        {
            case 'O':
                {
                    grid[currentZ, currentX] = 'B';
                    break;
                }
            case 'E':
                {
                    grid[currentZ, currentX] = 'F';
                    rend.sharedMaterial = onEndPoint;
                    break;
                }
        }
        
    }
}
