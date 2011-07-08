using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zombie_RTS
{
    class Wall : Unit
    {
        public static Resources getRequiredResources()
        {
            return new Resources(0, 0, 100, 100);
        }

        public Wall(float X, float Y, float Rotation)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, 25, 25);
            m_origin = new Vector2(Width / 2, Height / 2);
            m_rotation = Rotation;

            m_rectangle = new Rectangle((int)X, (int)Y, 100, 10);
        }

        public override void draw(SpriteBatch sb)
        {
            //base.draw(sb);
            if (m_unitHealth > 0)
            {
                sb.Begin();
                sb.Draw(m_texture, m_rectangle, null, Color.White, m_rotation, m_origin, SpriteEffects.None, 0);
                sb.End();

                sb.Begin();
               
                if (m_displayInfo)
                {
                    drawDisplayInfo(sb);
                }
            }

            sb.End();
        }

        public override void update(Vector2 MousePos, Microsoft.Xna.Framework.Input.ButtonState btnState)
        {
            //base.update(MousePos, btnState);
            Rectangle area = new Rectangle((int)MousePos.X - 25, (int)MousePos.Y - 25, 50, 50);
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
    }
}
