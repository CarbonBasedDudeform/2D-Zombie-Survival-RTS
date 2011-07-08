using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zombie_RTS
{
    class Maintainer : Unit
    {
        public Maintainer(float X, float Y, int Width, int Height)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, 25, 25);
            m_origin = new Vector2(Width / 2, Height / 2);
            m_scanArea = new Rectangle((int)m_position.X - 25, (int)m_position.Y - 25, SCAN_RADIUS * 4, SCAN_RADIUS * 4);
            SPEED = 4;
        }

        private static Resources requiredResources = new Resources(100, 500, 100, 50);

        public static Resources getRequiredResources()
        {
            return requiredResources;
        }

        protected static float m_creationTime = 20;

        public static float getCreationTime()
        {
            return m_creationTime;
        }

        public static void setCreationTime(float time)
        {
            m_creationTime = time;
        }

        private TimeSpan consumedTime = new TimeSpan();

        Rectangle m_scanArea;
        const int SCAN_RADIUS = 100;

        public void update(List<Unit> worldResources, GameTime time)
        {
            //go through list
            //if one nearby go to it and chop it down
            //mark down index and remove from list after chop down

            //Rectangle scanArea = new Rectangle((int)m_position.X - 25, (int)m_position.Y - 25, 100, 100);

            /*foreach (Unit curM in worldResources)
            {
                if (scanArea.Intersects(curM.getRect()))
                {
                    if (curM.getHealth() < 100)
                    {
                        performAction(time, curM);
                        break;
                    }
                }
            }*/

            Unit target = scan(worldResources);

            if (target != null)
            {
                m_rotation = (float)Math.Atan2(m_position.X - target.X, target.Y - m_position.Y);
                performAction(time, target);

                m_position.X -= (float)(m_speed * Math.Sin(m_rotation));
                m_position.Y += (float)(m_speed * Math.Cos(m_rotation));
            }
            else
            {
                m_rotation = (float)Math.Atan2(m_position.X - m_curDestination.X, m_curDestination.Y - m_position.Y);
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

        public Unit scan(List<Unit> targets)
        {

            m_scanArea.X = (int)m_position.X - SCAN_RADIUS;
            m_scanArea.Y = (int)m_position.Y - SCAN_RADIUS;

            foreach (Unit sub in targets)
            {
                if (m_scanArea.Intersects(sub.getRect()))
                {
                    if (sub.getHealth() < sub.getFullHealth())
                    {
                        return sub;
                    }
                }
            }

            return null;
        }

        private TimeSpan elapsedTime = new TimeSpan();
        private TimeSpan oneSecond = new TimeSpan(0, 0, 1);
        const int WORK_RATE = 2;

        public void performAction(GameTime time, Unit mat)
        {
            //base.performAction();

            if (elapsedTime.Seconds > WORK_RATE)
            {
                elapsedTime = elapsedTime.Subtract(elapsedTime);
                mat.repair(20);
                m_rotation = (float)Math.Atan2(m_position.X - mat.X, mat.Y - m_position.Y);
                move(new Vector2(mat.X, mat.Y));
            }

            elapsedTime += time.ElapsedGameTime;

            if (elapsedTime.Milliseconds > 900)
            {
                elapsedTime.Add(oneSecond);
            }
        }


    }
}
