/*
 * 
 * SKYCONS for Android
 * https://github.com/torryharris/Skycons
 * 
 * Ported to C# by Andreas Jacobsen, 2014
 * 
 */

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace Learn2Run.UI.Android.Skycons
{
    public class CloudView : View 
    {

        private Paint paint;
        private int screenW, screenH;
        private float X, Y;
        private double count;
        Cloud c;
        bool isStatic;
        bool isAnimated;
        Color strokeColor;
        Color bgColor;

        public CloudView(Context context) : this(context, null) { }

        public CloudView(Context context, IAttributeSet attrs) : base(context, attrs) 
        {
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.custom_view);

            // get attributes from layout
            isStatic =    a.GetBoolean(Resource.Styleable.custom_view_isStatic, isStatic);
            strokeColor =    a.GetColor(Resource.Styleable.custom_view_strokeColor, strokeColor);
            if(strokeColor == 0){
                strokeColor = Color.White;
            }
            bgColor =    a.GetColor(Resource.Styleable.custom_view_bgColor, this.bgColor);
            if(bgColor == 0){
                bgColor = Color.Black;
            }
            init();
        }


        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            screenW = w; //getting Screen Width
            screenH = h; // getting Screen Height

            // center point  of Screen
            X = screenW/2;
            Y = (screenH/2);

            c =  new Cloud();
        }

        private void init() {

            // initialize default values
            count = 0;
            isAnimated = true;

            paint = new Paint
            {
                Color = (strokeColor),
                StrokeWidth = ((screenW/25)),
                AntiAlias = (true),
                StrokeCap = (Paint.Cap.Round),
                StrokeJoin = (Paint.Join.Round)
            };
            paint.SetStyle(Paint.Style.Stroke);
            paint.SetShadowLayer(0, 0, 0, Color.Black);

        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // set canvas background color
            // canvas.DrawColor(bgColor);

            // set stroke width
            paint.StrokeWidth = ((float)(0.02083*screenW));

            //incrementing counter for rotation

            count = count+0.5;

            //comparison to check 360 degrees rotation
            int retval = Double.Compare(count, 360.00);

            if(retval == 0) {

                if(!isAnimated) {
                    // mark completion of animation
                    isAnimated = true;
                    //resetting counter on completion of a rotation
                    count = 0;
                } else {
                    //resetting counter on completion of a rotation
                    count = 0;
                }
            }

            // draw cloud
            canvas.DrawPath(c.getCloud(X,Y,screenW,count), paint);

            if(!isStatic || !isAnimated) {
                // invalidate if not static or not animating
                Invalidate();
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action) {
                case MotionEventActions.Down:
                    // nothing to do
                    return true;
                case MotionEventActions.Move:
                    // nothing to do
                    break ;
                case MotionEventActions.Up:
                    // start animation if it is not animating
                    if(isStatic && isAnimated) {
                        isAnimated = false;
                    }
                    break;
                default:
                    return false;
            }

            // Schedules a repaint.
            if(!isAnimated) {
                Invalidate();
            }
            return true;
        }
    }
}