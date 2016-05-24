using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;

namespace Com.Panduo.Common
{
    /// <summary>
    /// SoapException辅助处理类。返回引发SoapException异常的原是异常，也可以获得用户可读的错误信息。
    /// </summary>
    /// <remarks>
    /// 在.Net中实现Web服务时，在Web服务接口中产生的任何用户异常（非SoapException之外的异常）都将被包装为SoapException传递给客户端，
    /// 错误信息被放置在SoapException的Message中，无法直接获得原来的异常信息。SoapExceptionHelper提取原来的异常信息，并返回原来的异常。
    /// <p>如果SoapException由用户程序产生，则通过<see cref="UserException"/>返回原来产生SoapException的用户定义异常。
    /// 如果异常不是由用户产生的，则返回详细的异常信息。</p>
    /// </remarks>
    public class SoapExceptionHelper
    {
        private SoapException soapException;

        public SoapExceptionHelper(SoapException se)
        {
            this.soapException = se;
            ReadUserExceptionInfo();
        }
        public SoapExceptionHelper(Exception exception)
        {
            ReadUserExceptionInfo(exception);
        }

        /// <summary>
        /// SoapException是否是由SoapException以外的用户异常引发。
        /// </summary>
        public bool IsUserException
        {
            get { return userExceptionClass != null; }
        }

        /// <summary>
        /// 根据<see cref="#UserExceptionClass"/>创建用户异常示例。
        /// </summary>
        /// <remarks>此接口不能保证创建正确的用户异常类型，如果要更可靠的创建使用<see cref="#GetUserException"/>.</remarks>
        public Exception UserException
        {
            get
            {
                //获得调用者的集成块，以便创建正确的异常类型
                Assembly callingAssemply = Assembly.GetCallingAssembly();
                Type type = GetExceptionType(callingAssemply);
                return GetUserException(type);
            }
        }

        /// <summary>
        /// 引发<see cref="System.Web.Services.Protocols.SoapException">SoapException</see>的Web服务方法的异常。
        /// 如果原始异常也为SoapException则为null。
        /// </summary>
        /// <remarks>
        /// 因为SoapExceptionHelper无法正确的获取异常类型，可能无法知道它的位置，如一个与SoapExceptionHelper所在集成块和调用集成块(CallingAssembly)不再同一个引用范围内
        /// 的异常类。所以为了创建真正的原始异常类，调用者可能需要在外部获得实际的类型，示例如下：
        /// <code>
        /// ...
        /// SoapExceptionHelper helper = new SoapExceptionHelper(se);
        /// Type type = Type.GetType(helper.UserExceptionClass, "异常类所在的集成块");
        /// Exception e = helper.GetUserException(type);
        /// </code>
        /// 如果无法创建用户异常类型，则创建一个System.Exception对象实例。可用通过调用接口<see cref="#CanCreateUserExceptionInstance"/>检查能否创建用户异常。
        /// </remarks>
        /// <param name="exceptionType">
        /// 异常对象类型。传入null则由程序通过Type.GetType来查找。
        /// </param>
        /// <exception cref="SoapExceptionHelperException">
        /// 当不是用户异常时抛出。
        /// </exception>
        /// <returns>返回创建的用户异常实例。如果无法创建用户异常类型，则创建一个System.Exception对象实例。</returns>
        public Exception GetUserException(Type exceptionType)
        {
            if (!IsUserException)
            {
                throw new SoapExceptionHelperException("SoapException is not throwed by user exception.");
            }

            if (exceptionType == null)
            {
                //获得调用者的集成块，以便创建正确的异常类型
                Assembly callingAssemply = Assembly.GetCallingAssembly();
                exceptionType = GetExceptionType(callingAssemply);
                if (exceptionType == null) exceptionType = typeof(Exception);
            }

            Exception e = null;
            try
            {
                try
                {
                    e = Activator.CreateInstance(exceptionType, new object[] { UserExceptionMessage }, null) as Exception;
                }
                catch
                {
                }
                //if no exists constructor with message parameter, use no parameters constructor.
                if (e == null) e = Activator.CreateInstance(exceptionType) as Exception;
            }
            catch
            {
                //如果无法创建用户异常类，则使用Exception创建。
                e = new Exception(UserExceptionClass);
            }

            return e;
        }

        /// <summary>
        /// 能否创建用户异常实例。
        /// </summary>
        public bool CanCreateUserExceptionInstance
        {
            get
            {
                if (!IsUserException) return false;

                //获得调用者的集成块，以便创建正确的异常类型
                Assembly callingAssemply = Assembly.GetCallingAssembly();
                return GetExceptionType(callingAssemply) != null;
            }
        }

        /// <summary>
        /// 获得用户异常类型信息。查找顺序为：Excuting Assemply - Calling Assemply - Referenced Assemplies
        /// </summary>
        private Type GetExceptionType(Assembly callingAssemply)
        {
            Type type = Type.GetType(userExceptionClass);
            if (type == null)
            {
                type = Type.GetType(userExceptionClass + "," + callingAssemply.FullName);
                if (type == null)
                {
                    foreach (AssemblyName assembly in callingAssemply.GetReferencedAssemblies())
                    {
                        type = Type.GetType(userExceptionClass + "," + assembly.FullName);
                        if (type != null) break;
                    }
                }
            }

            return type;
        }

        /// <summary>
        /// 读取UserException信息。
        /// </summary>
        private void ReadUserExceptionInfo()
        {
            ////match user exception class
            //System.Text.RegularExpressions.MatchCollection mc =
            //    Regex.Matches(soapException.Message, "---> ([^:]+):");
            //if (mc.Count >= 1)
            //{
            //    userExceptionClass = mc[0].Groups[1].Value;
            //    //match user exception message
            //    mc = Regex.Matches(soapException.Message, "---> [^:]+:(.*)\n");
            //    if (mc.Count > 0) UserExceptionMessage = mc[0].Groups[1].Value;
            //}

            this.UserExceptionMessage = Regex.Replace(soapException.Message, @"\s*([\s\S]+)[--->.*Exception|SoapException]\s*:\s*(?<ErrorMessage>[\s\S]*)\n\s*at([\s\S]*)", "${ErrorMessage}", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        private void ReadUserExceptionInfo(Exception exception)
        {
            this.UserExceptionMessage = Regex.Replace(exception.Message, @"\s*([^:]+):\s*(?<ErrorMessage>.*)\n?\r?(.*)", "${ErrorMessage}");
        }
        private string userExceptionClass=string.Empty;
        /// <summary>
        /// 触发SoapException异常的用户异常类。如果不存在返回null。
        /// </summary>
        public string UserExceptionClass
        {
            get { return userExceptionClass; }
        }

        private string UserExceptionMessage = string.Empty;
        /// <summary>
        /// 触发SoapException异常的用户异常的Message信息。如果不存在用户异常返回null。
        /// </summary>
        public string Message
        {
            get { return UserExceptionMessage; }
        }
    }

    /// <summary>
    /// 不是用户异常或无法创建用户异常实例。
    /// </summary>
    public class SoapExceptionHelperException : Exception
    {
        private static string ExceptionMessage = "Can not create instance of exception class :";

        /// <summary>
        /// 当无法创建用户异常实例时利用此接口创建一个SoapExceptionHelperException实例。
        /// </summary>
        /// <param name="exceptionClass">用户异常类。</param>
        /// <param name="innerException">创建错误的异常。</param>
        public SoapExceptionHelperException(string exceptionClass, Exception innerException) :
            base(ExceptionMessage + exceptionClass, innerException)
        {
        }

        /// <summary>
        /// 创建一个SoapExceptionHelperException异常实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public SoapExceptionHelperException(string message)
            : base(message)
        {
        }
    }
}
