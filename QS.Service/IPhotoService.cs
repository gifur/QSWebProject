using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface IPhotoService
    {
        void AddPhoto(PhotoDto photoDto);
        void DeletePhoto(Guid photoId);
        PhotoDto GetPhotoById(Guid photoId);
        bool ChangePhotoDetail(Guid photoId, PhotoDto updatedPhotoDto);
        IEnumerable<PhotoDto> GetPhotosWithCategory(string category, int pageIndex, int pageCount, out int count);
        IEnumerable<PhotoDto> GetPhotosWithCategory(int categoryId, int pageIndex, int pageCount, out int count);
        IEnumerable<PhotoDto> GetPhotoPaged(int pageIndex, int pageCount, out int count);
        IEnumerable<PhotoDto> GetAllPhotos();
        IEnumerable<PhotoDto> GetPhotosUnderAtlasId(Guid id);

    }
}
