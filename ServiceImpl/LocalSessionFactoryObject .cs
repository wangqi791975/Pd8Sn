using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.ServiceImpl
{
    /// <summary>
    /// phc
    /// </summary>
    public class LocalSessionFactoryObject : Spring.Data.NHibernate.LocalSessionFactoryObject
    { 
        public string[] EntityAssemblyNames { get; set; }
         
        protected override void PostProcessConfiguration(NHibernate.Cfg.Configuration config)
        {
            Initialize(config);
            base.PostProcessConfiguration(config);
        }


        private void Initialize(NHibernate.Cfg.Configuration config)
        {
            using (var stream = new MemoryStream())
            {
                HbmSerializer.Default.Validate = true;

                foreach (var modelAssemblyName in EntityAssemblyNames)
                {
                    //HbmSerializer.Default.Serialize(@"D:\test\",Assembly.Load(modelAssemblyName));

                    HbmSerializer.Default.Serialize(stream, Assembly.Load(modelAssemblyName));

                    stream.Position = 0;

                    config.AddInputStream(stream);
                }
            }
        }

    } 
}
