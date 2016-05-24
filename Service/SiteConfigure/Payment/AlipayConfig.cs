namespace Com.Panduo.Service.SiteConfigure.Payment
{
    public class AlipayConfig
    {
      /// <summary>
      /// 合作身份者id，以2088开头的16位纯数字
      /// </summary>
     public virtual string Partner { get; set; }

      /// <summary>
      /// 安全检验码，以数字和字母组成的32位字符
      /// </summary>
     public virtual string Key { get; set; }

      /// <summary>
      /// 签名方式默认MD5，不需修改
      /// </summary>
     public virtual string SignType { get; set; }

      /// <summary>
      /// 字符编码格式 目前支持 gbk 或 utf-8
      /// </summary>
     public virtual string InputCharset{ get; set; }

      /// <summary>
      /// 访问模式,根据自己的服务器是否支持ssl访问，若支持请选择https；若不支持请选择http
      /// </summary>
     public virtual string Transport{ get; set; }

      /// <summary>
      /// 交易信息提交地址
      /// </summary>
     public virtual string RequestUrl{ get; set; }

      /// <summary>
      /// 付款成功后浏览器跳转URL(含有POST信息)，但用户可能直接关闭浏览器
      /// </summary>
      public virtual string ResponseUrl{ get; set; }

      /// <summary>
      /// 付款成功后alipay异步通知地址（含有POST信息）
      /// </summary>
     public virtual string ResponseUrlNotify{ get; set; }

    }
}
