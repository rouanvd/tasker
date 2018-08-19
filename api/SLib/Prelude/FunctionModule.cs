using System;
using System.Diagnostics.Contracts;

namespace SLib.Prelude
{
    public static class FunctionModule
    {
        public static T IdentityF<T>(T val)
        {
            return val;
        }


        public static Func<T1,T3> Compose<T1,T2,T3>(Func<T1,T2> funcLeft, Func<T2, T3> funcRight)
        {
            Contract.Requires( funcLeft != null );
            Contract.Requires( funcRight != null );

            return (T1 v1) => funcRight( funcLeft( v1 ) );
        }
    }
}
