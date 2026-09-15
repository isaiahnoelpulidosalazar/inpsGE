using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace inpsGE
{
    public class Core
    {
        static int SCREEN_WIDTH, SCREEN_HEIGHT;
        public const int TILE_SIZE = 32, VERY_SHORT = 5, SHORT = 15, MEDIUM = 30, LONG = 75;

        static GraphicsDevice GraphicsDevice;

        static Texture2D MessageBackground;
        static Texture2D WhiteTexture;
        static RenderTarget2D LightMask;

        static BlendState LightCutoutBlend = new BlendState
        {
            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };

        static List<GameMap> Maps = new List<GameMap>();
        static List<GameScenario> Scenarios = new List<GameScenario>();
        static GameMap CurrentGameMap;
        static GameScenario CurrentGameScenario;

        static List<GameTimer> GameTimers = new List<GameTimer>();

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine))
            {
                throw new InvalidOperationException("The Core.Initialize() method can only be called from the Engine class.");
            }

            SCREEN_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            SCREEN_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            GraphicsDevice = graphicsDevice;

            MessageBackground = new Texture2D(GraphicsDevice, 1, 1);
            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            MessageBackground.SetData(new[] { Color.Black });
            WhiteTexture.SetData(new[] { Color.White });

            LightMask = new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);
        }

        public static int GetScreenWidth()
        {
            return SCREEN_WIDTH;
        }

        public static int GetScreenHeight()
        {
            return SCREEN_HEIGHT;
        }

        public static GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }

        public static Texture2D GetMessageBackground()
        {
            return MessageBackground;
        }

        public static Texture2D GetWhiteTexture()
        {
            return WhiteTexture;
        }

        public static RenderTarget2D GetLightMask()
        {
            return LightMask;
        }

        public static BlendState GetLightCutoutBlend()
        {
            return LightCutoutBlend;
        }

        public static void AddMap(GameMap Map)
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine) && EngineCheck != typeof(TitleScreen) && EngineCheck?.DeclaringType != typeof(TitleScreen))
            {
                throw new InvalidOperationException("The Core.AddMap() method can only be called from the Engine and TitleScreen class.");
            }

            Maps.Add(Map);
        }

        public static void ChangeGameMap(string Name)
        {
            CurrentGameMap = Maps.Find(Map => Map.GetName() == Name);
        }

        public static void DrawGameMap(ContentManager Content, SpriteBatch _spriteBatch, Entity Entity)
        {
            for (int a = 0; a < CurrentGameMap.GetTiles().Count; a++)
            {
                GameTile Tile = CurrentGameMap.GetTiles()[a];
                
                if (Tile.GetRoomID().Count <= 0)
                {
                    _spriteBatch.Draw(Tile.GetImage(), new Vector2(Tile.GetPositionX(), Tile.GetPositionY()), Color.White);
                }
                else
                {
                    if (Tile.GetRoomID().Contains(Entity.CurrentRoomID))
                    {
                        _spriteBatch.Draw(Tile.GetImage(), new Vector2(Tile.GetPositionX(), Tile.GetPositionY()), Color.White);
                    }
                }
            }

            for (int a = 0; a < CurrentGameMap.GetObjects().Count; a++)
            {
                GameObject Object = CurrentGameMap.GetObjects()[a];
                
                if (Object.GetRoomID().Count <= 0)
                {
                    _spriteBatch.Draw(Object.GetImage(), new Vector2(Object.GetPositionX(), Object.GetPositionY()), Color.White);
                }
                else
                {
                    if (Object.GetRoomID().Contains(Entity.CurrentRoomID))
                    {
                        _spriteBatch.Draw(Object.GetImage(), new Vector2(Object.GetPositionX(), Object.GetPositionY()), Color.White);
                    }
                }
            }
        }

        public static void AddScenario(GameScenario Scenario)
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine) && EngineCheck != typeof(TitleScreen) && EngineCheck?.DeclaringType != typeof(TitleScreen))
            {
                throw new InvalidOperationException("The Core.AddScenario() method can only be called from the Engine and TitleScreen class.");
            }

            Scenarios.Add(Scenario);
        }

        public static void ChangeGameScenario(string Name)
        {
            CurrentGameScenario = Scenarios.Find(Scenario => Scenario.GetName() == Name);
            CurrentGameScenario.Load();
        }

        public static GameMap GetCurrentGameMap()
        {
            return CurrentGameMap;
        }

        public static GameScenario GetCurrentGameScenario()
        {
            return CurrentGameScenario;
        }

        public static GameObject GetGameObject(int Index)
        {
            foreach (GameObject Object in CurrentGameMap.GetObjects())
            {
                if (Object.GetIndex() == Index)
                {
                    return Object;
                }
            }
            return null;
        }

        public static void DestroyGameObject(int Index)
        {
            foreach (GameObject Object in CurrentGameMap.GetObjects())
            {
                if (Object.GetIndex() == Index)
                {
                    CurrentGameMap.RemoveObject(Object);
                    break;
                }
            }
        }

        public static void AddGlobalGameTimer(GameTimer GameTimer)
        {
            GameTimers.Add(GameTimer);
        }

        public static void GlobalGameTimerUpdate(GameTime gameTime)
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine))
            {
                throw new InvalidOperationException("The Core.GlobalGameTimerUpdate() method can only be called from the Engine class.");
            }

            for (int a = 0; a < GameTimers.Count; a++)
            {
                if (GameTimers[a].IsActive)
                {
                    GameTimers[a].Update(gameTime);
                }
                else
                {
                    GameTimers.RemoveAt(a);
                }
            }
        }

        public static void SetGameObjectLightLevel(int Index, int LightLevel)
        {
            foreach (GameObject Object in CurrentGameMap.GetObjects())
            {
                if (Object.GetIndex() == Index)
                {
                    Object.SetLightLevel(LightLevel);
                    break;
                }
            }
        }

        public static void SetGameObjectEvent(int Index, Action Event)
        {
            foreach (GameObject Object in CurrentGameMap.GetObjects())
            {
                if (Object.GetIndex() == Index)
                {
                    Object.SetEvent(Event);
                    break;
                }
            }
        }

        public static void SetAllGameObjectsLightLevel(int LightLevel)
        {
            foreach (GameObject Object in CurrentGameMap.GetObjects())
            {
                Object.SetLightLevel(LightLevel);
            }
        }

        public static void SetAllGameObjectsEvent(Action Event)
        {
            foreach (GameObject Object in CurrentGameMap.GetObjects())
            {
                Object.SetEvent(Event);
            }
        }
    }
}
