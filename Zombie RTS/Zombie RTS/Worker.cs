using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zombie_RTS
{
    class Worker : Unit
    {
        public Worker(float X, float Y, int Width, int Height)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, 25, 25);
            m_origin = new Vector2(Width / 2, Height / 2);
            SPEED = 5;
        }

        private static Resources requiredResources = new Resources(100, 500, 0, 0);

        public static Resources getRequiredResources()
        {
            return requiredResources;
        }

        protected static float m_creationTime = 5;

        public static float getCreationTime()
        {
            return m_creationTime;
        }

        public static void setCreationTime(float time)
        {
            m_creationTime = time;
        }

        public override void update(Vector2 MousePos, ButtonState btnState)
        {
            base.update(MousePos, btnState);
        }

        private TimeSpan consumedTime = new TimeSpan();

        public void update(List<Material> worldResources, GameTime time)
        {
            //go through list
            //if one nearby go to it and chop it down
            //mark down index and remove from list after chop down

            Rectangle scanArea = new Rectangle((int)m_position.X - 25, (int)m_position.Y - 25, 100, 100);

            foreach (Material curM in worldResources)
            {
                if (scanArea.Intersects(curM.getRect()))
                {
                    performAction(time, curM);
                    break;
                }
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
                consumedTime.Add(new TimeSpan(0, 0, 1));
            }
        }

        private TimeSpan elapsedTime = new TimeSpan();
        private TimeSpan oneSecond = new TimeSpan(0, 0, 1);
        const int WORK_RATE = 2;

        public void performAction(GameTime time, Material mat)
        {
            //base.performAction();

            if (elapsedTime.Seconds > WORK_RATE)
            {
                elapsedTime = elapsedTime.Subtract(elapsedTime);
                mat.doDamage(20);
                m_rotation = (float)Math.Atan2(m_position.X - mat.X, mat.Y - m_position.Y);
            }

            elapsedTime += time.ElapsedGameTime;

            if (elapsedTime.Milliseconds > 900)
            {
                elapsedTime.Add(oneSecond);
            }
        }
    }
}
