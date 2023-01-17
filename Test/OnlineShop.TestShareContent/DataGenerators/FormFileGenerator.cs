using Microsoft.AspNetCore.Http;

namespace OnlineShop.TestShareContent.DataGenerators;

public class FormFileGenerator:IDisposable
{
    private MemoryStream _stream;

    public FormFileGenerator()
    {
        _stream = new MemoryStream();
        var writer = new StreamWriter(_stream);
        writer.Write("Fake File Content");
        writer.Flush();
        _stream.Position = 0;
    }
    public IFormFile CreateImageFormFileJpg()
    {
        return new FormFile(_stream, 0, _stream.Length, "ImageFile", "FakeImage.jpg");
    }
    public IFormFile CreateImageFormFileJfif()
    {
        return new FormFile(_stream, 0, _stream.Length, "ImageFile", "FakeImage.jfif");
    }
    public IFormFile CreateFileFormFilePdf()
    {
        return new FormFile(_stream, 0, _stream.Length, "ImageFile", "FakeImage.pdf");
    }

    public IFormFile CreateEmptyImageFormFile()
    {
        return new FormFile(_stream, 0, 0, "ImageFile", "FakeImage.jpg");
    }



    public void Dispose()
    {
        _stream.Dispose();
    }
}