using System;
using System.Collections.Generic;
using QS.DTO.Module;

namespace QS.Service
{
    public interface IMessageService
    {
        void AddMessage(MessageDto messageDto);
        void DeleteMessage(Int64 messageId);
        MessageDto GetMessageById(Int64 messageId);
        bool ChangeMessageDescription(Int64 messageId, MessageDto updatedMessageDto);
        IEnumerable<MessageDto> GetAllMessages();
        IEnumerable<MessageDto> GetMessagePaged(int pageIndex, int pageCount, out int count);
    }
}
