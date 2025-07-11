using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Vml;
using FluentAssertions;
using NUnit.Framework;
using ShapeCrawler.DevTests.Helpers;

namespace ShapeCrawler.DevTests;

public class FontColorTests : SCTest
{
    [Test]
    public void Hex_returns_White_color()
    {
        // Arrange
        var pres = new Presentation(TestAsset("020.pptx"));
        var shape = pres.Slides[0].Shapes.First(sp => sp.Id == 4);

        // Act & Assert
        shape.TextBox!.Paragraphs[0].Portions[0].Font!.Color.Hex.Should().Be("FFFFFF");
    }
    
    [Test]
    public void Hex_returns_Black_color()
    {
        // Arrange
        var pres = new Presentation(TestAsset("078 textbox.pptx"));
        var shape = pres.Slide(1).Shape("TextBox 1");

        // Act-Assert
        shape.TextBox!.Paragraphs[0].Portions[0].Font!.Color.Hex.Should().Be("000000");
    }

    [Test]
    public void Hex_returns_Slide_Layout_Placeholder_color()
    {
        // Arrange
        var pres = new Presentation(TestAsset("001.pptx"));
        var titlePlaceholder = pres.Slides[0].SlideLayout.Shapes.GetById<IShape>(2);
        var fontColor = titlePlaceholder.TextBox!.Paragraphs[0].Portions[0].Font!.Color;

        // Act & Assert
        fontColor.Hex.Should().Be("000000");
    }

    [Test]
    public void ColorHex_Getter_returns_color_of_SlideMaster_Non_Placeholder()
    {
        // Arrange
        IShape nonPlaceholder = (IShape)new Presentation(TestAsset("001.pptx")).SlideMasters[0].Shapes.First(sp => sp.Id == 8);
        IFontColor colorFormat = nonPlaceholder.TextBox.Paragraphs[0].Portions[0].Font.Color;

        // Act-Assert
        colorFormat.Hex.Should().Be("FFFFFF");
    }

    [Test]
    public void ColorHex_Getter_returns_color_of_Title_SlideMaster_Placeholder()
    {
        // Arrange
        IShape titlePlaceholder = (IShape)new Presentation(TestAsset("001.pptx")).SlideMasters[0].Shapes.First(sp => sp.Id == 2);
        IFontColor colorFormat = titlePlaceholder.TextBox.Paragraphs[0].Portions[0].Font.Color;

        // Act-Assert
        colorFormat.Hex.Should().Be("000000");
    }

    [Test]
    public void ColorHex_Getter_returns_color_of_Table_Cell_on_Slide()
    {
        // Arrange
        var pres = new Presentation(TestAsset("001.pptx"));
        var table = pres.Slides[1].Shapes.GetById<ITable>(4);
        var fontColor = table.Rows[0].Cells[0].TextBox.Paragraphs[0].Portions[0].Font.Color;

        // Act-Assert
        fontColor.Hex.Should().Be("FF0000");
    }

    [Test]
    public void ColorType_ReturnsSchemeColorType_WhenFontColorIsTakenFromThemeScheme()
    {
        // Arrange
        var pres = new Presentation(TestAsset("020.pptx"));
        var nonPhAutoShape = pres.Slides[0].Shapes.GetById<IShape>(2);
        var fontColor = nonPhAutoShape.TextBox.Paragraphs[0].Portions[0].Font.Color;

        // Act
        var colorType = fontColor.Type;

        // Assert
        colorType.Should().Be(ColorType.Theme);
    }

    [Test]
    public void Type_ReturnsSchemeColorType_WhenFontColorIsSetAsRGB()
    {
        // Arrange
        var pres = new Presentation(TestAsset("014.pptx"));
        var placeholder = pres.Slides[5].Shapes.GetById<IShape>(52);
        var fontColor = placeholder.TextBox.Paragraphs[0].Portions[0].Font.Color;

        // Act
        var colorType = fontColor.Type;

        // Assert
        colorType.Should().Be(ColorType.RGB);
    }

    [Test]
    [SlideQueryPortion("020.pptx", 1, "TextBox 1", 1,  1)]
    [SlideQueryPortion("001.pptx", 1, 3, 1,  1)]
    [SlideQueryPortion("001.pptx", 3, 4, 1,  1)]
    [SlideQueryPortion("001.pptx", 5, 5, 1,  1)]
    public void Update_updates_font_color(IPresentation pres, TestPortionQuery portionQuery)
    {
        // Arrange
        var mStream = new MemoryStream();
        var fontColor = portionQuery.Get(pres).Font!.Color;

        // Act
        fontColor.Set("#008000");

        // Assert
        fontColor.Hex.Should().Be("008000");

        pres.Save(mStream);
        pres = new Presentation(mStream);
        fontColor = portionQuery.Get(pres).Font!.Color;
        fontColor.Hex.Should().Be("008000");
    }
    
    [Test]
    public void Update_updates_font_color_of_master_shape()
    {
        // Arrange
        var stream = new MemoryStream();
        var pres = new Presentation(TestAsset("061_font-color.pptx"));
        var fontColor = pres.SlideMasters[0].Shapes.Shape("TextBox 1").TextBox.Paragraphs[0].Portions[0].Font!.Color;
        
        // Act
        fontColor.Set("#007F00");
        
        // Assert
        pres.Save(stream);
        pres = new Presentation(stream);
        pres.Validate();
        fontColor = pres.SlideMasters[0].Shapes.Shape("TextBox 1").TextBox.Paragraphs[0].Portions[0].Font!.Color;
        fontColor.Hex.Should().Be("007F00");
    }
    
    [Test]
    [MasterPortion("autoshape-case001.pptx", "AutoShape 1", 1,  1)]
    public void Update_updates_font_color_of_master(IPresentation pres, TestPortionQuery portionQuery)
    {
        // Arrange
        var mStream = new MemoryStream();
        var color = portionQuery.Get(pres).Font!.Color;

        // Act
        color.Set("#008000");

        // Assert
        color.Hex.Should().Be("008000");

        pres.Save(mStream);
        pres = new Presentation(mStream);
        color = portionQuery.Get(pres).Font!.Color;
        color.Hex.Should().Be("008000");
    }
    
    [Test]
    [SlidePortion("Test Case #1", "020.pptx", slide: 1, shapeId: 2, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #2", "020.pptx", slide: 1, shapeId: 3, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #3", "020.pptx", slide: 3, shapeId: 8, paragraph: 2, portion: 1, expectedResult: "FFFF00")]
    [SlidePortion("Test Case #4", "001.pptx", slide: 1, shapeId: 4, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #5", "002.pptx", slide: 2, shapeId: 3, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #6", "026.pptx", slide: 1, shapeId: 128, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #7", "autoshape-case017_slide-number.pptx", slide: 1, shapeId: 5, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #8", "031.pptx", slide: 1, shapeId: 44, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #9", "033.pptx", slide: 1, shapeId: 3, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #10", "038.pptx", slide: 1, shapeId: 102, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #11", "001.pptx", slide: 3, shapeId: 4, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #12", "001.pptx", slide: 5, shapeId: 5, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #13", "034.pptx", slide: 1, shapeId: 2, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #14", "035.pptx", slide: 1, shapeId: 9, paragraph: 1, portion: 1, expectedResult: "000000")]
    [SlidePortion("Test Case #15", "036.pptx", slide: 1, shapeId: 6146, paragraph: 1, portion: 1, expectedResult: "404040")]
    [SlidePortion("Test Case #16", "037.pptx", slide: 1, shapeId: 7, paragraph: 1, portion: 1, expectedResult: "1A1A1A")]
    [SlidePortion("Test Case #17", "014.pptx", slide: 1, shapeId: 61, paragraph: 1, portion: 1, expectedResult: "595959")]
    [SlidePortion("Test Case #18", "014.pptx", slide: 6, shapeId: 52, paragraph: 1, portion: 1, expectedResult: "FFFFFF")]
    [SlidePortion("Test Case #19", "032.pptx", slide: 1, shapeId: 10242, paragraph: 1, portion: 1, expectedResult: "0070C0")]
    public void ColorHex_Getter_returns_color_hex(IParagraphPortion portion, string expectedColorHex)
    {
        // Arrange
        var fontColor = portion.Font!.Color;

        // Act
        var colorHex = fontColor.Hex;

        // Assert
        colorHex.Should().Be(expectedColorHex);
    }
}