//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ChannelServiceTest.cs
//创 建 人：罗海明
//创建时间：2015/02/08 15:40:40 
//功能说明：渠道商服务单元测试
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using NUnit.Framework;
using Com.Panduo.Service;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class ChannelServiceTest : SpringTest
    {
        [Test]
        public void AddChannelTest()
        {
            var begin = ServiceFactory.ChannelService.GetChannel(1, 10, null, null).Pager.TotalRowCount;
            ServiceFactory.ChannelService.AddChannel("haiming.luo@panduo.com",1);
            var end = ServiceFactory.ChannelService.GetChannel(1, 10, null, null).Pager.TotalRowCount;
            Assert.AreEqual(begin + 1, end);
        }

        [Test]
        public void DeleteChannelById()
        {
            var begin = ServiceFactory.ChannelService.GetChannel(1, 10, null, null).Pager.TotalRowCount;
            ServiceFactory.ChannelService.DeleteChannelById(1, 1);
            var end = ServiceFactory.ChannelService.GetChannel(1, 10, null, null).Pager.TotalRowCount;
            Assert.AreEqual(begin - 1, end);
        }

        [Test]
        public void DeleteChannelByCustomerId()
        {
            var begin = ServiceFactory.ChannelService.GetChannel(1, 10, null, null).Pager.TotalRowCount;
            ServiceFactory.ChannelService.DeleteChannelByCustomerId(71, 1);
            var end = ServiceFactory.ChannelService.GetChannel(1, 10, null, null).Pager.TotalRowCount;
            Assert.AreEqual(begin- 1, end);
        }

        [Test]
        public void GetChannelTest()
        {
          var c=ServiceFactory.ChannelService.GetChannel(1, 10, null, null);
          Assert.AreEqual(c.Pager.TotalRowCount, 5);
        }
    }
}
