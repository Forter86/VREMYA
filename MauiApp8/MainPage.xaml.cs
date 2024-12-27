using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Timers;

namespace DigitalClockMaui
{
    public partial class MainPage : ContentPage
    {
        private readonly System.Timers.Timer _timer;
        private readonly float _digitWidth = 50;
        private readonly float _digitHeight = 80;
        private readonly float _digitSpacing = 10;
        private readonly float _colonWidth = 10;
        private readonly float _xStart = 50;
        private readonly float _yStart = 50;
        private readonly Color _digitColor = Colors.Black;
        private readonly Color _colonColor = Colors.Black;
        private readonly float _segmentThickness = 8;
        private DateTime _currentTime;
        private PointF _digitTopLeftPoint;
        private ClockDrawable _clockDrawable;

        public MainPage()
        {
            InitializeComponent();
            _digitTopLeftPoint = new PointF(_xStart, _yStart);
            _currentTime = DateTime.Now;
            _clockDrawable = new ClockDrawable(this);
            ClockCanvas.Drawable = _clockDrawable;

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _currentTime = DateTime.Now;
            Dispatcher.Dispatch(() => ClockCanvas.Invalidate());
        }

        public class ClockDrawable : IDrawable
        {
            private readonly MainPage _page;

            public ClockDrawable(MainPage page)
            {
                _page = page;
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                // Clear Canvas
                canvas.FillColor = Colors.White;
                canvas.FillRectangle(dirtyRect);

                var hours = _page._currentTime.Hour;
                var minutes = _page._currentTime.Minute;
                var seconds = _page._currentTime.Second;

                DrawDigits(canvas, hours / 10, _page._digitTopLeftPoint);
                DrawDigits(canvas, hours % 10, new PointF(_page._digitTopLeftPoint.X + _page._digitWidth + _page._digitSpacing, _page._digitTopLeftPoint.Y));

                DrawColon(canvas, new PointF(_page._digitTopLeftPoint.X + _page._digitWidth * 2 + _page._digitSpacing * 2, _page._digitTopLeftPoint.Y));

                DrawDigits(canvas, minutes / 10, new PointF(_page._digitTopLeftPoint.X + _page._digitWidth * 2 + _page._digitSpacing * 2 + _page._colonWidth + _page._digitSpacing, _page._digitTopLeftPoint.Y));
                DrawDigits(canvas, minutes % 10, new PointF(_page._digitTopLeftPoint.X + _page._digitWidth * 3 + _page._digitSpacing * 3 + _page._colonWidth + _page._digitSpacing, _page._digitTopLeftPoint.Y));

                DrawColon(canvas, new PointF(_page._digitTopLeftPoint.X + _page._digitWidth * 4 + _page._digitSpacing * 4 + _page._colonWidth * 2 + _page._digitSpacing, _page._digitTopLeftPoint.Y));

                DrawDigits(canvas, seconds / 10, new PointF(_page._digitTopLeftPoint.X + _page._digitWidth * 4 + _page._digitSpacing * 4 + _page._colonWidth * 2 + _page._digitSpacing * 2, _page._digitTopLeftPoint.Y));
                DrawDigits(canvas, seconds % 10, new PointF(_page._digitTopLeftPoint.X + _page._digitWidth * 5 + _page._digitSpacing * 5 + _page._colonWidth * 2 + _page._digitSpacing * 2, _page._digitTopLeftPoint.Y));
            }

            private void DrawDigits(ICanvas canvas, int digit, PointF point)
            {
                switch (digit)
                {
                    case 0:
                        DrawSegment(canvas, point, true, true, true, false, true, true, true);
                        break;
                    case 1:
                        DrawSegment(canvas, point, false, false, true, false, false, true, false);
                        break;
                    case 2:
                        DrawSegment(canvas, point, true, false, true, true, true, false, true);
                        break;
                    case 3:
                        DrawSegment(canvas, point, true, false, true, true, false, true, true);
                        break;
                    case 4:
                        DrawSegment(canvas, point, false, true, true, true, false, true, false);
                        break;
                    case 5:
                        DrawSegment(canvas, point, true, true, false, true, false, true, true);
                        break;
                    case 6:
                        DrawSegment(canvas, point, true, true, false, true, true, true, true);
                        break;
                    case 7:
                        DrawSegment(canvas, point, true, false, true, false, false, true, false);
                        break;
                    case 8:
                        DrawSegment(canvas, point, true, true, true, true, true, true, true);
                        break;
                    case 9:
                        DrawSegment(canvas, point, true, true, true, true, false, true, true);
                        break;
                }
            }

            private void DrawSegment(ICanvas canvas, PointF point, bool a, bool b, bool c, bool d, bool e, bool f, bool g)
            {
                float segmentThickness = _page._segmentThickness;
                float digitWidth = _page._digitWidth;
                float digitHeight = _page._digitHeight;
                Color segmentColor = _page._digitColor;

                if (a)
                {
                    canvas.FillColor = segmentColor;
                    canvas.FillRoundedRectangle(new RectF(point.X, point.Y, digitWidth, segmentThickness), 10);
                }

                if (b)
                {
                    canvas.FillColor = segmentColor;
                    canvas.FillRoundedRectangle(new RectF(point.X, point.Y, segmentThickness, digitHeight / 2), 10);
                }

                if (c)
                {
                    canvas.FillColor = segmentColor;
                    canvas.FillRoundedRectangle(new RectF(point.X + digitWidth - segmentThickness, point.Y, segmentThickness, digitHeight / 2), 10);
                }

                if (d)
                {
                    canvas.FillColor = segmentColor;
                    canvas.FillRoundedRectangle(new RectF(point.X, point.Y + digitHeight / 2, digitWidth, segmentThickness), 10);
                }

                if (e)
                {
                    canvas.FillColor = segmentColor;
                    canvas.FillRoundedRectangle(new RectF(point.X, point.Y + digitHeight / 2, segmentThickness, digitHeight / 2), 10);
                }

                if (f)
                {
                    canvas.FillColor = segmentColor;
                    canvas.FillRoundedRectangle(new RectF(point.X + digitWidth - segmentThickness, point.Y + digitHeight / 2, segmentThickness, digitHeight / 2), 10);
                }

                if (g)
                {
                    canvas.FillColor = segmentColor;
                    canvas.FillRoundedRectangle(new RectF(point.X, point.Y + digitHeight - segmentThickness, digitWidth, segmentThickness), 10);
                }
            }

            private void DrawColon(ICanvas canvas, PointF point)
            {
                canvas.FillColor = _page._colonColor;
                canvas.FillCircle(new PointF(point.X + _page._colonWidth / 2, point.Y + _page._digitHeight / 4), 5);
                canvas.FillCircle(new PointF(point.X + _page._colonWidth / 2, point.Y + _page._digitHeight / 4 + _page._digitHeight / 2), 5);
            }
        }
    }
}