using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zombie_RTS
{
    class Bullet : Unit
    {
        #region Variable Declartion & Constructer

        public bool isVisible = false;
        private float RotationAngle;
        private const int MAX_DISTANCE = 1000;

        private const int BULLET_SPEED = 5;
        private Vector2 velocity;
        private Vector2 origin;
        private Vector2 RotationOrigin = new Vector2(0, 0);
        private Rectangle m_drawingRect;

        /// <summary>
        /// Make a bullet, baby
        /// </summary>
        /// <param name="texture">Texture to be used on bullet</param>
        public Bullet(Texture2D texture)
        {
            m_texture = texture;
            m_drawingRect = new Rectangle(0, 0, 25, 25);
            m_position = new Vector2(0,0);
            m_rectangle = new Rectangle(0, 0, 25, 25);
        }

        #endregion

        /// <summary>
        /// Starts the bullet a-movin'
        /// </summary>
        /// <param name="angle">Angle for the bullet to be fired at</param>
        /// <param name="x">Initial X position</param>
        /// <param name="y">Initial Y position</param>
        public void FireBullet(float angle, float x, float y)
        {
            isVisible = true;
            RotationAngle = angle;
            origin = new Vector2(x, y);

            velocity.X = (float)(BULLET_SPEED * Math.Sin(RotationAngle));
            velocity.Y = (float)(BULLET_SPEED * Math.Cos(RotationAngle));

            m_position.X = x;
            m_position.Y = y;
            m_rectangle.X = (int)m_position.X;
            m_rectangle.Y = (int)m_position.Y;
        }

        public void SetRotationOrigin(int X, int Y)
        {
            RotationOrigin.X = X;
            RotationOrigin.Y = Y;
        }

        public void SetRotationOriginCenter()
        {
            RotationOrigin.X = m_drawingRect.Width / 2;
            RotationOrigin.Y = m_drawingRect.Height / 2;
        }

        #region O-O-O-OVERIDE

        public void Update(List<Zombie> zombies)
        {
            if (isVisible)
            {
                //make the bullet travel at the correct velocities
                m_rectangle.X -= (int)velocity.X;
                m_rectangle.Y += (int)velocity.Y;

                m_position.X -= velocity.X;
                m_position.Y += velocity.Y;

                //animation code
                if (m_drawingRect.Y < 100)
                {
                    m_drawingRect.Y += 25;
                }
                else
                {
                    m_drawingRect.Y = 0;
                }

                if (Math.Abs(m_position.X - origin.X) >= MAX_DISTANCE || Math.Abs(m_position.Y - origin.Y) >= MAX_DISTANCE) //if bullet gone to far, bullet go bye bye
                {
                    isVisible = false;
                }

                foreach (Zombie zomb in zombies)
                {
                    if (m_rectangle.Intersects(zomb.getRect()))
                    {
                        zomb.doDamage(20);
                        isVisible = false;
                        break;
                    }
                }
            }

        }

        public override void draw(SpriteBatch sb)
        {
            if (isVisible)
            {
                //sb.Draw(spriteTexture, spriteRectangle, null, Color.White, RotationAngle, new Vector2(spriteRectangle.Width / 2, spriteRectangle.Height / 2), SpriteEffects.None, 0f);
                //sb.Draw(spriteTexture, spriteRectangle,drawingRectangle , Color.White, RotationAngle, RotationOrigin, SpriteEffects.None, 0f);
                sb.Draw(m_texture, m_position, m_drawingRect, Color.White, RotationAngle, RotationOrigin, 0.5f, SpriteEffects.None, 0);
            }
        }
        #endregion
    }
}
