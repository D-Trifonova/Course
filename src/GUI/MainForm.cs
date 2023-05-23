using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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
            // this
            dialogProcessor.SceneSize = Size;
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
            int strokeWidth = int.Parse(StrokeWidthTextBox.Text);

            dialogProcessor.AddRandomRectangle(strokeWidth);

            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник.";

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
            if (pickUpSpeedButton.Checked)
            {
                Shape temp = dialogProcessor.ContainsPoint(e.Location);
                if (temp != null)
                {
                    if (dialogProcessor.Selection.Contains(temp))
                    {
                        dialogProcessor.Selection.Remove(temp);
                    }
                    else
                    {
                        dialogProcessor.Selection.Add(temp);
                    }
                }

                statusBar.Items[0].Text = "Последно действие: Селекция на примитив.";
                dialogProcessor.IsDragging = true;
                dialogProcessor.LastLocation = e.Location;
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режм на "влачене", то избрания елемент се транслира.
        /// </summary>
        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging)
            {
                if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Действие: Влачене."; // Последно д...
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

            if (dialogProcessor.IsDragging == false)
            {
                if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Drop.";
                dialogProcessor.TranslateTo(e.Location);
                viewPort.Invalidate();
            }
        }

        private void DrawEllipseButton_Click(object sender, EventArgs e)
        {
            int strokeWidth = int.Parse(StrokeWidthTextBox.Text);

            dialogProcessor.AddRandomEllipse(strokeWidth);

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса.";

            viewPort.Invalidate();
        }

        private void DrawStarButton_Click(object sender, EventArgs e)
        {
            int strokeWidth = int.Parse(StrokeWidthTextBox.Text);

            dialogProcessor.AddRandomStar(strokeWidth);

            statusBar.Items[0].Text = "Последно действие: Рисуване на звезда.";

            viewPort.Invalidate();
        }

        private void OpacityTrackBar_Scroll(object sender, EventArgs e)
        {
            dialogProcessor.SetOpacity(OpacityTreckBarScroll.Value);

            statusBar.Items[0].Text = "Последно действие: Избор на прозрачност. \"scroller\"";

            viewPort.Invalidate();

            //dialogProcessor.Selection.Opacity = (int)opacity.Value; // DEL
            //statusBar.Items[0].Text = "Последно действие: Избор на прозрачност.";
            //viewPort.Invalidate();
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e) // ne raboti
        {
            dialogProcessor.SetRotation((float)numericUpDown2.Value);

            statusBar.Items[0].Text = "Последно действие: Избор на завъртане.";

            viewPort.Invalidate();
        }

        private void CurveShapeButton_Click(object sender, EventArgs e)
        {
            int strokeWidth = int.Parse(StrokeWidthTextBox.Text);

            dialogProcessor.AddRandomCurveShape(strokeWidth);

            statusBar.Items[0].Text = "Последно действие: Рисуване на трапец.";

            viewPort.Invalidate();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            dialogProcessor.SetStrokeWidth((float)numericUpDown1.Value);

            statusBar.Items[0].Text = "Последно действие: Удебеляване на контура.";

            viewPort.Invalidate();
        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e) // ne raboti~!
        {
            //dialogProcessor.SetOpacity((int)numericUpDown3.Value);
            dialogProcessor.SetFillColor(Color.Empty);
            statusBar.Items[0].Text = "Последно действие: Прозрачност на контура.";

            viewPort.Invalidate();

            //dialogProcessor.Selection.Transparency = (int)transparency.Value; // tova DEL
            //statusBar.Items[0].Text = "Последно действие: Прозрачност на контура.";
            //viewPort.Invalidate(); // tyk
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //int strokeWidth = int.Parse(StrokeWidthTextBox.Text);
            //dialogProcessor.RotatePrimitive(strokeWidth);

            dialogProcessor.RotatePrimitive(45);

            statusBar.Items[0].Text = "Последно действие: Избор на завъртане 45 градуса.";

            viewPort.Invalidate();
        }

        private void DrawGroupRimitives_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupPrimitives();

            statusBar.Items[0].Text = "Последно действие: Групиране на примитиви.";

            viewPort.Invalidate();
        }

        private void UngroupSpeedButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.UngroupPrimitives();

            statusBar.Items[0].Text = "Последно действие: Разгрупиране на примитиви.";

            viewPort.Invalidate();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            if (dialogProcessor.Selection.Count > 0)
            {
                contextMenuStrip1.Items.Add(
                "Rotate Primitives",
                null, // null = icon.png
                new EventHandler(
                    DrawGroupRimitives_Click
                    )
                );
            }
            else if (dialogProcessor.Selection.Count > 1)
            {
                contextMenuStrip1.Items.Add(
                    "New Rectangle",
                    null,
                    new EventHandler(
                        DrawRectangleSpeedButtonClick
                    )
                );
            }
            else if (dialogProcessor.Selection.Count > 2)
            {
                contextMenuStrip1.Items.Add(
                    "New Ellipse",
                    null,
                    new EventHandler(
                        DrawEllipseButton_Click
                        )
                    );
            }
            else if (dialogProcessor.Selection.Count > 3)
            {
                contextMenuStrip1.Items.Add(
                    "New Star",
                    null,
                    new EventHandler(
                        DrawStarButton_Click
                        )
                    );
            }
            else
            {
                contextMenuStrip1.Items.Add(
                    "New Curve",
                    null,
                    new EventHandler(
                        CurveShapeButton_Click
                        )
                    );
            }
        }

        private void viewPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.G && e.Shift == false) // ctrl+g grupirane
            {
                dialogProcessor.GroupPrimitives();
                viewPort.Invalidate();
            }
            else if (e.Control == true && e.KeyCode == Keys.G && e.Shift == true) // cntr+shifr+g razgrupirane
            {
                dialogProcessor.UngroupPrimitives();
                viewPort.Invalidate();
            }
            else if (e.Control == true && e.KeyCode == Keys.A && e.Shift == false) // cntl+a
            {
                // TODO logika za selektirane na vsi4ko
                dialogProcessor.SelectAll();
                viewPort.Invalidate();
            }
            else if (e.Control == true && e.KeyCode == Keys.C && e.Shift == false) // ctrl+c
            {
                //DrawEllipseButton_Click.Copy();
                //dialogProcessor.Copy();
                viewPort.Invalidate();
            }
            else if (e.Control == true && e.KeyCode == Keys.V && e.Shift == false) // ctrl+v
            {
                //dialogProcessor.Paste();
                viewPort.Invalidate();
            }
            else if (e.Control == true && e.KeyCode == Keys.X && e.Shift == false) // ctrl+x
            {
                //dialogProcessor.Cut();
                viewPort.Invalidate();
            }
        }

        // [Serializable] binary format na klasa moje bi v dialog processor ili shape

        private void primitivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupPrimitives();
            viewPort.Invalidate();
        }

        private void удебеляванеНаКонтураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SetStrokeWidth((float)numericUpDown1.Value);

            statusBar.Items[0].Text = "Последно действие: Удебеляване на контура. 1";

            viewPort.Invalidate();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            //toolTip1.SetToolTip(numericUpDown1, "Оразмеряване на контура.");
        }

        private void fillOutlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetStrokeColor(colorDialog1.Color); // V1 kontur
                statusBar.Items[0].Text = "Последно действие: Избор на смяна на цвят на контура.";
                //dialogProcessor.Selection.FillColor = colorDialog1.Color; // V2 v1tre
                //viewPort.Invalidate(); // V2 v1tre
            }

            viewPort.Invalidate(); // V1 kontur
        }

        private void fillInlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetFillColor(colorDialog1.Color);
                statusBar.Items[0].Text = "Избор на цвят за запълване.";
            }
            viewPort.Invalidate();
        }

        private void transparentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SetFillColor(Color.Transparent);

            statusBar.Items[0].Text = "Примитив с прозрачно тяло.";

            viewPort.Invalidate();
        }

        private void tansparentOtherLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SetStrokeColor(Color.Transparent);

            statusBar.Items[0].Text = "Примитив с прозрачен контур.";

            viewPort.Invalidate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.Save(saveFileDialog.FileName);
            }
        }

        private void openToolStripMenuItemClick_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.Open(openFileDialog.FileName);
            }

            viewPort.Invalidate();
        }

        private void saveStripMenuItemClick_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.Save(saveFileDialog.FileName);
            }

            viewPort.Invalidate();
        }

        private void bigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.Resize(10, 10);

            statusBar.Items[0].Text = "Уголемяване на примитива.";

            viewPort.Invalidate();
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.Resize(-10, -10);

            statusBar.Items[0].Text = "Смаляване на примитива.";

            viewPort.Invalidate();
        }

        private void AngleSelection(object sender, EventArgs e)
        {
            int angle = Convert.ToInt32(((ToolStripMenuItem)sender).Text);

            dialogProcessor.RotateAt(angle);

            statusBar.Items[0].Text = "Завъртане на " + angle + " градуса.";

            viewPort.Invalidate();

        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dialogProcessor.Delete();

            statusBar.Items[0].Text = "Изтриване примитив.";

            viewPort.Invalidate();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // about = new ();
            //about.Show();
        }

        // TODO end
    }
}