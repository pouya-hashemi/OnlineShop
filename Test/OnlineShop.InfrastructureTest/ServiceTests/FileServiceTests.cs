using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Services;
using OnlineShop.InfrastructureTest.Fixtures;
using OnlineShop.TestShareContent.DataGenerators;


namespace OnlineShop.InfrastructureTest.ServiceTests;
[Collection("Service collection")]
public class FileServiceTests : IAsyncLifetime
{
    private readonly IFileService _fileService;
    private readonly IConfiguration _configuration;
    private readonly FormFileGenerator _formFileGenerator;

    public FileServiceTests(ServiceFixture serviceFixture)
    {

        _fileService = serviceFixture.ServiceProvider.GetService<IFileService>();
        _configuration = serviceFixture.ServiceProvider.GetService<IConfiguration>();
        _formFileGenerator = new FormFileGenerator();
    }

    [Fact]
    public async Task SaveImageFile_ShouldThrowFileExtensionException_WhenFileExtensionIsNotCorrect()
    {
        //Arrange
        var pdfFile = _formFileGenerator.CreateFileFormFilePdf();
        //Act
        var act = async () => { await _fileService.SaveImageFileAsync(pdfFile); };
        //Assert
        await Assert.ThrowsAsync<FileExtensionException>(act);
    }

    //*********************************************
    [Fact]
    public async Task SaveImageFile_ShouldThrowNullOrEmptyException_WhenFileIsEmpty()
    {
        //Arrange
        var file = _formFileGenerator.CreateEmptyImageFormFile();
        //Act
        var act = async () => { await _fileService.SaveImageFileAsync(file); };
        //Assert
        await Assert.ThrowsAsync<NullOrEmptyException>(act);
    }

    //****************************************************
    [Fact]
    public async Task SaveImageFile_ShouldSaveImage_WhenInputDataIsCorrect()
    {
        //Arrange
        var jpgFile = _formFileGenerator.CreateImageFormFileJpg();
        //Act
        var path = await _fileService.SaveImageFileAsync(jpgFile);
        //Assert
        Assert.True(File.Exists(_configuration["FileStoringBasePath"] + path));
    }

    //******************************************
    [Fact]
    public async Task RemoveFile_ShouldDeleteFile_WhenFileExists()
    {
        //Arrange
        var jpgFile = _formFileGenerator.CreateImageFormFileJpg();
        var path = await _fileService.SaveImageFileAsync(jpgFile);

        //Act
        _fileService.RemoveFile(_configuration["FileStoringBasePath"] + path);
        //Assert
        Assert.False(File.Exists(_configuration["FileStoringBasePath"] + path));
    }

    //******************************************


    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _formFileGenerator.Dispose();
        
        return Task.CompletedTask;
    }
}