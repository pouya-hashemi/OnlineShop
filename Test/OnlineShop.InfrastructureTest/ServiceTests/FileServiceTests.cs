using Microsoft.Extensions.Configuration;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Services;
using OnlineShop.TestShareContent.Common;
using OnlineShop.TestShareContent.DataGenerators;
using OnlineShop.TestShareContent.SharedFixtures;

namespace OnlineShop.InfrastructureTest.ServiceTests;

public class FileServiceTests : IAsyncLifetime, IClassFixture<FileServiceFixture>
{
    private readonly IFileService _fileService;
    private readonly IConfiguration _configuration;
    private readonly FormFileGenerator _formFileGenerator;

    public FileServiceTests(FileServiceFixture fileServiceFixture)
    {
        var utilities = new Utilities();
        _fileService = fileServiceFixture.FileService;
        _configuration = utilities.Configuration;
        _formFileGenerator = new FormFileGenerator();
    }

    [Fact]
    public async Task SaveImageFile_ShouldThrowFileExtensionException_WhenFileExtensionIsNotCorrect()
    {
        //Arrange
        var pdfFile = _formFileGenerator.CreateFileFormFilePdf();
        //Act
        var act = async () => { await _fileService.SaveImageFile(pdfFile); };
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
        var act = async () => { await _fileService.SaveImageFile(file); };
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
        var path = await _fileService.SaveImageFile(jpgFile);
        //Assert
        Assert.True(File.Exists(_configuration["FileStoringBasePath"] + path));
    }

    //******************************************
    [Fact]
    public async Task RemoveFile_ShouldDeleteFile_WhenFileExists()
    {
        //Arrange
        var jpgFile = _formFileGenerator.CreateImageFormFileJpg();
        var path = await _fileService.SaveImageFile(jpgFile);

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