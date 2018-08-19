using System;
using System.Linq.Expressions;

namespace SLib.Data
{
    public class Lens<TObject, TProperty>
    {
        public Func<TObject, TProperty> Get {get;}
        public Action<TObject, TProperty> Set {get;}
    
    
        public Lens(Func<TObject, TProperty> getter, Action<TObject, TProperty> setter)
        {
            Get = getter;
            Set = setter;
        }
    
    
        /// <summary>
        ///    Creates a new lens by taking the property-getter function, and deriving from 
        ///    that, the property-setter function, returning a new lens with both the
        ///    getter/setter filled-in.
        /// </summary>
        public static Lens<TObject,TProperty> New(Expression<Func<TObject, TProperty>> propertyGetter)
        {
            var getter = propertyGetter.Compile();
            var setter = DeriveSetterLambda(propertyGetter);

            var lens = new Lens<TObject, TProperty>(getter, setter);
            return lens;
        }        
    
    
        /// <summary>
        ///    Derives the property-setter function from the supplied property-getter function.
        /// </summary>
        static Action<TObject,TProperty> DeriveSetterLambda(Expression<Func<TObject, TProperty>> propertyGetter)
        {
            string propName = GetPropertyName(propertyGetter);                    

            var paramTargetExp = Expression.Parameter(typeof(TObject));
            var paramValueExp  = Expression.Parameter(typeof(TProperty));

            var propExp   = Expression.PropertyOrField(paramTargetExp , propName);
            var assignExp = Expression.Assign(propExp, paramValueExp);

            var setter = Expression.Lambda<Action<TObject,TProperty>>(assignExp, paramTargetExp, paramValueExp);

            return setter.Compile();
        }
    
    
        /// <summary>
        ///    Get the name of the property mentioned in the property-getter function as a string.
        /// </summary>        
        static string GetPropertyName(Expression<Func<TObject,TProperty>> exp)
        {
            var propExp = (MemberExpression)exp.Body;
            var n = propExp.Member.Name;
            return n;
        }        
    }
}
