using System;
using System.Collections.Generic;
using QS.DTO.Module;

namespace QS.Service
{
    public interface ITagService
    {
        void AddTag(TagDto tagDto);
        void DeleteTag(int tagId);
        TagDto GetTagById(int tagId);
        bool ChangeTagDescription(int tagId, TagDto updatedTagDto);
        IEnumerable<TagDto> GetAllTags();
        IEnumerable<TagDto> GetTagPaged(int pageIndex, int pageCount, out int count);
    }
}
