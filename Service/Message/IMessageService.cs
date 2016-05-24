using System.Collections.Generic;

namespace Com.Panduo.Service.Message
{
    public interface IMessageService
    {
        /// <summary>
        /// 发起站内信主题
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="reply"></param>
        void StartLetterStation(LetterStation letter, LetterReply reply);

        /// <summary>
        /// 查看一个客户所有站内信
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        List<LetterStation> GetLetterStationByCustomerId(int customerId);

        /// <summary>
        /// 答复站内信
        /// </summary>
        /// <param name="reply">站内信答复实体</param>
        /// <param name="attachment">站内信答复附件列表</param>
        void Reply(LetterReply reply, List<ReplyAttachment> attachment);

        /// <summary>
        /// 关闭站内信
        /// </summary>
        /// <param name="letterId">站内信Id</param>
        void Close(int letterId);


        /// <summary>
        /// 删除站内信
        /// </summary>
        /// <param name="letterId">站内信Id</param>
        void Delete(int letterId);

        /// <summary>
        /// 读取一个站内信答复附件
        /// </summary>
        /// <param name="attachmentId">附件ID</param>
        ReplyAttachment ReadLetterReplyAttachment(int attachmentId);

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="replyId">站内信答复Id</param>
        /// <returns></returns>
        List<ReplyAttachment> DownloadAttachment(int replyId);

        /// <summary>
        /// 未读数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        int UnReadCount(int customerId);

        /// <summary>
        /// 获取一个站内信所有答复
        /// </summary>
        /// <param name="letterId">站内信Id</param>
        /// <returns></returns>
        List<LetterReply> GetLetterReplyByLetterId(int letterId);

        /// <summary>
        /// 查看客户所有站内信
        /// </summary>
        /// <param name="带分页条件"></param>
        /// <returns></returns>
        List<LetterStation> GetLetterStation(object 带分页条件);

        /// <summary>
        /// 读取一个站内信主题
        /// </summary>
        /// <param name="letterId">站内信主题Id</param>
        /// <returns></returns>
        LetterStation ReadLetterStation(int letterId);

        /// <summary>
        /// 读取一个站内信答复
        /// </summary>
        /// <param name="replyId">站内信答复ID</param>
        /// <returns></returns>
        LetterReply ReadLetterReply(int replyId);

    }
}
