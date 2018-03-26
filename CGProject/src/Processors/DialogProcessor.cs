using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}

        #endregion

        #region Properties

        /// <summary>
        /// Избран елемент.
        /// </summary>
        /// tuk shte napravim mnojestvena selekciq pravim list ot shape
        private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection {
			get { return selection; }
			set { selection = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}

        private Shape shapeSelectionMenu;
        public Shape ShapeSelectionMenu
        {
            get { return shapeSelectionMenu; }
            set { shapeSelectionMenu = value; }
        }
		
		#endregion
		
		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.White;
            // tyk promenqme
            rect.BorderColor = Color.Black;

			ShapeList.Add(rect);
		}
		
		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					//ShapeList[i].FillColor = Color.Red; <--- tyk komentirame
						
					return ShapeList[i];
				}	
			}
			return null;
		}
        // this is copy paste by c# documentation for graphics
        public void MySerialize(object obj, string filePath = null)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream;
            if (filePath != null)
            {
                stream = new FileStream(filePath + ".bin",
                                  FileMode.Create);
            }
            else
            {
                stream = new FileStream("MyFile.bin",
                                        FileMode.Create,
                                        FileAccess.Write, FileShare.None);
            }
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public object MyDeSerialize(string filePath = null)
        {
            object obj;
            IFormatter formatter = new BinaryFormatter();
            Stream stream;
            if (filePath != null)
            {
                stream = new FileStream(filePath,
                                     FileMode.Open,
                                     FileAccess.Read, FileShare.None);
            }
            else
            {
                stream = new FileStream("MyFile.bin",
                                    FileMode.Open);
            }
            obj = formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }


        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        public void TranslateTo(PointF p)
		{
            //tyk promenqvame
			foreach(var item in Selection) {
                //nqkakuv pomoshtem metod
                item.Move(p.X - lastLocation.X, p.Y - lastLocation.Y);
				//item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);
				
			}
            lastLocation = p;
        }

        public void AddRandomEllipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape elipse = new EllipseShape(new Rectangle(x, y, 200, 100));
            elipse.FillColor = Color.White;

            ShapeList.Add(elipse);
        }

        public void AddRandomCircle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            CircleShape elipse = new CircleShape(new Rectangle(x, y, 100, 100));
            elipse.FillColor = Color.White;

            ShapeList.Add(elipse);
        }

        public void AddRandomTriangle()
        {

            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TriangleShape triangle = new TriangleShape(new Rectangle(x, y, 100, 200));
            triangle.FillColor = Color.White;
            triangle.BorderColor = Color.Black;


            ShapeList.Add(triangle);

        }


        public void SetSelectFillColor(Color color)
        {     //trqbva da napravim seleciqtaa v mouse up a toi sega e v mouse down toest sega ima bug ako selektirame nqkolko elementa i iskame da gi premestim selectiraniq se deselektira. Trqbva da go napravim s nqkakav flag
            // tyk pishem logikata za smqna na cveta
            // promenqme ifa za selekciqta
            foreach(var item in Selection)
            {
                // selection.borderColor = color dobavqme cvqt na bordera
                item.FillColor = color; // parametyra color
            }
        }
                // tozi metod go kopnahme ot display procesora tuk prenapisahme go
        public override void Draw(Graphics grfx)
        {
            base.Draw(grfx);
            // sled kato napravihme promenlivata shape gore v selekciqta s list tuk trqbwa da gi obhodim s cikul
            //if(Selection != null)                 <-- tova beshe po stariq nachi samo s 1 selekciq
            // zamenihme Selection.Location s item veche
            foreach(var item in Selection)
            {
                // tuk risuvame rectangle ponktira pokrai figurata         . Pishem + 6 za da hvane elipsata
                grfx.DrawRectangle(Pens.Black, item.Location.X - 3, item.Location.Y - 3, item.Width + 6, item.Height + 6);
            }
        }
        //tova e metoda ot butona noviq 3tiq 
        public void GroupSelected()
        {     
            //ako imame 1 selektirano da izleze ot funkciqta 
            if(Selection.Count < 2)
            {
                return;
            }
            //otkrivane na obhvashtashtiq pravougulnik na grupata
            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;
             
            //proverqvame minimum i maximum
            foreach(var item in Selection)
            {
                if(minX > item.Location.X)
                {
                    minX = item.Location.X;
                }
                if(minY > item.Location.Y)
                {
                    minY = item.Location.Y;
                }
                if(maxX < item.Location.X + item.Width)
                {
                    maxX = item.Location.X + item.Width;
                }
                if(maxY < item.Location.Y + item.Height)
                {
                    maxY = item.Location.Y + item.Height;
                }
            }

            var group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));

            group.SubItem = Selection;
            Selection = new List<Shape>();
            Selection.Add(group);

            // vsichki koito sme gi selektirali sme gi slojili kato podelementi na grupata i veche gi premahvame ot grupata
            foreach(var item in Selection)
                 ShapeList.Remove(item);
            Selection.Add(group);
            // v shape list trqbva da dobavqme dobavenata grupa
           // ShapeList.Add(group);
        }

        public void Delete()
        {
            foreach (var item in Selection)
                ShapeList.Remove(item);
            Selection.Clear();
        }

        internal void CopySelected()
        {
            MySerialize(Selection);
        }

        internal void PasteSelected()
        {
            List<Shape> copy = (List<Shape>)MyDeSerialize();
            ShapeList.AddRange(copy);
        }

        public void SelectAll()
        {
            Selection = new List<Shape>(ShapeList);
        }

        internal void SaveAs(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create);
            formatter.Serialize(stream, ShapeList);
            stream.Close();

        }

        internal void DeleteSelected()
        {
            foreach (var item in Selection)
            {
                ShapeList.Remove(item);

            }
            Selection.Clear();
        }

        public void Rotate(float rotate, string rotateBtn = " ")
        {
            if (rotateBtn.Equals("right"))
            {
                ShapeSelectionMenu.GroupRotate(rotate);
                ShapeSelectionMenu.Rotate = rotate;
            }
            else
            {
                if (Selection.Count != 0)
                {
                    foreach (var item in Selection)
                    {
                        item.GroupRotate(rotate);
                        item.Rotate = rotate;

                    }
                }
            }
        }


    }
}
