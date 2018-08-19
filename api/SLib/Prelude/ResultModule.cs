using System;

namespace SLib.Prelude
{
    /// <summary>
    ///   Represents if an operation was successful or not, and a possible ERROR value.
    /// </summary>
    /// <remarks>
    ///   We use this often as the result of Ajax operations, which return data if the operation was successful, or returns
    ///   an error message if the operation failed.
    /// </remarks>
    public struct Result<TError>
    {
        public bool IsOk {get;set;}

        public bool IsError {get;set;}
        public TError ErrorVal {get;set;}
    }


    /// <summary>
    ///   Represents if an operation was successful or not, and a possible SUCCESS or ERROR value.
    /// </summary>
    /// <remarks>
    ///   We use this often as the result of Ajax operations, which return data if the operation was successful, or returns
    ///   an error message if the operation failed.
    /// </remarks>
    public struct Result<TSuccess, TError>
    {
        public bool IsOk {get;set;}
        public TSuccess OkVal {get;set;}

        public bool IsError {get;set;}
        public TError ErrorVal {get;set;}
    }


    public static class ResultModule
    {
        ///--------------------------------------------------------------------
        /// PROTOCOL: Result[TError]
        ///--------------------------------------------------------------------

        // -- Constructors

        public static Result<TError> Ok<TError>()
        {
            var opResult = new Result<TError> { IsOk = true };
            return opResult;
        }


        public static Result<TError> Err<TError>(TError val = default(TError))
        {
            var opResult = new Result<TError> { IsError = true, ErrorVal = val };
            return opResult;
        }


        ///--------------------------------------------------------------------
        /// PROTOCOL: Result[TSuccess, TError]
        ///--------------------------------------------------------------------

        // -- Constructors

        public static Result<TSuccess,TError> Ok<TSuccess,TError>(TSuccess val = default(TSuccess))
        {
            var opResult = new Result<TSuccess,TError> { IsOk = true, OkVal = val };
            return opResult;
        }


        public static Result<TSuccess,TError> Err<TSuccess,TError>(TError val = default(TError))
        {
            var opResult = new Result<TSuccess,TError> { IsError = true, ErrorVal = val };
            return opResult;
        }

        // -- Functor

        /// <summary>
        ///   Applies the function F to the value in the Option functor.
        ///   (Part of the Functor interface for Option.)
        /// </summary>
        public static Result<USuccess,TError> Map<TSuccess, TError, USuccess>(this Result<TSuccess,TError> result, Func<TSuccess, USuccess> f)
        {
            if (result.IsOk)
            {
                var mappedValue = f( result.OkVal );
                return Ok<USuccess,TError>( mappedValue );
            }

            return Err<USuccess,TError>( result.ErrorVal );
        }

        // -- Applicative

        /// <summary>
        ///   Inject a value into the applicative type.
        ///   (Part of the Applicative interface.)
        /// </summary>
        public static Result<TSuccess,TError> Of<TSuccess,TError>(TSuccess val)
        {
            return Ok<TSuccess,TError>( val );
        }

        // -- Monad

        /// <summary>
        ///   Sequentially compose two actions, passing any value produced by the first as an argument to the second.
        ///   (Part of the Monad interface.)
        /// </summary>
        public static Result<USuccess,TError> Bind<TSuccess, TError, USuccess>(this Result<TSuccess,TError> result, Func<TSuccess, Result<USuccess,TError>> f)
        {
            if (result.IsOk)
                return f( result.OkVal );

            return Err<USuccess,TError>( result.ErrorVal );
        }


        /// <summary>
        ///   Sequentially compose two actions, ignoring the value produced by the first action.
        ///   (Part of the Monad interface.)
        /// </summary>
        public static Result<USuccess,TError> Bind<TSuccess, TError, USuccess>(this Result<TSuccess,TError> result, Func<Result<USuccess,TError>> f)
        {
            if (result.IsOk)
                return f();

            return Err<USuccess,TError>( result.ErrorVal );
        }
    }
}
