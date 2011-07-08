using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombie_RTS
{
    class GameOptions
    {
        private static int m_screenWidth = 1920;
        private static int m_screenHeight = 1080;

        public static void setWidth(int width)
        {
            m_screenWidth = width;
            m_graphics.PreferredBackBufferWidth = m_screenWidth;
            m_graphics.ApplyChanges();
        }

        public static void setHeight(int height)
        {
            m_screenHeight = height;
            m_graphics.PreferredBackBufferHeight = m_screenHeight;
            m_graphics.ApplyChanges();
        }

        public static void setScreen(int width, int height)
        {
            setHeight(height);
            setWidth(width);
        }

        public static int getWidth()
        {
            return m_screenWidth;
        }

        public static int getHeight()
        {
            return m_screenHeight;
        }

        public enum GameDifficulties
        {
            Easy,
            Medium,
            Hard
        }

        private static GameDifficulties curDifficulty = GameDifficulties.Easy;

        public static void setDifficulty(GameDifficulties newDifficulty)
        {
            curDifficulty = newDifficulty;
        }

        public static GameDifficulties getDifficulty()
        {
            return curDifficulty;
        }

        private GameOptions()
        {
        }

        public static GameOptions getInstance()
        {
            return new GameOptions();
        }

        public static int getNumOfZombiesToAdd()
        {
            switch (curDifficulty)
            {
                case GameDifficulties.Easy:
                    return 10;
                case GameDifficulties.Medium:
                    return 50;
                case GameDifficulties.Hard:
                    return 100;
            }

            return 10;
        }

        public static int getZombiesAddInterval()
        {
            switch (curDifficulty)
            {
                case GameDifficulties.Easy:
                    return 5;
                case GameDifficulties.Medium:
                    return 2;
                case GameDifficulties.Hard:
                    return 2;
            }

            return 10;
        }

        public static int getZombieStrength()
        {
            switch (curDifficulty)
            {
                case GameDifficulties.Easy:
                    return 25;
                case GameDifficulties.Medium:
                    return 75;
                case GameDifficulties.Hard:
                    return 100;
            }

            return 5;
        }

        private static GraphicsDeviceManager m_graphics;

        public static void setGraphics(GraphicsDeviceManager graphics)
        {
            m_graphics = graphics;
        }

        public static void toggleFullScreen()
        {
            m_graphics.ToggleFullScreen();
        }

        public static Resources getStartResources()
        {
            switch (curDifficulty)
            {
                case GameDifficulties.Easy:
                    return new Resources(5100, 5100, 5300, 5300);
                case GameDifficulties.Medium:
                    return new Resources(2100, 2100, 1300, 1300);
                case GameDifficulties.Hard:
                    return new Resources(1100, 1100, 800, 800);
            }

            return new Resources(10000, 10000, 10000, 10000);
        }

        public static bool m_gameOver = false;

        public static bool isGameOver()
        {
            return m_gameOver;
        }

        public static void gameOver()
        {
            m_gameOver = true;
            m_gamePlaying = false;
        }

        private static bool m_gamePlaying = false;

        public static void startGame()
        {
            m_gamePlaying = true;
            m_gameOver = false;
        }

        public static bool isGamePlaying()
        {
            return m_gamePlaying;
        }
    }
}
