using System;
using System.Collections.Generic;
using QS.DTO.Module;

namespace QS.Service
{
    public interface IMyMessageService
    {
        void AddMyMessage(MyMessageDto myMessageDto);
        void DeleteMyMessage(Int64 myMessageId);
        MyMessageDto GetMyMessageById(Int64 myMessageId);
        bool ChangeMyMessageDescription(Int64 myMessageId, MyMessageDto updatedMyMessageDto);
        IEnumerable<MyMessageDto> GetAllMyMessages();
        IEnumerable<MyMessageDto> GetMyMessagePaged(int pageIndex, int pageCount, out int count);
        IEnumerable<MyMessageDto> GetMyMessages(int userid);
        IEnumerable<MyMessageDto> GetMyMessagesWithStatus(int userid, bool read);
        int GetUnreadMessage(int userid, bool read = false);
    }
}
