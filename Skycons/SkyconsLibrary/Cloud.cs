/*
 * 
 * SKYCONS for Android
 * https://github.com/torryharris/Skycons
 * 
 * Ported to C# by Andreas Jacobsen, 2014
 * 
 */

using Android.Graphics;
using Java.Lang;

namespace Learn2Run.UI.Android.Skycons
{
    public class Cloud
    {
        private Path path;
        
        public Path getCloud(float centerX, float centerY, int dimension, double count)
        {
            //different radius values for the cloud coordinates

            path = new Path();

            var r1 = (int) (0.1875*dimension);
            var r2 = (int) (0.1041667*dimension);
            var offset = 0.00023125*dimension;

            // cloud coordinates from the center of the screen
            var X1 = (float) (r1*Math.Cos(Math.ToRadians(0 + (0.222*count))) + centerX);
                //x value of coordinate 1 at radius r1 from center of Screen and angle incremented with counter
            var Y1 = ((float) (r2*Math.Sin(Math.ToRadians(0 + (0.222*count))) + centerY));
                //y value of coordinate 1 at radius r2 from center of Screen and angle incremented with counter
            var P1X = (float) (r1*Math.Cos(Math.ToRadians(80 + (0.111*count))) + centerX);
                //x value of coordinate 2 at radius r1 from center of Screen and angle incremented with counter
            var P1Y = ((float) (r2*Math.Sin(Math.ToRadians(80 + (0.111*count))) + centerY));
                //y value of coordinate 2 at radius r2 from center of Screen and angle incremented with counter
            var P2X = (float) (r1*Math.Cos(Math.ToRadians(120 + (0.222*count))) + centerX);
                //x value of coordinate 3 at radius r1 from center of Screen and angle incremented with counter
            var P2Y = ((float) ((r2 + (offset*count))*Math.Sin(Math.ToRadians(120 + (0.222*count))) + centerY));
                //y value of coordinate 3 at varying radius from center of Screen and angle incremented with counter
            var P3X = (float) (r1*Math.Cos(Math.ToRadians(200 + (0.222*count))) + centerX);
                //x value of coordinate 4 at radius r1 from center of Screen and angle incremented with counter
            var P3Y = ((float) (r1*Math.Sin(Math.ToRadians(200 + (0.222*count))) + centerY));
                //y value of coordinate 4 at radius r1 from center of Screen and angle incremented with counter
            var P4X = (float) (r1*Math.Cos(Math.ToRadians(280 + (0.222*count))) + centerX);
                //x value of coordinate 5 at radius r1 from center of Screen and angle incremented with counter
            var P4Y = ((float) (r1*Math.Sin(Math.ToRadians(280 + (0.222*count))) + centerY));
                //y value of coordinate 5 at radius r1 from center of Screen and angle incremented with counter


            path.MoveTo(X1, Y1);

            // getting points in between coordinates for drawing arc between them
            var P1c1 = calculateTriangle(X1, Y1, P1X, P1Y, true);
            var P1c2 = calculateTriangle(X1, Y1, P1X, P1Y, false);
            var P2c1 = calculateTriangle(P1X, P1Y, P2X, P2Y, true);
            var P2c2 = calculateTriangle(P1X, P1Y, P2X, P2Y, false);
            var P3c1 = calculateTriangle(P2X, P2Y, P3X, P3Y, true);
            var P3c2 = calculateTriangle(P2X, P2Y, P3X, P3Y, false);
            var P4c1 = calculateTriangle(P3X, P3Y, P4X, P4Y, true);
            var P4c2 = calculateTriangle(P3X, P3Y, P4X, P4Y, false);
            var P5c1 = calculateTriangle(P4X, P4Y, X1, Y1, true);
            var P5c2 = calculateTriangle(P4X, P4Y, X1, Y1, false);

            // drawing arcs between coordinates
            path.MoveTo(X1, Y1);
            path.CubicTo(P1c1.X, P1c1.Y, P1c2.X, P1c2.Y, P1X, P1Y);
            path.CubicTo(P2c1.X, P2c1.Y, P2c2.X, P2c2.Y, P2X, P2Y);
            path.CubicTo(P3c1.X, P3c1.Y, P3c2.X, P3c2.Y, P3X, P3Y);
            path.CubicTo(P4c1.X, P4c1.Y, P4c2.X, P4c2.Y, P4X, P4Y);
            path.CubicTo(P5c1.X, P5c1.Y, P5c2.X, P5c2.Y, X1, Y1);

            return path;
        }


        private PointF calculateTriangle(float x1, float y1, float x2,
            float y2, bool left)
        {

            var result = new PointF(0, 0);
            // finding center point between the coordinates
            var dy = y2 - y1;
            var dx = x2 - x1;
            float dangle = 0;
            // calculating angle and the distance between center and the two points
            if (left)
            {
                dangle = (float) ((Math.Atan2(dy, dx) - Math.Pi/3f));
            }
            else
            {
                dangle = (float) ((Math.Atan2(dy, dx) - Math.Pi/1.5f));
            }
            var sideDist = (float) 0.45*(float) Math.Sqrt(dx*dx + dy*dy); //square

            // sideDist = sideDist + sideDist/10;

            if (left)
            {
                //point from center to the left
                result.X = (int) (Math.Cos(dangle)*sideDist + x1);
                result.Y = (int) (Math.Sin(dangle)*sideDist + y1);

            }
            else
            {
                //point from center to the right
                result.X = (int) (Math.Cos(dangle)*sideDist + x2);
                result.Y = (int) (Math.Sin(dangle)*sideDist + y2);
            }

            return result;

        }


        public PointF getP1c1(float centerX, float centerY, int dimension, double count)
        {
            var r1 = (int) (0.1875*dimension);
            var r2 = (int) (0.1041667*dimension);
            var offset = 0.00023125*dimension;

            // cloud coordinates from the center of the screen
            var X1 = (float) (r1*Math.Cos(Math.ToRadians(0 + (0.222*count))) + centerX);
                //x value of coordinate 1 at radius r1 from center of Screen and angle incremented with counter
            var Y1 = ((float) (r2*Math.Sin(Math.ToRadians(0 + (0.222*count))) + centerY));
                //y value of coordinate 1 at radius r2 from center of Screen and angle incremented with counter
            var P1X = (float) (r1*Math.Cos(Math.ToRadians(80 + (0.111*count))) + centerX);
                //x value of coordinate 2 at radius r1 from center of Screen and angle incremented with counter
            var P1Y = ((float) (r2*Math.Sin(Math.ToRadians(80 + (0.111*count))) + centerY));
                //y value of coordinate 2 at radius r2 from center of Screen and angle incremented with counter

            // getting points in between coordinates for drawing arc between them
            var P1c1 = calculateTriangle(X1, Y1, P1X, P1Y, true);

            return P1c1;
        }

        public PointF getP1c2(float centerX, float centerY, int dimension, double count)
        {
            var r1 = (int) (0.1875*dimension);
            var r2 = (int) (0.1041667*dimension);
            var offset = 0.00023125*dimension;

            // cloud coordinates from the center of the screen
            var X1 = (float) (r1*Math.Cos(Math.ToRadians(0 + (0.222*count))) + centerX);
                //x value of coordinate 1 at radius r1 from center of Screen and angle incremented with counter
            var Y1 = ((float) (r2*Math.Sin(Math.ToRadians(0 + (0.222*count))) + centerY));
                //y value of coordinate 1 at radius r2 from center of Screen and angle incremented with counter
            float P1X = (float) (r1*Math.Cos(Math.ToRadians(80 + (0.111*count))) + centerX);
                //x value of coordinate 2 at radius r1 from center of Screen and angle incremented with counter
            float P1Y = ((float) (r2*Math.Sin(Math.ToRadians(80 + (0.111*count))) + centerY));
                //y value of coordinate 2 at radius r2 from center of Screen and angle incremented with counter

            PointF P1c2 = calculateTriangle(X1, Y1, P1X, P1Y, false);

            return P1c2;
        }


        public PointF getP2c1(float centerX, float centerY, int dimension, double count)
        {
            int r1 = (int) (0.1875*dimension);
            int r2 = (int) (0.1041667*dimension);
            double offset = 0.00023125*dimension;

            // cloud coordinates from the center of the screen
            float P1X = (float) (r1*Math.Cos(Math.ToRadians(80 + (0.111*count))) + centerX);
                //x value of coordinate 2 at radius r1 from center of Screen and angle incremented with counter
            float P1Y = ((float) (r2*Math.Sin(Math.ToRadians(80 + (0.111*count))) + centerY));
                //y value of coordinate 2 at radius r2 from center of Screen and angle incremented with counter
            float P2X = (float) (r1*Math.Cos(Math.ToRadians(120 + (0.222*count))) + centerX);
                //x value of coordinate 3 at radius r1 from center of Screen and angle incremented with counter
            float P2Y = ((float) ((r2 + (offset*count))*Math.Sin(Math.ToRadians(120 + (0.222*count))) + centerY));
                //y value of coordinate 3 at varying radius from center of Screen and angle incremented with counter

            PointF P2c1 = calculateTriangle(P1X, P1Y, P2X, P2Y, true);

            return P2c1;
        }


        public PointF getP2c2(float centerX, float centerY, int dimension, double count)
        {
            int r1 = (int) (0.1875*dimension);
            int r2 = (int) (0.1041667*dimension);
            double offset = 0.00023125*dimension;

            float P1X = (float) (r1*Math.Cos(Math.ToRadians(80 + (0.111*count))) + centerX);
                //x value of coordinate 2 at radius r1 from center of Screen and angle incremented with counter
            float P1Y = ((float) (r2*Math.Sin(Math.ToRadians(80 + (0.111*count))) + centerY));
                //y value of coordinate 2 at radius r2 from center of Screen and angle incremented with counter
            float P2X = (float) (r1*Math.Cos(Math.ToRadians(120 + (0.222*count))) + centerX);
                //x value of coordinate 3 at radius r1 from center of Screen and angle incremented with counter
            float P2Y = ((float) ((r2 + (offset*count))*Math.Sin(Math.ToRadians(120 + (0.222*count))) + centerY));
                //y value of coordinate 3 at varying radius from center of Screen and angle incremented with counter

            PointF P2c2 = calculateTriangle(P1X, P1Y, P2X, P2Y, false);

            return P2c2;
        }

        public PointF getP5c1(float centerX, float centerY, int dimension, double count)
        {
            int r1 = (int) (0.1875*dimension);
            int r2 = (int) (0.1041667*dimension);
            double offset = 0.00023125*dimension;

            // cloud coordinates from the center of the screen
            float X1 = (float) (r1*Math.Cos(Math.ToRadians(0 + (0.222*count))) + centerX);
                //x value of coordinate 1 at radius r1 from center of Screen and angle incremented with counter
            float Y1 = ((float) (r2*Math.Sin(Math.ToRadians(0 + (0.222*count))) + centerY));
                //y value of coordinate 1 at radius r2 from center of Screen and angle incremented with counter
            float P4X = (float) (r1*Math.Cos(Math.ToRadians(280 + (0.222*count))) + centerX);
                //x value of coordinate 5 at radius r1 from center of Screen and angle incremented with counter
            float P4Y = ((float) (r1*Math.Sin(Math.ToRadians(280 + (0.222*count))) + centerY));
                //y value of coordinate 5 at radius r1 from center of Screen and angle incremented with counter

            PointF P5c1 = calculateTriangle(P4X, P4Y, X1, Y1, true);

            return P5c1;
        }
    }
}