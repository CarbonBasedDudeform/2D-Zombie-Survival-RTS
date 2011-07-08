using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zombie_RTS
{
    class Resources
    {
        private int m_wood = 0;
        private int m_metal = 0;
        private int m_food = 0;
        private int m_water = 0;

        public int Wood
        {
            get { return m_wood; }
            set { m_wood = value; }
        }

        public int Metal
        {
            get { return m_metal; }
            set { m_metal = value; }
        }

        public int Food
        {
            get { return m_food; }
            set { m_food = value; }
        }

        public int Water
        {
            get { return m_water; }
            set { m_water = value; }
        }

        public static bool operator >=(Resources r1, Resources r2)
        {
            if (r1.Food >= r2.Food)
            {
                if (r1.Metal >= r2.Metal)
                {
                    if (r1.Water >= r2.Water)
                    {
                        if (r1.Wood >= r2.Wood)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool operator <=(Resources r1, Resources r2)
        {
            if (r1.Food <= r2.Food)
            {
                if (r1.Metal <= r2.Metal)
                {
                    if (r1.Water <= r2.Water)
                    {
                        if (r1.Wood <= r2.Wood)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static Resources operator -(Resources r1, Resources r2)
        {
            return new Resources(r1.Food - r2.Food, r1.Water - r2.Water, r1.Metal - r2.Metal, r1.Wood - r2.Wood);
        }

        public Resources(int Food, int Water, int Metal, int Wood)
        {
            m_food = Food;
            m_water = Water;
            m_metal = Metal;
            m_wood = Wood;
        }

        public override string ToString()
        {
            return "Wood: " + m_wood + "\nMetal: " + m_metal + "\nFood: " + m_food + "\nWater: " + m_water;
        }
    }
}
