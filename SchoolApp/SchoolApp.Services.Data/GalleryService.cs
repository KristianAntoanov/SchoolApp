using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Gallery;

namespace SchoolApp.Services.Data
{
	public class GalleryService : IGalleryService
    {
        private readonly IRepository _repository;

        public GalleryService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AlbumViewModel>> GetAllAlbumsWithImagesAsync()
        {
            IEnumerable<AlbumViewModel> model = await _repository
                .GetAllAttached<Album>()
                .Select(a => new AlbumViewModel()
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Images = a.Images.Select(i => new GalleryImageViewModel()
                    {
                        ImageUrl = i.ImageUrl
                    })
                    .ToArray()
                })
                .ToArrayAsync();

            return model;
        }
    }
}