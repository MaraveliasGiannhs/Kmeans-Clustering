using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;


namespace Kmeans_Clustering_With_Random_Data
{
    class KMeansChart
    {
        private Chart chart;


        public KMeansChart()
        {
            InitializeChart();

        }

        private void InitializeChart()
        {
            chart = new Chart();
            chart.Dock = DockStyle.Fill; // Dock the chart to fill the form


            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Minimum = 0; // Set minimum value for horizontal axis
            chartArea.AxisX.Maximum = 500; // Set maximum value for horizontal axis
            chartArea.AxisX.Interval = 5; // Set interval between gridlines for horizontal axis

            chartArea.AxisY.Minimum = 0; // Set minimum value for vertical axis
            chartArea.AxisY.Maximum = 500; // Set maximum value for vertical axis
            chartArea.AxisY.Interval = 5; // Set interval between gridlines for vertical axis
            chart.ChartAreas.Add(chartArea);


            Series dataSeries = new Series("DataPoints");
            dataSeries.ChartType = SeriesChartType.Point;
            dataSeries.MarkerSize = 8;
            //dataSeries.Color = System.Drawing.Color.Blue;
            chart.Series.Add(dataSeries);

            Series clusterSeries = new Series("Clusters");
            clusterSeries.ChartType = SeriesChartType.Point;
            clusterSeries.MarkerSize = 12;
            clusterSeries.Color = System.Drawing.Color.Red;

            chart.Series.Add(clusterSeries);

        }




        public void Plot(List<Point> data, List<Point> centroids, KMeans kmeans)
        {


            // Clear existing points from both series
            chart.Series["DataPoints"].Points.Clear();
            chart.Series["Clusters"].Points.Clear();

            // Add data points to the data series (circles)
            Series dataSeries = chart.Series["DataPoints"];
            foreach (var point in data)
            {
                var dataPoint = new DataPoint(point.X, point.Y);
                dataSeries.Points.Add(dataPoint);
            }


            Series clusterSeries = chart.Series["Clusters"];
            foreach (var centroid in centroids)
            {
                clusterSeries.Points.AddXY(centroid.X, centroid.Y);

            }


            // Generate colors for each cluster
            List<System.Drawing.Color> clusterColors = GenerateRandomColors(centroids.Count);
            for (int i = 0; i < centroids.Count; i++)
            {
                Point centroid = centroids[i];
                List<Point> clusterPoints = data.Where(p => kmeans.GetNearestCentroid(p) == centroid).ToList();

                // Assign color to the cluster
                System.Drawing.Color clusterColor = clusterColors[i];

                foreach (var point in clusterPoints)
                {
                    clusterSeries.Points.AddXY(point.X, point.Y);
                    clusterSeries.Points.Last().Color = clusterColor;
                }
            }



            // Create a form to host the chart
            Form chartForm = new Form();
            chartForm.Text = "K-Means Clustering Chart";
            chartForm.Width = 600;
            chartForm.Height = 400;
            chartForm.Controls.Add(chart);
            chartForm.ShowDialog();

            
        }

        private List<System.Drawing.Color> GenerateRandomColors(int count)
        {
            Random random = new Random();
            List<System.Drawing.Color> colors = new List<System.Drawing.Color>();

            for (int i = 0; i < count; i++)
            {
                colors.Add(System.Drawing.Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
            }

            return colors;
        }
    }
}
