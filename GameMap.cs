using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

            for (int a = 0; a < TileMap.Length; a++)
            {
                string[] temp = TileMap[a].Split(',');
                for (int b = 0; b < temp.Length; b++)
                {
                    string RawTile = temp[b];
                    int Index = Convert.ToInt32(RawTile.Split('[')[0]) - 1;
                    List<int> RoomID = GetRoomID(RawTile);

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
            int PreviousIndex = -1;
            for (int a = 0; a < ObjectMap.Length; a++)
            {
                string[] temp = ObjectMap[a].Split(',');

                for (int b = 0; b < temp.Length; b++)
                {
                    string RawTile = temp[b];
                    int Index = Convert.ToInt32(RawTile.Split('[')[0]) - 1;
                    List<int> RoomID = GetRoomID(RawTile);

                    if (Index >= 0)
                    {
                        if (Objects.Count <= 0)
                        {
                            Objects.Add(new GameObject(
                                Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Objects\\" + (ObjectImages[Index].Contains("!") ? ObjectImages[Index].Substring(0, ObjectImages[Index].Length - 1) : ObjectImages[Index]) + ".png"),
                                Core.TILE_SIZE * b,
                                Core.TILE_SIZE * a,
                                ObjectImages[Index].Contains("!"),
                                RoomID
                            ));
                        }
                        else
                        {
                            if (Index > PreviousIndex)
                            {
                                Objects.Add(new GameObject(
                                    Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Objects\\" + (ObjectImages[Index].Contains("!") ? ObjectImages[Index].Substring(0, ObjectImages[Index].Length - 1) : ObjectImages[Index]) + ".png"),
                                    Core.TILE_SIZE * b,
                                    Core.TILE_SIZE * a,
                                    ObjectImages[Index].Contains("!"),
                                    RoomID
                                ));
                            }
                            else
                            {
                                Objects.Insert(0, new GameObject(
                                    Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Objects\\" + (ObjectImages[Index].Contains("!") ? ObjectImages[Index].Substring(0, ObjectImages[Index].Length - 1) : ObjectImages[Index]) + ".png"),
                                    Core.TILE_SIZE * b,
                                    Core.TILE_SIZE * a,
                                    ObjectImages[Index].Contains("!"),
                                    RoomID
                                ));
                            }
                        }
                        PreviousIndex = Index;
                    }
                }
            }
        }

        List<int> GetRoomID(string Index)
        {
            List<int> temp = new List<int>();

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
                    temp.Add(Convert.ToInt32(Part));
                }
            }
            else
            {
                temp.Add(Convert.ToInt32(ID));
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
