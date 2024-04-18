using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kmeans_Clustering_With_Random_Data
{
    public class KMeans
    {
        private int k;
        private int maxIterations;
        private List<Point> centroids;

        public KMeans(int k, int maxIterations)
        {
            this.k = k;
            this.maxIterations = maxIterations;
        }

        public List<Point> Cluster(List<Point> data)
        {
            // Initialize centroids randomly
            Random random = new Random();
            centroids = new List<Point>();
            for (int i = 0; i < k; i++)
            {
                centroids.Add(data[random.Next(data.Count)]);
            }

            // Iteratively update centroids
            for (int iter = 0; iter < maxIterations; iter++)
            {
                // Assign each data point to the nearest centroid
                Dictionary<Point, List<Point>> clusters = new Dictionary<Point, List<Point>>();
                foreach (Point point in data)
                {
                    Point nearestCentroid = GetNearestCentroid(point);
                    if (!clusters.ContainsKey(nearestCentroid))
                    {
                        clusters[nearestCentroid] = new List<Point>();
                    }
                    clusters[nearestCentroid].Add(point);
                }

                // Update centroids based on the mean of the points in each cluster
                List<Point> newCentroids = new List<Point>();
                foreach (Point centroid in centroids)
                {
                    List<Point> clusterPoints = clusters[centroid];
                    if (clusterPoints.Count > 0)
                    {
                        double meanX = clusterPoints.Select(p => p.X).Average();
                        double meanY = clusterPoints.Select(p => p.Y).Average();
                        newCentroids.Add(new Point(meanX, meanY));
                    }
                    else
                    {
                        newCentroids.Add(centroid);
                    }
                }
                centroids = newCentroids;
            }

            return centroids;
        }
        public Point GetNearestCentroid(Point point)
        {
            double minDistance = double.MaxValue;
            Point nearestCentroid = null;
            foreach (Point centroid in centroids)
            {
                double distance = point.DistanceTo(centroid);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCentroid = centroid;
                }
            }
            return nearestCentroid;
        }
    }
}
