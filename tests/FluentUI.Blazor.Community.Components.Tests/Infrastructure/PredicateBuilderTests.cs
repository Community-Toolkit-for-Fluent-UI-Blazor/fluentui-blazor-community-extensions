using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Infrastructure;

namespace FluentUI.Blazor.Community.Components.Facts.Infrastructure;

public class PredicateBuilderFacts
{
    private class FactClass
    {
        public int Value { get; set; }
    }

    [Fact]
    public void True_ShouldReturnTruePredicate()
    {
        var predicate = PredicateBuilder<FactClass>.True;
        var result = predicate.Compile()(new FactClass());
        Assert.True(result);
    }

    [Fact]
    public void False_ShouldReturnFalsePredicate()
    {
        var predicate = PredicateBuilder<FactClass>.False;
        var result = predicate.Compile()(new FactClass());
        Assert.False(result);
    }

    [Fact]
    public void Or_WithBothNull_ReturnsFalse()
    {
        var result = PredicateBuilder<FactClass>.Or(null, null);
        Assert.False(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Or_WithFirstNull_ReturnsSecondPredicate()
    {
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Or(null, expr2);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Or_WithSecondNull_ReturnsFirstPredicate()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Or(expr1, null);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Or_WithTwoTrue_ReturnsTrue()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Or(expr1, expr2);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Or_WithOneTrueOneFalse_ReturnsTrue()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.False;
        var result = PredicateBuilder<FactClass>.Or(expr1, expr2);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void And_WithBothNull_ReturnsTrue()
    {
        var result = PredicateBuilder<FactClass>.And(null, null);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void And_WithFirstNull_ReturnsSecondPredicate()
    {
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.And(null, expr2);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void And_WithSecondNull_ReturnsFirstPredicate()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.And(expr1, null);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void And_WithTwoTrue_ReturnsTrue()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.And(expr1, expr2);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void And_WithOneTrueOneFalse_ReturnsFalse()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.False;
        var result = PredicateBuilder<FactClass>.And(expr1, expr2);
        Assert.False(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Not_ShouldNegatePredicate()
    {
        var expr = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Not(expr);
        Assert.False(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Xor_WithBothTrue_ReturnsFalse()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Xor(expr1, expr2);
        Assert.False(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Xor_WithOneTrueOneFalse_ReturnsTrue()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.False;
        var result = PredicateBuilder<FactClass>.Xor(expr1, expr2);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Xnor_WithBothTrue_ReturnsTrue()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Xnor(expr1, expr2);
        Assert.True(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Nand_WithBothTrue_ReturnsFalse()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Nand(expr1, expr2);
        Assert.False(result.Compile()(new FactClass()));
    }

    [Fact]
    public void Nor_WithBothTrue_ReturnsFalse()
    {
        var expr1 = PredicateBuilder<FactClass>.True;
        var expr2 = PredicateBuilder<FactClass>.True;
        var result = PredicateBuilder<FactClass>.Nor(expr1, expr2);
        Assert.False(result.Compile()(new FactClass()));
    }
}
