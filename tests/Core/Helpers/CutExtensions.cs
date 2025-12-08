using System;
using System.IO;
using System.Runtime.CompilerServices;
using Bunit;
using Microsoft.AspNetCore.Components;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Bunit.Rendering;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Verify;

public static class CutExtensions
{
    public static void Verify(this IRenderedComponent<ContainerFragment> cut, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "")
    {
        if (cut == null)
        {
            throw new ArgumentNullException(nameof(cut));
        }

        var className = Path.GetFileNameWithoutExtension(callerFilePath) ?? "UnknownTestClass";
        var fileNameBase = $"{className}.{memberName}";
        var dir = Path.GetDirectoryName(callerFilePath) ?? Directory.GetCurrentDirectory();
        Directory.CreateDirectory(dir);

        var verifiedPath = Path.Combine(dir, $"{fileNameBase}.verified.razor.html");
        var receivedPath = Path.Combine(dir, $"{fileNameBase}.received.razor.html");

        var markup = cut.Markup.Trim();
        File.WriteAllText(receivedPath, markup);

        if (!File.Exists(verifiedPath))
        {
            File.WriteAllText(verifiedPath, markup);
        }

        var expected = File.ReadAllText(verifiedPath);

        if (!NodesEqual(expected, markup))
        {
            throw new Exception($"Markup does not match verified file: {verifiedPath}");
        }
    }

    public static void Verify<TComponent>(this IRenderedComponent<TComponent> cut, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") where TComponent : IComponent
    {
        ArgumentNullException.ThrowIfNull(cut);

        var className = Path.GetFileNameWithoutExtension(callerFilePath) ?? "UnknownTestClass";
        var fileNameBase = $"{className}.{memberName}";
        var dir = Path.GetDirectoryName(callerFilePath) ?? Directory.GetCurrentDirectory();
        Directory.CreateDirectory(dir);

        var verifiedPath = Path.Combine(dir, $"{fileNameBase}.verified.razor.html");
        var receivedPath = Path.Combine(dir, $"{fileNameBase}.received.razor.html");

        var markup = cut.Markup.Trim();
        File.WriteAllText(receivedPath, markup);

        if (!File.Exists(verifiedPath))
        {
            File.WriteAllText(verifiedPath, markup);
        }

        var expected = File.ReadAllText(verifiedPath);

        if (!NodesEqual(expected, markup))
        {
            throw new Exception($"Markup does not match verified file: {verifiedPath}");
        }
    }

    private static bool NodesEqual(string leftHtml, string rightHtml)
    {
        var parser = new HtmlParser();
        var leftDoc = parser.ParseDocument(leftHtml);
        var rightDoc = parser.ParseDocument(rightHtml);

        var leftRoot = leftDoc.Body ?? leftDoc.DocumentElement;
        var rightRoot = rightDoc.Body ?? rightDoc.DocumentElement;

        return NodeEquals(leftRoot, rightRoot);
    }

    private static bool NodeEquals(INode a, INode b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        if (a.NodeType != b.NodeType)
        {
            return false;
        }

        switch (a.NodeType)
        {
            case NodeType.Text:
                {
                    return NormalizeWhitespace(a.TextContent) == NormalizeWhitespace(b.TextContent);
                }
            case NodeType.Element:
                {
                    var ae = (IElement)a;
                    var be = (IElement)b;
                    if (!string.Equals(ae.TagName, be.TagName, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    // Compare attributes (order-insensitive)
                    if (ae.Attributes.Length != be.Attributes.Length)
                    {
                        return false;
                    }

                    foreach (var attr in ae.Attributes)
                    {
                        var bAttr = be.GetAttribute(attr.Name);
                        if (bAttr == null)
                        {
                            return false;
                        }

                        if (!string.Equals(attr.Value, bAttr, StringComparison.Ordinal))
                        {
                            return false;
                        }
                    }

                    var aChildren = GetSignificantChildren(ae);
                    var bChildren = GetSignificantChildren(be);
                    if (aChildren.Length != bChildren.Length)
                    {
                        return false;
                    }

                    for (var i = 0; i < aChildren.Length; i++)
                    {
                        if (!NodeEquals(aChildren[i], bChildren[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            default:
                {
                    string aHtml;
                    string bHtml;

                    if (a is IElement aElem)
                    {
                        aHtml = aElem.OuterHtml;
                    }
                    else
                    {
                        aHtml = a?.ToString() ?? string.Empty;
                    }

                    if (b is IElement bElem)
                    {
                        bHtml = bElem.OuterHtml;
                    }
                    else
                    {
                        bHtml = b?.ToString() ?? string.Empty;
                    }

                    return NormalizeWhitespace(aHtml) == NormalizeWhitespace(bHtml);
                }
        }
    }

    private static INode[] GetSignificantChildren(INode node)
    {
        var list = new System.Collections.Generic.List<INode>();
        foreach (var child in node.ChildNodes)
        {
            if (child.NodeType == NodeType.Text)
            {
                if (string.IsNullOrWhiteSpace(child.TextContent))
                {
                    continue;
                }
            }

            list.Add(child);
        }

        return list.ToArray();
    }

    private static string NormalizeWhitespace(string s)
    {
        if (s == null)
        {
            return string.Empty;
        }

        return System.Text.RegularExpressions.Regex.Replace(s, "\\s+", " ").Trim();
    }
}
