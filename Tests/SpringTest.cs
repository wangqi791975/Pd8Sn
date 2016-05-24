using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Spring.Testing.NUnit;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class SpringTest : AbstractDependencyInjectionSpringContextTests
    {
        protected override string[] ConfigLocations
        {
            get
            { 
                return new[]
                           {
                               "config://spring/objects"                               
                               ,"assembly://ServiceImpl/Com.Panduo.ServiceImpl.Config/ApplicationContent.config"  
                           };
            }
        }
    }
}
