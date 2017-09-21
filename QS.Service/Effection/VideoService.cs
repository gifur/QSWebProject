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
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        public VideoService() { }

        public VideoService(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }
        public void AddVideo(VideoDto videoDto)
        {
            videoDto.CreateTime = DateTime.Now;
            _videoRepository.Add(QsMapper.CreateMap<VideoDto, Video>(videoDto));
            _videoRepository.UnitOfWork.Commit();
        }

        public void DeleteVideo(Int64 videoId)
        {
            var temp = _videoRepository.Get(videoId);
            if (temp != null)
            {
                _videoRepository.Remove(temp);
                _videoRepository.UnitOfWork.Commit();
            }
        }

        public VideoDto GetVideoById(Int64 videoId)
        {
            var temp = _videoRepository.Get(videoId);
            return temp == null ? null : (QsMapper.CreateMap<Video, VideoDto>(temp));
        }

        public bool ChangeVideoDetail(Int64 videoId, VideoDto updatedVideoDto)
        {
            var original = _videoRepository.Get(videoId);
            var recent = QsMapper.CreateMap<VideoDto, Video>(updatedVideoDto);
            if (original != null && recent != null)
            {
                _videoRepository.Merge(original, recent);
                _videoRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<VideoDto> GetVideosWithCategory(string category, out int count, bool new2Old = true)
        {
            var result = _videoRepository.GetFiltered(v => v.Category.Contains(category), out count);
            if (new2Old)
            {
                result = result.OrderByDescending(v => v.CreateTime);                
            }
            return QsMapper.CreateMapIEnume<Video, VideoDto>(result);
        }

        public IEnumerable<VideoDto> GetVideosWithCategory(string category, int pageIndex, int pageCount, out int count)
        {
            var result = _videoRepository.GetPagedWithFilter(v => v.Category.Contains(category), pageIndex, pageCount,
                out count, v => v.CreateTime, false);
            return QsMapper.CreateMapIEnume<Video, VideoDto>(result);
        }

        public IEnumerable<VideoDto> GetVideosWithCategory(int categoryId, int pageIndex, int pageCount, out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VideoDto> GetVideoPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageCount <= 0 || pageIndex <= 0)
            {
                count = 0;
                return null;
            }

            var videoEnumrable = _videoRepository.GetPaged<DateTime>(pageIndex, pageCount, out count, p => p.CreateTime, false);
            return QsMapper.CreateMapIEnume<Video, VideoDto>(videoEnumrable);
        }

        public IEnumerable<VideoDto> GetAllVideos()
        {
            var allVideo = _videoRepository.GetAllWithOrder(p => p.CreateTime);
            return QsMapper.CreateMapIEnume<Video, VideoDto>(allVideo);
        }

        public IEnumerable<VideoDto> GetTheCommendVideos(int number, out int count)
        {
            var result = _videoRepository.GetPaged<Boolean, DateTime>(1, number, n => n.Recommend, n => n.CreateTime, false, out count);
            return QsMapper.CreateMapIEnume<Video, VideoDto>(result);
        }

        public IEnumerable<VideoDto> GetMostClickVideos(int number)
        {
            var result = _videoRepository.GetPaged(0, number, v => v.Hits, false);
            return QsMapper.CreateMapIEnume<Video, VideoDto>(result);
        }

        public int IncreaseViewsOf(long videoId)
        {
            var sql = String.Format("UPDATE Video SET Hits = Hits + 1 WHERE VideoId = {0}", videoId);
            return _videoRepository.ExecuteCommand(sql);
        }
    }
}
