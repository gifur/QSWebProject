using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.SharedAggregate;
using QS.DTO.SharedModule;

namespace QS.Service.Effection
{
    public class AtlasService : IAtlasService
    {
        private readonly IAtlasRepository _atlasRepository;
        public AtlasService() { }

        public AtlasService(IAtlasRepository atlasRepository)
        {
            _atlasRepository = atlasRepository;
        }
        public void AddAtlas(AtlasDto atlasDto)
        {
            atlasDto.CreateTime = DateTime.Now;
            _atlasRepository.Add(QsMapper.CreateMap<AtlasDto, Atlas>(atlasDto));
            _atlasRepository.UnitOfWork.Commit();
        }

        public void DeleteAtlas(Guid atlasId)
        {
            var temp = _atlasRepository.Get(atlasId);
            if (temp == null) return;
            _atlasRepository.Remove(temp);
            _atlasRepository.UnitOfWork.Commit();
        }

        public AtlasDto GetAtlasById(Guid atlasId)
        {
            var temp = _atlasRepository.Get(atlasId);
            return temp == null ? null : (QsMapper.CreateMap<Atlas, AtlasDto>(temp));
        }

        public bool ChangeAtlasDetail(Guid atlasId, AtlasDto updatedAtlasDto)
        {
            var original = _atlasRepository.Get(atlasId);
            var recent = QsMapper.CreateMap<AtlasDto, Atlas>(updatedAtlasDto);
            if (original != null && recent != null)
            {
                _atlasRepository.Merge(original, recent);
                _atlasRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<AtlasDto> GetAtlasesWithCategory(string category, int pageIndex, int pageCount, out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AtlasDto> GetAtlasesWithCategory(int categoryId, int pageIndex, int pageCount, out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AtlasDto> GetAtlasPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageCount <= 0 || pageIndex <= 0)
            {
                count = 0;
                return null;
            }

            var atlasEnumrable = _atlasRepository.GetPaged<DateTime>(pageIndex, pageCount, out count, p => p.CreateTime, false);
            return QsMapper.CreateMapIEnume<Atlas, AtlasDto>(atlasEnumrable);
        }

        public IEnumerable<AtlasDto> GetAllAtlases()
        {
            var allAtlas = _atlasRepository.GetAllWithOrder(p => p.CreateTime);
            return QsMapper.CreateMapIEnume<Atlas, AtlasDto>(allAtlas);
        }

        public IEnumerable<AtlasDto> GetRandomAtlases(int num = 8)
        {
            var sql = String.Format("SELECT TOP {0} * FROM Atlas ORDER BY NewID()", num);
            var results = _atlasRepository.ExecuteQuery(sql);
            return QsMapper.CreateMapIEnume<Atlas, AtlasDto>(results);
        }

        public IEnumerable<AtlasDto> GetPopularAtlases(int num = 5)
        {
            var results = _atlasRepository.GetPaged(0, num, at => at.Hits, false);
            return QsMapper.CreateMapIEnume<Atlas, AtlasDto>(results);
        }

        public IEnumerable<AtlasDto> GetNewestAtlases(int num = 6)
        {
            var results = _atlasRepository.GetPaged(0, num, at => at.CreateTime, false);
            return QsMapper.CreateMapIEnume<Atlas, AtlasDto>(results);
        }

        public int IncreaseViewsOf(Guid atlasId)
        {
            var sql = String.Format("UPDATE Atlas SET Hits = Hits + 1 WHERE AtlasId = '{0}'", atlasId);
            return _atlasRepository.ExecuteCommand(sql);
        }
    }
}
