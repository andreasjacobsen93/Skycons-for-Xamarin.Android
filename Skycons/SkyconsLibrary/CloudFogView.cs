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
    public class CloudFogView : View
    {
        private Paint paintCloud, paintFog;
        private bool expanding = false;
        private bool moving = true;
        private bool isStatic;
        private bool isAnimated;
        private Color strokeColor;
        private Color bgColor;

        float ctr = 0;
        float i, j;
        private int screenW, screenH;
        bool check;
        private float X, Y;
        private Path path1, path2;
        private double count;
        Cloud cloud;
        bool compress = false;
        float line1Y = 0, line2Y = 0, lineStartX, lineEndX;


        public CloudFogView(Context context) : this(context, null)
        {
            init();
        }

        public CloudFogView(Context context, IAttributeSet attrs) : base(context, attrs)
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
                bgColor = Color.Black;
            }
            init();
        }

        public CloudFogView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            init();
        }

        private void init()
        {
            count = 0;
            i = 0f;
            j = (int)0.5;
            check = false;

            if (isStatic)
            {
                isAnimated = false;
            }
            else
            {
                isAnimated = true;
            }
            paintCloud = new Paint();
            paintFog = new Paint();

            //Setting paint for cloud
            paintCloud.Color = (strokeColor);
            paintCloud.AntiAlias = (true);
            paintCloud.StrokeCap = (Paint.Cap.Round);
            paintCloud.StrokeJoin = (Paint.Join.Round);
            paintCloud.SetStyle(Paint.Style.Stroke);
            paintCloud.SetShadowLayer(0, 0, 0, Color.Black);

            //Setting paint for fog
            paintFog.Color = (strokeColor);
            paintFog.AntiAlias = (true);
            paintFog.StrokeCap = (Paint.Cap.Round);
            paintFog.StrokeJoin = (Paint.Join.Round);
            paintFog.SetStyle(Paint.Style.FillAndStroke);
            paintFog.SetShadowLayer(0, 0, 0, Color.Black);

            cloud = new Cloud();
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            screenH = h;
            screenW = w;

            X = screenW/2;
            Y = screenH/2;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // canvas.DrawColor(bgColor);

            paintCloud.StrokeWidth = ((float)(0.02083 * screenW));
            paintFog.StrokeWidth = ((float)(0.02083 * screenW));

            path1 = new Path();
            path2 = new Path();

            count = count + 0.5;

            int retval = Double.Compare(count, 360.00);

            if (retval == 0)
            {
                count = 0;
            }

            canvas.DrawPath(cloud.getCloud(X, Y, screenW, count), paintCloud);
            PointF P1c2 = cloud.getP1c2(X, Y, screenW, count);

            path1 = new Path();
            path2 = new Path();

            if (line1Y == 0)
            {
                line1Y = P1c2.Y + (float)(0.1042 * screenW);   //Calculating Y coordinate for foglines.
                line2Y = P1c2.Y + (float)(0.15625 * screenW);

                lineStartX = (float)(X - X * 50.0 / 100.0);  //Calculating X coordinate for foglines.
                lineEndX = (float)(X + X * 50.0 / 100);
            }

            float temp = (lineEndX - lineStartX) * (float)95.0 / (float)100; //Calculating fogline length

            path1.MoveTo(lineStartX, line1Y);
            path1.LineTo(lineStartX + temp, line1Y);

            path2.MoveTo(lineEndX, line2Y);
            path2.LineTo(lineEndX - temp, line2Y);

            //Code to move foglines from one point to another

            if (moving && (lineStartX + temp + ctr) <= lineEndX)
            {

                path1.Reset();
                path1.MoveTo(lineStartX + ctr + i, line1Y);
                path1.LineTo(lineStartX + ctr + temp + i, line1Y);

                path2.Reset();
                path2.MoveTo(lineEndX - ctr + i + i, line2Y);
                path2.LineTo(lineEndX - ctr - temp + i - i, line2Y);

                ctr = ctr + (float)0.5;
                if ((lineStartX + temp + ctr) > lineEndX)
                {
                    expanding = true;
                    moving = false;
                }
            }

            //Code to expand foglines

            if (expanding)
            {

                if (i <= 5f)
                {
                    i = i + 0.1f;
                    path1.Reset();
                    path1.MoveTo(lineStartX + ctr + temp + i, line1Y);
                    path1.LineTo(lineStartX + ctr - i, line1Y);

                    path2.Reset();
                    path2.MoveTo(lineEndX - ctr - temp + i, line2Y);
                    path2.LineTo(lineEndX - ctr - i, line2Y);


                }
                else
                {
                    //Moving the fogline to the other end after expanding

                    path1.Reset();
                    path1.MoveTo(lineStartX + ctr + temp + i, line1Y);
                    path1.LineTo(lineStartX + ctr - i, line1Y);

                    path2.Reset();
                    path2.MoveTo(lineEndX - ctr - temp + i, line2Y);
                    path2.LineTo(lineEndX - ctr - i, line2Y);

                    ctr = ctr - 0.2f;
                    if (lineStartX + ctr <= lineStartX)
                    {
                        expanding = false;
                        compress = true;
                        ctr = 0.0f;
                    }
                }
            }


            //Compressing the fogline to normal length
            if (compress)
            {

                if (i > 0.0f)
                {
                    i = i - 0.1f;
                    path1.Reset();
                    path1.MoveTo(lineStartX + ctr - i, line1Y);
                    path1.LineTo(lineStartX + ctr + temp + i, line1Y);

                    path2.Reset();
                    path2.MoveTo(lineEndX - ctr - i, line2Y);
                    path2.LineTo(lineEndX - ctr - temp + i, line2Y);

                }
                else
                {
                    compress = false;
                    moving = true;
                    if (isStatic)
                    {
                        isAnimated = false;
                    }

                }

            }


            canvas.DrawPath(path1, paintFog);
            canvas.DrawPath(path2, paintFog);

            if (isAnimated)
            {
                Invalidate();
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!isStatic) return true;

            switch (e.Action)
            {
                case MotionEventActions.Up:
                    isAnimated = true;
                    Invalidate();
                    break;
            }

            return true;
        }
    }
}