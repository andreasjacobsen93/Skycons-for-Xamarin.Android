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
    public class CloudThunderView : View
    {
        int ctr = 0;
        int ctr2 = 0;
        Color strokeColor;
        Color bgColor;
        float thHeight;
        bool isStatic;
        bool isAnimated;
        PathPoints[] leftPoints;
        bool check;
        private Paint paintCloud, paintThunder;
        private int screenW, screenH;
        private float X, Y;
        private Path thPath, thFillPath;

        private double count;
        Cloud cloud;

        public CloudThunderView(Context context) : this(context,null)
        {
        }

        public CloudThunderView(Context context, IAttributeSet attrs) : base(context, attrs)
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
            count = 0;
            check = false;
            thHeight = 0;
            thPath = new Path();
            thFillPath = new Path();

            if (isStatic)
            {
                isAnimated = false;
            }
            else
            {
                isAnimated = true;
            }
            paintCloud = new Paint
            {
                Color = (strokeColor),
                StrokeWidth = ((screenW/25)),
                AntiAlias = (true),
                StrokeCap = (Paint.Cap.Round),
                StrokeJoin = (Paint.Join.Round)
            };
            paintCloud.SetStyle(Paint.Style.Stroke);
            paintCloud.SetShadowLayer(0, 0, 0, Color.Black);

            paintThunder = new Paint
            {
                Color = (strokeColor),
                StrokeWidth = (10),
                AntiAlias = (true),
                StrokeCap = (Paint.Cap.Round),
                StrokeJoin = (Paint.Join.Round)
            };
            paintThunder.SetStyle(Paint.Style.Stroke);
            paintThunder.SetShadowLayer(0, 0, 0, Color.Black);

            cloud = new Cloud();

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
            canvas.DrawColor(bgColor);

            paintCloud.StrokeWidth = ((float)(0.02083 * screenW));
            paintThunder.StrokeWidth = ((float)(0.02083 * screenW));

            //incrementing counter for rotation
            count = count + 0.5;

            //comparison to check 360 degrees rotation
            int retval = Double.Compare(count, 360.00);

            if (retval == 0)
            {
                //resetting counter on completion of a rotation
                count = 0;
            }

            PointF P2c1 = cloud.getP2c1(X, Y, screenW, count);

            //Setting up the height of thunder from the cloud
            if (thHeight == 0)
            {
                thHeight = P2c1.Y;
            }
            float startHeight = thHeight - (thHeight * 0.1f);

            //Setting up X coordinates of thunder
            float path2StartX = X + (X * 0.04f);



            //Calculating coordinates of thunder

            thPath.MoveTo(path2StartX, startHeight);


            thPath.LineTo(X - (X * 0.1f), startHeight + (startHeight * 0.2f)); //1


            thPath.LineTo(X + (X * 0.03f), startHeight + (startHeight * 0.15f));


            thPath.LineTo(X - (X * 0.08f), startHeight + (startHeight * 0.3f));


            leftPoints = getPoints(thPath);

            if (ctr <= 98)
            {

                if (check == false)
                {

                    thFillPath.MoveTo(leftPoints[ctr].getX(), leftPoints[ctr].getY());
                    thFillPath.LineTo(leftPoints[ctr + 1].getX(), leftPoints[ctr + 1].getY());



                }
                else
                {
                    //Once filled, erasing the fill from top to bottom
                    thFillPath.Reset();
                    thFillPath.MoveTo(leftPoints[ctr].getX(), leftPoints[ctr].getY());
                    for (int i = ctr + 1; i < leftPoints.Length - 1; i++)
                    {
                        thFillPath.LineTo(leftPoints[i].getX(), leftPoints[i].getY());

                    }
                }
                ctr = ctr + 1;
            }
            else
            {
                if (isStatic)
                {
                    if (ctr2 == 2)
                    {
                        isAnimated = false;
                        ctr2 = 0;
                    }
                    ctr2++;
                }

                ctr = 0;
                if (check == false)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }


            }

            if (!isAnimated)
            {
                thFillPath.Reset();
                thFillPath.MoveTo(leftPoints[0].getX(), leftPoints[0].getY());
                for (int i = ctr + 1; i < leftPoints.Length - 1; i++)
                {
                    thFillPath.LineTo(leftPoints[i].getX(), leftPoints[i].getY());

                }
            }

            canvas.DrawPath(thFillPath, paintThunder);

            paintCloud.SetStyle(Paint.Style.Fill);
            paintCloud.Color = (Colors.Dark);
            canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

            paintCloud.SetStyle(Paint.Style.Stroke);
            paintCloud.Color = (Color.White);
            canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

            if (isAnimated)
            {
                Invalidate();
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

        private PathPoints[] getPoints(Path path)
        {
            PathPoints[] pointArray = new PathPoints[100];
            PathMeasure pm = new PathMeasure(path, false);
            float length = pm.Length;
            float distance = 0f;
            float speed = length / 100;
            int counter = 0;
            float[] aCoordinates = new float[2];

            while ((distance < length) && (counter < 100))
            {
                // get point from the pathMoon
                pm.GetPosTan(distance, aCoordinates, null);
                pointArray[counter] = new PathPoints(aCoordinates[0], aCoordinates[1]);
                counter++;
                distance = distance + speed;
            }

            return pointArray;
        }

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
         
    }
}