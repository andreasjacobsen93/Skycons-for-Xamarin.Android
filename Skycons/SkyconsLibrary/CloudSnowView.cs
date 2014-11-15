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
    public class CloudSnowView : View
    {
        private Paint paintCloud, paintSnow;

        PathPoints[] pathPoints11, pathPoints12, pathPoints21, pathPoints22,
                pointsCircle11, pointsCircle12, pointsCircle21, pointsCircle22;

        private int screenW, screenH;
        private float X, Y;

        private Path path11, path12, path13,
                path21, path22, path23, //visible drawn paths

                cubicPath11, cubicPath12,
                cubicPath21, cubicPath22, //Invisible paths for drop movement

                pathCircle1, pathCircle2; //Invisible paths for rotate operation

        int m = 0, n = 0, x1 = 0, y1 = 0, x2 = 0, y2 = 0;

        bool drop11 = true, drop12 = false, drop21 = false,
                drop22 = false, pointsStored = false;

        private double count;

        Cloud cloud;

        bool isStatic;
        bool isAnimated;
        Color strokeColor;
        Color bgColor;

        public CloudSnowView(Context context) : this(context, null)
        {
        }

        public CloudSnowView(Context context, IAttributeSet attrs) : base(context, attrs)
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
            paintSnow = new Paint();

            paintCloud.Color = (strokeColor);
            paintCloud.AntiAlias = (true);
            paintCloud.StrokeCap = (Paint.Cap.Round);
            paintCloud.StrokeJoin = (Paint.Join.Round);
            paintCloud.SetStyle(Paint.Style.Stroke);
            paintCloud.SetShadowLayer(0, 0, 0, strokeColor);

            paintSnow.Color = (strokeColor);
            paintSnow.AntiAlias = (true);
            paintSnow.StrokeCap = (Paint.Cap.Round);
            paintSnow.SetStyle(Paint.Style.Stroke);

            cloud = new Cloud();
            pathCircle1 = new Path();

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

            count = count + 0.5;

            paintCloud.StrokeWidth = ((float)(0.02083 * screenW));
            paintSnow.StrokeWidth = ((float)(0.01 * screenW));

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

            PointF P1c1 = cloud.getP1c1(X, Y, screenW, count);
            PointF P1c2 = cloud.getP1c2(X, Y, screenW, count);
            PointF P2c1 = cloud.getP2c1(X, Y, screenW, count);
            PointF P2c2 = cloud.getP2c2(X, Y, screenW, count);

            float P1Y = ((float)((int)(0.1041667 * screenW) * Math.Sin(Math.ToRadians(80 + (0.111 * count))) + Y));
            float P2Y = ((float)(((int)(0.1041667 * screenW) + ((0.00023125 * screenW) * count))
                    * Math.Sin(Math.ToRadians(120 + (0.222 * count))) + Y));


            if (x1 == 0)
            {
                x1 = (int)P1c2.X + 10;
            }
            if (y1 == 0)
            {
                float value = (int)P1c2.Y - ((P1c1.Y + P1Y) / 2);
                y1 = (int)(P1c2.Y - value / 2);
            }

            if (x2 == 0)
            {
                x2 = (int)P2c2.X + 10;
            }
            if (y2 == 0)
            {
                float value = (int)P2c2.Y - ((P2c1.Y + P2Y) / 2);
                y2 = (int)(P2c2.Y - value / 2);
            }

            if (!pointsStored)
            {

                // Store path coordinates for snow fall 1
                cubicPath11 = new Path();
                int height = screenH - y1;
                cubicPath11.MoveTo(x1, y1);
                cubicPath11.CubicTo(x1 - screenW * 0.06f, y1 + height * 0.3f, x1 - screenW * 0.12f,
                        y1 + height * 0.7f, x1 - screenW * 0.18f, y1 + height * 1.1f);
                pathPoints11 = getPoints(cubicPath11);

                // Store path coordinates for snow fall 2
                cubicPath12 = new Path();
                int x = x1 - 5;
                cubicPath12.MoveTo(x, y1);
                cubicPath12.CubicTo(x + screenW * 0.06f, y1 + height * 0.3f, x + screenW * 0.1f,
                        y1 + height * 0.7f, x - screenW * 0.03f, y1 + height * 1.1f);
                pathPoints12 = getPoints(cubicPath12);

                // Store path coordinates for snow fall 3
                cubicPath21 = new Path();
                cubicPath21.MoveTo(x2, y2);
                cubicPath21.CubicTo(x2 + screenW * 0.06f, y2 + height * 0.3f, x2 + screenW * 0.12f,
                        y2 + height * 0.7f, x2 + screenW * 0.18f, y2 + height * 1.1f);
                pathPoints21 = getPoints(cubicPath21);

                // Store path coordinates for snow fall 4
                cubicPath22 = new Path();
                int xx = x2 + 5;
                cubicPath22.MoveTo(xx, y2);
                cubicPath22.CubicTo(xx - screenW * 0.06f, y2 + height * 0.3f, xx - screenW * 0.1f,
                        y2 + height * 0.6f, xx + screenW * 0.03f, y2 + height * 1.1f);
                pathPoints22 = getPoints(cubicPath22);

                pointsStored = true;
            } if (isAnimated && isStatic)
            { //Initial static view

                int x = 55;

                pathCircle2 = new Path();
                pathCircle2.AddCircle(pathPoints12[x].getX(), pathPoints12[x].getY(),
                        screenW * 0.03f, Path.Direction.Cw);
                pointsCircle12 = getPoints(pathCircle2);

                //2nd drop
                path21 = new Path();
                path22 = new Path();
                path23 = new Path();

                int a = (25 + x / 5) >= 100 ? 25 + x / 5 - 100 : 25 + x / 5;
                int b = (8 + x / 5) >= 100 ? 8 + x / 5 - 100 : 8 + x / 5;
                int c = (40 + x / 5) >= 100 ? 40 + x / 5 - 100 : 40 + x / 5;

                path21.MoveTo(pointsCircle12[a].getX(), pointsCircle12[a].getY());
                path22.MoveTo(pointsCircle12[b].getX(), pointsCircle12[b].getY());
                path23.MoveTo(pointsCircle12[c].getX(), pointsCircle12[c].getY());

                a = (75 + x / 5) >= 100 ? 75 + x / 5 - 100 : 75 + x / 5;
                b = (59 + x / 5) >= 100 ? 59 + x / 5 - 100 : 59 + x / 5;
                c = (90 + x / 5) >= 100 ? 90 + x / 5 - 100 : 90 + x / 5;

                path21.LineTo(pointsCircle12[a].getX(), (pointsCircle12[a].getY()));
                path22.LineTo(pointsCircle12[b].getX(), (pointsCircle12[b].getY()));
                path23.LineTo(pointsCircle12[c].getX(), (pointsCircle12[c].getY()));

                canvas.DrawPath(path21, paintSnow);
                canvas.DrawPath(path22, paintSnow);
                canvas.DrawPath(path23, paintSnow);

                // drawing cloud with fill
                paintCloud.Color = (bgColor);
                paintCloud.SetStyle(Paint.Style.Fill);
                canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                // drawing cloud with stroke
                paintCloud.Color = (strokeColor);
                paintCloud.SetStyle(Paint.Style.Stroke);
                canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                int y = 35;

                pathCircle2 = new Path();
                pathCircle2.AddCircle(pathPoints22[y].getX(), pathPoints22[y].getY(),
                        screenW * 0.03f, Path.Direction.Cw);
                pointsCircle22 = getPoints(pathCircle2);

                //2nd drop
                path21 = new Path();
                path22 = new Path();
                path23 = new Path();

                a = (25 + y / 5) >= 100 ? 25 + y / 5 - 100 : 25 + y / 5;
                b = (8 + y / 5) >= 100 ? 8 + y / 5 - 100 : 8 + y / 5;
                c = (40 + y / 5) >= 100 ? 40 + y / 5 - 100 : 40 + y / 5;

                path21.MoveTo(pointsCircle22[a].getX(), pointsCircle22[a].getY());
                path22.MoveTo(pointsCircle22[b].getX(), pointsCircle22[b].getY());
                path23.MoveTo(pointsCircle22[c].getX(), pointsCircle22[c].getY());

                a = (75 + y / 5) >= 100 ? 75 + y / 5 - 100 : 75 + y / 5;
                b = (59 + y / 5) >= 100 ? 59 + y / 5 - 100 : 59 + y / 5;
                c = (90 + y / 5) >= 100 ? 90 + y / 5 - 100 : 90 + y / 5;

                path21.LineTo(pointsCircle22[a].getX(), (pointsCircle22[a].getY()));
                path22.LineTo(pointsCircle22[b].getX(), (pointsCircle22[b].getY()));
                path23.LineTo(pointsCircle22[c].getX(), (pointsCircle22[c].getY()));

                canvas.DrawPath(path21, paintSnow);
                canvas.DrawPath(path22, paintSnow);
                canvas.DrawPath(path23, paintSnow);

                // drawing cloud with fill
                paintCloud.Color = (bgColor);
                paintCloud.SetStyle(Paint.Style.Fill);
                canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                // drawing cloud with stroke
                paintCloud.Color = (strokeColor);
                paintCloud.SetStyle(Paint.Style.Stroke);
                canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);


            } else { // Animating view

                if (drop11)
                {

                    pathCircle1 = new Path();
                    pathCircle1.AddCircle(pathPoints11[m].getX(), pathPoints11[m].getY(),
                        screenW*0.03f, Path.Direction.Cw);
                    pointsCircle11 = getPoints(pathCircle1);

                    //1st drop
                    path11 = new Path();
                    path12 = new Path();
                    path13 = new Path();

                    int a = (25 + m/5) >= 100 ? 25 + m/5 - 100 : 25 + m/5;
                    int b = (8 + m/5) >= 100 ? 8 + m/5 - 100 : 8 + m/5;
                    int c = (40 + m/5) >= 100 ? 40 + m/5 - 100 : 40 + m/5;

                    path11.MoveTo(pointsCircle11[a].getX(), pointsCircle11[a].getY());
                    path12.MoveTo(pointsCircle11[b].getX(), pointsCircle11[b].getY());
                    path13.MoveTo(pointsCircle11[c].getX(), pointsCircle11[c].getY());

                    a = (75 + m/5) >= 100 ? 75 + m/5 - 100 : 75 + m/5;
                    b = (59 + m/5) >= 100 ? 59 + m/5 - 100 : 59 + m/5;
                    c = (90 + m/5) >= 100 ? 90 + m/5 - 100 : 90 + m/5;

                    path11.LineTo(pointsCircle11[a].getX(), (pointsCircle11[a].getY()));
                    path12.LineTo(pointsCircle11[b].getX(), (pointsCircle11[b].getY()));
                    path13.LineTo(pointsCircle11[c].getX(), (pointsCircle11[c].getY()));

                    canvas.DrawPath(path11, paintSnow);
                    canvas.DrawPath(path12, paintSnow);
                    canvas.DrawPath(path13, paintSnow);

                    // drawing cloud with fill
                    paintCloud.Color = (bgColor);
                    paintCloud.SetStyle(Paint.Style.Fill);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    // drawing cloud with stroke
                    paintCloud.Color = (strokeColor);
                    paintCloud.SetStyle(Paint.Style.Stroke);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    m = m + 1;

                    if (m > 75)
                    {

                        pathCircle2 = new Path();
                        pathCircle2.AddCircle(pathPoints12[n].getX(), pathPoints12[n].getY(),
                            screenW*0.03f, Path.Direction.Cw);
                        pointsCircle12 = getPoints(pathCircle2);

                        //2nd drop
                        path21 = new Path();
                        path22 = new Path();
                        path23 = new Path();

                        a = (25 + n/5) >= 100 ? 25 + n/5 - 100 : 25 + n/5;
                        b = (8 + n/5) >= 100 ? 8 + n/5 - 100 : 8 + n/5;
                        c = (40 + n/5) >= 100 ? 40 + n/5 - 100 : 40 + n/5;

                        path21.MoveTo(pointsCircle12[a].getX(), pointsCircle12[a].getY());
                        path22.MoveTo(pointsCircle12[b].getX(), pointsCircle12[b].getY());
                        path23.MoveTo(pointsCircle12[c].getX(), pointsCircle12[c].getY());

                        a = (75 + n/5) >= 100 ? 75 + n/5 - 100 : 75 + n/5;
                        b = (59 + n/5) >= 100 ? 59 + n/5 - 100 : 59 + n/5;
                        c = (90 + n/5) >= 100 ? 90 + n/5 - 100 : 90 + n/5;

                        path21.LineTo(pointsCircle12[a].getX(), (pointsCircle12[a].getY()));
                        path22.LineTo(pointsCircle12[b].getX(), (pointsCircle12[b].getY()));
                        path23.LineTo(pointsCircle12[c].getX(), (pointsCircle12[c].getY()));

                        canvas.DrawPath(path21, paintSnow);
                        canvas.DrawPath(path22, paintSnow);
                        canvas.DrawPath(path23, paintSnow);

                        // drawing cloud with fill
                        paintCloud.Color = (bgColor);
                        paintCloud.SetStyle(Paint.Style.Fill);
                        canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                        // drawing cloud with stroke
                        paintCloud.Color = (strokeColor);
                        paintCloud.SetStyle(Paint.Style.Stroke);
                        canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                        n = n + 1;

                    }

                    if (m == 100)
                    {
                        m = 0;
                        path11.Reset();
                        path11.MoveTo(0, 0);
                        path12.Reset();
                        path12.MoveTo(0, 0);
                        path13.Reset();
                        path13.MoveTo(0, 0);

                        x1 = 0;
                        y1 = 0;

                        x2 = 0;
                        y2 = 0;

                        drop12 = true;
                        drop11 = false;

                    }
                }

                if (drop12)
                {

                    pathCircle2 = new Path();
                    pathCircle2.AddCircle(pathPoints12[n].getX(), pathPoints12[n].getY(),
                            screenW * 0.03f, Path.Direction.Cw);
                    pointsCircle12 = getPoints(pathCircle2);

                    //2nd drop
                    path21 = new Path();
                    path22 = new Path();
                    path23 = new Path();

                    int a = (25 + n / 5) >= 100 ? 25 + n / 5 - 100 : 25 + n / 5;
                    int b = (8 + n / 5) >= 100 ? 8 + n / 5 - 100 : 8 + n / 5;
                    int c = (40 + n / 5) >= 100 ? 40 + n / 5 - 100 : 40 + n / 5;

                    path21.MoveTo(pointsCircle12[a].getX(), pointsCircle12[a].getY());
                    path22.MoveTo(pointsCircle12[b].getX(), pointsCircle12[b].getY());
                    path23.MoveTo(pointsCircle12[c].getX(), pointsCircle12[c].getY());

                    a = (75 + n / 5) >= 100 ? 75 + n / 5 - 100 : 75 + n / 5;
                    b = (59 + n / 5) >= 100 ? 59 + n / 5 - 100 : 59 + n / 5;
                    c = (90 + n / 5) >= 100 ? 90 + n / 5 - 100 : 90 + n / 5;

                    path21.LineTo(pointsCircle12[a].getX(), (pointsCircle12[a].getY()));
                    path22.LineTo(pointsCircle12[b].getX(), (pointsCircle12[b].getY()));
                    path23.LineTo(pointsCircle12[c].getX(), (pointsCircle12[c].getY()));

                    canvas.DrawPath(path21, paintSnow);
                    canvas.DrawPath(path22, paintSnow);
                    canvas.DrawPath(path23, paintSnow);

                    // drawing cloud with fill
                    paintCloud.Color = (bgColor);
                    paintCloud.SetStyle(Paint.Style.Fill);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    // drawing cloud with stroke
                    paintCloud.Color = (strokeColor);
                    paintCloud.SetStyle(Paint.Style.Stroke);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    n = n + 1;

                    if (n == 100)
                    {
                        m = 0;
                        n = 0;
                        path21.Reset();
                        path21.MoveTo(0, 0);
                        path22.Reset();
                        path22.MoveTo(0, 0);
                        path23.Reset();
                        path23.MoveTo(0, 0);

                        x1 = 0;
                        y1 = 0;

                        drop21 = true;
                        drop11 = false;
                        drop12 = false;

                    }

                }

            if(drop21) {

                pathCircle1 = new Path();
                pathCircle1.AddCircle(pathPoints21[m].getX(), pathPoints21[m].getY(),
                        screenW*0.03f, Path.Direction.Cw);
                pointsCircle21 = getPoints(pathCircle1);

                //1st drop
                path11 = new Path();
                path12 = new Path();
                path13 = new Path();

                int a = (25+m/5) >= 100 ? 25+m/5 - 100 : 25+m/5;
                int b = (8+m/5) >= 100 ? 8+m/5 - 100 : 8+m/5;
                int c = (40+m/5) >= 100 ? 40+m/5 - 100 : 40+m/5;

                path11.MoveTo(pointsCircle21[a].getX(), pointsCircle21[a].getY());
                path12.MoveTo(pointsCircle21[b].getX(), pointsCircle21[b].getY());
                path13.MoveTo(pointsCircle21[c].getX(), pointsCircle21[c].getY());

                a = (75+m/5) >= 100 ? 75+m/5 - 100 : 75+m/5;
                b = (59+m/5) >= 100 ? 59+m/5 - 100 : 59+m/5;
                c = (90+m/5) >= 100 ? 90+m/5 - 100 : 90+m/5;

                path11.LineTo(pointsCircle21[a].getX(), (pointsCircle21[a].getY()));
                path12.LineTo(pointsCircle21[b].getX(), (pointsCircle21[b].getY()));
                path13.LineTo(pointsCircle21[c].getX(), (pointsCircle21[c].getY()));

                canvas.DrawPath(path11, paintSnow);
                canvas.DrawPath(path12, paintSnow);
                canvas.DrawPath(path13, paintSnow);

                // drawing cloud with fill
                paintCloud.Color = (bgColor);
                paintCloud.SetStyle(Paint.Style.Fill);
                canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                // drawing cloud with stroke
                paintCloud.Color = (strokeColor);
                paintCloud.SetStyle(Paint.Style.Stroke);
                canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                m = m+1;

                if(m > 75) {

                    pathCircle2 = new Path();
                    pathCircle2.AddCircle(pathPoints22[n].getX(), pathPoints22[n].getY(),
                            screenW*0.03f, Path.Direction.Cw);
                    pointsCircle22 = getPoints(pathCircle2);

                    //2nd drop
                    path21 = new Path();
                    path22 = new Path();
                    path23 = new Path();

                    a = (25+n/5) >= 100 ? 25+n/5 - 100 : 25+n/5;
                    b = (8+n/5) >= 100 ? 8+n/5 - 100 : 8+n/5;
                    c = (40+n/5) >= 100 ? 40+n/5 - 100 : 40+n/5;

                    path21.MoveTo(pointsCircle22[a].getX(), pointsCircle22[a].getY());
                    path22.MoveTo(pointsCircle22[b].getX(), pointsCircle22[b].getY());
                    path23.MoveTo(pointsCircle22[c].getX(), pointsCircle22[c].getY());

                    a = (75+n/5) >= 100 ? 75+n/5 - 100 : 75+n/5;
                    b = (59+n/5) >= 100 ? 59+n/5 - 100 : 59+n/5;
                    c = (90+n/5) >= 100 ? 90+n/5 - 100 : 90+n/5;

                    path21.LineTo(pointsCircle22[a].getX(), (pointsCircle22[a].getY()));
                    path22.LineTo(pointsCircle22[b].getX(), (pointsCircle22[b].getY()));
                    path23.LineTo(pointsCircle22[c].getX(), (pointsCircle22[c].getY()));

                    canvas.DrawPath(path21, paintSnow);
                    canvas.DrawPath(path22, paintSnow);
                    canvas.DrawPath(path23, paintSnow);

                    // drawing cloud with fill
                    paintCloud.Color = (bgColor);
                    paintCloud.SetStyle(Paint.Style.Fill);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    // drawing cloud with stroke
                    paintCloud.Color = (strokeColor);
                    paintCloud.SetStyle(Paint.Style.Stroke);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    n = n+1;

                }

                if(m==100) {
                    m=0;
                    path11.Reset();
                    path11.MoveTo(0, 0);
                    path12.Reset();
                    path12.MoveTo(0, 0);
                    path13.Reset();
                    path13.MoveTo(0, 0);

                    x1=0;
                    y1=0;

                    drop22 = true;
                    drop21 = false;
                }

            }

                if (drop22)
                {

                    pathCircle2 = new Path();
                    pathCircle2.AddCircle(pathPoints22[n].getX(), pathPoints22[n].getY(),
                        screenW*0.03f, Path.Direction.Cw);
                    pointsCircle22 = getPoints(pathCircle2);

                    //2nd drop
                    path21 = new Path();
                    path22 = new Path();
                    path23 = new Path();

                    int a = (25 + n/5) >= 100 ? 25 + n/5 - 100 : 25 + n/5;
                    int b = (8 + n/5) >= 100 ? 8 + n/5 - 100 : 8 + n/5;
                    int c = (40 + n/5) >= 100 ? 40 + n/5 - 100 : 40 + n/5;

                    path21.MoveTo(pointsCircle22[a].getX(), pointsCircle22[a].getY());
                    path22.MoveTo(pointsCircle22[b].getX(), pointsCircle22[b].getY());
                    path23.MoveTo(pointsCircle22[c].getX(), pointsCircle22[c].getY());

                    a = (75 + n/5) >= 100 ? 75 + n/5 - 100 : 75 + n/5;
                    b = (59 + n/5) >= 100 ? 59 + n/5 - 100 : 59 + n/5;
                    c = (90 + n/5) >= 100 ? 90 + n/5 - 100 : 90 + n/5;

                    path21.LineTo(pointsCircle22[a].getX(), (pointsCircle22[a].getY()));
                    path22.LineTo(pointsCircle22[b].getX(), (pointsCircle22[b].getY()));
                    path23.LineTo(pointsCircle22[c].getX(), (pointsCircle22[c].getY()));

                    canvas.DrawPath(path21, paintSnow);
                    canvas.DrawPath(path22, paintSnow);
                    canvas.DrawPath(path23, paintSnow);

                    // drawing cloud with fill
                    paintCloud.Color = (bgColor);
                    paintCloud.SetStyle(Paint.Style.Fill);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    // drawing cloud with stroke
                    paintCloud.Color = (strokeColor);
                    paintCloud.SetStyle(Paint.Style.Stroke);
                    canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);

                    n = n + 1;

                    if (n == 100)
                    {
                        m = 0;
                        n = 0;
                        path21.Reset();
                        path21.MoveTo(0, 0);
                        path22.Reset();
                        path22.MoveTo(0, 0);
                        path23.Reset();
                        path23.MoveTo(0, 0);

                        x1 = 0;
                        y1 = 0;

                        drop11 = true;
                        drop12 = false;
                        drop21 = false;
                        drop22 = false;

                    }
                }

            }

            if(!isStatic || !isAnimated) {
                Invalidate();
            }

        }

        // Used to fetch points from given path.
        private PathPoints[] getPoints(Path path)
        {

            //Size of 100 indicates that, 100 points
            // would be extracted from the path
            PathPoints[] pointArray = new PathPoints[100];
            PathMeasure pm = new PathMeasure(path, false);
            float length = pm.Length;
            float distance = 0f;
            float speed = length / 100;
            int counter = 0;
            float[] aCoordinates = new float[2];

            while ((distance < length) && (counter < 100))
            {
                pm.GetPosTan(distance, aCoordinates, null);
                pointArray[counter] = new PathPoints(aCoordinates[0], aCoordinates[1]);
                counter++;
                distance = distance + speed;
            }

            return pointArray;
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
    }
}