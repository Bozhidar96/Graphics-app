using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{   

            //tyk promenqme kato napravihme shape kato list
			if (pickUpSpeedButton.Checked) {
                var sel = dialogProcessor.ContainsPoint(e.Location);       // <-- tova e nova promenliva prisvoihme dialog.... 
                
				if (sel != null) { // tuk slojihme sel v if-a
                    //pravim oshte edin if
                    if(dialogProcessor.Selection.Contains(sel)) // pravim nova proverka dali sushtestvuva elementa v kolekciqta    ako go ima go mahame ako ne e go dobavqme v selekciqta i toi shte se selektira
                    {
                        dialogProcessor.Selection.Remove(sel);
                    }
                    else
                    {
                        dialogProcessor.Selection.Add(sel);
                    }
					statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
					dialogProcessor.IsDragging = true;
					dialogProcessor.LastLocation = e.Location;
					viewPort.Invalidate();
				}
			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void EllipseButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomEllipse();

            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

            viewPort.Invalidate();
        }
        /// <summary>
        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        // tyk nov buton
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // butona za cvqt
            // trqbva da dobavim dialogprocess ot toolboxa
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetSelectFillColor(colorDialog1.Color); // napravihme nqkakyv metod koito shte go napravim v displayprocessor.cs
                viewPort.Invalidate();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //nqkakuv metoth
            dialogProcessor.GroupSelected();
            viewPort.Invalidate();
        }

        private void CircleButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomCircle();

            //statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

            viewPort.Invalidate();
        }

        private void TriangleButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTriangle();
            viewPort.Invalidate();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dialogProcessor.SaveAs(saveFileDialog1.FileName);
            }
        }

        private void RotateTrackBar_Scroll(object sender, EventArgs e)
        {
            dialogProcessor.Rotate((float)RotateTrackBar.Value);
             statusBar.Items[0].Text = "Последно действие: Завъртане";
            viewPort.Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            dialogProcessor.DeleteSelected();
            statusBar.Items[0].Text = "Последно действие: изтриване на селектираните примитиви";
            viewPort.Invalidate();
        }

        private void DeleteMenuButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.DeleteSelected();
            statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните примитиви";
            viewPort.Invalidate();
        }

        private void CopyMenuButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.CopySelected();
            statusBar.Items[0].Text = "Последно действие: Копиране на селектираните примитиви";
            viewPort.Invalidate();
        }

        private void PasteMenuButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.PasteSelected();
            statusBar.Items[0].Text = "Последно действие: Поставяне на селектираните примитиви";
            viewPort.Invalidate();
        }
    }
}
