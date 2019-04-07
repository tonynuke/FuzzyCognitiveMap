using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Msagl.Core.Layout;

namespace SampleCode
{
    /// <summary>
    /// Defines a simple straight arrow draw along a line.
    /// </summary>
    public class Arrow : Shape
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty ArrowHeadLengthProperty =
            DependencyProperty.Register("ArrowHeadLength", typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowHeadWidthProperty =
            DependencyProperty.Register("ArrowHeadWidth", typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DotSizeProperty =
            DependencyProperty.Register("DotSize", typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(Point), typeof(Arrow),
                new FrameworkPropertyMetadata(new Point(0.0, 0.0), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(Point), typeof(Arrow),
                new FrameworkPropertyMetadata(new Point(0.0, 0.0), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Arrow),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion Dependency Property/Event Definitions

        /// <summary>
        /// The tag of the arrow head.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// The length of the arrow head.
        /// </summary>
        public double ArrowHeadLength
        {
            get => (double)GetValue(ArrowHeadLengthProperty);
            set => SetValue(ArrowHeadLengthProperty, value);
        }

        /// <summary>
        /// The width of the arrow head.
        /// </summary>
        public double ArrowHeadWidth
        {
            get => (double)GetValue(ArrowHeadWidthProperty);
            set => SetValue(ArrowHeadWidthProperty, value);
        }

        /// <summary>
        /// The size of the dot at the start of the arrow.
        /// </summary>
        public double DotSize
        {
            get => (double)GetValue(DotSizeProperty);
            set => SetValue(DotSizeProperty, value);
        }

        /// <summary>
        /// The start point of the arrow.
        /// </summary>
        public Point Start
        {
            get => (Point)GetValue(StartProperty);
            set => SetValue(StartProperty, value);
        }

        /// <summary>
        /// The end point of the arrow.
        /// </summary>
        public Point End
        {
            get => (Point)GetValue(EndProperty);
            set => SetValue(EndProperty, value);
        }

        #region Private Methods

        /// <summary>
        /// Return the shape's geometry.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                //
                // Geometry has not yet been generated.
                // Generate geometry and cache it.
                //
                LineGeometry geometry = new LineGeometry
                {
                    StartPoint = this.Start,
                    EndPoint = this.End
                };

                GeometryGroup group = new GeometryGroup();
                group.Children.Add(geometry);

                GenerateArrowHeadGeometry(group);

                //
                // Return cached geometry.
                //
                return group;
            }
        }

        /// <summary>
        /// Generate the geometry for the three optional arrow symbols at the start, middle and end of the arrow.
        /// </summary>
        private void GenerateArrowHeadGeometry(GeometryGroup geometryGroup)
        {
            EllipseGeometry ellipse = new EllipseGeometry(this.Start, DotSize, DotSize);
            geometryGroup.Children.Add(ellipse);

            Vector startDir = this.End - this.Start;
            startDir.Normalize();
            Point basePoint = this.End - (startDir * ArrowHeadLength);
            Vector crossDir = new Vector(-startDir.Y, startDir.X);

            Point[] arrowHeadPoints = new Point[3];
            arrowHeadPoints[0] = this.End;
            arrowHeadPoints[1] = basePoint - (crossDir * (ArrowHeadWidth / 2));
            arrowHeadPoints[2] = basePoint + (crossDir * (ArrowHeadWidth / 2));

            //
            // Build geometry for the arrow head.
            //
            PathFigure arrowHeadFig = new PathFigure
            {
                IsClosed = true,
                IsFilled = true,
                StartPoint = arrowHeadPoints[0]
            };
            arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[1], true));
            arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[2], true));

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(arrowHeadFig);

            geometryGroup.Children.Add(pathGeometry);

            FormattedText ft = new FormattedText(
                this.Text,
                Thread.CurrentThread.CurrentCulture,
                System.Windows.FlowDirection.LeftToRight,
                new Typeface("Verdana"), 16, Brushes.Black);

            Geometry geometry = ft.BuildGeometry(Start);
            geometryGroup.Children.Add(geometry);
        }

        #endregion Private Methods
    }
}