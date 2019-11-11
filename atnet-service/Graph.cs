using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using yrno;


namespace atnet_service
{
    public class Graph
    {
        private readonly List<ForecastRecordModel> _records;
        private int _width;
        private int _height = 200;
        private int _padding = 30;
        private int _center;
        private int _divY = 5;
        private int _hoursCount;


        private Bitmap _bmp;
        private Graphics _g;
        public Graph(List<ForecastRecordModel> records)
        {
            _records = records;
            if (records == null || records.Count <= 0) return;
            
            
            DrawEmptyGraph();
            DrawScale();
            DrawValues();
        }

        protected void DrawEmptyGraph()
        {
            _hoursCount = (int) (_records[_records.Count - 1].To - _records[0].From).TotalHours;
            _width = _padding + _hoursCount * _divY + 15;

            _bmp = new Bitmap(_width, _height);
            _g = Graphics.FromImage(_bmp);
            _center = (_height - _padding) / 2 + 5;

            // bílé plátno
            _g.FillRectangle(new SolidBrush(Color.White), 0, 0, _width, _height);
            // osa X
            _g.DrawLine(new Pen(color: Color.DimGray, width: 1), 5, _height - _padding, _width - 5, _height - _padding);
            // osa X 0
            _g.DrawLine(new Pen(color: Color.DimGray, width: 2), 25, _center, _width - 5, _center);
            // osa Y
            _g.DrawLine(new Pen(color: Color.DimGray, width: 2), _padding, 5, _padding, _height - 5);
        }

        protected void DrawScale()
        {
            Pen grayPen = new Pen(color: Color.DimGray, width: 1);
            // osa Y - kladná část
            for (int i = 1; i < 9; i++)
            {
                _g.DrawLine(grayPen, 27, _center - i * 10, 33, _center - i * 10);
                if (i % 2 == 0)
                {
                    RectangleF rect = new RectangleF(7, _center - i * 10 - 7, 27, _center - i * 10 + 5);
                    _g.DrawString((i * 5).ToString(), new Font("Tahoma", 8), Brushes.DimGray, rect);
                }
            }
            // osa Y - záporná část
            for (int i = 1; i < 9; i++)
            {
                _g.DrawLine(grayPen, 27, _center + i * 10, 33, _center + i * 10);
                if (i < 8 && i % 2 == 0)
                {
                    RectangleF rect = new RectangleF(2, _center + i * 10 - 7, 25, 25);
                    _g.DrawString((i * -5).ToString(), new Font("Tahoma", 8), Brushes.DimGray, rect);
                }
            }
            // osa X
            for (int i = 1; i < _hoursCount + 2; i++)
            {
                _g.DrawLine(grayPen, _padding + i * _divY, _height - _padding - 2, _padding + i * _divY, _height - _padding + 2);
            }
        }

        protected void DrawValues()
        {
            Pen grayPen = new Pen(color: Color.DimGray, width: 1);
            Pen redPen = new Pen(color: Color.Red, width: 1);
            Pen bluePen = new Pen(color: Color.Blue, width: 1);

            //int actX = _padding + markSize;
            DateTime firstTime = _records[0].From;

            foreach (var t in _records)
            {
                int actX = _padding + _divY + ((int) (t.From - firstTime).TotalHours) * _divY;
                int totalHours = (int) (t.To - t.From).TotalHours;
                int startHour = t.From.Hour;
                for (int i = 0; i < totalHours; i++)
                {
                    // nakreslíme čas
                    if ((startHour + i) % 6 == 0)
                    {
                        string text = (startHour + i).ToString("00");
                        if (text == "24") text = "00";
                        RectangleF rect = new RectangleF(actX - 8, _height - _padding + 2, 25, 25);
                        _g.DrawString(text, new Font("Tahoma", 8), Brushes.DimGray, rect);
                        // uděláme svislici na rozhraní dne
                        if (text == "00")
                        {
                            _g.DrawLine(new Pen(color: Color.LightGray, width: 1), actX, 5, actX, _height - _padding - 5);
                        }
                        else if (text == "12")
                        {
                            RectangleF rect1 = new RectangleF(actX - 30, 5, 60, 15);
                            _g.DrawString(t.From.ToShortDateString(), new Font("Tahoma", 8), Brushes.DimGray, rect1);
                        }
                    }

                    // nakreslíme teplotu
                    float temperature = (float)t.Temperature;
                    if (temperature <= 5)
                    {
                        _g.DrawEllipse(bluePen, actX, _center - 2 * temperature, 2, 2);
                        _g.FillEllipse(new SolidBrush(Color.Blue), actX, _center - 2 * temperature, 2, 2);
                    }
                    else
                    {
                        _g.DrawEllipse(redPen, actX, _center - 2 * temperature, 2, 2);
                        _g.FillEllipse(new SolidBrush(Color.Red), actX, _center - 2 * temperature, 2, 2);
                    }
                    
                    actX += _divY;
                }
            }
        }

        public void Save()
        {
            _bmp.Save(@"D:\atnet.png", ImageFormat.Png);
        }

        ~Graph()
        {
            _g.Dispose();
            _bmp.Dispose();
        }
    }
}
