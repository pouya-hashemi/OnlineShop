using Microsoft.AspNetCore.Http;

namespace OnlineShop.ApplicationTest.Common;

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
    public IFormFile CreateImageFormFile()
    {
        return new FormFile(_stream, 0, _stream.Length, "ImageFile", "FakeImage.jpg");
    }



    public void Dispose()
    {
        _stream.Dispose();
    }
}