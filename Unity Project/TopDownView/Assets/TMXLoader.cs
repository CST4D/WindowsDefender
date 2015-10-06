using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class TMXLoader {
    private TextAsset tAsset;
    public int mapWidth, mapHeight;
    public Sprite[] sprites;
    public Tile[,] tiles;
    private MonoBehaviour context;
    private XmlDocument doc;

    private XmlNodeList mapList;
    private XmlNode mapNode;

    public TMXLoader(TextAsset tAsset, MonoBehaviour context)
    {
        this.tAsset = tAsset;
        this.context = context;
    }
    public void loadMeta()
    {
        doc = new XmlDocument();
        doc.LoadXml(tAsset.text);

        mapList = doc["map"].ChildNodes;
        mapNode = doc["map"];

        int tilewidth = Convert.ToInt32(((XmlElement)mapNode).Attributes["tilewidth"].InnerText);
        int tileheight = Convert.ToInt32(((XmlElement)mapNode).Attributes["tileheight"].InnerText);
        mapWidth = Convert.ToInt32(((XmlElement)mapNode).Attributes["width"].InnerText);
        mapHeight = Convert.ToInt32(((XmlElement)mapNode).Attributes["height"].InnerText);
    }
    public void load()
    {
        sprites = new Sprite[1];
        int spriteArraySize = 1;

        for (int i = 0; i < mapList.Count; i++)
        {
            XmlNode i2 = mapList[i];
            if (i2.Name == "tileset")
            {
                Texture2D tileset = Resources.Load<Texture2D>(i2["image"].Attributes["source"].InnerText);
                Rect rec = new Rect(0, 0, 32, 32);
                int imgWidth = Convert.ToInt32(i2["image"].Attributes["width"].InnerText);
                int firstgid = Convert.ToInt32(i2.Attributes["firstgid"].InnerText);
                
                int tilecount = Convert.ToInt32(i2.Attributes["tilecount"].InnerText);
                int afterlastgid = firstgid + tilecount;
                spriteArraySize += tilecount;


                Array.Resize<Sprite>(ref sprites, spriteArraySize);
                for (int y = 0; firstgid < afterlastgid; y++) 
                {
                    for (int x = 0; (firstgid < afterlastgid) && (x*32 < imgWidth); x++) {
                        Sprite.Create(tileset, rec, new Vector2(x, y), 1);
                        firstgid++;
                    }
                }
            }
            if (i2.Name == "layer")
            {
                bool walkable = false;
                bool buildable = false;
                if (i2.Attributes["name"].InnerText.ToLower() == "walls")
                {
                    buildable = true;
                }
                if (i2.Attributes["name"].InnerText.ToLower() == "walkables")
                {
                    walkable = true;
                }
                string[] linesplit = i2["data"].InnerText.Split(',');
                for (int y = 0; y < mapHeight; y++)
                { 
                    for (int x = 0; x < mapWidth; x++)
                    {
                        if (Convert.ToInt32(linesplit[(y * mapHeight) + x]) > 0)
                        {
                            tiles[y, x].Buildable = walkable;
                            tiles[y, x].Walkable = buildable;
                            tiles[y, x].mapSprite = sprites[Convert.ToInt32(linesplit[(y * mapHeight) + x])];
                        }
                    }
                }
                    
                
            }
        }
    }
}
