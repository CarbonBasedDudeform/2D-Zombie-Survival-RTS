using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zombie_RTS
{
    class Material : Unit
    {
        public enum type
        {
            Wood,
            Metal,
            Food,
            Water
        }

        private type curType;

        public type getType()
        {
            return curType;
        }

        public Material(float X, float Y, int Width, int Height, type matType)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, 25, 25);
            curType = matType;
            m_origin = new Vector2(Width / 2, Height / 2);

            switch (curType)
            {
                case type.Metal:
                    m_unitHealth = 200;
                    break;
                case type.Wood:
                    m_unitHealth = 150;
                    break;
                case type.Food:
                    m_unitHealth = 75;
                    break;
                case type.Water:
                    m_unitHealth = 50;
                    break;
            }
        }

        public override void update(Microsoft.Xna.Framework.Vector2 MousePos, Microsoft.Xna.Framework.Input.ButtonState btnState)
        {
            //do nothing.
        }

        protected override void drawDisplayInfo(SpriteBatch sb)
        {
            //base.drawDisplayInfo(sb);
        }

        public override void draw(SpriteBatch sb)
        {
            sb.Begin();

            if (m_unitHealth > 0)
            {
                switch (curType)
                {
                    case type.Food: sb.Draw(m_texture, m_position, null, Color.White, m_rotation, m_origin, 0.5f, SpriteEffects.None, 0); break;
                    case type.Metal: sb.Draw(m_texture, m_position, null, Color.White, m_rotation, m_origin, 0.2f, SpriteEffects.None, 0); break;
                    case type.Water: sb.Draw(m_texture, m_position, null, Color.White, m_rotation, m_origin, 0.5f, SpriteEffects.None, 0); break;
                    case type.Wood: sb.Draw(m_texture, m_position, null, Color.White, m_rotation, m_origin, 0.5f, SpriteEffects.None, 0); break;

                    default:
                        sb.Draw(m_texture, m_position, null, Color.White, m_rotation, m_origin, 0.5f, SpriteEffects.None, 0);
                        break;
                }
            }

            sb.End();
        }
    }
}
