using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zombie_RTS
{
    class SentryGun : Unit
    {
        public List<Bullet> bullets = new List<Bullet>();
        private Texture2D m_bulletTexture;

        public SentryGun(float X, float Y, Texture2D bulletTex)
        {
            m_position = new Vector2(X, Y);
            m_width = 100;
            m_height = 100;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, 25, 25);
            m_origin = new Vector2(m_width / 2, m_height / 2);
            area = new Rectangle(0, 0, 50, 50);
            m_bulletTexture = bulletTex;

            m_scanArea = new Rectangle((int)m_position.X - SCAN_RADIUS, (int)m_position.Y - SCAN_RADIUS, SCAN_RADIUS * 2, SCAN_RADIUS * 2);
        }

        private Texture2D m_baseTexture;

        public void setBaseTexture(Texture2D texture)
        {
            m_baseTexture = texture;
        }

        public static Resources getRequiredResources()
        {
            return new Resources(0, 0, 300, 200);
        }

        public override void draw(SpriteBatch sb)
        {
            if (m_unitHealth > 0)
            {
                sb.Begin();
                sb.Draw(m_baseTexture, m_position, null, Color.White, 0, m_origin, 0.5f, SpriteEffects.None, 0);
                sb.End();

                sb.Begin();

                foreach (Bullet curB in bullets)
                {
                    curB.draw(sb);
                }

                sb.Draw(m_texture, m_position, null, Color.White, m_rotation, m_origin, 0.5f, SpriteEffects.None, 0);

                if (m_displayInfo)
                {
                    drawDisplayInfo(sb);
                }
                
                sb.End();
            }
        }

        Rectangle area;

        public override void update(Vector2 MousePos, Microsoft.Xna.Framework.Input.ButtonState btnState)
        {
            area.X = (int)MousePos.X - 25;
            area.Y = (int)MousePos.Y - 25;

            m_rectangle.X = (int)m_position.X;
            m_rectangle.Y = (int)m_position.Y;

            if (btnState == ButtonState.Pressed)
            {
                //if (area.Contains((int)m_position.X, (int)m_position.Y))
                if (area.Intersects(m_rectangle))
                {
                    selectUnit();
                }
                else
                {
                    deselectUnit();
                }
            }
        }

        public void update(List<Zombie> zombies, GameTime time)
        {

            if (scan(zombies))
            {
                performAction(time);
            }
            else
            {
                m_rotation += 0.1f;
            }
            
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(zombies);

                if (bullets[i].isVisible == false)
                {
                    bullets[i] = null;
                    bullets.RemoveAt(i);
                    GC.Collect();
                }
            }
        }

        Rectangle m_scanArea;
        const int SCAN_RADIUS = 100;

        public bool scan(List<Zombie> targets)
        {
            foreach (Unit sub in targets)
            {
                if (m_scanArea.Intersects(sub.getRect()))
                {
                    m_rotation = (float)Math.Atan2(m_position.X - sub.X, sub.Y - m_position.Y);
                    return true;
                }
            }

            return false;
        }

        private TimeSpan elapsedTime = new TimeSpan();
        private TimeSpan oneSecond = new TimeSpan(0, 0, 1);
        const int ATTACK_RATE = 500;

        public void performAction(GameTime time)
        {
            if (elapsedTime.Milliseconds > ATTACK_RATE)
            {
                elapsedTime = elapsedTime.Subtract(elapsedTime);
                bullets.Add(new Bullet(m_bulletTexture));
                bullets.Last().FireBullet(m_rotation, m_position.X, m_position.Y);
            }

            elapsedTime += time.ElapsedGameTime;
        }

        public void moveBulletsDown(int amount)
        {
            foreach (Bullet b in bullets)
            {
                b.Y -= amount;
            }
        }

        public void moveBulletsUp(int amount)
        {
            foreach (Bullet b in bullets)
            {
                b.Y += amount;
            }
        }

        public void moveBulletsRight(int amount)
        {
            foreach (Bullet b in bullets)
            {
                b.X -= amount;
            }
        }

        public void moveBulletsLeft(int amount)
        {
            foreach (Bullet b in bullets)
            {
                b.X += amount;
            }
        }
    }
}
