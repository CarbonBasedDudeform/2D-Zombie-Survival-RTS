using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Threading;

namespace Zombie_RTS
{
    class RTSManager
    {
        const int PEOPLE_SIZE = 25;

        private static List<Worker> workers = new List<Worker>();
        private static List<Builder> builders = new List<Builder>();
        private static List<Maintainer> maintainers = new List<Maintainer>();
        private static List<Soldier> soldiers = new List<Soldier>();

        private static List<Hub> hubs = new List<Hub>();
        private static List<Wall> walls = new List<Wall>();
        private static List<SentryGun> sentryguns = new List<SentryGun>();

        private static List<Material> worldResources = new List<Material>();
        private static List<Unit> buildings = new List<Unit>();

        private static List<Zombie> zombies = new List<Zombie>();

        private static List<Unit> units = new List<Unit>();

        private static List<Unit> zombieTargets = new List<Unit>();

        private static Random rand = new Random(DateTime.Now.Millisecond);

        public static void generateWorld(int numOfTrees, int numOfRock, int numOfFood, int numOfWater)
        {
            for (int i = 0; i < 20; i++)
            {
                generateTreeCluster(numOfTrees);
                generateRocks(numOfRock);
                generateFood(numOfFood);
                generateWater(numOfWater);
            }
        }

        private static int m_worldWidth, m_worldHeight;
        private static Rectangle world;

        public static void setWorld(int Width, int Height)
        {
            m_worldWidth = Width;
            m_worldHeight = Height;
            world = new Rectangle(0, 0, Width, Height);
        }

        private static void generateTreeCluster(int numOfTrees)
        {
            Material curMat;
            Rectangle range = new Rectangle(rand.Next(0, m_worldWidth), rand.Next(0, m_worldHeight), 100, 100);

            for (int i = 0; i < numOfTrees; i++)
            {
                curMat = new Material(rand.Next((int)range.X, (int)range.X + range.Width), rand.Next((int)range.Y, (int)range.Y + range.Height), 20, 20, Material.type.Wood);
                curMat.setTexture(m_content.Load<Texture2D>("tree"));
                curMat.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
                curMat.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
                curMat.setDisplayText("Can't see this");
                worldResources.Add(curMat);
                units.Add(curMat);
            }
        }

        private static void generateRocks(int numOfRocks)
        {
            Material curMat;
            Rectangle range = new Rectangle(rand.Next(0, m_worldWidth), rand.Next(0, m_worldHeight), 100, 100);

            for (int i = 0; i < numOfRocks; i++)
            {
                curMat = new Material(rand.Next((int)range.X, (int)range.X + range.Width), rand.Next((int)range.Y, (int)range.Y + range.Height), 20, 20, Material.type.Metal);
                curMat.setTexture(m_content.Load<Texture2D>("rock"));
                curMat.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
                curMat.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
                curMat.setDisplayText("Can't see this");
                worldResources.Add(curMat);
                units.Add(curMat);
            }
        }

        private static void generateFood(int numOfFood)
        {
            Material curMat;
            Rectangle range = new Rectangle(rand.Next(0, m_worldWidth), rand.Next(0, m_worldHeight), 100, 100);

            for (int i = 0; i < numOfFood; i++)
            {
                curMat = new Material(rand.Next((int)range.X, (int)range.X + range.Width), rand.Next((int)range.Y, (int)range.Y + range.Height), 50, 50, Material.type.Food);
                curMat.setTexture(m_content.Load<Texture2D>("food"));
                curMat.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
                curMat.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
                curMat.setDisplayText("Can't see this");
                worldResources.Add(curMat);
                units.Add(curMat);
            }
        }

        private static void generateWater(int numOfWater)
        {
            Material curMat;
            Rectangle range = new Rectangle(rand.Next(0, m_worldWidth), rand.Next(0, m_worldHeight), 100, 100);

            for (int i = 0; i < numOfWater; i++)
            {
                curMat = new Material(rand.Next((int)range.X, (int)range.X + range.Width), rand.Next((int)range.Y, (int)range.Y + range.Height), 50, 50, Material.type.Water);
                curMat.setTexture(m_content.Load<Texture2D>("water"));
                curMat.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
                curMat.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
                curMat.setDisplayText("Can't see this");

                worldResources.Add(curMat);
                units.Add(curMat);
            }
        }

        private static void generateZombies(int numOfZombs)
        {
            Zombie curZombie;
            Rectangle range = new Rectangle(rand.Next(0, m_worldWidth), rand.Next(0, m_worldHeight), 100, 100);

            for (int i = 0; i < numOfZombs; i++)
            {
                curZombie = new Zombie(rand.Next((int)range.X, (int)range.X + range.Width), rand.Next((int)range.Y, (int)range.Y + range.Height), 25, 25);
                curZombie.setTexture(m_content.Load<Texture2D>("zombie"));
                curZombie.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
                curZombie.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
                curZombie.setDisplayText("Can't see this");

                zombies.Add(curZombie);
                //units.Add(curZombie); //possibly add in to make zombies attack each other?

                Thread.Sleep(10);
            }
        }

        private static Texture2D m_texture;

        public static void setBackground(Texture2D texture)
        {
            m_texture = texture;
        }

        private static GameTime gameTime;
        private static TimeSpan elapsedTime;
        private static TimeSpan oneSecond = new TimeSpan(0, 0, 1);

        public void update(Vector2 mousePos, ButtonState LbtnState, ButtonState RbtnState, KeyboardState keyState, GameTime time)
        {
            gameTime = time;

            elapsedTime += time.ElapsedGameTime;

            if (elapsedTime.Milliseconds > 900)
            {
                elapsedTime.Add(oneSecond);
                incScore();
            }

            if (elapsedTime.Minutes > GameOptions.getZombiesAddInterval())
            {
                generateZombies(GameOptions.getNumOfZombiesToAdd());
                elapsedTime = elapsedTime.Subtract(elapsedTime);
            }

            for (int i = 0; i < units.Count; i++)
            {
                units[i].update(mousePos, LbtnState);

                if (units[i].getHealth() <= 0)
                {
                    units.RemoveAt(i);
                }

                if (RbtnState == ButtonState.Pressed)
                {
                    units[i].deselectUnit();
                }
            }
            
            for (int i = 0; i < soldiers.Count; i++)
            {
                soldiers[i].update(zombies, time);

                if (soldiers[i].getHealth() <= 0)
                {
                    soldiers.RemoveAt(i);
                }
            }

            for (int i = 0; i < workers.Count; i++)
            {
                workers[i].update(worldResources, time);

                if (workers[i].getHealth() <= 0)
                {
                    workers.RemoveAt(i);
                }
            }

            for (int i = 0; i < builders.Count; i++)
            {
                builders[i].update(keyState, time);

                if (builders[i].getHealth() <= 0)
                {
                    builders.RemoveAt(i);
                }
            }

            for (int i = 0; i < maintainers.Count; i++)
            {
                maintainers[i].update(buildings, time);

                if (maintainers[i].getHealth() <= 0)
                {
                    maintainers.RemoveAt(i);
                }
            }

            

            if (hubs.Count <= 0)
            {
                GameOptions.gameOver();
            }
            else
            {
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].update(mousePos, LbtnState, keyState, time);

                    if (hubs[i].getHealth() <= 0)
                    {
                        hubs.RemoveAt(i);
                    }
                }
            }

            for (int i = 0; i < zombies.Count; i++)
            {
                if (zombieTargets.Count > 0)
                {
                    zombies[i].update(zombieTargets, time);
                }
                else
                {
                    zombies[i].update(units, time);
                }

                if (zombies[i].getHealth() <= 0)
                {
                    zombies.RemoveAt(i);
                }
            }

            for (int i = 0; i < zombieTargets.Count; i++)
            {
                if (zombieTargets[i].getHealth() <= 0)
                {
                    zombieTargets.RemoveAt(i);
                }
            }

            for (int i = 0; i < sentryguns.Count; i++)
            {
                sentryguns[i].update(zombies, time);

                if (sentryguns[i].getHealth() <= 0)
                {
                    sentryguns.RemoveAt(i);
                }
            }

            for (int i = 0; i < worldResources.Count; i++)
            {
                if (worldResources[i].getHealth() <= 0)
                {
                    if (worldResources[i].getType() == Material.type.Wood)
                    {
                        curResources.Wood += 50;
                    }
                    else if (worldResources[i].getType() == Material.type.Metal)
                    {
                        curResources.Metal += 50;
                    }
                    else if (worldResources[i].getType() == Material.type.Food)
                    {
                        curResources.Food += 50;
                    }
                    else if (worldResources[i].getType() == Material.type.Water)
                    {
                        curResources.Water += 50;
                    }

                    worldResources.RemoveAt(i);
                }
            }
        }

        private static SpriteFont m_font;
        private static Vector2 zombiePop = new Vector2(GameOptions.getWidth() - 300, 0);

        public static void setFont(SpriteFont font)
        {
            m_font = font;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Begin();
                sb.Draw(m_texture, world, Color.White);
            sb.End();

            foreach (Unit curUnit in units)
            {
                curUnit.draw(sb);
            }
            
            foreach (Zombie zomb in zombies)
            {
                zomb.draw(sb);
            }

            zombiePop.X = GameOptions.getWidth() - 300;

            sb.Begin();
            sb.DrawString(m_font, "Zombie Population: " + zombies.Count + "\nScore: " + m_score, zombiePop, Color.White);
            sb.End();
            
        }

        public static void createWorker(float X, float Y)
        {
            Worker newWorker = new Worker(X, Y, PEOPLE_SIZE, PEOPLE_SIZE);

            newWorker.setTexture(m_content.Load<Texture2D>("worker"));
            newWorker.setDisplayTexture(m_content.Load<Texture2D>("displayInfo"));
            newWorker.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            newWorker.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
            newWorker.setDisplayText("Worker:\nMove to a location and the worker shall start gathering any nearby materials\nsuch as metal, wood, food or water");

            curResources = curResources - Worker.getRequiredResources();

            workers.Add(newWorker);
            units.Add(newWorker);
            zombieTargets.Add(newWorker);
        }

        public static void createBuilder(float X, float Y)
        {
            Builder newBuilder = new Builder(X, Y, PEOPLE_SIZE, PEOPLE_SIZE);

            newBuilder.setTexture(m_content.Load<Texture2D>("builder"));
            newBuilder.setDisplayTexture(m_content.Load<Texture2D>("displayInfo"));
            newBuilder.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            newBuilder.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
            newBuilder.setDisplayText("Builder:\nW - Build Wall at current location     H - Build Hub at current location\nS - Build Sentry Gun at current location");

            curResources = curResources - Builder.getRequiredResources();

            builders.Add(newBuilder);
            units.Add(newBuilder);
            zombieTargets.Add(newBuilder);
        }

        public static void createMaintainer(float X, float Y)
        {
            Maintainer newMaintainer = new Maintainer(X, Y, PEOPLE_SIZE, PEOPLE_SIZE);

            newMaintainer.setTexture(m_content.Load<Texture2D>("maintainer"));
            newMaintainer.setDisplayTexture(m_content.Load<Texture2D>("displayInfo"));
            newMaintainer.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            newMaintainer.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
            newMaintainer.setDisplayText("Maintainer:\nMove to the location of a wall, sentry gun or hub that needs repairing\nand he will repair it");

            curResources = curResources - Maintainer.getRequiredResources();

            maintainers.Add(newMaintainer);
            units.Add(newMaintainer);
            zombieTargets.Add(newMaintainer);
        }

        public static void createSoldier(float X, float Y)
        {
            Soldier newSoldier = new Soldier(X , Y, PEOPLE_SIZE, PEOPLE_SIZE);

            newSoldier.setTexture(m_content.Load<Texture2D>("soldier"));
            newSoldier.setDisplayTexture(m_content.Load<Texture2D>("displayInfo"));
            newSoldier.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            newSoldier.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
            newSoldier.setDisplayText("Soldier:\nMove to a location and this soldier will attack any zombies that come near him");

            curResources = curResources - Soldier.getRequiredResources();
            soldiers.Add(newSoldier);
            units.Add(newSoldier);
            zombieTargets.Add(newSoldier);
        }

        const int HUB_SIZE = 100;

        public static void createHub(float X, float Y)
        {
            Hub newHub = new Hub(X, Y, HUB_SIZE, HUB_SIZE);

            newHub.setTexture(m_content.Load<Texture2D>("hub"));
            newHub.setDisplayTexture(m_content.Load<Texture2D>("displayInfo"));
            newHub.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            newHub.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
            newHub.setDisplayText("W - create worker     B - create builder\nS - create soldier    M - create maintainer");

            curResources = curResources - Hub.getRequiredResources();

            hubs.Add(newHub);
            units.Add(newHub);
            buildings.Add(newHub);
            zombieTargets.Add(newHub);
        }

        public static void createSentryGun(float X, float Y)
        {
            SentryGun newSG = new SentryGun(X, Y, m_content.Load<Texture2D>("bittybullet"));

            newSG.setTexture(m_content.Load<Texture2D>("sentryTop"));
            newSG.setBaseTexture(m_content.Load<Texture2D>("sentryBase"));
            newSG.setDisplayTexture(m_content.Load<Texture2D>("displayInfo"));
            newSG.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            newSG.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
            newSG.setDisplayText("Sentry Gun");

            curResources = curResources - Wall.getRequiredResources();

            sentryguns.Add(newSG);
            units.Add(newSG);
            buildings.Add(newSG);
            zombieTargets.Add(newSG);
        }

        public static void createWall(float X, float Y, float Rotation)
        {
            Wall newWall = new Wall(X, Y, Rotation);

            newWall.setTexture(m_content.Load<Texture2D>("wall"));
            newWall.setDisplayTexture(m_content.Load<Texture2D>("displayInfo"));
            newWall.setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            newWall.setDisplayArea(new Rectangle(0, m_windowHeight - 100, m_windowWidth, 100));
            newWall.setDisplayText("Wall");

            curResources = curResources - Wall.getRequiredResources();

            walls.Add(newWall);
            units.Add(newWall);
            buildings.Add(newWall);
            zombieTargets.Add(newWall);
        }

        private static int m_windowWidth, m_windowHeight;

        public void setWindowLimits(int Width, int Height)
        {
            m_windowWidth = Width;
            m_windowHeight = Height;
        }

        private static ContentManager m_content;

        public void setContentManger(ContentManager content)
        {
            m_content = content;
        }

        static RTSManager instance = null;

        public static RTSManager Instance(int WindowWidth, int WindowHeight, ContentManager content)
        {
            instance = new RTSManager();
            instance.setContentManger(content);
            instance.setWindowLimits(WindowWidth, WindowHeight);

            return instance;
        }

        public static Resources curResources = GameOptions.getStartResources();

        private int m_cameraSpeed = 5;

        public void setCameraSpeed(int amount)
        {
            m_cameraSpeed = amount;
        }

        public void moveWorldLeft()
        {
            if (world.X < -15)
            {
                foreach (Unit curUnit in units)
                {
                    curUnit.X += m_cameraSpeed;
                    curUnit.moveCurDestLeft(m_cameraSpeed);
                }

                foreach (Zombie curUnit in zombies)
                {
                    curUnit.X += m_cameraSpeed;
                    curUnit.moveCurDestLeft(m_cameraSpeed);
                }

                foreach (SentryGun sg in sentryguns)
                {
                    sg.moveBulletsLeft(m_cameraSpeed);
                }

                world.X += m_cameraSpeed;
            }
        }

        public void moveWorldRight()
        {
            if ((world.X - m_windowWidth - 15) > -m_worldWidth)
            {
                foreach (Unit curUnit in units)
                {
                    curUnit.X -= m_cameraSpeed;
                    curUnit.moveCurDestRight(m_cameraSpeed);
                }

                foreach (Zombie curUnit in zombies)
                {
                    curUnit.X -= m_cameraSpeed;
                    curUnit.moveCurDestRight(m_cameraSpeed);
                }

                foreach (SentryGun sg in sentryguns)
                {
                    sg.moveBulletsRight(m_cameraSpeed);
                }

                world.X -= m_cameraSpeed;
            }
        }

        public void moveWorldUp()
        {
            if (world.Y < -15)
            {
                foreach (Unit curUnit in units)
                {
                    curUnit.Y += m_cameraSpeed;
                    curUnit.moveCurDestUp(m_cameraSpeed);
                }

                foreach (Zombie curUnit in zombies)
                {
                    curUnit.Y += m_cameraSpeed;
                    curUnit.moveCurDestUp(m_cameraSpeed);
                }

                foreach (SentryGun sg in sentryguns)
                {
                    sg.moveBulletsUp(m_cameraSpeed);
                }

                world.Y += m_cameraSpeed;
            }
        }

        public void moveWorldDown()
        {
            if ((world.Y - m_windowHeight - 25) > -m_worldHeight)
            {
                foreach (Unit curUnit in units)
                {
                    curUnit.Y -= m_cameraSpeed;
                    curUnit.moveCurDestDown(m_cameraSpeed);
                }

                foreach (Zombie curUnit in zombies)
                {
                    curUnit.Y -= m_cameraSpeed;
                    curUnit.moveCurDestDown(m_cameraSpeed);
                }

                foreach (SentryGun sg in sentryguns)
                {
                    sg.moveBulletsDown(m_cameraSpeed);
                }

                world.Y -= m_cameraSpeed;
            }
        }

        public static int getWorldWidth()
        {
            return world.Width;
        }

        public static int getWorldHeight()
        {
            return world.Height;
        }

        public static void NewGame()
        {
            workers = new List<Worker>();
            builders = new List<Builder>();
            maintainers = new List<Maintainer>();
            soldiers = new List<Soldier>();

            hubs = new List<Hub>();
            walls = new List<Wall>();
            sentryguns = new List<SentryGun>();

            worldResources = new List<Material>();
            buildings = new List<Unit>();

            zombies = new List<Zombie>();
            units = new List<Unit>();
            zombieTargets = new List<Unit>();

            rand = new Random(DateTime.Now.Millisecond);

            setFont(m_content.Load<SpriteFont>("displayInfoFont"));
            setWorld(5000, 5000);
            setBackground(m_content.Load<Texture2D>("terrain"));

            curResources = GameOptions.getStartResources();

            generateWorld(10, 20, 5, 125);
            createHub(300, 300);
            m_score = 0;
            GameOptions.startGame();
        }

        private static int m_score = 0;

        public static void incScore()
        {
            switch (GameOptions.getDifficulty())
            {
                case GameOptions.GameDifficulties.Easy:
                    m_score++;
                    break;
                case GameOptions.GameDifficulties.Medium:
                    m_score += 2;
                    break;
                case GameOptions.GameDifficulties.Hard:
                    m_score += 5;
                    break;
                default:
                    m_score++;
                    break;
            }
        }

        public static int getScore()
        {
            return m_score;
        }

        public static void REMOVEME()
        {
            generateZombies(1);
        }
    }
}
