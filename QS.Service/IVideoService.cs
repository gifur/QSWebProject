using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface IVideoService
    {
        void AddVideo(VideoDto videoDto);
        void DeleteVideo(Int64 videoId);
        VideoDto GetVideoById(Int64 videoId);
        bool ChangeVideoDetail(Int64 videoId, VideoDto updatedVideoDto);
        IEnumerable<VideoDto> GetVideosWithCategory(string category, out int count, bool new2Old = true);
        IEnumerable<VideoDto> GetVideosWithCategory(string category, int pageIndex, int pageCount, out int count);
        IEnumerable<VideoDto> GetVideosWithCategory(int categoryId, int pageIndex, int pageCount, out int count);
        IEnumerable<VideoDto> GetVideoPaged(int pageIndex, int pageCount, out int count);
        IEnumerable<VideoDto> GetAllVideos();
        IEnumerable<VideoDto> GetTheCommendVideos(int number, out int count);
        IEnumerable<VideoDto> GetMostClickVideos(int number);
        int IncreaseViewsOf(Int64 videoId);
    }
}
