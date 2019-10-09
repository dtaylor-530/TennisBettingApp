using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TennisBettingChartApp.CSharp
{
    using System.Reflection;

    using OxyPlot;
    using OxyPlot.Series;

    using RegressionSmooth;

    using DataPoint = RegressionSmooth.DataPoint;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            var range = Enumerable.Range(0, 25);

            PlotView.Model = new PlotModel();
            foreach (var x in this.GetLineSeries())
            {
                this.PlotView.Model.Series.Add(x);
            }
            //let combine = Chart.Combine(arr).WithLegend(true)
            //Chart.Show combine
        }

        private IEnumerable<LineSeries> GetLineSeries()
        {

            
            //let xs = points |> Seq.map (fun x -> x.ratio)
            foreach (var points in GetPoints())
            {
                var points2 = one.getPoints(points.Item2);
                var modPoints = one.SmoothLine(points2).ToArray();
                var n = one.normalizeLine(points2, modPoints);
                var mod2Points = modPoints.Select(xy => (xy.Item1, xy.Item2 * n));
              

                var ps = mod2Points.Select(a => new OxyPlot.DataPoint(a.Item1, a.Item2));

                var mType = rand<MarkerType>();
                mType = mType == MarkerType.Custom ? MarkerType.Cross : mType;
                var ls1 = new LineSeries()
                {

                    Color =(OxyColor)rand2(typeof(OxyColors)),
                    MarkerType =mType,
                    MarkerSize = 6,
                    //MarkerStroke = OxyColors.White,
                    //MarkerFill = OxyColors.SkyBlue,
                    MarkerStrokeThickness = 1.5
                };

                ls1.Points.AddRange(ps);

                yield return ls1;


                var ps2 = points2.Select(a => new OxyPlot.DataPoint(a.Item1, a.Item2));

                 ls1 = new LineSeries()
                              {

                                  Color = (OxyColor)rand2(typeof(OxyColors)),
                                  MarkerType = mType,
                                  MarkerSize = 6,
                                  //MarkerStroke = OxyColors.White,
                                  //MarkerFill = OxyColors.SkyBlue,
                                  MarkerStrokeThickness = 1.5
                              };

                ls1.Points.AddRange(ps2);

                yield return ls1;

            }
        }

        static Random random = new Random();
        private static T rand<T>()
        {
            Array values = Enum.GetValues(typeof(T));
         
            return (T)values.GetValue(random.Next(values.Length));
        }

        private static object rand2(Type type)
        {
            var values = type.GetFields().Cast<FieldInfo>().ToArray();

            return values[random.Next(values.Length)].GetValue(null);
        }

        private IEnumerable<(int atp, IEnumerable<DataPoint>)> GetPoints()
        {
            for (int i = 0; i < 25; i++)
            {
                yield return (i, TennisBetting.datapoints.getAllPoints.Where(p => p.atp == i));
            }
        }
    }
}
