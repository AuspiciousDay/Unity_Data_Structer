using System.Linq;
using UnityEngine;

public enum TileTypes
{
    Empty = -1,
    // 0~14 해안선타일
    Grass = 15,
    Trees,
    Hills,
    Mountains,
    Towns,
    Castle,
    Dungeon,
}

public class Map
{
    public int rows = 0;
    public int cols = 0;

    public Tile[] tiles;

    public Tile[] CoastTiles => tiles.Where(t => t.autoTileId >= 0 && t.autoTileId < (int)TileTypes.Grass).ToArray();
    public Tile[] LandTiles => tiles.Where(t => t.autoTileId >= (int)TileTypes.Grass).ToArray();

    public Tile castleTile;
    public Tile startTownTile;

    public void Init(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        tiles = new Tile[rows * cols];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new Tile
            {
                id = i
            };
        }

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int index = r * cols + c;
                var adjacents = tiles[index].adjacents;
                if ((r - 1) >= 0)
                {
                    adjacents[(int)Sides.Top] = tiles[index - cols];
                }

                if ((c + 1) < cols)
                {
                    adjacents[(int)Sides.Right] = tiles[index + 1];
                }

                if ((c - 1) >= 0)
                {
                    adjacents[(int)Sides.Left] = tiles[index - 1];
                }

                if ((r + 1) < rows)
                {
                    adjacents[(int)Sides.Bottom] = tiles[index + cols];
                }
            }
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].UpdateAutoTileId();
        }
    }

    public bool CreateIsland(float erodePercent, float erodeIterations, float lakePercent)
    {
        DecorateTiles(LandTiles, lakePercent, TileTypes.Empty);

        for (int i = 0; i < erodeIterations; i++)
        {
            DecorateTiles(CoastTiles, erodePercent, TileTypes.Empty);
        }

        return true;
    }

    public void DecorateTiles(Tile[] tiles, float percent, TileTypes tileType)
    {
        int total = Mathf.FloorToInt(tiles.Length * percent);

        ShuffleTiles(tiles);

        for (int i = 0; i < total; i++)
        {
            if (tileType == TileTypes.Empty)
            {
                tiles[i].ClearAdjacents();
            }
            tiles[i].autoTileId = (int)tileType;
        }
    }

    public void ShuffleTiles(Tile[] tiles)
    {
        // 피셔 예이츠 셔플 알고리즘
        for (int i = tiles.Length - 1; i > 0; i--)
        {
            int index = Random.Range(0, i + 1);
            Tile temp = tiles[i];
            tiles[i] = tiles[index];
            tiles[index] = temp;
        }
    }

    public bool CreatePlace(Tile[] tiles)
    {
        if (tiles == null || tiles.Length < 2)
        {
            return false;
        }

        ShuffleTiles(tiles);

        castleTile = tiles[0];
        castleTile.featureType = TileTypes.Castle;

        startTownTile = tiles[1];
        startTownTile.featureType = TileTypes.Towns;

        return true;
    }

    public void CreateRandomFeatures()
    {
        TileTypes[] randomTypes =
        {
            TileTypes.Grass,
            TileTypes.Trees,
            TileTypes.Hills,
            TileTypes.Mountains,
            TileTypes.Towns
        };

        foreach (Tile tile in LandTiles)
        {
            if (tile.featureType.HasValue)
            {
                continue;
            }

            int index = Random.Range(0, randomTypes.Length);
            tile.featureType = randomTypes[index];
        }
    }
}
