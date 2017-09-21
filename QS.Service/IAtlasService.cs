using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface IAtlasService
    {
        void AddAtlas(AtlasDto atlasDto);
        void DeleteAtlas(Guid atlasId);
        AtlasDto GetAtlasById(Guid atlasId);
        bool ChangeAtlasDetail(Guid atlasId, AtlasDto updatedAtlasDto);
        IEnumerable<AtlasDto> GetAtlasesWithCategory(string category, int pageIndex, int pageCount, out int count);
        IEnumerable<AtlasDto> GetAtlasesWithCategory(int categoryId, int pageIndex, int pageCount, out int count);
        IEnumerable<AtlasDto> GetAtlasPaged(int pageIndex, int pageCount, out int count);
        IEnumerable<AtlasDto> GetAllAtlases();

        IEnumerable<AtlasDto> GetRandomAtlases(int num = 8);
        IEnumerable<AtlasDto> GetPopularAtlases(int num = 5);
        IEnumerable<AtlasDto> GetNewestAtlases(int num = 6);
        int IncreaseViewsOf(Guid atlasId);
    }
}
