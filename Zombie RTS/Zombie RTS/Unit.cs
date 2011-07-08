using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zombie_RTS
{
    class Unit
    {
        protected Vector2 m_position;
        protected int m_width, m_height;
        protected Vector2 m_origin;

        /*
        public Unit(float X, float Y, int Width, int Height)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;

            m_origin = new Vector2(Width / 2, Height / 2);
        }*/

        
        public float X
        {
            get { return m_position.X; }
            set { 
                m_position.X = value;
                m_rectangle.X = (int)value;
            }
        }

        public float Y
        {
            get { return m_position.Y; }
            set { 
                m_position.Y = value;
                m_rectangle.Y = (int)value;
            }
        }

        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        protected Texture2D m_texture;

        public void setTexture(Texture2D texture)
        {
            m_texture = texture;
        }

        public Texture2D getTexture()
        {
            return m_texture;
        }

        protected float m_rotation;
        protected SpriteFont m_font;

        public void setFont(SpriteFont font)
        {
            m_font = font;
        }

        protected Texture2D m_displayTexture;

        public void setDisplayTexture(Texture2D texture)
        {
            m_displayTexture = texture;
        }

        protected Rectangle m_displayArea;
        protected Vector2 m_displayTextPos;

        public void setDisplayArea(Rectangle area)
        {
            m_displayArea = area;
            m_displayTextPos = new Vector2(area.X + 100, area.Y);
        }

        protected string m_displayText = "i need to be changed";

        public void setDisplayText(string text)
        {
            m_displayText = text;
        }

        public virtual void draw(SpriteBatch sb)
        {
            sb.Begin();

            if (m_unitHealth > 0)
            {
                sb.Draw(m_texture, m_position, null, Color.White, m_rotation, m_origin, 1, SpriteEffects.None, 0);

                if (m_displayInfo)
                {
                    drawDisplayInfo(sb);
                }
            }

            sb.End();
        }

        protected virtual void drawDisplayInfo(SpriteBatch sb)
        {
            sb.Draw(m_displayTexture, m_displayArea, Color.White);
            sb.DrawString(m_font, m_displayText + "\nHealth: " + m_unitHealth, m_displayTextPos, Color.Black);
        }

        protected Rectangle m_rectangle;

        public Rectangle getRect()
        {
            return m_rectangle;
        }

        Rectangle area = new Rectangle(0, 0, 50, 50);

        public virtual void update(Vector2 MousePos, ButtonState btnState)
        {
            area.X = (int)MousePos.X - 25;
            area.Y = (int)MousePos.Y - 25;

            m_rectangle.X =(int)m_position.X;
            m_rectangle.Y = (int)m_position.Y;

            if (btnState == ButtonState.Pressed)
            {
                //if (area.Contains((int)m_position.X, (int)m_position.Y))
                if(area.Intersects(m_rectangle))
                {
                    selectUnit();
                }
                else
                {
                    m_selected = false;
                    m_displayInfo = false;
                    curState = State.Walking;
                }
            }

            move(MousePos);
        }

        protected bool m_selected = false;

        protected int SPEED = 3;

        public void selectUnit()
        {
            m_selected = true;
            m_displayInfo = true;
            m_speed = SPEED;
        }

        public void deselectUnit()
        {
            m_selected = false;
            m_displayInfo = false;
            if (curState != State.Walking)
                m_curDestination = m_position;
        }

        protected enum State
        {
            Idle,
            Walking,
            BuildingSentry,
            BuildingWall,
            BuildingHub,
            Attacking,
            RepairingSentry,
            RepairingWall,
            RepairingHub
        }

        protected State curState = State.Idle;
        protected int m_speed = 0;

        public void setSpeed(int Speed)
        {
            m_speed = Speed;
        }

        public int getSpeed()
        {
            return m_speed;
        }

        protected Vector2 m_curDestination;

        public void moveCurDestLeft(float amount)
        {
            m_curDestination.X += amount;
        }

        public void moveCurDestRight(float amount)
        {
            m_curDestination.X -= amount;
        }

        public void moveCurDestUp(float amount)
        {
            m_curDestination.Y += amount;
        }

        public void moveCurDestDown(float amount)
        {
            m_curDestination.Y -= amount;
        }

        public void move(Vector2 destination)
        {
            if (m_selected)
            {
                m_rotation = (float)Math.Atan2(m_position.X - destination.X, destination.Y - m_position.Y);

                m_curDestination = destination;
                //curState = State.Walking;
            } 

            if (curState == State.Walking && !arrivedAtDestination())
            {
                m_position.X -= m_speed * (float)Math.Sin(m_rotation);
                m_position.Y += m_speed * (float)Math.Cos(m_rotation);
            }
        }

        Rectangle arrivedArea = new Rectangle(0, 0, 20, 20);

        private bool arrivedAtDestination()
        {
            arrivedArea.X = (int)m_curDestination.X - 10;
            arrivedArea.Y = (int)m_curDestination.Y - 10;

            if (arrivedArea.Contains((int)m_position.X, (int)m_position.Y))
            {
                curState = State.Idle;
                return true;
            }

            return false;
        }

        protected int m_unitHealth = 100;

        public void setHealth(int health)
        {
            m_unitHealth = health;
        }

        public int getHealth()
        {
            return m_unitHealth;
        }

        public void doDamage(int amount)
        {
            m_unitHealth -= amount;
        }

        protected bool m_displayInfo = false;

        public virtual void performAction()
        {
            Console.WriteLine("performing action");
        }

        public void repair(int amount)
        {
            if (m_unitHealth < getFullHealth() && RTSManager.curResources.Wood > 0 && RTSManager.curResources.Metal > 0)
            {
                m_unitHealth += amount;
                RTSManager.curResources.Wood -= 5;
                RTSManager.curResources.Metal -= 5;
            }

            if (m_unitHealth > getFullHealth())
                m_unitHealth = getFullHealth();
        }

        public virtual int getFullHealth()
        {
            return 100;
        }
    }
}
