using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IHK.Repositorys
{
    public static class HelperExtension
    {
        public static IQueryable<T> MultiValueContainsAnyAll<T>(this IQueryable<T> source, ICollection<string> searchKeys, bool all, Expression<Func<T, string[]>> fieldSelectors)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (fieldSelectors == null)
                throw new ArgumentNullException("fieldSelectors");
            NewArrayExpression newArray = fieldSelectors.Body as NewArrayExpression;
            if (newArray == null)
                throw new ArgumentOutOfRangeException("fieldSelectors", fieldSelectors, "You need to use fieldSelectors similar to 'x => new string [] { x.LastName, x.FirstName, x.NickName }'; other forms not handled.");
            if (newArray.Expressions.Count == 0)
                throw new ArgumentException("No field selected.");
            if (searchKeys == null || searchKeys.Count == 0)
                return source;

            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            Expression expression = null;

            foreach (var searchKeyPart in searchKeys)
            {
                Tuple<string> tmp = new Tuple<string>(searchKeyPart);
                Expression searchKeyExpression = Expression.Property(Expression.Constant(tmp), tmp.GetType().GetProperty("Item1"));

                Expression oneValueExpression = null;
                foreach (var fieldSelector in newArray.Expressions)
                {
                    Expression act = Expression.Call(fieldSelector, containsMethod, searchKeyExpression);
                    if (oneValueExpression == null)
                        oneValueExpression = act;
                    else
                        oneValueExpression = Expression.OrElse(oneValueExpression, act);
                }

                if (expression == null)
                    expression = oneValueExpression;
                else if (all)
                    expression = Expression.AndAlso(expression, oneValueExpression);
                else
                    expression = Expression.OrElse(expression, oneValueExpression);
            }
            return source.Where(Expression.Lambda<Func<T, bool>>(expression, fieldSelectors.Parameters));
        }
    }
}
