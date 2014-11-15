/*
 * 
 * SKYCONS for Android
 * https://github.com/torryharris/Skycons
 * 
 * Ported to C# by Andreas Jacobsen, 2014
 * 
 */

using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace Learn2Run.UI.Android.Skycons
{
    public class SunView : View
    {
        private static Paint paint;
        private int screenW, screenH;
		private int sX, sY;
        private Path path, path1;
        private double count;
        bool isAnimated = true;
        Color strokeColor;

		public SunView(Context context) : base(context)
		{
			init();
		}

        public SunView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            init();
        }

        private void init()
        {
			strokeColor = Color.White;

			sX = screenW / 2;
			sY = screenH / 2;		

            // initialize default values
            count = 0;
            paint = new Paint
            {
                Color = (strokeColor),
                AntiAlias = (true),
                StrokeCap = (Paint.Cap.Round),
                StrokeJoin = (Paint.Join.Round)
            };

            paint.SetStyle(Paint.Style.Stroke);
            paint.SetShadowLayer(0, 0, 0, Color.Black);

            path = new Path();
            isAnimated = true;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            screenW = w; //getting Screen Width
            screenH = h;// getting Screen Height

            // center point  of Screen
            sX = screenW / 2;
            sY = screenH / 2;

			/*if (path == null)
				init();*/

            path.MoveTo(sX, sY);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // set canvas background color
            //canvas.DrawColor(bgColor);

            // initializing paths
            path = new Path();
            path1 = new Path();
            // set stroke width
            paint.StrokeWidth = ((float)(0.02083 * screenW));

            //incrementing counter for rotation
            count = count + 0.1;

            //comparison to check 360 degrees rotation
            var retval = count.CompareTo(360.00);

            if (retval > 0)
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
            path.AddCircle(sX, sY, (int)(0.1042 * screenW), Path.Direction.Cw);
            
            // drawing arms of sun
            for (var i = 0; i < 360; i += 45)
            {
                path1.MoveTo(sX, sY);
                var x1 = (float)((int)(0.1458 * screenW) * Math.Cos(Java.Lang.Math.ToRadians(i + count)) + sX); //arm pointX at radius 50 with incrementing angle from center of sun
                var y1 = (float)((int)(0.1458 * screenW) * Math.Sin(Java.Lang.Math.ToRadians(i + count)) + sY);//arm pointY at radius 50 with incrementing angle from center of sun
                var X2 = (float)((int)(0.1875 * screenW) * Math.Cos(Java.Lang.Math.ToRadians(i + count)) + sX);//arm pointX at radius 65 with incrementing angle from center of sun
                var Y2 = (float)((int)(0.1875 * screenW) * Math.Sin(Java.Lang.Math.ToRadians(i + count)) + sY);//arm pointY at radius 65 with incrementing angle from center of sun
                path1.MoveTo(x1, y1); // draw arms of sun
                path1.LineTo(X2, Y2);

            }

            // drawing paths on canvas
            canvas.DrawPath(path, paint);
            canvas.DrawPath(path1, paint);

            if (isAnimated)
            {
                // invalidate if not static or not animating
                Invalidate();
            }
        }
    }
}