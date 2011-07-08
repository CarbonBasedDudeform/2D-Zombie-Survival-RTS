using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombie_RTS
{
    class Zombie : Unit
    {
        Random rand = new Random(DateTime.Now.Millisecond);

        public Zombie(float X, float Y, int Width, int Height)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, 30, 30);
            m_origin = new Vector2(Width / 2, Height / 2);
            m_speed = 2;

            m_curDestination = new Vector2(0, 0);

            m_scanArea = new Rectangle((int)m_position.X - 50, (int)m_position.Y - 50, SCAN_RADIUS * 2, SCAN_RADIUS * 2);
        }

        public override void update(Vector2 MousePos, Microsoft.Xna.Framework.Input.ButtonState btnState)
        {
            //base.update(MousePos, btnState);
        }
        public void update(List<Unit> enemies, GameTime time)
        {
            Unit target = scan(enemies);

            m_rectangle.X = (int)m_position.X;
            m_rectangle.Y = (int)m_position.Y;

            if (target != null)
            {
                m_rotation = (float)Math.Atan2(m_position.X - target.X, target.Y - m_position.Y);
                performAction(time, target);
            }
            else
            {
                

                int mov = rand.Next(-100, 100);

                if (mov < 0)
                {
                    m_rotation -= 0.1f;
                }
                else
                {
                    m_rotation += 0.1f;
                }
            }

            if (!collisionDetection(enemies))
            {
                m_position.X -= (float)(m_speed * Math.Sin(m_rotation));
                m_position.Y += (float)(m_speed * Math.Cos(m_rotation));
            }
        }

        public bool collisionDetection(List<Unit> collideables)
        {
            foreach (Unit curU in collideables)
            {
                if (curU.getRect().Intersects(m_rectangle))
                {
                    return true;
                }
            }

            return false;
        }

        Rectangle m_scanArea;
        const int SCAN_RADIUS = 50;

        public Unit scan(List<Unit> targets)
        {

            m_scanArea.X = (int)m_position.X - SCAN_RADIUS;
            m_scanArea.Y = (int)m_position.Y - SCAN_RADIUS;

            foreach (Unit sub in targets)
            {
                if (m_scanArea.Intersects(sub.getRect()))
                {
                    return sub;
                }

            }

            return null;
        }

        protected override void drawDisplayInfo(SpriteBatch sb)
        {
            //base.drawDisplayInfo(sb);
        }

        private TimeSpan elapsedTime = new TimeSpan();
        private TimeSpan oneSecond = new TimeSpan(0, 0, 1);
        const int ATTACK_RATE = 5;

        public void performAction(GameTime time, Unit target)
        {
            //base.performAction();

            if (elapsedTime.Seconds > ATTACK_RATE)
            {
                elapsedTime = elapsedTime.Subtract(elapsedTime);
                target.doDamage(GameOptions.getZombieStrength());
            }

            elapsedTime += time.ElapsedGameTime;

            if (elapsedTime.Milliseconds > 900)
            {
                elapsedTime.Add(oneSecond);
            }
        }
    }
}
