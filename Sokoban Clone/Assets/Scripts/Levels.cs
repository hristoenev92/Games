using UnityEngine;

public class Levels : MonoBehaviour
{
    // Legend:
    // W - wall, B - box, O - open slot, P - player, ' ' - non-playable area
    // E - end point, F - box on end point, R -player on end point
    
    public static int levelSelector = 0;

    public static readonly char[,] level1Grid = new char[,]
    {
        {'W','W','W','W','W','W','W'},
        {'W','O','O','E','O','O','W'},
        {'W','O','O','B','O','O','W'},
        {'W','E','B','P','B','E','W'},
        {'W','O','O','B','O','O','W'},
        {'W','O','O','E','O','O','W'},
        {'W','W','W','W','W','W','W'}
    };

    public static readonly char[,] level2Grid = new char[,]
    {
        {'W','W','W','W','W','W','W'},
        {'W','E','O','O','O','O','W'},
        {'W','O','B','B','O','P','W'},
        {'W','E','O','O','O','O','W'},
        {'W','W','W','W','W','W','W'}
    };

    public static readonly char[,] level3Grid = new char[,]
    {
        {' ',' ',' ',' ','W','W','W','W','W','W','W',' ',' ',' ',' ',' ',' ',' ',' '},
        {' ',' ',' ',' ','W','O','O','O','O','O','W','W','W','W','W','W','W','W','W'},
        {'W','W','W','W','W','O','W','W','W','O','W','P','W','W','O','O','E','E','W'},
        {'W','O','B','O','O','B','O','O','O','O','O','O','O','O','O','O','E','E','W'},
        {'W','O','O','O','W','O','W','W','O','W','W','W','W','W','O','O','E','E','W'},
        {'W','W','W','O','W','O','W','W','O','W',' ',' ',' ','W','W','W','W','W','W'},
        {' ',' ','W','O','O','B','O','B','O','W',' ',' ',' ',' ',' ',' ',' ',' ',' '},
        {' ',' ','W','W','W','O','O','B','W','W',' ',' ',' ',' ',' ',' ',' ',' ',' '},
        {' ',' ',' ',' ','W','B','O','O','W',' ',' ',' ',' ',' ',' ',' ',' ',' ',' '},
        {' ',' ',' ',' ','W','O','O','O','W',' ',' ',' ',' ',' ',' ',' ',' ',' ',' '},
        {' ',' ',' ',' ','W','W','W','W','W',' ',' ',' ',' ',' ',' ',' ',' ',' ',' '}
    };

    public void SelectLevel(int level)
    {
        levelSelector = level;
    }
}
