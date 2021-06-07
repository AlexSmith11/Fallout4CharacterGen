﻿using System;
using System.Collections.Generic;

namespace Fallout4CharacterGen.Utilities
{
    public static class MyExtensions
    {
        private static readonly Random _rng = new Random();

        /// <summary>
        /// Shuffle a list randomly.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = _rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}