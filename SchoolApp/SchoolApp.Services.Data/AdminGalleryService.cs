﻿using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Gallery;
using SchoolApp.Web.ViewModels.Admin.Gallery.Components;

using static SchoolApp.Common.TempDataMessages.Gallery;

namespace SchoolApp.Services.Data;

public class AdminGalleryService : IAdminGalleryService
{
    private readonly IRepository _repository;
    private readonly IAzureBlobService _blobService;

    public AdminGalleryService(IRepository repository, IAzureBlobService blobService)
    {
        _repository = repository;
        _blobService = blobService;
    }

    public async Task<IEnumerable<MenageAlbumsViewModel>> GetAllAlbumsWithImagesAsync()
    {
        IEnumerable<MenageAlbumsViewModel> model = await _repository
            .GetAllAttached<Album>()
            .Select(a => new MenageAlbumsViewModel()
            {
                Id = a.Id.ToString(),
                Title = a.Title,
                Description = a.Description,
                Images = a.Images.Select(i => new MenageAlbumImageViewModel()
                {
                    Id = i.Id.ToString(),
                    ImageUrl = i.ImageUrl
                })
                .ToArray()
            })
            .ToArrayAsync();

        return model;
    }

    public async Task<(bool success, string message)> AddAlbumAsync(AddAlbumViewModel model)
    {
        if (model == null)
        {
            return (false, AlbumCreateError);
        }

        if (await _repository.FirstOrDefaultAsync<Album>(a => a.Title == model.Title) != null)
        {
            return (false, AlbumExistsError);
        }

        Album album = new Album()
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description
        };

        await _repository.AddAsync(album);

        return (true, AlbumCreateSuccess);
    }

    public async Task<MenageAlbumsViewModel?> GetDetailsForAlbumAsync(string id)
    {
        bool isIdValid = Guid.TryParse(id, out Guid guidId);

        if (!isIdValid)
        {
            return null;
        }

        MenageAlbumsViewModel? album = await _repository
            .GetAllAttached<Album>()
            .Where(a => a.Id == guidId)
            .Select(a => new MenageAlbumsViewModel()
            {
                Id = a.Id.ToString(),
                Title = a.Title,
                Description = a.Description,
                Images = a.Images.Select(i => new MenageAlbumImageViewModel()
                {
                    Id = i.Id.ToString(),
                    ImageUrl = i.ImageUrl
                })
                .ToArray()
            })
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return null;
        }

        return album;
    }

    public async Task<(bool success, string message)> AddImagesAsync(AddAlbumImageFormModel model)
    {
        bool isIdValid = Guid.TryParse(model.AlbumId, out Guid guidId);

        if (!isIdValid)
        {
            return (false, InvalidAlbumId);
        }

        if (model?.Image == null)
        {
            return (false, InvalidImage);
        }

        Album? album = await _repository.GetByGuidIdAsync<Album>(guidId);

        if (album == null)
        {
            return (false, AlbumNotFound);
        }

        if (model.Image.Length > 2 * 1024 * 1024)
        {
            return (false, ImageSizeError);
        }

        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        string extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return (false, AllowedFormatsMessage);
        }

        Guid imageId = Guid.NewGuid();

        var (isSuccessful, errorMessage, imageUrl) =
            await _blobService.UploadGalleryImageAsync(model.Image, imageId);

        if (!isSuccessful || string.IsNullOrEmpty(imageUrl))
        {
            return (false, errorMessage ?? ImageUploadError);
        }

        GalleryImage image = new GalleryImage
        {
            Id = imageId,
            ImageUrl = imageUrl,
            AlbumId = guidId
        };

        await _repository.AddAsync(image);

        return (true, ImageUploadSuccess);
    }

    public async Task<(bool success, string message)> DeleteImageAsync(string imageId)
    {
        bool isIdValid = Guid.TryParse(imageId, out Guid guidId);

        if (!isIdValid)
        {
            return (false, InvalidImageId);
        }

        GalleryImage? image = await _repository
            .GetAllAttached<GalleryImage>()
            .FirstOrDefaultAsync(i => i.Id == guidId);

        if (image == null)
        {
            return (false, ImageNotFound);
        }

        bool isDeleted = await _blobService.DeleteGalleryImageAsync(image.ImageUrl);

        if (!isDeleted)
        {
            return (false, ImageDeleteBlobError);
        }

        await _repository.DeleteByGuidIdAsync<GalleryImage>(guidId);

        return (true, ImageDeleteSuccess);
    }

    public async Task<(bool success, string message)> DeleteAlbumAsync(string albumId)
    {
        bool isIdValid = Guid.TryParse(albumId, out Guid guidId);
        if (!isIdValid)
        {
            return (false, InvalidAlbumId);
        }

        Album? album = await _repository
            .GetAllAttached<Album>()
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == guidId);

        if (album == null)
        {
            return (false, AlbumNotFound);
        }

        if (album.Images.Count != 0)
        {
            foreach (var image in album.Images)
            {
                bool isImageDeleted = await _blobService.DeleteGalleryImageAsync(image.ImageUrl);
                if (!isImageDeleted)
                {
                    return (false, ImagesDeleteBlobError);
                }
            }
        }
        await _repository.DeleteByGuidIdAsync<Album>(guidId);

        return (true, AlbumDeleteSuccess);
    }
}