using System;
using System.Collections.Generic;
using System.Linq;

namespace SLib.Prelude
{
    /// <summary>
    ///   Represents an optional value.  Using this type is more explicit than just relying on NULL.
    /// </summary>
    public struct Option<T>
    {
        readonly T _val;
        readonly bool _isSome;


        public Option(T val)
        {
            _val    = val;
            _isSome = true;
        }


        public T Value
        {
            get 
            {
                if (IsNone)
                    throw new Exception("Cannot take the value of a None option.");

                return _val;
            }
        }
        

        public bool IsSome => _isSome;

        public bool IsNone => ! _isSome;


        public U Match<U>(Func<U> noneF, Func<T, U> someF)
        {
            if (noneF == null || someF == null)
                throw new ArgumentException("Functions for both Option.Match() cases must be provided.");

            if (IsNone)
                noneF();
            
            return someF( _val );
        }
    }


    public static class OptionModule
    {
        public static Option<U> Some<U>(U val) 
        {
            if (ReferenceEquals(val, null))
                throw new ArgumentNullException( nameof( val ) );

            return new Option<U>( val );
        }


        public static Option<T> None<T>()
        {
            return new Option<T>();
        }


        public static Option<IEnumerable<T>> OfSeq<T>(IEnumerable<T> seq)
        {
            if (seq == null || ! seq.Any())
                return None<IEnumerable<T>>();

            return Some( seq );
        }


        public static Option<T> OfSingletonSeq<T>(IEnumerable<T> seq)
        {
            if (seq == null || ! seq.Any())
                return None<T>();

            if (seq.Count() > 1)
                throw new ArgumentException("sequence must be a singleton sequence containing only 1 element.");

            return Some( seq.First() );
        }


        public static Option<T> OfNullable<T>(T val)
          where T : class
        {
            if (val == null)
                return None<T>();

            return Some<T>( val );
        }


        public static Option<T> OfNullable<T>(Nullable<T> val)
          where T : struct
        {
            if (! val.HasValue)
                return None<T>();

            return Some<T>( val.Value );
        }


        /// <summary>
        ///   Applies the function F to the value in the Option functor.
        ///   (Part of the Functor interface for Option.)
        /// </summary>
        public static Option<U> Map<T, U>(this Option<T> opt, Func<T, U> f)
        {
            return opt.Match( () => None<U>()
                            , v  => Some( f( v ) )
                            );
        }


        /// <summary>
        ///   Applies the action F to the value in the Option functor.
        ///   (Part of the Functor interface for Option.)
        /// </summary>
        public static void For<T>(this Option<T> opt, Action<T> f)
        {
            if (opt.IsSome)
                f( opt.Value );
        }


        /// <summary>
        ///   Implements lazy choice.
        ///   (Part of the Alternative interface for Option.)
        /// </summary>
        public static Option<T> Or<T>(this Option<T> opt, Func<Option<T>> f)
        {
            return opt.Match( () => f()
                            , _  => opt );
        }


        /// <summary>
        ///   Inject a value into the monadic type.
        ///   (Part of the Monad interface.)
        /// </summary>
        public static Option<T> Of<T>(T val)
        {
            return Some( val );
        }


        /// <summary>
        ///   Sequentially compose two actions, passing any value produced by the first as an argument to the second.
        ///   (Part of the Monad interface.)
        /// </summary>
        public static Option<U> Bind<T,U>(this Option<T> opt, Func<T, Option<U>> f)
        {
            return opt.Match( () => None<U>()
                            , v  => f( v )
                            );
        }


        /// <summary>
        ///   Sequentially compose two actions, ignoring the value produced by the first action.
        ///   (Part of the Monad interface.)
        /// </summary>
        public static Option<U> Chain<T,U>(this Option<T> opt, Func<Option<U>> f)
        {
            return opt.Match( () => None<U>()
                            , _  => f()
                            );
        }


        public static Option<IEnumerable<U>> MapOfSeq<T, U>(this Option<IEnumerable<T>> opt, Func<IEnumerable<T>, IEnumerable<U>> mapF)
        {
            return opt.Match( () => None<IEnumerable<U>>()
                            , v  => OfSeq( mapF(v) ) );
        }
    }
}
