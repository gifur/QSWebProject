using System.Collections.Generic;
using AutoMapper;

namespace QS.Common
{
    /// <summary>
    /// AutoMapper makes it incredibly easy to map one class to another using conventions (and configurations if needed)
    /// QSMapper return the T class
    /// </summary>
    public static class QsMapper
    {
        public static TD CreateMap<TS, TD>(TS model) where TD : class where TS : class
        {
            Mapper.CreateMap<TS, TD>();
            return Mapper.Map<TS, TD>(model);
        }

        public static List<TD> CreateMapList<TS, TD>(List<TS> items)
        {
            Mapper.CreateMap<TS, TD>();
            return Mapper.Map<List<TS>, List<TD>>(items);
        }

        public static IEnumerable<TD> CreateMapIEnume<TS, TD>(IEnumerable<TS> items)
        {
            Mapper.CreateMap<TS, TD>();
            return Mapper.Map<IEnumerable<TS>, IEnumerable<TD>>(items);
        }
    }
}
