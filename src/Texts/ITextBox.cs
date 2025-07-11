﻿// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
#pragma warning disable IDE0130
namespace ShapeCrawler;
using ShapeCrawler.Texts;
#pragma warning restore IDE0130

/// <summary>
///     Represents a text box.
/// </summary>
public interface ITextBox
{
    /// <summary>
    ///     Gets the collection of paragraphs.
    /// </summary>
    IParagraphCollection Paragraphs { get; }

    /// <summary>
    ///     Gets text.
    /// </summary>
    string Text { get; }

    /// <summary>
    ///     Gets or sets the text vertical alignment.
    /// </summary>
    TextVerticalAlignment VerticalAlignment { get; set; }

    /// <summary>
    ///     Gets or sets the Autofit type.
    /// </summary>
    AutofitType AutofitType { get; set; }

    /// <summary>
    ///     Gets or sets the TextDirection.
    /// </summary>
    TextDirection TextDirection { get; set; }

    /// <summary>
    ///     Gets or sets the left margin in points.
    /// </summary>
    decimal LeftMargin { get; set; }

    /// <summary>
    ///     Gets or sets the right margin in points.
    /// </summary>
    decimal RightMargin { get; set; }

    /// <summary>
    ///     Gets or sets the top margin in points.
    /// </summary>
    decimal TopMargin { get; set; }

    /// <summary>
    ///     Gets or sets the bottom margin in points.
    /// </summary>
    decimal BottomMargin { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the text is wrapped in the shape.
    /// </summary>
    bool TextWrapped { get; }

    /// <summary>
    ///     Gets XPath.
    /// </summary>
    public string SDKXPath { get; }

    /// <summary>
    ///     Sets text.
    /// </summary>
    public void SetText(string text);
    
    /// <summary>
    ///     Sets Markdown text.
    /// </summary>
    public void SetMarkdownText(string text);
}