using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace inpsGE
{
    public class GameMap
    {
        string Name;
        List<GameTile> Tiles = new List<GameTile>();
        List<GameObject> Objects = new List<GameObject>();

        public GameMap(string Name)
        {
            this.Name = Name;

            DirectoryInfo TileDirectoryInfo = new DirectoryInfo("Content\\Tiles");
            DirectoryInfo ObjectDirectoryInfo = new DirectoryInfo("Content\\Objects");

            string[] Map = File.ReadAllLines("Content\\Maps\\" + Name + ".igemap");
            string[] TileMap = Map.Skip(1).Take(Array.IndexOf(Map, "[OBJECTS]") - 1).ToArray();
            string[] ObjectMap = Map.Skip(Array.IndexOf(Map, "[OBJECTS]") + 1).ToArray();
            string[] TileImages = File.ReadAllLines("Content\\Maps\\" + Name + ".igetilemap");
            string[] ObjectImages = File.ReadAllLines("Content\\Maps\\" + Name + ".igeobjectmap");

            int TileGroupIndex = 0;
            var TileResultGroups = TileMap
                .Select(Line => new { NewLine = Line, Group = Line == "+" ? ++TileGroupIndex : TileGroupIndex })
                .Where(X => X.NewLine != "+")
                .GroupBy(X => X.Group)
                .Select(Group => Group.Select(X => X.NewLine).ToArray())
                .ToList();

            int ObjectGroupIndex = 0;
            var ObjectResultGroups = ObjectMap
                .Select(Line => new { NewLine = Line, Group = Line == "+" ? ++ObjectGroupIndex : ObjectGroupIndex })
                .Where(X => X.NewLine != "+")
                .GroupBy(X => X.Group)
                .Select(Group => Group.Select(X => X.NewLine).ToArray())
                .ToList();

            Debug.WriteLine(TileResultGroups.Count);

            foreach (string[] TileMapGroup in TileResultGroups)
            {
                for (int a = 0; a < TileMapGroup.Length; a++)
                {
                    string[] temp = TileMapGroup[a].Split(',');
                    for (int b = 0; b < temp.Length; b++)
                    {
                        string RawTile = temp[b];
                        int Index = Convert.ToInt32(RawTile.Split('[')[0]) - 1;
                        List<string> RoomID = GetRoomID(RawTile);

                        if (Index >= 0)
                        {
                            Tiles.Add(new GameTile(
                                Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Tiles\\" + (TileImages[Index].Contains("!") ? TileImages[Index].Substring(0, TileImages[Index].Length - 1) : TileImages[Index]) + ".png"),
                                Core.TILE_SIZE * b,
                                Core.TILE_SIZE * a,
                                TileImages[Index].Contains("!"),
                                RoomID
                            ));
                        }
                    }
                }
            }

            foreach (string[] ObjectMapGroup in ObjectResultGroups)
            {
                for (int a = 0; a < ObjectMapGroup.Length; a++)
                {
                    string[] temp = ObjectMapGroup[a].Split(',');

                    for (int b = 0; b < temp.Length; b++)
                    {
                        string RawObject = temp[b];
                        int Index = Convert.ToInt32(RawObject.Split('[')[0]) - 1;
                        List<string> RoomID = GetRoomID(RawObject);

                        if (Index >= 0)
                        {
                            Objects.Add(new GameObject(
                                Index,
                                Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Objects\\" + (ObjectImages[Index].Contains("!") ? ObjectImages[Index].Substring(0, ObjectImages[Index].Length - 1) : ObjectImages[Index]) + ".png"),
                                Core.TILE_SIZE * b,
                                Core.TILE_SIZE * a,
                                ObjectImages[Index].Contains("!"),
                                RoomID
                            ));
                        }
                    }
                }
            }
        }

        List<string> GetRoomID(string Index)
        {
            List<string> temp = new List<string>();

            if (!Index.Contains("["))
            {
                return temp;
            }

            string ID = Index.Substring(Index.IndexOf("[") + 1).Replace("]", "");
            
            if (ID.Contains("-"))
            {
                string[] Parts = ID.Split('-');
                foreach (string Part in Parts)
                {
                    temp.Add(Part);
                }
            }
            else
            {
                temp.Add(ID);
            }

            return temp;
        }

        public void RemoveObject(GameObject Object)
        {
            Objects.Remove(Object);
        }

        public void RemoveObject(int Index)
        {
            Objects.RemoveAt(Index);
        }

        public string GetName()
        {
            return Name;
        }

        public List<GameTile> GetTiles()
        {
            return Tiles;
        }

        public List<GameObject> GetObjects()
        {
            return Objects;
        }
    }
}
