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
    public class CloudRainView : View
    {
        private static Paint paintCloud, paintRain;
        private int screenW, screenH;
        private float X, Y;
        private Path pathRain;
        int x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0;
        float m = 0;
        bool drop1 = true, drop2 = false, drop3 = false;
        private double count;
        Cloud cloud;

        bool isStatic;
        bool isAnimated;
        Color strokeColor;
        Color bgColor;

        public CloudRainView(Context context) : this(context, null)
        {
        }

        public CloudRainView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.custom_view);

            // get attributes from layout
            isStatic = a.GetBoolean(Resource.Styleable.custom_view_isStatic, isStatic);
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

            paintCloud = new Paint();
            paintRain = new Paint();

            //Paint for drawing cloud
            paintCloud.Color = (strokeColor);
            paintCloud.StrokeWidth = (10);
            paintCloud.AntiAlias = (true);
            paintCloud.StrokeCap = (Paint.Cap.Round);
            paintCloud.StrokeJoin = (Paint.Join.Round);
            paintCloud.SetStyle(Paint.Style.Stroke);
            paintCloud.SetShadowLayer(0, 0, 0, strokeColor);

            //Paint for drawing rain drops
            paintRain.Color = (strokeColor);
            paintRain.AntiAlias = (true);
            paintRain.StrokeCap = (Paint.Cap.Round);
            paintRain.SetStyle(Paint.Style.Fill);

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
            paintRain.StrokeWidth = ((float)(0.015 * screenW));

            pathRain = new Path(); // pathCloud for drop

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

            PointF P1c2 = cloud.getP1c2(X, Y, screenW, count);
            PointF P1c1 = cloud.getP1c2(X, Y, screenW, count);
            float P1Y = ((float)((int)(0.1041667 * screenW) *
                    Math.Sin(Math.ToRadians(80 + (0.111 * count))) + Y));

            PointF P2c2 = cloud.getP2c2(X, Y, screenW, count);
            PointF P2c1 = cloud.getP2c2(X, Y, screenW, count);
            float P2Y = ((float)(((int)(0.1041667 * screenW) + ((0.00023125 * screenW) * count))
                    * Math.Sin(Math.ToRadians(120 + (0.222 * count))) + Y));

            if (isAnimated && isStatic)
            { //Initial static view

                pathRain = new Path();

                // 1st drop
                if (x1 == 0)
                {
                    x1 = (int)P1c2.X;
                }
                if (y1 == 0)
                {
                    float value = (int)P1c2.Y - ((P1c1.Y + P1Y) / 2);
                    y1 = (int)(P1c2.Y - value / 2);
                }

                m = 95;
                // Shape for rain drop
                pathRain.MoveTo(x1, y1);
                pathRain.AddArc(new RectF(x1 - 5, (y1 - 5) + m, x1 + 5, y1 + 5 + m), 180, -180);
                pathRain.LineTo(x1, (y1 - 10) + m);
                pathRain.Close();


                // 2nd drop
                if (x2 == 0)
                {
                    x2 = (int)P2c2.X;
                }
                if (y2 == 0)
                {
                    float value = (int)P2c2.Y - ((P2c1.Y + P2Y) / 2);
                    y2 = (int)(P2c2.Y - value / 2);
                }

                pathRain.MoveTo(x2, y2);
                pathRain.AddArc(new RectF(x2 - 5, (y2 - 5) + m, x2 + 5, y2 + 5 + m), 180, -180);
                pathRain.LineTo(x2, (y2 - 10) + m);
                pathRain.Close();

                // 3rd drop
                if (x3 == 0)
                {
                    x3 = (x1 + x2) / 2;
                }
                if (y3 == 0)
                {
                    y3 = (y1 + y2) / 2;
                }

                pathRain.MoveTo(x3, y3);
                pathRain.AddArc(new RectF(x3 - 5, (y3 - 5) + m / 2, x3 + 5, y3 + 5 + m / 2), 180, -180);
                pathRain.LineTo(x3, (y3 - 10) + m / 2);
                pathRain.Close();


            } else { // Animating view

                if(drop1) { // Drop 1 of the rain

                    pathRain = new Path();

                    if(x1==0) {
                        x1 = (int) P1c2.X;
                    }
                    if(y1==0) {
                        float value = (int) P1c2.Y-((P1c1.Y+P1Y)/2);
                        y1 = (int) (P1c2.Y-value/2);
                    }

                    // Shape for rain drop
                    pathRain.MoveTo(x1, y1);
                    pathRain.AddArc(new RectF(x1 - 5, (y1 - 5) + m, x1 + 5, y1 + 5 + m), 180, -180);
                    pathRain.LineTo(x1, (y1 - 10) + m);
                    pathRain.Close();

                    if(m==100) {
                        m=0;
                        pathRain.Reset();
                        pathRain.MoveTo(0, 0);
                        drop2 = true;
                        drop1 = false;
                    }
                }

                if(drop2) { // Drop 2 of the rain
                    pathRain = new Path();

                    if(x2==0) {
                        x2 = (int) P2c2.X;
                    }
                    if(y2==0) {
                        float value = (int) P2c2.Y-((P2c1.Y+P2Y)/2);
                        y2 = (int) (P2c2.Y-value/2);
                    }

                    pathRain.MoveTo(x2, y2);
                    pathRain.AddArc(new RectF(x2 - 5, (y2 - 5) + m, x2 + 5, y2 + 5 + m), 180, -180);
                    pathRain.LineTo(x2, (y2 - 10) + m);
                    pathRain.Close();

                    if(m==100) {
                        m=0;
                        pathRain.Reset();
                        pathRain.MoveTo(0, 0);
                        drop2 = false;
                        drop3 = true;
                    }
                }

                if(drop3) 
                { 
                    // Drop 3 of the rain
                    pathRain = new Path();

                    if(x3==0) {
                        x3 = (x1+x2)/2;
                    }
                    if(y3==0) {
                        y3 = (y1+y2)/2;
                    }

                    pathRain.MoveTo(x3, y3);
                    pathRain.AddArc(new RectF(x3 - 5, (y3 - 5) + m, x3 + 5, y3 + 5 + m), 180, -180);
                    pathRain.LineTo(x3, (y3 - 10) + m);
                    pathRain.Close();

                    if(m==100) {
                        m=0;
                        pathRain.Reset();
                        pathRain.MoveTo(0, 0);
                        // animate = false;
                        drop3 = false;
                        drop1 = true;
                    }
                }


                // First fill the shape with paint
                paintRain.SetStyle(Paint.Style.Fill);
                canvas.DrawPath(pathRain, paintRain);

                // Then, draw the same pathCloud with paint stroke
                paintRain.SetStyle(Paint.Style.Stroke);
                canvas.DrawPath(pathRain, paintRain);

                m = m + 2.5f;

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