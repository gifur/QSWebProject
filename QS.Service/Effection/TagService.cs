using System;
using System.Collections.Generic;
using System.Linq;
using QS.Common;
using QS.Core.IRepository;
using QS.DTO.Module;

namespace QS.Service.Effection
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService() { }

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public void AddTag(TagDto tagDto)
        {
            tagDto.CreateTime = DateTime.Now;
            _tagRepository.Add(QsMapper.CreateMap<TagDto, Core.Module.Tag>(tagDto));
            _tagRepository.UnitOfWork.Commit();
        }

        public void DeleteTag(int tagId)
        {
            var temp = _tagRepository.Get(tagId);
            if (temp == null) return;
            _tagRepository.Remove(temp);
            _tagRepository.UnitOfWork.Commit();
        }

        public TagDto GetTagById(int tagId)
        {
            var temp = _tagRepository.Get(tagId);
            return temp == null ? new TagDto() : (QsMapper.CreateMap<Core.Module.Tag, TagDto>(temp));
        }

        public bool ChangeTagDescription(int tagId, TagDto updatedTagDto)
        {
   
            var original = _tagRepository.Get(tagId);
            var recent = QsMapper.CreateMap<TagDto, Core.Module.Tag>(updatedTagDto);
            if (original != null && recent != null)
            {
                _tagRepository.Merge(original, recent);
                _tagRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<TagDto> GetAllTags()
        {
            var allTag = _tagRepository.GetAll().OrderByDescending(n => n.TagSum).AsEnumerable();
            //var allTag = _tagRepository.GetAll().OrderByDescending(n => n.IsTop).ThenByDescending(n => n.CreateTime).AsEnumerable();
            return QsMapper.CreateMapIEnume<Core.Module.Tag, TagDto>(allTag);
        }

        public IEnumerable<TagDto> GetTagPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            var tagEnumrable = _tagRepository.GetPaged(pageIndex, pageCount, out count, n => n.TagSum, false); 
            //var tagEnumrable = _tagRepository.GetPaged<Boolean, DateTime>(pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            return QsMapper.CreateMapIEnume<Core.Module.Tag, TagDto>(tagEnumrable);
        }
    }
}
