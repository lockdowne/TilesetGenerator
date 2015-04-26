﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TilesetGenerator.Common
{
    public static class MathExtensions
    {
        /// <summary>
        /// Applies correction to screen pixels to a matrix transformation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector2 InvertMatrixAtVector(int x, int y, Matrix matrix)
        {
            return InvertMatrixAtVector(new Vector2(x, y), matrix);
        }

        /// <summary>
        /// Applies correction to screen pixels to a matrix transformation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector2 InvertMatrixAtVector(Vector2 vector, Matrix matrix)
        {
            return Vector2.Transform(new Vector2(vector.X, vector.Y), Matrix.Invert(matrix));
        }

        /// <summary>
        /// Converts matrix coordinate into pixels 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        /// <returns></returns>
        public static Vector2 IsoCoordinateToPixels(int x, int y, int tileWidth, int tileHeight)
        {
            return IsoCoordinateToPixels(x, y, tileWidth, tileHeight, 0, 0);
        }

        public static Vector2 IsoCoordinateToPixels(int x, int y, int tileWidth, int tileHeight, int offsetX, int offsetY)
        {
            return new Vector2((x - y) * (tileWidth / 2) - (offsetX), (x + y) * (tileHeight / 2) - (offsetY));
        }

        /// <summary>
        /// Converts pixels into matrix coordinate
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        /// <returns></returns>
        public static Point IsoPixelsToCoordinate(Vector2 vector, int tileWidth, int tileHeight)
        {
            return new Point((int)Math.Round(((vector.X / (tileWidth / 2)) + (vector.Y / (tileHeight / 2))) / 2),
               (int)Math.Round(((vector.Y / (tileHeight / 2)) - (vector.X / (tileWidth / 2))) / 2));
        }

        /// <summary>
        /// Rounds vector to align with isometric grid
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        /// <returns></returns>
        public static Vector2 IsoSnap(Vector2 vector, int tileWidth, int tileHeight)
        {
            Point toCoordinate = IsoPixelsToCoordinate(vector, tileWidth, tileHeight);
            Vector2 toPixels = IsoCoordinateToPixels(toCoordinate.X, toCoordinate.Y, tileWidth, tileHeight); // Check if I need that - 1

            return new Vector2(toPixels.X, toPixels.Y);
        }

        /// <summary>
        /// Rounds vector to align with orthogonal grid
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        /// <returns></returns>
        public static Vector2 OrthogonalSnap(Vector2 vector, int tileWidth, int tileHeight)
        {
            Point toCoordinate = new Point((int)vector.X / tileWidth, (int)vector.Y / tileHeight);

            return new Vector2(toCoordinate.X * tileWidth, toCoordinate.Y * tileHeight);
        }

        /// <summary>
        /// Selects all tile locations in pixels between two vectors
        /// </summary>
        /// <param name="startVector"></param>
        /// <param name="endVector"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        /// <returns>Collection of all pixel locations represented as the center</returns>
        public static IEnumerable<Vector2> IsoSelector(Vector2 startVector, Vector2 endVector, int tileWidth, int tileHeight)
        {
            // Prevent division by zero instead of throwing exception
            if (tileWidth <= 0 || tileHeight <= 0)
                yield break;

            int startX = Math.Min(IsoPixelsToCoordinate(startVector, tileWidth, tileHeight).X, IsoPixelsToCoordinate(endVector, tileWidth, tileHeight).X);
            int startY = Math.Min(IsoPixelsToCoordinate(startVector, tileWidth, tileHeight).Y, IsoPixelsToCoordinate(endVector, tileWidth, tileHeight).Y);
            int width = Math.Abs(IsoPixelsToCoordinate(startVector, tileWidth, tileHeight).X - IsoPixelsToCoordinate(endVector, tileWidth, tileHeight).X);
            int height = Math.Abs(IsoPixelsToCoordinate(startVector, tileWidth, tileHeight).Y - IsoPixelsToCoordinate(endVector, tileWidth, tileHeight).Y);

            for (int x = startX - 1; x < startX + width; x++)
            {
                for (int y = startY; y <= startY + height; y++)
                {
                    yield return IsoCoordinateToPixels(x, y, tileWidth, tileHeight);
                }
            }
        }

        /// <summary>
        /// Randomizes the elements within the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}
