using System;
using System.Collections.Generic;
using System.Linq;
using SLib.Prelude;

namespace SLib.Data
{
    /// <summary>
    ///   Module that provides common methods for working with IEnumerable values.
    /// </summary>
    public static class IEnumerableModule
    {
        public static Tuple<IEnumerable<T>, IEnumerable<T>> ReadUntil<T>(this IEnumerable<T> seq, Func<List<T>, T, bool> predicateF)
        {
            var readElems = new List<T>();
            foreach (T elem in seq)
            {
                if (predicateF( readElems, elem ))
                    break;

                readElems.Add( elem );
            }

            var restElems = seq.Skip( readElems.Count ).ToList();
            return Tuple.Create<IEnumerable<T>, IEnumerable<T>>( readElems, restElems );
        }


        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> seq, Func<List<T>, T, bool> predicateF)
        {
            var chunks = new List<List<T>>();
            var chunk = new List<T>();
            foreach (T elem in seq)
            {
                if (! predicateF( chunk, elem ))
                {
                    chunks.Add( chunk );
                    chunk = new List<T>();
                }

                if (predicateF( chunk, elem ))
                    chunk.Add( elem );
            }

            // make sure to add any remaining CHUNK values to the list of CHUNKS
            chunks.Add( chunk );

            return chunks;
        }


        public static void Each<T>(this IEnumerable<T> seq, Action<int, T> actionF)
        {
            if (seq.IsEmpty())
                return;

            int idx = 0;
            foreach (T elem in seq)
            {
                actionF( idx, elem );
                ++idx;
            }
        }


        public static void Each<T>(this IEnumerable<T> seq, Action<T> actionF)
        {
            if (seq.IsEmpty())
                return;

            int idx = 0;
            foreach (T elem in seq)
            {
                actionF( elem );
                ++idx;
            }
        }


        public static void Each<T>(this T[] arr, Action<T> actionF)
        {
            if (arr.IsEmpty())
                return;

            Array.ForEach( arr, actionF );
        }


        public static IEnumerable<Tuple<T,U>> Zip<T,U>(this IEnumerable<T> seq1, IEnumerable<U> seq2)
        {
            if (seq1.IsEmpty() && seq2.IsEmpty())
                return new Tuple<T,U>[0];

            if (seq1.IsEmpty() || seq2.IsEmpty())
                throw new  ArgumentException( "Both sequences must be NON-NULL for the Zip operation." );

            if (seq1.Count() != seq2.Count())
                throw new ArgumentException( "Both sequences must be the same length for the Zip operation." );

            IEnumerable<Tuple<T, U>> zippedSeq =  seq1.Zip( seq2, (v1, v2) => Tuple.Create( v1, v2 ) );
            return zippedSeq;
        }


        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> matrix)
        {
            var dimOuter = matrix.Count();
            var dimInner = matrix.First().Count();

            var tr = new T[dimInner][];

            for (int idxInner = 0; idxInner < dimInner; idxInner++)
            {
                tr[idxInner] = new T[dimOuter];

                for (int idxOuter = 0; idxOuter < dimOuter; idxOuter++)
                {
                    tr[idxInner][idxOuter] = matrix.ElementAt(idxOuter).ElementAt(idxInner);
                }
            }

            return tr;
        }


        public static Tuple<IEnumerable<T>, IEnumerable<T>> Partition<T>(this IEnumerable<T> collection, Func<T, bool> predicateF)
        {
            if (collection.IsEmpty())
                return Tuple.Create<IEnumerable<T>, IEnumerable<T>>(new T[0], new T[0]);

            var elemsSatisfyingPredicate    = new List<T>();
            var elemsNotSatisfyingPredicate = new List<T>();

            foreach (T elem in collection)
            {
                if (predicateF( elem ))
                    elemsSatisfyingPredicate.Add( elem );
                else
                    elemsNotSatisfyingPredicate.Add( elem );
            }

            return Tuple.Create<IEnumerable<T>, IEnumerable<T>>( elemsSatisfyingPredicate, elemsNotSatisfyingPredicate );
        }


        public static long SumOrDefault<T>(this IEnumerable<T> collection, Func<T, long> selectorF, long defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Sum(selectorF);
            return maxVal;
        }


        public static decimal SumOrDefault<T>(this IEnumerable<T> collection, Func<T, decimal> selectorF, decimal defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Sum(selectorF);
            return maxVal;
        }


        public static float SumOrDefault<T>(this IEnumerable<T> collection, Func<T, float> selectorF, float defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Sum(selectorF);
            return maxVal;
        }


        public static double SumOrDefault<T>(this IEnumerable<T> collection, Func<T, double> selectorF, double defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Sum(selectorF);
            return maxVal;
        }


        public static int MaxOrDefault<T>(this IEnumerable<T> collection, Func<T, int> selectorF, int defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Max(selectorF);
            return maxVal;
        }


        public static long MaxOrDefault<T>(this IEnumerable<T> collection, Func<T, long> selectorF, long defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Max(selectorF);
            return maxVal;
        }


        public static decimal MaxOrDefault<T>(this IEnumerable<T> collection, Func<T, decimal> selectorF, decimal defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Max(selectorF);
            return maxVal;
        }


        public static float MaxOrDefault<T>(this IEnumerable<T> collection, Func<T, float> selectorF, float defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Max(selectorF);
            return maxVal;
        }


        public static double MaxOrDefault<T>(this IEnumerable<T> collection, Func<T, double> selectorF, double defaultVal)
        {
            if (collection.IsEmpty())
                return defaultVal;

            var maxVal = collection.Max(selectorF);
            return maxVal;
        }
    }
}
