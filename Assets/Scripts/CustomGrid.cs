using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public Cell[,] cells;
    public GameObject[] spawnObjects;

    public Cell cell;
    public int width, height;
    public Vector2 Offset;    
    public float rowOffset;


    private void Awake()
    {
        CreateGrid();
        StartCoroutine(CreateItems());
    }

    private void CreateGrid()
    {
        cells = new Cell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                cells[i, j] = Instantiate(cell, transform.position + new Vector3(i * Offset.x + j * rowOffset, j * Offset.y, 0f),
                    new Quaternion(0f, 0f, 0f, 0f), transform).GetComponent<Cell>();
                cells[i, j].GetComponent<Cell>().index = new Vector2Int(i, j);
            }
        }
    }

    public List<Cell> GetNeighbours(Cell node)
    {
        List<Cell> neighbours = new List<Cell>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && y == 0) || (x * y == 1) || (x * y == -1))
                    continue;

                int checkX = node.index.x + x;
                int checkY = node.index.y + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    if (cells[checkX,checkY].isWalkable)
                        neighbours.Add(cells[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Vector3 GetDirection(Vector2 direction, Vector2Int currentCoord)
    {
        Vector2Int index = currentCoord;
        if (direction.x > 0)
            index += new Vector2Int(1, 0);
        else if (direction.x < 0)
            index += new Vector2Int(-1, 0);
        if (direction.y < 0)
            index += new Vector2Int(0, -1);
        else if (direction.y > 0)
            index += new Vector2Int(0, 1);

        if (index.x < width && index.x > -1 && index.y < height && index.y > -1)
            if (cells[index.x,index.y].isWalkable)
                return GetCellCoordinate(index);
        return GetCellCoordinate(currentCoord);
    }


    public Vector3 GetCellCoordinate(Vector2Int index)
    {
        return cells[index.x, index.y].transform.position;
    }

    IEnumerator CreateItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            
            int posX, posY;
            do
            {
                posX = Random.Range(0, width);
                posY = Random.Range(0, height);
            }
            while (!cells[posX, posY].isWalkable);

            cells[posX, posY].go = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)], cells[posX, posY].transform.position, new Quaternion(0f, 0f, 0f, 0f));
        }
    }

    private void OnDrawGizmos()
    {  
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (cells!=null)
                    Gizmos.color = ((cells[i, j].isWalkable) ? Color.blue : Color.red);
                Gizmos.DrawSphere(transform.position + new Vector3(i * Offset.x + j*rowOffset, j * Offset.y, 0f), 0.1f);
            }
        }
    }
}
