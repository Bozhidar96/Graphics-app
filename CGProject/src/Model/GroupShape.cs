using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// pak kopirahme vsichko
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    /// 
    [Serializable]
    public class GroupShape : Shape
    {
        #region Constructor

        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        //tyk dobavqme novosti
        // tuk razshirqvame modela koito ni dava vuzmojnosti za po struktorno izobrajenie
        private List<Shape> subItem = new List<Shape>();
        public List<Shape> SubItem {
            get { return subItem; }
            set { subItem = value; }
        }

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            // tyk promenqme 
            if (base.Contains(point))
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                // koga nqkoi ot elementite e v podelemntite
                foreach(var item in SubItem)
                {
                    if (item.Contains(point))
                    {
                        return true;
                    }
                    return false;
                }
                
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
            return false;
        }

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        /// tuk promenqme
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            foreach(var item in SubItem)
            {
                item.DrawSelf(grfx);
            }
            /*grfx.FillRectangle(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            // tyk promenqme new pen
            grfx.DrawRectangle(new Pen(BorderColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);    */

        }
                   // prenapisvame methoda ot shape
        public override void Move(float dx, float dy)
        {
            base.Move(dx, dy);
            foreach(var item in SubItem)
            {
                item.Move(dx, dy);
            }
        }
        // za smqna na cveta
        public override Color FillColor
        {
            set { foreach (var item in SubItem) item.FillColor = value; }
        }
        //trqbva da imame i za border color i za drugite harakteristiki koito imame, trqbva da imame i vurtene na grupa


        public override void GroupRotate(float rotate)
        {
            base.GroupRotate(rotate);
            foreach (var item in SubItem)
            {
                item.Rotate = rotate;
            }
        }
    }
}
