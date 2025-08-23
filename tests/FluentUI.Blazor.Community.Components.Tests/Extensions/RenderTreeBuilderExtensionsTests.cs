using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace FluentUI.Blazor.Community.Components.Tests.Extensions;

public class RenderTreeBuilderExtensionsTests
{
    [Fact]
    public void AddChildContent_Should_Add_ChildContent_Parameter()
    {
        // Arrange
        var builder = new RenderTreeBuilder();
        var sequence = 123;
        RenderFragment fragment = b => b.AddContent(1, "Test content");

        // Act
        builder.OpenComponent<FluentCxSleekDial>(0);
        builder.AddChildContent(sequence, fragment);
        builder.CloseElement();

        // Assert
        var frames = builder.GetFrames().Array;
        Assert.Contains(frames, f =>
            f.FrameType == RenderTreeFrameType.Attribute &&
            f.AttributeName == "ChildContent" &&
            f.AttributeValue == fragment
        );
    }

    [Fact]
    public void AddChildContent_With_Null_RenderFragment_Should_Add_Null_ChildContent()
    {
        // Arrange
        var builder = new RenderTreeBuilder();
        RenderFragment fragment = null;

        // Act
        builder.OpenComponent<FluentCxSleekDial>(0);
        builder.AddChildContent(1, fragment);
        builder.CloseElement();

        // Assert
        var frames = builder.GetFrames().Array;
        Assert.Contains(frames, f =>
            f.FrameType == RenderTreeFrameType.Attribute &&
            f.AttributeName == "ChildContent" &&
            f.AttributeValue == null
        );
    }

    [Fact]
    public void AddChildContent_With_Different_Sequence_Should_Respect_Sequence()
    {
        // Arrange
        var builder = new RenderTreeBuilder();
        var sequence = 999;
        RenderFragment fragment = b => b.AddContent(0, "Another content");

        // Act
        builder.OpenComponent<FluentCxSleekDial>(0);
        builder.AddChildContent(sequence, fragment);
        builder.CloseComponent();


        // Assert
        var frames = builder.GetFrames().Array;
        Assert.Contains(frames, f =>
            f.FrameType == RenderTreeFrameType.Attribute &&
            f.Sequence == sequence &&
            f.AttributeName == "ChildContent"
        );
    }
}
