using System.Linq.Expressions;

namespace FluentUI.Blazor.Community.Infrastructure;

/// <summary>
/// Represents a predicate builder that allows for dynamic construction of boolean expressions.
/// </summary>
/// <typeparam name="T">Type of the data.</typeparam>
public static class PredicateBuilder<T>
{
    /// <summary>
    /// Represents a predicate that always evaluates to true.
    /// </summary>
    public static readonly Expression<Func<T, bool>> True = trueFunc => true;

    /// <summary>
    /// Represents a predicate that always evaluates to false.
    /// </summary>
    public static readonly Expression<Func<T, bool>> False = falseFunc => false;

    /// <summary>
    /// Builds a predicate that represents a logical OR operation between two expressions.
    /// </summary>
    /// <param name="expr1">Represents the first logical expression.</param>
    /// <param name="expr2">Represents the second logical expression.</param>
    /// <returns>Returns the expression of a logical OR operation.</returns>
    public static Expression<Func<T, bool>> Or(Expression<Func<T, bool>>? expr1,
                                               Expression<Func<T, bool>>? expr2)
    {
        if (expr1 is null)
        {
            return expr2 is null ? False : expr2;
        }

        if (expr2 is null)
        {
            return expr1 is null ? False : expr1;
        }

        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

        return Expression.Lambda<Func<T, bool>>
              (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }

    /// <summary>
    /// Builds a predicate that represents a logical OR operation between expressions.
    /// </summary>
    /// <param name="expressions">Represents the expressions to join.</param>
    /// <returns>Returns the expression of a logical OR operation.</returns>
    public static Expression<Func<T, bool>> Or(params Expression<Func<T, bool>>?[] expressions)
    {
        var firstExpression = False;

        foreach (var item in expressions)
        {
            if (item is null)
            {
                continue;
            }

            firstExpression = Or(firstExpression, item);
        }

        return firstExpression;
    }

    /// <summary>
    /// Builds a predicate that represents a logical NOR operation between two expressions.
    /// </summary>
    /// <param name="expr1">Represents the first logical expression.</param>
    /// <param name="expr2">Represents the second logical expression.</param>
    /// <returns>Returns the expression of a logical NOR operation.</returns>
    public static Expression<Func<T, bool>> Nor(
       Expression<Func<T, bool>>? expr1,
       Expression<Func<T, bool>>? expr2)
    {
        var orExpr = Or(expr1, expr2);

        return Not(orExpr);
    }

    /// <summary>
    /// Builds a predicate that represents a logical NAND operation between two expressions.
    /// </summary>
    /// <param name="expr1">Represents the first logical expression.</param>
    /// <param name="expr2">Represents the second logical expression.</param>
    /// <returns>Returns the expression of a logical NAND operation.</returns>
    public static Expression<Func<T, bool>> Nand(
        Expression<Func<T, bool>>? expr1,
        Expression<Func<T, bool>>? expr2)
    {
        var andExpr = And(expr1, expr2);

        return Not(andExpr);
    }

    /// <summary>
    /// Builds a predicate that represents a logical NOT operation between two expressions.
    /// </summary>
    /// <param name="expression">Represents the logical expression.</param>
    /// <returns>Returns the expression of a logical NOT operation.</returns>
    public static Expression<Func<T, bool>> Not(Expression<Func<T, bool>> expression)
    {
        var negated = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
    }

    /// <summary>
    /// Builds a predicate that represents a logical AND operation between two expressions.
    /// </summary>
    /// <param name="expr1">Represents the first logical expression.</param>
    /// <param name="expr2">Represents the second logical expression.</param>
    /// <returns>Returns the expression of a logical AND operation.</returns>
    public static Expression<Func<T, bool>> And(Expression<Func<T, bool>>? expr1,
                                                Expression<Func<T, bool>>? expr2)
    {
        if (expr1 is null)
        {
            return expr2 is null ? True : expr2;
        }

        if (expr2 is null)
        {
            return expr1 is null ? True : expr1;
        }

        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>
              (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }

    /// <summary>
    /// Builds a predicate that represents a logical AND operation between expressions.
    /// </summary>
    /// <param name="expressions">Represents the expressions to join.</param>
    /// <returns>Returns the expression of the logical AND operation.</returns>
    public static Expression<Func<T, bool>> And(params Expression<Func<T, bool>>?[] expressions)
    {
        var firstExpression = True;

        foreach (var item in expressions)
        {
            if (item is null)
            {
                continue;
            }

            firstExpression = And(firstExpression, item);
        }

        return firstExpression;
    }

    /// <summary>
    /// Builds a predicate that represents a logical XNOR operation between two expressions.
    /// </summary>
    /// <param name="expr1">Represents the first logical expression.</param>
    /// <param name="expr2">Represents the second logical expression.</param>
    /// <returns>Returns the expression of a logical XNOR operation.</returns>
    public static Expression<Func<T, bool>> Xnor(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var xorExpr = Xor(expr1, expr2);

        return Not(xorExpr);
    }

    /// <summary>
    /// Builds a predicate that represents a logical NOR operation between two expressions.
    /// </summary>
    /// <param name="expr1">Represents the first logical expression.</param>
    /// <param name="expr2">Represents the second logical expression.</param>
    /// <returns>Returns the expression of a logical NOR operation.</returns>
    public static Expression<Func<T, bool>> Xor(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var xorExpression = Expression.AndAlso(
            Expression.OrElse(Expression.Invoke(expr1, parameter), Expression.Invoke(expr2, parameter)),
            Expression.Not(Expression.AndAlso(Expression.Invoke(expr1, parameter), Expression.Invoke(expr2, parameter)))
        );

        return Expression.Lambda<Func<T, bool>>(xorExpression, parameter);
    }
}

