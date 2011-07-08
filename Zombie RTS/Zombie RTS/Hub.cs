using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Zombie_RTS
{
    class Hub : Unit
    {
        public Hub(float X, float Y, int Width, int Height)
        {
            m_position = new Vector2(X, Y);
            m_width = Width;
            m_height = Height;
            m_rectangle = new Rectangle((int)m_position.X, (int)m_position.Y, Width, Height);
            m_origin = new Vector2(Width / 2, Height / 2);
            m_unitHealth = 1000;
        }

        public override void update(Microsoft.Xna.Framework.Vector2 MousePos, Microsoft.Xna.Framework.Input.ButtonState btnState)
        {
            Rectangle area = new Rectangle((int)MousePos.X - m_width, (int)MousePos.Y - m_height, m_width * 2, m_height * 2);

            if (btnState == ButtonState.Pressed)
            {
                if (area.Contains((int)m_position.X, (int)m_position.Y))
                {
                    selectUnit();
                }
                else
                {
                    deselectUnit();
                }
            }
        }

        public override int getFullHealth()
        {
            //return base.getFullHealth();

            return 1000;
        }

        private TimeSpan elapsedTime;

        enum creationType
        {
            Soldier,
            Worker,
            Maintainer,
            Builder,
            Nothing
        }

        creationType curType = creationType.Nothing;
        enum displayType
        {
            Worker,
            Builder,
            Soldier,
            Maintainer,
            Nothing
        }

        private displayType curDispType = displayType.Nothing;
        private TimeSpan displTime = new TimeSpan();
        private TimeSpan oneSecond = new TimeSpan(0, 0, 1);

        public void update(Vector2 mousePos, ButtonState btnState, KeyboardState keyState, GameTime time)
        {
            if (m_selected)
            {
                if (!keyState.IsKeyDown(Keys.R))
                {
                    if (keyState.IsKeyDown(Keys.S))
                    {
                        if (RTSManager.curResources >= Soldier.getRequiredResources())
                        {
                            elapsedTime = new TimeSpan();
                            curType = creationType.Soldier;
                        }
                    }
                    else if (keyState.IsKeyDown(Keys.W))
                    {
                        if (RTSManager.curResources >= Worker.getRequiredResources())
                        {
                            elapsedTime = new TimeSpan();
                            curType = creationType.Worker;
                        }
                    }
                    else if (keyState.IsKeyDown(Keys.M))
                    {
                        if (RTSManager.curResources >= Maintainer.getRequiredResources())
                        {
                            elapsedTime = new TimeSpan();
                            curType = creationType.Maintainer;
                        }
                    }
                    else if (keyState.IsKeyDown(Keys.B))
                    {
                        if (RTSManager.curResources >= Builder.getRequiredResources())
                        {
                            elapsedTime = new TimeSpan();
                            curType = creationType.Builder;
                        }
                    }
                }

                if (keyState.IsKeyDown(Keys.R) && keyState.IsKeyDown(Keys.W))
                {
                    curDispType = displayType.Worker;
                    displTime = new TimeSpan();
                }
                else if (keyState.IsKeyDown(Keys.R) && keyState.IsKeyDown(Keys.B))
                {
                    curDispType = displayType.Builder;
                    displTime = new TimeSpan();
                }
                else if (keyState.IsKeyDown(Keys.R) && keyState.IsKeyDown(Keys.S))
                {
                    curDispType = displayType.Soldier;
                    displTime = new TimeSpan();
                }
                else if (keyState.IsKeyDown(Keys.R) && keyState.IsKeyDown(Keys.M))
                {
                    curDispType = displayType.Maintainer;
                    displTime = new TimeSpan();
                }
            }

            if (elapsedTime.Seconds > Soldier.getCreationTime() && curType == creationType.Soldier)
            {
                RTSManager.createSoldier(m_position.X - 50, m_position.Y);
                elapsedTime = new TimeSpan();
                curType = creationType.Nothing;
                curDispType = displayType.Nothing;
            }
            else if (elapsedTime.Seconds > Worker.getCreationTime() && curType == creationType.Worker)
            {
                RTSManager.createWorker(m_position.X - 50, m_position.Y);
                elapsedTime = new TimeSpan();
                curType = creationType.Nothing;
                curDispType = displayType.Nothing;
            }
            else if (elapsedTime.Seconds > Builder.getCreationTime() && curType == creationType.Builder)
            {
                RTSManager.createBuilder(m_position.X - 50, m_position.Y);
                elapsedTime = new TimeSpan();
                curType = creationType.Nothing;
                curDispType = displayType.Nothing;
            }
            else if (elapsedTime.Seconds > Maintainer.getCreationTime() && curType == creationType.Maintainer)
            {
                RTSManager.createMaintainer(m_position.X - 50, m_position.Y);
                elapsedTime = new TimeSpan();
                curType = creationType.Nothing;
                curDispType = displayType.Nothing;
            }

            switch (curType)
            {
                case creationType.Soldier:
                    m_buildCompletion = (int)((elapsedTime.Seconds / Soldier.getCreationTime()) * 100);
                    break;
                case creationType.Worker:
                    m_buildCompletion = (int)((elapsedTime.Seconds / Worker.getCreationTime()) * 100);
                    break;
                case creationType.Builder:
                    m_buildCompletion = (int)((elapsedTime.Seconds / Builder.getCreationTime()) * 100);
                    break;
                case creationType.Maintainer:
                    m_buildCompletion = (int)((elapsedTime.Seconds / Maintainer.getCreationTime()) * 100);
                    break;
                case creationType.Nothing:
                    m_buildCompletion = 0;
                    break;
            }

            elapsedTime += time.ElapsedGameTime;

            if (elapsedTime.Milliseconds > 900)
            {
                elapsedTime.Add(oneSecond);
            }

            displTime += time.ElapsedGameTime;

            if (displTime.Milliseconds > 900)
            {
                displTime.Add(oneSecond);
            }

            if (displTime.Seconds > 5)
            {
                curDispType = displayType.Nothing;
            }
        }

        private int m_buildCompletion = 0;

        protected override void drawDisplayInfo(SpriteBatch sb)
        {
            base.drawDisplayInfo(sb);
            string curBuilding = "Nothing";

            switch (curType)
            {
                case creationType.Builder:
                    curBuilding = "Builder";
                    break;
                case creationType.Maintainer:
                    curBuilding = "Maintainer";
                    break;
                case creationType.Soldier:
                    curBuilding = "Soldier";
                    break;
                case creationType.Worker:
                    curBuilding = "Worker";
                    break;
            }

            sb.DrawString(m_font, "Build Completion: " + m_buildCompletion + "%\nCurrently Building: " + curBuilding , new Vector2(m_displayArea.Width - 350, m_displayTextPos.Y + m_displayArea.Height - 50), Color.White);
        }

        public override void draw(SpriteBatch sb)
        {
            base.draw(sb);
            
            sb.Begin();
            sb.DrawString(m_font, "Current Resources\nWood: " + RTSManager.curResources.Wood + "\nMetal:" + RTSManager.curResources.Metal + "\nFood: " + RTSManager.curResources.Food + "\nWater:" + RTSManager.curResources.Water, new Vector2(0,0), Color.White);
            Vector2 dispVec = new Vector2(200, 0);
            switch (curDispType)
            {
                case displayType.Worker:
                    sb.DrawString(m_font, "Worker Requirements\n" + Worker.getRequiredResources().ToString(), dispVec, Color.White);
                    break;
                case displayType.Builder:
                    sb.DrawString(m_font, "Builder Requirements\n" + Builder.getRequiredResources().ToString(), dispVec, Color.White);
                    break;
                case displayType.Maintainer:
                    sb.DrawString(m_font, "Maintainer Requirements\n" + Maintainer.getRequiredResources().ToString(), dispVec, Color.White);
                    break;
                case displayType.Soldier:
                    sb.DrawString(m_font, "Soldier Requirements\n" + Soldier.getRequiredResources().ToString(), dispVec, Color.White);
                    break;
                default:
                    break;
            }
            sb.End();
        }

        public static Resources getRequiredResources()
        {
            return new Resources(100, 100, 300, 300);
        }
    }
}
