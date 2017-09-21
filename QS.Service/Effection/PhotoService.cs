using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.SharedAggregate;
using QS.DTO.SharedModule;

namespace QS.Service.Effection
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        public PhotoService() { }

        public PhotoService(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public void AddPhoto(PhotoDto photoDto)
        {
            photoDto.CreateTime = DateTime.Now;
            _photoRepository.Add(QsMapper.CreateMap<PhotoDto, Photo>(photoDto));
            _photoRepository.UnitOfWork.Commit();
        }

        public void DeletePhoto(Guid photoId)
        {
            var temp = _photoRepository.Get(photoId);
            if (temp != null)
            {
                _photoRepository.Remove(temp);
                _photoRepository.UnitOfWork.Commit();
            }
        }

        public PhotoDto GetPhotoById(Guid photoId)
        {
            var temp = _photoRepository.Get(photoId);
            return temp == null ? new PhotoDto() : (QsMapper.CreateMap<Photo, PhotoDto>(temp));
        }

        public bool ChangePhotoDetail(Guid photoId, PhotoDto updatedPhotoDto)
        {
            var original = _photoRepository.Get(photoId);
            var recent = QsMapper.CreateMap<PhotoDto, Photo>(updatedPhotoDto);
            if (original != null && recent != null)
            {
                _photoRepository.Merge(original, recent);
                _photoRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<PhotoDto> GetPhotosWithCategory(string category, int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            if (String.IsNullOrEmpty(category))
            {
                var photoEnumrable = _photoRepository.GetPaged<DateTime>(pageIndex, pageCount, out count, p => p.CreateTime, false);
                return QsMapper.CreateMapIEnume<Photo, PhotoDto>(photoEnumrable);
            }

            throw new NotImplementedException();
        }

        public IEnumerable<PhotoDto> GetPhotosWithCategory(int categoryId, int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            throw new NotImplementedException();

        }

        public IEnumerable<PhotoDto> GetPhotoPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageCount <= 0 || pageIndex <= 0)
            {
                count = 0;
                return null;
            }

            var photoEnumrable = _photoRepository.GetPaged<DateTime>(pageIndex, pageCount, out count, p => p.CreateTime, false);
            return QsMapper.CreateMapIEnume<Photo, PhotoDto>(photoEnumrable);
        }

        public IEnumerable<PhotoDto> GetAllPhotos()
        {
            var allPhotos = _photoRepository.GetAllWithOrder(p => p.CreateTime);
            return QsMapper.CreateMapIEnume<Photo, PhotoDto>(allPhotos);
        }

        public IEnumerable<PhotoDto> GetPhotosUnderAtlasId(Guid id)
        {
            var photosUnderal = _photoRepository.GetFiltered(it => it.AtlasId.Equals(id));
            return QsMapper.CreateMapIEnume<Photo, PhotoDto>(photosUnderal);
        }
    }
}
