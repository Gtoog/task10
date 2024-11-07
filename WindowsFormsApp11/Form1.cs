using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    public partial class Form1 : Form
    {
        private bool isDrawing = false;
        private Point startPoint;
        private Point endPoint;
        private Color currentColor = Color.Black;
        private readonly Bitmap canvasBitmap;

        public Form1()
        {
            InitializeComponent();
            // Создаем Bitmap для хранения рисунка
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = canvasBitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPoint = e.Location;
            endPoint = e.Location; // Обновляем конечную точку
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                endPoint = e.Location; // Обновляем конечную точку
                pictureBox1.Invalidate(); // Обновляем холст, чтобы отобразить фигуру, которую мы собираемся нарисовать
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;

                using (Graphics g = Graphics.FromImage(canvasBitmap))
                {
                    DrawShape(g, startPoint, endPoint);
                }
                pictureBox1.Invalidate(); // Обновляем холст для отображения нарисованной фигуры
            }
        }

        private void DrawShape(Graphics g, Point start, Point end)
        {
            Pen pen = new Pen(currentColor, (float)numericUpDown1.Value);

            // Определяем выбранный тип фигуры
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Линия":
                    g.DrawLine(pen, start, end);
                    break;
                case "Квадрат":
                    g.DrawRectangle(pen, GetRectangle(start, end));
                    break;
                case "Треуголник":
                    g.DrawPolygon(pen, GetTriangle(start, end));
                    break;
            }
        }

        private Rectangle GetRectangle(Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(start.X - end.X);
            int height = Math.Abs(start.Y - end.Y);
            return new Rectangle(x, y, width, height);
        }

        private Point[] GetTriangle(Point start, Point end)
        {
            // Получаем вершины треугольника
            Point top = new Point((start.X + end.X) / 2, start.Y);
            Point left = new Point(start.X, end.Y);
            Point right = new Point(end.X, end.Y);
            return new Point[] { top, left, right };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    currentColor = colorDialog.Color;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Очищает Bitmap и обновляет PictureBox
            using (Graphics g = Graphics.FromImage(canvasBitmap))
            {
                g.Clear(Color.White);
            }
            pictureBox1.Invalidate(); // Очищает PictureBox
        }

        // Перегрузка метода Paint для отображения нарисованных фигур
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(canvasBitmap, Point.Empty);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}