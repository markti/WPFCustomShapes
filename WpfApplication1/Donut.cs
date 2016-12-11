using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication1
{
    public class Donut : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geom = new StreamGeometry();
                using(StreamGeometryContext gc = geom.Open())
                {
                    DrawArrowGeometry(gc);
                }
                return geom;
            }
        }

        public double InnerWidth
        {
            get { return (double)GetValue(InnerWidthProperty); }
            set { SetValue(InnerWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InnerWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InnerWidthProperty =
            DependencyProperty.Register("InnerWidth", typeof(double), typeof(Donut), new PropertyMetadata(0D));



        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Donut), new PropertyMetadata(0D));

        
        public double StopAngle
        {
            get { return (double)GetValue(StopAngleProperty); }
            set { SetValue(StopAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopAngleProperty =
            DependencyProperty.Register("StopAngle", typeof(double), typeof(Donut), new PropertyMetadata(0D));

        public Donut()
        {

        }



        //public double Angle
        //{
        //    get
        //    {
        //        double rc = WedgeAngle;
        //        if (WedgeAngle > 360)
        //        { rc = WedgeAngle % 360; if (rc == 0) { rc = 360; } }
        //        return rc;
        //    }
        //}

        private double GetRadian(double angle)
        {
            return (Math.PI / 180.0) * (angle - 90.0);
        }

        private void DrawArrowGeometry(StreamGeometryContext context)
        {
            // Setup the Center Point & Radius
            Point c = new Point(ActualWidth / 2, ActualHeight / 2);
            double rOutterX = ActualWidth / 2;
            double rOutterY = ActualHeight / 2;
            double rInnerX = rOutterX - InnerWidth;
            double rInnerY = rOutterY - InnerWidth;

            double theta = 0;
            bool hasBegun = false;
            double x;
            double y;
            Point currentPoint;

            // Draw the Outside Edge
            for (theta = StartAngle; theta <= StopAngle; theta++)
            {
                x = c.X + rOutterX * Math.Cos(GetRadian(theta));
                y = c.Y + rOutterY * Math.Sin(GetRadian(theta));
                currentPoint = new Point(x, y);
                if (!hasBegun)
                {
                    context.BeginFigure(currentPoint, true, true);
                    hasBegun = true;
                }
                context.LineTo(currentPoint, true, true);
            }

            // Connect the Outside Edge to the Inner Edge
            x = c.X + rInnerX * Math.Cos(GetRadian(StopAngle));
            y = c.Y + rInnerY * Math.Sin(GetRadian(StopAngle));
            currentPoint = new Point(x, y);
            context.LineTo(currentPoint, true, true);

            // Draw the Inner Edge
            for (theta = StopAngle; theta >= StartAngle; theta--)
            {
                x = c.X + rInnerX * Math.Cos(GetRadian(theta));
                y = c.Y + rInnerY * Math.Sin(GetRadian(theta));
                currentPoint = new Point(x, y);
                context.LineTo(currentPoint, true, true);
            }

            // Connect the Inner Edge to the Outside Edge
            x = c.X + rOutterX * Math.Cos(GetRadian(StartAngle));
            y = c.Y + rOutterY * Math.Sin(GetRadian(StartAngle));
            currentPoint = new Point(x, y);
            context.LineTo(currentPoint, true, true);

            context.Close();
        }
    }
}