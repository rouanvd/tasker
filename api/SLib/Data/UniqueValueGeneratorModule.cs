namespace SLib.Data
{
    /// <summary>
    ///   Represents the abstraction of a Unique Value Generator (UVG).  A unique value generator returns a unique value
    ///   of type T on each call to .Next(), or if no more unique values can be returned, throws an OverflowException.
    /// </summary>
    /// <remarks>
    ///   Any implementation of IUniqueValueGenerator must satisfy the following rules:
    /// 
    ///     - For the lifetime of the IUniqueValueGenerator instance, no duplicate values must be returned.
    ///     - .Next() must return a value, returning NULLs should be avoided.
    /// 
    ///   The only guarantee that a UVG must uphold is that for its lifetime, it must produce a unique value each time .Next()
    ///   is called.  There is no guarantee that the values are in a specific order.
    /// </remarks>
    /// <typeparam name="T">Type of the values returned by the Value Generator.</typeparam>
    public interface IUniqueValueGenerator<out T>
    {
        T Current {get;}
        T Next();
    }


    public class UniqueIntGenerator : IUniqueValueGenerator<int>
    {
        int _currentNum = 0;

        public int Current => _currentNum;

        public int Next()
        {
            checked
            {
                _currentNum += 1;
                return _currentNum;
            }
        }
    }
}
