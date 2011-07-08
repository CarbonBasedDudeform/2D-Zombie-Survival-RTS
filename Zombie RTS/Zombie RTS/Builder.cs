using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zombie_RTS
{
    class Builder : Unit
    {
        public Builder(float X, float Y, int Width, int Height)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, 25, 25);
            m_origin = new Vector2(Width / 2, Height / 2);
        }

        private static Resources requiredResources = new Resources(100, 500, 200, 100);

        public static Resources getRequiredResources()
        {
            return requiredResources;
        }

        protected static float m_creationTime = 15;

        public static float getCreationTime()
        {
            return m_creationTime;
        }

        public static void setCreationTime(float time)
        {
            m_creationTime = time;
        }

        private TimeSpan consumedTime = new TimeSpan();
        private TimeSpan displTime = new TimeSpan();
        //private TimeSpan oneSecond = new TimeSpan(0, 0, 1);

        public void update(KeyboardState keys, GameTime time)
        {
            //go through list
            //if one nearby go to it and chop it down
            //mark down index and remove from list after chop down
            if (m_selected)
            {
                if (!keys.IsKeyDown(Keys.R))
                {
                    if (keys.IsKeyDown(Keys.W))
                    {
                        if (RTSManager.curResources >= Wall.getRequiredResources())
                        {
                            performAction(time, BuildType.Wall);
                            curBulid = BuildType.Wall;

                            consumedTime = new TimeSpan();
                        }
                    }
                    else if (keys.IsKeyDown(Keys.S))
                    {
                        if (RTSManager.curResources >= SentryGun.getRequiredResources())
                        {
                            performAction(time, BuildType.SentryGun);
                            curBulid = BuildType.SentryGun;

                            consumedTime = new TimeSpan();
                        }
                    }
                    else if (keys.IsKeyDown(Keys.H))
                    {
                        if (RTSManager.curResources >= Hub.getRequiredResources())
                        {
                            performAction(time, BuildType.Hub);
                            curBulid = BuildType.Hub;

                            consumedTime = new TimeSpan();
                        }
                    }
                }

                if (keys.IsKeyDown(Keys.R) && keys.IsKeyDown(Keys.H))
                {
                    curDispType = BuildType.Hub;
                    displTime = new TimeSpan();
                }
                else if (keys.IsKeyDown(Keys.R) && keys.IsKeyDown(Keys.S))
                {
                    curDispType = BuildType.SentryGun;
                    displTime = new TimeSpan();
                }
                else if (keys.IsKeyDown(Keys.R) && keys.IsKeyDown(Keys.W))
                {
                    curDispType = BuildType.Wall;
                    displTime = new TimeSpan();
                }
            }

            if (curBulid != BuildType.Nothing)
            {
                performAction(time, curBulid);
            }

            if (consumedTime.Seconds > 30)
            {
                consumedTime = new TimeSpan();

                if (RTSManager.curResources.Food > 0 && m_unitHealth > 0)
                {
                    RTSManager.curResources.Food--;
                }
                else
                {
                    m_unitHealth--;
                }

                if (RTSManager.curResources.Water > 0 && m_unitHealth > 0)
                {
                    RTSManager.curResources.Water--;
                }
                else
                {
                    m_unitHealth--;
                }
            }

            consumedTime += time.ElapsedGameTime;

            if (consumedTime.Milliseconds > 900)
            {
                consumedTime.Add(oneSecond);
            }

            displTime += time.ElapsedGameTime;

            if (displTime.Milliseconds > 900)
            {
                displTime.Add(oneSecond);
            }

            if (displTime.Seconds > 5)
            {
                curDispType = BuildType.Nothing;
            }
        }

        private TimeSpan elapsedTime = new TimeSpan();
        const int WORK_RATE = 5;

        public enum BuildType
        {
            Wall,
            SentryGun,
            Hub,
            Nothing
        }

        BuildType curBulid = BuildType.Nothing;
        private TimeSpan oneSecond = new TimeSpan(0, 0, 1);
        private int buildTime = 60;

        public void performAction(GameTime time, BuildType typeToBuild)
        {
            //base.performAction();

            switch (typeToBuild)
            {
                case BuildType.Wall:
                    buildTime = 20;
                    break;
                case BuildType.SentryGun:
                    buildTime = 10;
                    break;
                case BuildType.Hub:
                    buildTime = 10;
                    break;
            }

            if (elapsedTime.Seconds > buildTime)
            {
                elapsedTime = elapsedTime.Subtract(elapsedTime);

                switch (typeToBuild)
                {
                    case BuildType.Wall:
                        RTSManager.createWall(m_position.X, m_position.Y, m_rotation);
                        curBulid = BuildType.Nothing;
                        break;
                    case BuildType.SentryGun:
                        RTSManager.createSentryGun(m_position.X, m_position.Y);
                        curBulid = BuildType.Nothing;
                        break;
                    case BuildType.Hub:
                        RTSManager.createHub(m_position.X, m_position.Y);
                        curBulid = BuildType.Nothing;
                        break;
                }
            }

            elapsedTime += time.ElapsedGameTime;

            if (elapsedTime.Milliseconds > 900)
            {
                elapsedTime.Add(oneSecond);
            }
        }

        protected override void drawDisplayInfo(SpriteBatch sb)
        {
            base.drawDisplayInfo(sb);

            sb.DrawString(m_font, "Build Progress: " + (((float)elapsedTime.Seconds / (float)buildTime) * 100) + "%", new Vector2(m_displayArea.Width - 300, m_displayTextPos.Y), Color.White);
        }

        BuildType curDispType = BuildType.Nothing;

        public override void draw(SpriteBatch sb)
        {
            base.draw(sb);

            sb.Begin();

            Vector2 dispVec = new Vector2(200, 0);
            switch (curDispType)
            {
                case BuildType.Hub:
                    sb.DrawString(m_font, "Hub Requirements\n" + Hub.getRequiredResources().ToString(), dispVec, Color.White);
                    break;
                case BuildType.Wall:
                    sb.DrawString(m_font, "Wall Requirements\n" + Wall.getRequiredResources().ToString(), dispVec, Color.White);
                    break;
                case BuildType.SentryGun:
                    sb.DrawString(m_font, "Sentry Gun Requirements\n" + SentryGun.getRequiredResources().ToString(), dispVec, Color.White);
                    break;
                default:
                    break;
            }

            sb.End();
        }
    }
}
