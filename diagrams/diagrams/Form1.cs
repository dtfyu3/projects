using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace diagrams
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            graphics1 = Graphics.FromImage(pictureBox1.Image);
            graphics2 = Graphics.FromImage(pictureBox2.Image);
            graphics1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawStartCircle(graphics1);
            DrawStartCircle(graphics2);
            newpen.Width = 5;
        }
        public void DrawStartCircle(Graphics g)
        {
            g.Clear(backGroundColor);
            g.DrawEllipse(pen, startPositionX, startPositionY, radius * 2, radius * 2);
            g.FillEllipse(Brushes.Black, new Rectangle(x1 - 3, y1 - 3, 5, 5));
        }
        readonly static int startPositionX = 20;
        readonly static int startPositionY = 20;
        List<NumericUpDown> list;
        List<NumericUpDown> list1;
        Point[] points;
        Point[] polygon;
        Bitmap bitmap;
        readonly Pen pen = new Pen(Brushes.Black, 1);
        Graphics graphics1;
        Graphics graphics2;
        readonly Color backGroundColor = SystemColors.Control;
        readonly Pen newpen = new Pen(Color.FromArgb(100, Color.Blue), 1);
        int polyNumber = 0;
        int polyNumber2 = 0;
        int maxPolygonCount = 10;
        List<Point[]> polygonsList = new List<Point[]>();
        List<float[]> score = new List<float[]>();
        readonly int x1 = startPositionX + radius;
        readonly int y1 = startPositionY + radius;
        const int radius = 150;
        float maxArea = 0;
        float maxPossibleArea = 0;
        public void ButtonClick(Button button, NumericUpDown numericUpDown, ref Graphics g, ref PictureBox pictureBox, List<NumericUpDown> list)
        {
            Size size = new Size(pictureBox.Width, pictureBox.Height);
            Point zero = new Point() { X = 0, Y = 0 };
            Rectangle rect = new Rectangle(zero, size);
            button.Enabled = false;
            numericUpDown.Enabled = false;
            int dem = (int)numericUpDown.Value;
            for (int i = 0; i < dem; i++)
            {
                list[i].Enabled = true;
                list[i].Visible = true;
            }
            points = new Point[dem];
            polygon = new Point[dem];
            polygonsList.Add(polygon);
            score.Add(new float[dem]);
            DrawStartCircle(g);
            points[0].X = x1;
            points[0].Y = startPositionY;
            float x2;
            float y2;
            g.DrawLine(pen, x1, y1, x1, startPositionY);
            float degree;
            double ang;
            for (int i = 1; i < numericUpDown.Value; i++)
            {
                degree = Convert.ToSingle(360.0 / Convert.ToInt32(numericUpDown.Value));
                degree *= i;
                ang = degree * Math.PI / 180.0;
                x2 = Convert.ToSingle(x1 + (radius * Math.Sin(ang) / Math.Sin(90 * Math.PI / 180)));
                if (Math.Sin(Math.Abs(90 - degree)) == 0)
                {
                    y2 = y1;
                    g.DrawLine(pen, x1, y1, x2, y2);
                    points[i].X = Convert.ToInt32(x2);
                    points[i].Y = Convert.ToInt32(y2);
                    continue;
                }
                else
                    y2 = Convert.ToSingle((Math.Sin(Math.Abs(90 - degree) * Math.PI / 180) * radius / Math.Sin(90 * Math.PI / 180)));

                if (degree > 90)
                {
                    y2 += y1;
                }
                else
                {
                    y2 = y1 - y2;
                }
                points[i].X = Convert.ToInt32(x2);
                points[i].Y = Convert.ToInt32(y2);
                g.DrawLine(pen, x1, y1, x2, y2);

            }
            maxPossibleArea = GetPolygonArea(points);
            pictureBox.DrawToBitmap(bitmap, rect);
            pictureBox.Invalidate();
        }
        public void RandomInput(List<NumericUpDown> list)
        {
            Random rnd = new Random();
            for (int i = 0; i < list.Count - 1; i++)
            {
                list[i].Value = Convert.ToDecimal(rnd.Next(0, 101) / 10.0);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            list = new List<NumericUpDown> { numericUpDown3, numericUpDown4, numericUpDown5, numericUpDown6, numericUpDown7, numericUpDown8, numericUpDown9, numericUpDown10 };
            list1 = new List<NumericUpDown> { numericUpDown11, numericUpDown12, numericUpDown13, numericUpDown14, numericUpDown15, numericUpDown16, numericUpDown17, numericUpDown18 };
            Random rnd = new Random();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            RandomInput(list);
            RandomInput(list1);
            DisableNumerics(list);
            DisableNumerics(list1);
            button4.Enabled = false;
            numericUpDown1.Value = rnd.Next(3, 9);
            numericUpDown2.Value = rnd.Next(3, 9);
            pictureBox2.Enabled = false;
            button2.Enabled = false;
            button5.Enabled = false;
            numericUpDown2.Enabled = false;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (polyNumber >= maxPolygonCount)
            {
                button4.Enabled = false;
                button1.Enabled = false;
                pictureBox2.Enabled = true;
                button2.Enabled = true;
                pictureBox2.Enabled = true;
                button5.Enabled = true;
                numericUpDown2.Enabled = true;
                polygonsList.Clear();
                score.Clear();
                maxArea = 0;
            }
            else
            {
                bool en = false;
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    if (list[i].Enabled == true)
                    {
                        break;
                    }
                    if (i == numericUpDown1.Value - 1 && list[i].Visible == true)
                    {
                        en = true;
                    }
                }
                button4.Enabled = en;
            }
        }
        public void DisableNumerics(List<NumericUpDown> list)
        {
            foreach (var item in list)
            {
                item.Visible = false;
                item.Enabled = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ButtonClick(button1, numericUpDown1, ref graphics1, ref pictureBox1, list);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ButtonClick(button2, numericUpDown2, ref graphics2, ref pictureBox2, list1);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        public void AddPolygonPoints(float a, float b, int index, Point[] polygon)
        {
            polygon[index].X = (int)a;
            polygon[index].Y = (int)b;
        }
        public void SetLabelValues(float value, int index, int polynumber)
        {
            score[polynumber][index] = (float)value;
        }
        public void DrawString(float value, float x, float y, int index, bool flag, ref Graphics g, ref PictureBox p, int polynumber)
        {
            g = Graphics.FromImage(p.Image);
            if (polynumber > 0 && flag != false)
            { //erase previous labels

                graphics1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                for (int i = 0; i < polynumber; i++)
                {
                    g.DrawString($"{score[polynumber - 1][index]}", new Font("Microsoft Sans Serif", 8), new SolidBrush(backGroundColor), x, y);
                    g.DrawString($"{score[polynumber - 1][index]}", new Font("Microsoft Sans Serif", 8), new SolidBrush(backGroundColor), x, y);
                    g.DrawString($"{score[polynumber - 1][index]}", new Font("Microsoft Sans Serif", 8), new SolidBrush(backGroundColor), x, y);
                    g.DrawString($"{score[polynumber - 1][index]}", new Font("Microsoft Sans Serif", 8), new SolidBrush(backGroundColor), x, y);
                }
            }
            if (flag == true)
                g.DrawString($"{value}", new Font("Microsoft Sans Serif", 8), Brushes.Black, x, y);
        }
        public void DrawPoint(float value, int i, int index, ref Graphics g, ref PictureBox p, ref int polynumber, Point[] polygon, float[] score)
        {
            float a = value / 10.0f;
            float x2;
            float y2;
            if (points[i].X == x1)
            {
                //cnaging only y
                if (points[i].Y == startPositionY)
                {
                    x2 = points[i].X;
                    y2 = y1 - (radius * a);
                    //g.DrawEllipse(newpen, x2, y2, 1, 1);
                    AddPolygonPoints(x2, y2, index, polygon);
                    SetLabelValues(value, index, polynumber);
                    if (a > 0.0)
                        DrawString(value, x2 - 6, 0, index, true, ref g, ref p, polynumber);
                    p.Invalidate();
                }
                else
                {
                    x2 = points[i].X;
                    y2 = y1 + radius * a;
                    AddPolygonPoints(x2, y2, index, polygon);
                    SetLabelValues(value, index, polynumber);
                    //g.FillEllipse(Brushes.Black, new Rectangle(x1 - 3, y1 - 3, 5, 5));
                    //g.DrawEllipse(newpen, x2, y2, 1, 1);
                    if (a > 0.0)
                        DrawString(value, x2 - 6, radius * 2 + startPositionY + 5, index, true, ref g, ref p, polynumber);
                    p.Invalidate();
                }
            }
            else if (points[i].X == startPositionX || points[i].X == radius * 2 + startPositionX)
            {
                //cnaging only x
                if (points[i].X == startPositionX)
                {
                    x2 = x1 - radius * a;
                    y2 = points[i].Y;
                    AddPolygonPoints(x2, y2, index, polygon);
                    SetLabelValues(value, index, polynumber);
                    //g.DrawEllipse(newpen, x2, y2, 1, 1);
                    if (a > 0.0)
                        DrawString(value, 0, y2 - 5, index, true, ref g, ref p, polynumber);
                    p.Invalidate();
                }
                else
                {
                    x2 = x1 + radius * a;
                    y2 = points[i].Y;
                    AddPolygonPoints(x2, y2, index, polygon);
                    SetLabelValues(value, index, polynumber);
                    //g.DrawEllipse(newpen, x2, y2, 1, 1);
                    if (a > 0.0)
                        DrawString(value, radius * 2 + startPositionX + 10, y2 - 5, index, true, ref g, ref p, polynumber);
                    p.Invalidate();
                }
            }
            else
            {
                if (points[i].X > x1)
                {
                    x2 = x1 + Math.Abs(points[i].X - x1) * a;
                }
                else
                {
                    x2 = x1 - (x1 - points[i].X) * a;
                }
                if (points[i].Y > x1)
                {
                    y2 = y1 + (points[i].Y - y1) * a;
                }
                else
                {
                    y2 = y1 - (y1 - points[i].Y) * a;
                }
                if (x2 < x1)
                    if (y2 < y1) //1 плоскость
                    {
                        AddPolygonPoints(x2, y2, index, polygonsList[polynumber]);
                        SetLabelValues(value, index, polynumber);
                        if (a > 0.0)
                            DrawString(value, points[i].X - 25, points[i].Y - 10, index, true, ref g, ref p, polynumber);
                    }
                    else //4 плоскость
                    {
                        AddPolygonPoints(x2, y2, index, polygonsList[polynumber]);
                        SetLabelValues(value, index, polynumber);
                        if (a > 0.0)
                            DrawString(value, points[i].X - 17, points[i].Y + 3, index, true, ref g, ref p, polynumber);
                    }
                else
                {
                    if (y2 < y1) //2 плоскость
                    {
                        AddPolygonPoints(x2, y2, index, polygonsList[polynumber]);
                        SetLabelValues(value, index, polynumber);
                        if (a > 0.0)
                            DrawString(value, points[i].X + 5, points[i].Y - 7, index, true, ref g, ref p, polynumber);
                    }
                    else //3 плоскость
                    {
                        AddPolygonPoints(x2, y2, index, polygonsList[polynumber]);
                        SetLabelValues(value, index, polynumber);
                        if (a > 0.0)
                            DrawString(value, points[i].X + 5, points[i].Y, index, true, ref g, ref p, polynumber);
                    }
                }
                AddPolygonPoints(x2, y2, index, polygonsList[polynumber]);
                SetLabelValues(value, index, polynumber);
                //g.DrawEllipse(newpen, x2, y2, 1, 1);
                p.Invalidate();
            }
        }
        public Color GetColor(int polynumber, int index, Random rnd)
        {
            Color color;
            if (index > 0)
            {
                color = Color.FromArgb(255 - Convert.ToInt32(Convert.ToSingle((index / (float)polynumber)) * 255), rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            }
            else
                color = Color.FromArgb(255, rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            return color;
        }
        public void PaintButtonClick(ref PictureBox p, ref Graphics g, NumericUpDown numericUpDown, List<NumericUpDown> list, ref int polynumber, Label label1, Label label2)
        {
            g = Graphics.FromImage(p.Image);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Random rnd = new Random();
            float area;
            area = GetPolygonArea(polygonsList[polynumber]);
            for (int i = 0; i < polygonsList.Count; i++)
            {
                if (GetPolygonArea(polygonsList[i]) > maxArea)
                    maxArea = GetPolygonArea(polygonsList[i]); //finding max area over existing polygons
            }
            Color color;
            if (polynumber == 0)
            {
                color = GetColor(0, 0, rnd);
                g.DrawPolygon(new Pen(Color.Black, 1), polygonsList[polynumber]);
                g.FillPolygon(new SolidBrush(color), polygonsList[polynumber]); // saving blank circle to bitmap
                label1.Text = (Math.Round((area / maxPossibleArea), 2) * 100).ToString() + "%";
            }
            if (!(polynumber >= maxPolygonCount - 1))
                for (int i = 0; i < numericUpDown.Value; i++)
                {
                    list[i].Enabled = true;
                    list[i].Value = Convert.ToDecimal(rnd.Next(1, 100) / 10.0);
                }
            if (polynumber > 0 && polynumber <= maxPolygonCount - 1)
            {
                if (area >= maxArea) //if current polygon is the biggest, clearing pictbox to blank circle, then drawing each polygon
                {
                    p.Image = bitmap;
                    p.Invalidate();
                    g = Graphics.FromImage(p.Image);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    List<Point[]> tempList = new List<Point[]>();
                    for (int i = 0; i < polygonsList.Count; i++)
                    {
                        tempList.Add(polygonsList[i]);
                    }
                    tempList.Sort((e1, e2) =>
                    {
                        return GetPolygonArea(e2).CompareTo(GetPolygonArea(e1));
                    });
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        color = GetColor(tempList.Count, i, rnd);
                        g.DrawPolygon(new Pen(Color.Black, 1), tempList[i]);
                        g.FillPolygon(new SolidBrush(color), tempList[i]);
                    }
                    label1.Text = (Math.Round((GetPolygonArea(polygonsList[polynumber]) / maxPossibleArea), 2) * 100).ToString() + "%";
                }
                else
                {
                    p.Invalidate();
                    g = Graphics.FromImage(p.Image);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    color = GetColor(polygonsList.Count, polynumber, rnd);
                    g.DrawPolygon(new Pen(Color.Black, 1), polygonsList[polynumber]);
                    g.FillPolygon(new SolidBrush(color), polygonsList[polynumber]);
                    label1.Text = (Math.Round((GetPolygonArea(polygonsList[polynumber]) / maxPossibleArea), 2) * 100).ToString() + "%";
                }
            }
            label2.Text = polygonsList.Count.ToString() + "/" + maxPolygonCount.ToString();
            p.Invalidate();
            polynumber++;
            polygonsList.Add(new Point[(int)numericUpDown.Value]);
            score.Add(new float[(int)numericUpDown.Value]);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            PaintButtonClick(ref pictureBox1, ref graphics1, numericUpDown1, list, ref polyNumber, label1, label2);
        }
        public float GetPolygonArea(Point[] polygon)
        {
            int n = polygon.Length;
            float sum = polygon[0].X * (polygon[1].Y - polygon[n - 1].Y);
            for (int i = 1; i < n - 1; ++i)
            {
                sum += polygon[i].X * (polygon[i + 1].Y - polygon[i - 1].Y);
            }
            sum += polygon[n - 1].X * (polygon[0].Y - polygon[n - 2].Y);
            return 0.5f * sum;
        }
        public void HandleNumericInput(KeyPressEventArgs e, NumericUpDown numericUpDown, int i, ref Graphics g, ref PictureBox p, ref int polynumber, Button button)
        {

            if (e.KeyChar == ((char)Keys.Enter))
            {
                if (i == points.Length-1)
                {
                    button.Enabled = true;
                    button.Focus();
                }
                DrawPoint((float)numericUpDown.Value, i, i, ref g, ref p, ref polynumber, polygonsList[polynumber], score[polynumber]);
                numericUpDown.Enabled = false;
            }
        }
        private void numericUpDown3_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown3, 0, ref graphics1, ref pictureBox1, ref polyNumber,button4);
        }
        private void numericUpDown4_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown4, 1, ref graphics1, ref pictureBox1, ref polyNumber, button4);
        }
        private void numericUpDown5_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown5, 2, ref graphics1, ref pictureBox1, ref polyNumber, button4);
        }
        private void numericUpDown6_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown6, 3, ref graphics1, ref pictureBox1, ref polyNumber, button4);
        }
        private void numericUpDown7_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown7, 4, ref graphics1, ref pictureBox1, ref polyNumber, button4);
        }
        private void numericUpDown8_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown8, 5, ref graphics1, ref pictureBox1, ref polyNumber, button4);
        }
        private void numericUpDown9_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown9, 6, ref graphics1, ref pictureBox1, ref polyNumber, button4);
        }
        private void numericUpDown10_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown10, 7, ref graphics1, ref pictureBox1, ref polyNumber, button4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            PaintButtonClick(ref pictureBox2, ref graphics2, numericUpDown2, list1, ref polyNumber2, label3, label4);
        }
        private void numericUpDown11_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown11, 0, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void numericUpDown12_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown12, 1, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void numericUpDown13_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown13, 2, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void numericUpDown14_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown14, 3, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void numericUpDown15_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown15, 4, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void numericUpDown16_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown16, 5, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void numericUpDown17_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown17, 6, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void numericUpDown18_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleNumericInput(e, numericUpDown18, 7, ref graphics2, ref pictureBox2, ref polyNumber2, button5);
        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (polyNumber2 >= maxPolygonCount)
            {
                button5.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                bool en = false;
                for (int i = 0; i < numericUpDown2.Value; i++)
                {
                    if (list1[i].Enabled == true)
                    {
                        break;
                    }
                    if (i == numericUpDown2.Value - 1 && list1[i].Visible == true)
                    {
                        en = true;
                    }
                }
                button5.Enabled = en;
            }
        }
    }
}
