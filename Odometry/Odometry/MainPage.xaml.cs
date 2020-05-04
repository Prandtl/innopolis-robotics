using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using OdometryApp;
using System.IO;
using System.Linq;
using System.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Odometry
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Line xAxis;
        private Line yAxis;
        private readonly string path = @"C:\Users\admin\Documents\TestInput\0.txt";

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += OnSizeChanged;
            xAxis = new Line() {Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 2};
            yAxis = new Line() {Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 2};

            RobotCanvas.Children.Add(xAxis);
            RobotCanvas.Children.Add(yAxis);
            RecalculateAxis();
            
            Task.Run(()=>RunOdometryApp(path))
                .ContinueWith(async (r)=> { await Dispatcher.RunAsync(CoreDispatcherPriority.High,()=> DrawPath(r.Result)); });
        }
        

        private Position[] history;
        private void DrawPath(Position[] history)
        {
            this.history = history;
            var originX = latestWidth / 2;
            var originY = latestHeight / 2;
            var polyline = new Polyline();
            polyline.Stroke = new SolidColorBrush(Windows.UI.Colors.Red);
            polyline.StrokeThickness = 1;
            var points = new PointCollection();
            foreach (var position in history) {
                points.Add(new Windows.Foundation.Point(originX + position.X/10, originY + position.Y/10));
            }
            polyline.Points = points;
            RobotCanvas.Children.Add(polyline);

        }

        private void OnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            latestHeight = e.Size.Height;
            latestWidth = e.Size.Width;
            RecalculateAxis();
        }

        private double latestWidth = 1200;
        private double latestHeight = 900;
        private void RecalculateAxis()
        {
            xAxis.X1 = 0;
            xAxis.X2 = latestWidth;
            xAxis.Y1 = xAxis.Y2 = latestHeight / 2;

            yAxis.X1 = yAxis.X2 = latestWidth / 2;
            yAxis.Y1 = 0;
            yAxis.Y2 = latestHeight;
        }

        public async Task<Position[]> RunOdometryApp(string inputsFile)
        {

            using (var s = new StreamReader(new FileStream(inputsFile, FileMode.Open))) {
                var app = new OdometryApp.OdometryApp(() => s.ReadLine());
                Debug.WriteLine(app.Run());
                return app.GetHistory();
            }
        }
    }
}