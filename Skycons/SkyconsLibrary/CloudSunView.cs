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
    public class CloudSunView : View
    {
        private static Paint paint;
        private int screenW, screenH;
        private float X, Y, XSun, YSun;
        private Path path, path1;
        private double count;
        int degrees;
        float startAngle;
        float sweepAngle;
        Cloud cloud;

        bool isStatic;
        bool isAnimated;
        Color strokeColor;
        Color bgColor;

        public CloudSunView(Context context) : this(context, null)
        {
        }

        public CloudSunView(Context context, IAttributeSet attrs) : base(context, attrs)
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
            // initialize default values
            degrees = 0;
            count = 0;
            startAngle = 45;
            sweepAngle = 165;
            isAnimated = true;
            paint = new Paint
            {
                Color = (strokeColor),
                StrokeWidth = (10),
                AntiAlias = (true),
                StrokeCap = (Paint.Cap.Round),
                StrokeJoin = (Paint.Join.Round)
            };

            paint.SetStyle(Paint.Style.Stroke);
            paint.SetShadowLayer(0, 0, 0, Color.Black);

            cloud = new Cloud();
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            screenW = w; //getting Screen Width
            screenH = h;// getting Screen Height

            // center point of Screen
            X = screenW / 2;
            Y = (screenH / 2);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // set canvas background color
            // canvas.DrawColor(bgColor);
            
            // set stroke width
            paint.StrokeWidth = ((float) (0.02083 * screenW));

            // initializing paths
            path = new Path();
            path1 = new Path();

            // positioning Sun with respect to cloud
            PointF P5c1 = cloud.getP5c1(X,Y,screenW,count);
            if (XSun == 0)
            {
                // center point for Sun
                XSun = P5c1.X;
                YSun = P5c1.Y + (int) (0.042*screenW);
            }

            //incrementing counter for rotation
            count = count + 0.5;

            //comparison to check 360 degrees rotation
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

            // drawing center circle
            path.AddCircle(XSun, YSun, (int)(0.083 * screenW), Path.Direction.Cw);

            // drawing arms of sun
            for (int i = 0; i < 360; i += 45)
            {
                path1.MoveTo(XSun, YSun);
                float x1 = (float)((int)(0.1146 * screenW) * Math.Cos(Math.ToRadians(i + count / 4)) + XSun); //arm pointX at radius 50 with incrementing angle from center of sun
                float y1 = (float)((int)(0.1146 * screenW) * Math.Sin(Math.ToRadians(i + count / 4)) + YSun);//arm pointY at radius 50 with incrementing angle from center of sun
                float X2 = (float)((int)(0.1563 * screenW) * Math.Cos(Math.ToRadians(i + count / 4)) + XSun);//arm pointX at radius 65 with incrementing angle from center of sun
                float Y2 = (float)((int)(0.1563 * screenW) * Math.Sin(Math.ToRadians(i + count / 4)) + YSun);//arm pointY at radius 65 with incrementing angle from center of sun
                path1.MoveTo(x1, y1); // draw arms of sun
                path1.LineTo(X2, Y2);

            }

            // drawing sun
            canvas.DrawPath(path, paint);
            canvas.DrawPath(path1, paint);

            // drawing cloud with fill
            paint.Color = (bgColor);
            paint.SetStyle(Paint.Style.Fill);
            canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paint);

            // drawing cloud with stroke
            paint.Color = (strokeColor);
            paint.SetStyle(Paint.Style.Stroke);
            canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paint);


            if (!isStatic || !isAnimated)
            {
                // invalidate if not static or not animating
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
                    return true;
                case MotionEventActions.Up:
                    if (isStatic && isAnimated) isAnimated = false;
                    break;
                default:
                    return false;
            }

            if (!isAnimated) Invalidate();

            return true;
        }
    }
}