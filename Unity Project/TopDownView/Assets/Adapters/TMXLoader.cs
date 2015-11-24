using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

/// <summary>
/// 
/// </summary>
public class TMXLoader {
    /// <summary>
    /// The t asset
    /// </summary>
    private TextAsset tAsset;
    /// <summary>
    /// The map width
    /// </summary>
    private int mapWidth, mapHeight;
    /// <summary>
    /// The real map width
    /// </summary>
    public int realMapWidth, realMapHeight;
    /// <summary>
    /// The sprites
    /// </summary>
    public Sprite[] sprites;
    /// <summary>
    /// The tiles
    /// </summary>
    public Tile[,] tiles;
    /// <summary>
    /// The context
    /// </summary>
    private GameController context;
    /// <summary>
    /// The document
    /// </summary>
    private XmlDocument doc;
    /// <summary>
    /// The tilesize
    /// </summary>
    private float tilesize;

    /// <summary>
    /// 
    /// </summary>
    private enum ReflectMode { Horizontal, Vertical };
    /// <summary>
    /// The r mode
    /// </summary>
    private ReflectMode rMode = ReflectMode.Horizontal;

    /// <summary>
    /// The map list
    /// </summary>
    private XmlNodeList mapList;
    /// <summary>
    /// The map node
    /// </summary>
    private XmlNode mapNode;

    /// <summary>
    /// The waypoint
    /// </summary>
    private Vector2 waypoint;
    /// <summary>
    /// The spawners
    /// </summary>
    private LinkedList<Vector2> spawners = new LinkedList<Vector2>();
    /// <summary>
    /// The team identifier
    /// </summary>
    private int teamId;
    /// <summary>
    /// The transform vector
    /// </summary>
    private Vector3 transformVector;
    /// <summary>
    /// Gets the transform vector.
    /// </summary>
    /// <value>
    /// The transform vector.
    /// </value>
    public Vector3 TransformVector { get { return transformVector; } }
    /// <summary>
    /// The background
    /// </summary>
    private Sprite background = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="TMXLoader"/> class.
    /// </summary>
    /// <param name="tAsset">The t asset.</param>
    /// <param name="context">The context.</param>
    /// <param name="teamId">The team identifier.</param>
    public TMXLoader(TextAsset tAsset, GameController context, int teamId)
    {
        this.tAsset = tAsset;
        this.context = context;
        this.teamId = teamId;
    }
    /// <summary>
    /// Loads the meta.
    /// </summary>
    public void loadMeta()
    {
        doc = new XmlDocument();
        doc.LoadXml(tAsset.text);

        mapList = doc["map"].ChildNodes;
        mapNode = doc["map"];

        XmlNode propNode = mapNode["properties"];
        foreach (XmlNode prop in propNode.ChildNodes)
        {
            if (prop.Attributes["name"].InnerText == "TeamReflectMode")
            {
                if (prop.Attributes["value"].InnerText == "vertical")
                {
                    rMode = ReflectMode.Vertical;
                }
            }
            if (prop.Attributes["name"].InnerText == "background")
            {
                background = Resources.Load<Sprite>(prop.Attributes["value"].InnerText);
                
            }
        }

        int tilewidth = Convert.ToInt32(((XmlElement)mapNode).Attributes["tilewidth"].InnerText);
        int tileheight = Convert.ToInt32(((XmlElement)mapNode).Attributes["tileheight"].InnerText);
        mapWidth = Convert.ToInt32(((XmlElement)mapNode).Attributes["width"].InnerText);
        mapHeight = Convert.ToInt32(((XmlElement)mapNode).Attributes["height"].InnerText);
        tilesize = 0.32f;

        realMapHeight = mapHeight * (rMode == ReflectMode.Vertical ? 2 : 1);
        realMapWidth = mapWidth * (rMode == ReflectMode.Horizontal ? 2 : 1);

        tiles = new Tile[realMapHeight, realMapWidth];

        if (background != null)
        {
            context.transform.Find("Tilemap").Find("Background").GetComponent<SpriteRenderer>().sprite = background;
            context.transform.Find("Tilemap").Find("Background").transform.position = new Vector2((background.rect.width/32)*0.32f/2-0.16f, (background.rect.height / 32) * 0.32f / 2 - 0.16f);
        }

        for (int i = 0; i < mapHeight * (rMode == ReflectMode.Vertical ? 2 : 1); i++)
        {
            for (int j = 0; j < mapWidth * (rMode == ReflectMode.Horizontal ? 2 : 1); j++)
            {
                tiles[i, j] = (Tile)UnityEngine.Object.Instantiate(context.tile, new Vector2((tilesize * j), (tilesize * i)), context.transform.rotation);
                tiles[i, j].Buildable = false;
                tiles[i, j].Walkable = true;
                tiles[i, j].transform.parent = context.transform.Find("Tilemap").transform;
            }
        }
    }
    /// <summary>
    /// Loads this instance.
    /// </summary>
    public void load()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tiles[mapHeight - y - 1, x].Walkable = true;
                tiles[mapHeight - y - 1, x].Buildable = false;
            }
        }

        sprites = new Sprite[1];
        int spriteArraySize = 1;

        for (int i = 0; i < mapList.Count; i++)
        {
            XmlNode i2 = mapList[i];
            if (i2.Name == "tileset")
            {
                Texture2D tileset = Resources.Load<Texture2D>(i2["image"].Attributes["source"].InnerText.Substring(0, i2["image"].Attributes["source"].InnerText.LastIndexOf('.')));
                int imgWidth = Convert.ToInt32(i2["image"].Attributes["width"].InnerText);
                int imgHeight = Convert.ToInt32(i2["image"].Attributes["height"].InnerText);
                int firstgid = Convert.ToInt32(i2.Attributes["firstgid"].InnerText);

                int tilecount = Convert.ToInt32(i2.Attributes["tilecount"].InnerText);
                int afterlastgid = firstgid + tilecount;
                spriteArraySize += tilecount;

                Vector2 pivot = new Vector2(0.5f, 0.5f);

                Array.Resize<Sprite>(ref sprites, spriteArraySize);
                for (int y = (imgHeight / 32) - 1; firstgid < afterlastgid; y--)
                {
                    for (int x = 0; (firstgid < afterlastgid) && (x * 32 < imgWidth); x++)
                    {
                        sprites[firstgid] = Sprite.Create(tileset, new Rect(x * 32, y * 32, 32, 32), pivot);
                        firstgid++;
                    }
                }
            }
            if (i2.Name == "layer")
            {
                bool walkable = false;
                bool buildable = false;
                bool visible = true;
                if (i2.Attributes["name"].InnerText.ToLower() == "walls")
                {
                    buildable = true;
                }
                if (i2.Attributes["name"].InnerText.ToLower() == "walkables")
                {
                    walkable = true;
                }
                if (i2.Attributes["name"].InnerText.ToLower() == "nowalk")
                {
                    walkable = false;
                }
                foreach (XmlNode prop in i2.ChildNodes)
                {
                    if (prop.Name == "properties")
                    {
                        foreach (XmlNode prop2 in prop.ChildNodes)
                        {
                            if (prop2.Attributes["name"].InnerText.ToLower() == "visible" && prop2.Attributes["value"].InnerText == "0")
                            {
                                visible = false;
                            }
                        }
                    }
                }
                string[] linesplit = i2["data"].InnerText.Split(',');
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        if (Convert.ToInt32(linesplit[(y * mapWidth) + x]) > 0)
                        {
                            tiles[mapHeight - y - 1, x].Buildable = buildable;
                            tiles[mapHeight - y - 1, x].Walkable = walkable;
                            if (visible)
                                tiles[mapHeight - y - 1, x].mapSprite = sprites[Convert.ToInt32(linesplit[(y * mapWidth) + x])];
                        }
                    }
                }


            }
            if (i2.Name == "objectgroup")
            {
                if (i2.Attributes["name"].InnerText.ToLower() == "entities")
                {
                    foreach (XmlNode obj in i2.ChildNodes)
                    {
                        int x = Convert.ToInt32(obj.Attributes["x"].InnerText) / 32;
                        int y = mapHeight - (Convert.ToInt32(obj.Attributes["y"].InnerText) / 32) - 1;
                        
                        if (obj.Attributes["type"].InnerText.ToLower() == "waypoint")
                        {
                            Waypoint point = (Waypoint)UnityEngine.Object.Instantiate(context.wayPoint, tiles[y, x].transform.position, context.transform.rotation);
                            point.transform.parent = context.transform;
                            context.setWayPoint(point, 1);
                            waypoint = new Vector2(x, y);
                        }
                    }
                    foreach (XmlNode obj in i2.ChildNodes)
                    {
                        int x = Convert.ToInt32(obj.Attributes["x"].InnerText) / 32;
                        int y = mapHeight - (Convert.ToInt32(obj.Attributes["y"].InnerText) / 32) - 1;
                        if (obj.Attributes["type"].InnerText.ToLower() == "spawner")
                        {
                            SpawnerAI enemySpawner = (SpawnerAI)UnityEngine.Object.Instantiate(context.spawner, tiles[y, x].transform.position, context.transform.rotation);
                            enemySpawner.transform.parent = context.transform.Find("Spawners").transform;
                            context.addSpawnerToSpawnerList(enemySpawner, 1);
                            spawners.AddLast(new Vector2(x, y));
                        }
                    }
                }

            }
        }
        if (rMode == ReflectMode.Horizontal)
            reflectMapHorizontal();
        else
            reflectMapVertical();
    }
    /// <summary>
    /// Reflects the map horizontal.
    /// </summary>
    private void reflectMapHorizontal()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tiles[mapHeight - y - 1, x + mapWidth].Buildable = tiles[mapHeight - y - 1, mapWidth - x - 1].Buildable;
                tiles[mapHeight - y - 1, x + mapWidth].Walkable = tiles[mapHeight - y - 1, mapWidth - x - 1].Walkable;
                tiles[mapHeight - y - 1, x + mapWidth].mapSprite = tiles[mapHeight - y - 1, mapWidth - x - 1].mapSprite;
            }
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
                tiles[mapHeight - y - 1, x + (teamId == 1 ? mapWidth : 0)].Buildable = false;
        }
        Waypoint point = (Waypoint)UnityEngine.Object.Instantiate(context.wayPoint, tiles[(int)waypoint.y, (mapWidth * 2 - (int)waypoint.x - 1)].transform.position, context.transform.rotation);
        point.transform.parent = context.transform;
        context.setWayPoint(point, 2);
        foreach (Vector2 pos in spawners)
        {
            SpawnerAI enemySpawner = (SpawnerAI)UnityEngine.Object.Instantiate(context.spawner, tiles[(int)pos.y, (mapWidth * 2 - (int)pos.x - 1)].transform.position, context.transform.rotation);
            enemySpawner.transform.parent = context.transform.Find("Spawners").transform;
            context.addSpawnerToSpawnerList(enemySpawner, 2);
        }
        transformVector = new Vector3(((teamId == 2 ? realMapWidth : 0) * 0.32f), realMapHeight / 2 * 0.32f, 0);
        if (background != null)
        {
            context.transform.Find("Tilemap").Find("BackgroundReflect").transform.position = new Vector2((background.rect.width / 32) * 0.32f / 2 - 0.16f + ((background.rect.width / 32) * 0.32f), (background.rect.height / 32) * 0.32f / 2 - 0.16f);
            context.transform.Find("Tilemap").Find("BackgroundReflect").transform.localScale = new Vector3(-1, 1, 1);
            context.transform.Find("Tilemap").Find("BackgroundReflect").GetComponent<SpriteRenderer>().sprite = background;
        }
        
    }
    /// <summary>
    /// Reflects the map vertical.
    /// </summary>
    private void reflectMapVertical()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tiles[mapHeight + y, x].Buildable = tiles[mapHeight - y - 1, x].Buildable;
                tiles[mapHeight + y, x].Walkable = tiles[mapHeight - y - 1, x].Walkable;
                tiles[mapHeight + y, x].mapSprite = tiles[mapHeight - y - 1, x].mapSprite;
                
            }
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
                tiles[(teamId == 1 ? mapHeight : 0) + y, x].Buildable = false;
        }
        Waypoint point = (Waypoint)UnityEngine.Object.Instantiate(context.wayPoint, tiles[(2 * mapHeight - (int)waypoint.y - 1), (int)waypoint.x].transform.position, context.transform.rotation);
        point.transform.parent = context.transform;
        context.setWayPoint(point, 2);
        foreach (Vector2 pos in spawners)
        {
            SpawnerAI enemySpawner = (SpawnerAI)UnityEngine.Object.Instantiate(context.spawner, tiles[(2 * mapHeight - (int)pos.y - 1), (int)pos.x].transform.position, context.transform.rotation);
            enemySpawner.transform.parent = context.transform.Find("Spawners").transform;
            context.addSpawnerToSpawnerList(enemySpawner, 2);
        }
        transformVector = new Vector3(realMapWidth / 2 * 0.32f, ((teamId == 2 ? realMapHeight : 0) * 0.32f), 0);
        if (background != null)
        {
            context.transform.Find("Tilemap").Find("BackgroundReflect").transform.position = new Vector2((background.rect.width / 32) * 0.32f / 2 - 0.16f, (background.rect.height / 32) * 0.32f / 2 - 0.16f + ((background.rect.height / 32) * 0.32f));
            context.transform.Find("Tilemap").Find("BackgroundReflect").transform.localScale = new Vector3(1, -1, 1);
            context.transform.Find("Tilemap").Find("BackgroundReflect").GetComponent<SpriteRenderer>().sprite = background;
        }
       
    }
}
