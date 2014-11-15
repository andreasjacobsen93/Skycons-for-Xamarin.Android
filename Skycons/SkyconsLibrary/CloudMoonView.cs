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
    public class CloudMoonView : View
    {
        Paint paintCloud, paintMoon;
        Path pathMoon;
        private int screenW, screenH;
        private float X, Y, X2, Y2;
        PathPoints[] pathPoints;
        float m = 0;
        float radius;
        bool clockwise = false;
        float a = 0, b = 0, c = 0, d = 0;
        private double count;
        Cloud cloud;

        bool isStatic;
        bool isAnimated;
        Color strokeColor;
        Color bgColor;

        public CloudMoonView(Context context)
            : this(context,null)
        {
        }

        public CloudMoonView(Context context, IAttributeSet attrs) : base(context, attrs)
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
            //Paint for drawing cloud
            paintCloud = new Paint {StrokeCap = (Paint.Cap.Round), StrokeJoin = (Paint.Join.Round)};
            paintCloud.SetStyle(Paint.Style.Stroke);
            paintCloud.AntiAlias = (true);
            paintCloud.SetShadowLayer(0, 0, 0, strokeColor);

            //Paint for drawing Moon
            paintMoon = new Paint();
            paintMoon.Color = (strokeColor);
            paintMoon.AntiAlias = (true);
            paintMoon.StrokeCap = (Paint.Cap.Round);
            paintMoon.SetStyle(Paint.Style.Stroke);

            count = 0;
            cloud = new Cloud();

            isAnimated = true;

        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            screenW = w;
            screenH = h;

            X = screenW / 2;
            Y = (screenH / 2);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // set canvas background color
            canvas.DrawColor(bgColor);

            paintCloud.StrokeWidth = ((float)(0.02083 * screenW));
            paintMoon.StrokeWidth = ((float)(0.02083 * screenW));

            count = count + 0.5;
            int retval = Double.Compare(count, 360.00);

            if (retval == 0)
            {
                if (!isAnimated)
                {
                    // mark completion of animation
                    isAnimated = true;
                    //resetting counter on completion of a rotation
                    count = 0;
                }
                else
                {
                    //resetting counter on completion of a rotation
                    count = 0;
                }
            }

            // Moon shape
            pathMoon = new Path();
            RectF rectF1 = new RectF();

            PointF P5c1 = cloud.getP5c1(X, Y, screenW, count);
            if (X2 == 0)
            {
                X2 = P5c1.X;
                Y2 = P5c1.Y + (int)(0.042 * screenW);

                radius = (int)(0.1042 * screenW);
            }

            if (!clockwise)
            { //Anticlockwise rotation

                // First arc of the Moon.
                rectF1.Set(X2 - radius, Y2 - radius, X2 + radius, Y2 + radius);
                pathMoon.AddArc(rectF1, 65 - (m / 2), 275);

                pathPoints = getPoints(pathMoon);

                a = pathPoints[999].getX();
                b = pathPoints[999].getY();
                c = pathPoints[0].getX();
                d = pathPoints[0].getY();

                PointF p1 = cubic2Points(a, b, c, d, true);
                PointF p2 = cubic2Points(a, b, c, d, false);

                // Second arc of the Moon in opposite face.
                pathMoon.MoveTo(a, b);
                pathMoon.CubicTo(p1.X, p1.Y, p2.X, p2.Y, c, d);

                canvas.DrawPath(pathMoon, paintMoon);

                m = m + 0.5f;

                if (m == 100)
                {
                    m = 0;
                    clockwise = !clockwise;
                }

            }
            else
            { //Clockwise rotation

                // First arc of the Moon.
                rectF1.Set(X2 - radius, Y2 - radius, X2 + radius, Y2 + radius);
                pathMoon.AddArc(rectF1, 15 + (m / 2), 275);

                pathPoints = getPoints(pathMoon);

                a = pathPoints[999].getX();
                b = pathPoints[999].getY();
                c = pathPoints[0].getX();
                d = pathPoints[0].getY();

                PointF p1 = cubic2Points(a, b, c, d, true);
                PointF p2 = cubic2Points(a, b, c, d, false);

                // Second arc of the Moon in opposite face.
                pathMoon.MoveTo(a, b);
                pathMoon.CubicTo(p1.X, p1.Y, p2.X, p2.Y, c, d);

                canvas.DrawPath(pathMoon, paintMoon);

                m = m + 0.5f;

                if (m == 100)
                {
                    m = 0;
                    clockwise = !clockwise;
                }

            }

            // drawing cloud with fill
            paintCloud.Color = (bgColor);
            paintCloud.SetStyle(Paint.Style.Fill);
            canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

            // drawing cloud with stroke
            paintCloud.Color = (strokeColor);
            paintCloud.SetStyle(Paint.Style.Stroke);
            canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

            if (!isStatic || !isAnimated)
            {
                Invalidate();
            }
        }

        // Used to get cubic 2 points between staring & end coordinates.
        private PointF cubic2Points(float x1, float y1, float x2,
                                         float y2, bool left)
        {

            PointF result = new PointF(0, 0);
            // finding center point between the coordinates
            float dy = y2 - y1;
            float dx = x2 - x1;
            // calculating angle and the distance between center and the two points
            float dangle = (float)((Math.Atan2(dy, dx) - Math.Pi / 2f));
            float sideDist = (float)-0.5 * (float)Math.Sqrt(dx * dx + dy * dy); //square

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
        private PathPoints[] getPoints(Path path)
        {

            //Size of 1000 indicates that, 1000 points
            // would be extracted from the path
            PathPoints[] pointArray = new PathPoints[1000];
            PathMeasure pm = new PathMeasure(path, false);
            float length = pm.Length;
            float distance = 0f;
            float speed = length / 1000;
            int counter = 0;
            float[] aCoordinates = new float[2];

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