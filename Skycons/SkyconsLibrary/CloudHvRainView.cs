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
    public class CloudHvRainView : View
    {
        private static Paint paintCloud, paintRain;
        private int screenW, screenH;
        private float X, Y;
        private Path path1, path2, path3;
        int m1 = 0, m2 = 0, m3 = 0, x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0;
        int count1 = 0, count2 = 0, count3 = 0, i = 0;
        private double count;
        bool pointsStored = false;
        double radius1, radius2;
        Cloud cloud;

        bool isStatic;
        bool isAnimated;
        Color strokeColor;
        Color bgColor;

        public CloudHvRainView(Context context) : this(context, null)
        {
        }

        public CloudHvRainView(Context context, IAttributeSet attrs) : base(context, attrs)
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
            paintRain.SetStyle(Paint.Style.FillAndStroke);

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

            radius1 = 90;
            radius2 = 50;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // set canvas background color
            canvas.DrawColor(bgColor);

            path1 = new Path(); // pathCloud for drop 1
            path2 = new Path(); // pathCloud for drop 2
            path3 = new Path(); // pathCloud for drop 3

            count = count + 0.5;

            paintCloud.StrokeWidth = ((float) (0.02083*screenW));
            paintRain.StrokeWidth = ((float) (0.015*screenW));

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
            PointF P2c2 = cloud.getP2c2(X, Y, screenW, count);
            PointF P1c1 = cloud.getP1c1(X, Y, screenW, count);

            float P1Y = ((float) ((int) (0.1041667*screenW)*Math.Sin(Math.ToRadians(80 + (0.111*count))) + Y));

            // Store starting x, y coordinates of rain drops
            if (!pointsStored)
            {
                x1 = (int) P1c2.X;
                x2 = (int) P2c2.X;
                x3 = (x1 + x2)/2;

                float value = (int) P1c2.Y - ((P1c1.Y + P1Y)/2);
                y1 = y2 = y3 = (int) (P1c2.Y - value/2) - 20;

                pointsStored = true;

            }

            if (isAnimated && isStatic)
            {
                //Initial static view
                int m = 95;

                path1.MoveTo(x1, y1 + (m - 24));
                path1.LineTo(x1, y1 + m + (float) (Y*0.1));
                canvas.DrawPath(path1, paintRain);

                path2.MoveTo(x2, y2 + (m - 24));
                path2.LineTo(x2, y2 + m + (float) (Y*0.1));
                canvas.DrawPath(path2, paintRain);

                path3.MoveTo(x3, y3 + ((m - 50) - 24));
                path3.LineTo(x3, y3 + (m - 50) + (float) (Y*0.1));
                canvas.DrawPath(path3, paintRain);

            }
            else
            {
                // Animating view

                if (i <= 2*49)
                {

                    if (i < 2*25)
                    {

                        //drop11 logic
                        if (m1 < 24)
                        {
                            path1.MoveTo(x1, y1);

                        }
                        else
                        {
                            count1 = count1 + 4;
                            path1.MoveTo(x1, y1 + count1);
                        }

                        path1.LineTo(x1, y1 + m1 + (float) (Y*0.1));
                        canvas.DrawPath(path1, paintRain);

                        m1 = m1 + 4;

                        if (m1 == 100)
                        {
                            m1 = 0;
                            count1 = 0;
                        }

                        //drop21 logic
                        if (i > 2*10)
                        {

                            if (m2 < 24)
                            {
                                path2.MoveTo(x2, y2);

                            }
                            else
                            {
                                count2 = count2 + 4;
                                path2.MoveTo(x2, y2 + count2);
                            }

                            path2.LineTo(x2, y2 + m2 + (float) (Y*0.1));
                            canvas.DrawPath(path2, paintRain);

                            m2 = m2 + 4;

                            if (m2 == 100)
                            {
                                m2 = 0;
                                count2 = 0;
                            }

                        }

                    }

                    if (i >= 2*25 && i <= 2*49)
                    {

                        // drop 3
                        if (m3 < 24)
                        {
                            path3.MoveTo(x3, y3);

                        }
                        else
                        {
                            count3 = count3 + 4;
                            path3.MoveTo(x3, y3 + count3);
                        }

                        path3.LineTo(x3, y3 + m3 + (float) (Y*0.1));
                        canvas.DrawPath(path3, paintRain);

                        m3 = m3 + 4;

                        if (m3 == 100)
                        {
                            m3 = 0;
                            count3 = 0;
                        }

                        // drop21
                        if (i < 2*36)
                        {

                            if (m2 < 24)
                            {
                                path2.MoveTo(x2, y2);

                            }
                            else
                            {
                                count2 = count2 + 4;
                                path2.MoveTo(x2, y2 + count2);
                            }

                            path2.LineTo(x2, y2 + m2 + (float) (Y*0.1));
                            canvas.DrawPath(path2, paintRain);

                            m2 = m2 + 4;

                            if (m2 == 100)
                            {
                                m2 = 0;
                                count2 = 0;
                            }

                        }

                    }


                }

                i += 2;

                if (i == 2*50)
                {
                    i = 0;
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