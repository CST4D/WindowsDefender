using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    public Waypoint wayPoint;
    public EnemyAI enemy;
    public Tile tile;
    public SpawnerAI spawner;

    private ArrayList enemies;
    private ArrayList spawners;
    private float timer;
    private Waypoint _point;
    private Tile[,] map;
    private int _mapWidth, _mapHeight;

    struct Square
    {
        public int g;
        public int h;
        public int f;
        public int x;
        public int y;
    };

    // Use this for initialization
    void Start()
    {
        enemies = new ArrayList();
        spawners = new ArrayList();
        timer = 0;

        // Placeholder Generation Code
        int tilesize = 50;
        _mapWidth = 800 / tilesize;
        _mapHeight = 600 / tilesize;

        map = new Tile[_mapHeight, _mapWidth];

        for (int i = 0; i < _mapHeight; i++)
        {
            for (int j = 0; j < _mapWidth; j++)
            {
                map[i, j] = (Tile)Instantiate(tile, new Vector2((0.5f * j), (0.5f * i)), transform.rotation);
                map[i, j].Buildable = true;
                map[i, j].Walkable = true;
                map[i, j].transform.parent = transform.Find("Tilemap").transform;
            }
        }

        for(int i = 0; i < (_mapWidth - 1); i++)
        {
            //map[2, i].Walkable = false;
            map[6, 1 + i].Walkable = false;
        }

        _point = (Waypoint)Instantiate(wayPoint, map[(_mapHeight - 1), (_mapWidth - 1)].transform.position, transform.rotation);
        _point.transform.parent = transform;

        SpawnerAI enemySpawner = (SpawnerAI)Instantiate(spawner, map[0, 0].transform.position, transform.rotation);
        enemySpawner.transform.parent = transform.Find("Spawners").transform;
        spawners.Add(enemySpawner);
        enemySpawner.wayPoints = pathFinding(enemySpawner);
    }

    // Update is called once per frame
    void Update()
    {
        // Check health of monsters
        CheckEnemy();

        // Create Monsters
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            foreach (SpawnerAI spawn in spawners)
            {
                EnemyAI temp;
                timer -= 0.5f;
                temp = (EnemyAI)GameObject.Instantiate(enemy, spawn.transform.position, transform.rotation);
                temp.health = 300;
                temp.movementSpeed = 1.0f;
                temp.transform.parent = transform.Find("Enemies").transform;
                // Some Path-finding Algorithm here
                temp.movementPoints = copyWaypoints(spawn);
                enemies.Add(temp);
            }
        }
    }

    void CheckEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyAI temp = (EnemyAI)enemies[i];
            if (temp.health < 0)
            {
                Destroy(temp.gameObject);
                enemies.Remove(temp);
            }
        }
    }

    /// <summary>
    /// A* Pathfinding Algorithm
    /// </summary>
    /// <param name="spawner"></param>
    /// <returns>List of Vectors that the Monsters will follow till they reach their destination</returns>
    LinkedList<Vector2> pathFinding(SpawnerAI spawner)
    {
        // 2D Array List of possible paths monsters can take to reach their destination
        LinkedList<Vector2> paths = new LinkedList<Vector2>();

        // List of tile indexes used for calculating waypoints
        LinkedList<Square> usedSquares = new LinkedList<Square>();
        LinkedList<Square> openSquares = new LinkedList<Square>();
        LinkedList<Square> leadingSquares = new LinkedList<Square>();

        Vector2 destSquare = FindTile(_point.gameObject);
        int fMin = 0, g = 0;
        leadingSquares.AddLast(createSquare(g, FindTile(spawner.gameObject), destSquare));
        openSquares.AddLast(currentSquare);

        int counter = 0;
        int counterMax = _mapWidth * _mapHeight;
        do
        {

            usedSquares.AddLast(currentSquare);
            openSquares.Remove(currentSquare);

            if (counter > counterMax)
                return paths;
            else
                counter++;

            if (currentSquare.h != 0)
            {
                // Check Adjacent Squares
                LinkedList<Vector2> adjacentTiles = AdjacentTiles((int)currentSquare.x, (int)currentSquare.y, usedSquares, openSquares);

                g = currentSquare.g + 1;
                foreach (Vector2 square in adjacentTiles)
                {
                    Square temp = createSquare(g, square, destSquare);

                    if (!usedSquares.Contains(temp) && !openSquares.Contains(temp))
                        openSquares.AddLast(temp);

                }

                fMin = int.MaxValue;
                foreach (Square square in openSquares)
                {
                    if (square.f < fMin)
                    {
                        currentSquare = square;
                        fMin = square.f;
                    }
                }

            }
            else
            {
                openSquares.Clear();

                do
                {
                    paths.AddFirst(map[currentSquare.y, currentSquare.x].transform.position);
                    usedSquares.Remove(currentSquare);
                    g--;
                    int smallestDist = int.MaxValue;
                    Square tempSquare = currentSquare;
                    foreach(Square square in usedSquares)
                    {
                        if(distSquare(square, currentSquare) < smallestDist)
                        {
                            smallestDist = distSquare(square, currentSquare);
                            tempSquare = square;
                        }
                    }

                    currentSquare = tempSquare;
                } while (g > 0);

                paths.AddFirst(map[currentSquare.y, currentSquare.x].transform.position);
                usedSquares.Remove(currentSquare);
                Debug.Log("Counter: " + counter);
                return paths;
            }
        } while (openSquares.Count > 0);

        return paths;
    }

    /// <summary>
    /// Calculates possible adjacent tiles that monsters can walk on
    /// Diagonal tiles are only included if the adjacent tiles are available
    /// to walk on too
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    LinkedList<Vector2> AdjacentTiles(int x, int y, LinkedList<Square> usedSquares, LinkedList<Square> openSquares)
    {
        LinkedList<Vector2> adjacentTiles = new LinkedList<Vector2>();

        for (int i = y - 1; i < y + 2; i++)
        {
            if (i < 0 || i >= _mapHeight)
                continue;

            // Check Left-side
            if ((x - 1 > 0))
            {
                // top-left & bottom-left corners
                if (i == (y - 1) || i == (y + 1))
                {
                    if (map[i, (x - 1)].Walkable && map[y, (x - 1)].Walkable && map[i, x].Walkable)
                    {
                        Vector2 position = new Vector2((x - 1), i);
                        adjacentTiles.AddLast(position);
                    }
                }
                else if (map[y, (x - 1)].Walkable)
                {
                    Vector2 position = new Vector2((x - 1), y);
                    adjacentTiles.AddLast(position);
                }
            }

            // Check Middle, we skip (x,y) since we have that data already
            if ((i != y) && map[i, x].Walkable)
            {
                Vector2 position = new Vector2(x, i);
                adjacentTiles.AddLast(position);
            }

            // Check Right-side
            if ((x + 1 < _mapWidth))
            {
                // top-right & bottom-right corners
                if (i == (y - 1) || i == (y + 1))
                {
                    if (map[i, (x + 1)].Walkable && map[y, (x + 1)].Walkable && map[i, x].Walkable)
                    {
                        Vector2 position = new Vector2((x + 1), i);
                        adjacentTiles.AddLast(position);
                    }
                }
                else if (map[y, (x + 1)].Walkable)
                {
                    Vector2 position = new Vector2((x + 1), y);
                    adjacentTiles.AddLast(position);
                }
            }
        }

        LinkedList<Vector2> removeTiles = new LinkedList<Vector2>();

        foreach (Vector2 adjacentTile in adjacentTiles)
        {
            foreach (Square usedSquare in usedSquares)
                if ((int)adjacentTile.x == usedSquare.x && (int)adjacentTile.y == usedSquare.y)
                    removeTiles.AddLast(adjacentTile);

            foreach (Square openSquare in openSquares)
                if ((int)adjacentTile.x == openSquare.x && (int)adjacentTile.y == openSquare.y)
                    removeTiles.AddLast(adjacentTile);
        }

        foreach (Vector2 removeTile in removeTiles)
            adjacentTiles.Remove(removeTile);

        return adjacentTiles;
    }

    /// <summary>
    /// Find the specific tile that the object is on
    /// </summary>
    /// <returns>Vector2 where x is the column index and y is the row index</returns>
    Vector2 FindTile(GameObject gameObject)
    {
        Vector2 index = new Vector2(-1, -1);

        for (int i = 0; i < _mapHeight; i++)
        {
            for (int j = 0; j < _mapWidth; j++)
            {
                float dist = Vector2.Distance(gameObject.transform.position, map[i, j].transform.position);

                if (dist == 0)
                {
                    index.x = j;
                    index.y = i;
                    return index;
                }
            }
        }

        return index;
    }

    /// <summary>
    /// Calculates the distance from point A to B
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Returns the distnce</returns>
    int getHeuristics(Vector2 a, Vector2 b)
    {
        int dx, dy;

        dx = (int)(b.x - a.x);
        dy = (int)(b.y - a.y);

        if (dx < 0)
            dx *= -1;
        if (dy < 0)
            dy *= -1;

        return dx + dy;
    }

    Square createSquare(int g, Vector2 v, Vector2 dv)
    {
        Square newSquare = new Square();

        newSquare.x = (int)v.x;
        newSquare.y = (int)v.y;
        newSquare.g = g;
        newSquare.h = getHeuristics(v, dv);
        newSquare.f = newSquare.g + newSquare.h;

        return newSquare;
    }

    int distSquare(Square a, Square b)
    {
        int dx, dy;

        dx = (int)(b.x - a.x);
        dy = (int)(b.y - a.y);

        if (dx < 0)
            dx *= -1;
        if (dy < 0)
            dy *= -1;

        return dx + dy;
    }

    LinkedList<Vector2> copyWaypoints(SpawnerAI spawner)
    {
        LinkedList<Vector2> copyWaypoints = new LinkedList<Vector2>();

        foreach(Vector2 v in spawner.wayPoints)
        {
            copyWaypoints.AddLast(v);
        }

        return copyWaypoints;
    }
}