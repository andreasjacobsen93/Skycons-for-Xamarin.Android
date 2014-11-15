/*
 * 
 * SKYCONS for Android
 * https://github.com/torryharris/Skycons
 * 
 * Ported to C# by Andreas Jacobsen, 2014
 * 
 */

using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace Learn2Run.UI.Android.Skycons
{
    public class MoonView : View
    {
        Paint paint;
        Path path;
        private int screenW, screenH;
        private float X, Y;
        PathPoints[] pathPoints;
        float m = 0;
        float radius;
        bool clockwise = false;
        float a = 0, b = 0, c = 0, d = 0;

        bool isStatic;
        bool isAnimated;
        Color strokeColor;
        Color bgColor;
        int count = 0; //counter for stopping animation

        // Used to get cubic 2 points between staring & end coordinates.
        public MoonView(Context context) : this(context,null)
        {
        }

        public MoonView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.custom_view);

            // get attributes from layout
            isStatic = a.GetBoolean(Resource.Styleable.custom_view_isStatic, this.isStatic);
            strokeColor = a.GetColor(Resource.Styleable.custom_view_strokeColor, this.strokeColor);

            if (strokeColor == 0)
            {
                strokeColor = Color.White;
            }

            bgColor = a.GetColor(Resource.Styleable.custom_view_bgColor, this.bgColor);

            if (bgColor == 0)
            {
                bgColor = Colors.Dark;
            }

            init();
        }

        private void init()
        {
            //Paint for drawing Moon
            paint = new Paint();
            paint.Color = (strokeColor);
            paint.SetStyle(Paint.Style.Stroke);
            paint.AntiAlias = (true);
            paint.StrokeCap = (Paint.Cap.Round);

            isAnimated = true;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            screenW = w;
            screenH = h;

            X = screenW / 2;
            Y = (screenH / 2);

            radius = (int)(0.1458 * screenW);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // set canvas background color
            canvas.DrawColor(bgColor);

            paint.StrokeWidth = ((float)(0.02083 * screenW));

            path = new Path();

            RectF rectF1 = new RectF();

            if (!clockwise)
            {//Anticlockwise rotation

                // First arc of the Moon.
                rectF1.Set(X - radius, Y - radius, X + radius, Y + radius);
                path.AddArc(rectF1, 65 - (m / 2), 275);

                pathPoints = getPoints(path);

                a = pathPoints[999].getX();
                b = pathPoints[999].getY();
                c = pathPoints[0].getX();
                d = pathPoints[0].getY();

                PointF P1c1 = cubic2Points(a, b, c, d, true);
                PointF P1c2 = cubic2Points(a, b, c, d, false);

                // Second arc of the Moon in opposite face.
                path.MoveTo(a, b);
                path.CubicTo(P1c1.X, P1c1.Y, P1c2.X, P1c2.Y, c, d);

                canvas.DrawPath(path, paint);

                m = m + 0.5f;

                if (m == 100)
                {
                    m = 0;
                    clockwise = !clockwise;
                }

            }
            else
            {//Clockwise rotation

                // First arc of the Moon.
                rectF1.Set(X - radius, Y - radius, X + radius, Y + radius);
                path.AddArc(rectF1, 15 + (m / 2), 275);

                pathPoints = getPoints(path);

                a = pathPoints[999].getX();
                b = pathPoints[999].getY();
                c = pathPoints[0].getX();
                d = pathPoints[0].getY();

                PointF P1c1 = cubic2Points(a, b, c, d, true);
                PointF P1c2 = cubic2Points(a, b, c, d, false);

                // Second arc of the Moon in opposite face.
                path.MoveTo(a, b);
                path.CubicTo(P1c1.X, P1c1.Y, P1c2.X, P1c2.Y, c, d);

                canvas.DrawPath(path, paint);

                m = m + 0.5f;

                if (m == 100)
                {
                    m = 0;
                    clockwise = !clockwise;

                    if (!isAnimated)
                    {
                        count++;
                    }

                }

            }

            if (!isStatic || !isAnimated)
            {

                if (count < 3)
                {
                    // invalidate if not static or not animating
                    Invalidate();

                }
                else
                {
                    count = 0;
                }

            }
        }

        private PointF cubic2Points(float x1, float y1, float x2,
                                         float y2, bool left)
        {

            PointF result = new PointF(0, 0);
            // finding center point between the coordinates
            float dy = y2 - y1;
            float dx = x2 - x1;
            // calculating angle and the distance between center and the two points
            float dangle = (float)((Math.Atan2(dy, dx) - Math.Pi / 2f));
            float sideDist = (float)-0.6 * (float)Math.Sqrt(dx * dx + dy * dy); //square

            if (left)
            {
                //point from center to the left
                result.X = (int)(Math.Cos(dangle) * sideDist + x1);
                result.Y = (int)(Math.Sin(dangle) * sideDist + y1);

            }
            else
            {
                //point from center to the right
                result.X = (int)(Math.Cos(dangle) * sideDist + x2);
                result.Y = (int)(Math.Sin(dangle) * sideDist + y2);
            }

            return result;
        }


        // Used to fetch points from given path.
        private PathPoints[] getPoints(Path p)
        {

            //Size of 1000 indicates that, 1000 points
            // would be extracted from the path
            var pointArray = new PathPoints[1000];
            var pm = new PathMeasure(p, false);
            var length = pm.Length;
            var distance = 0f;
            var speed = length / 1000;
            var counter = 0;
            var aCoordinates = new float[2];

            while ((distance < length) && (counter < 1000))
            {
                pm.GetPosTan(distance, aCoordinates, null);
                pointArray[counter] = new PathPoints(aCoordinates[0], aCoordinates[1]);
                counter++;
                distance = distance + speed;
            }

            return pointArray;
        }

        // Class for fetching path coordinates.
        class PathPoints
        {

            float x, y;

            public PathPoints(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public float getX()
            {
                return x;
            }

            public float getY()
            {
                return y;
            }

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    return true;
                case MotionEventActions.Move:
                    break;
                case MotionEventActions.Up:
                    if (isStatic && isAnimated)
                    {
                        isAnimated = false;
                    }
                    break;
                default:
                    return false;
            }

            if (!isAnimated)
            {
                Invalidate();
            }

            return true;
        }
    }
}