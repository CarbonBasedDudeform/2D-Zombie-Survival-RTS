using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zombie_RTS
{
    class Button
    {
        private Texture2D m_texture;
        private Rectangle m_spriteRectangle, m_drawingRectangle;
        private Vector2 m_textVec;
        private int m_buttonIndex = 1;

        public Button(int X, int Y, int Width, int Height, int buttonIndex)
        {
            m_spriteRectangle = new Rectangle(X, Y, Width, Height);
            m_drawingRectangle = new Rectangle(0, 0, Width, Height);
            m_textVec = new Vector2(X + (Width / 2) - 100, Y + (Height / 2));
            m_buttonIndex = buttonIndex;
        }

        public void setTexture(Texture2D texture)
        {
            m_texture = texture;
        }

        public void mouseOver(MouseState mouse)
        {
            if (m_spriteRectangle.Contains(mouse.X, mouse.Y))
            {
                m_drawingRectangle.Y += 100;
            }
            else
            {
                m_drawingRectangle.Y = 0;
            }
        }

        private SpriteFont m_font;

        public void setFont(SpriteFont font)
        {
            m_font = font;
        }

        private String m_text;

        public void setText(String text)
        {
            m_text = text;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Begin();
            m_spriteRectangle.Y = (GameOptions.getHeight() / 2) + (m_spriteRectangle.Height * m_buttonIndex);
            m_textVec.Y = (GameOptions.getHeight() / 2) + (m_spriteRectangle.Height * m_buttonIndex);// +(m_spriteRectangle.Height / 2);
            m_textVec.X = m_spriteRectangle.X + (GameOptions.getWidth() / 2) - 100;

            sb.Draw(m_texture, m_spriteRectangle, m_drawingRectangle, Color.White);
            sb.DrawString(m_font, m_text, m_textVec, Color.White);
            sb.End();
        }

        public bool pressed(MouseState mouse, MouseState oldMouse)
        {
            if (m_spriteRectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
