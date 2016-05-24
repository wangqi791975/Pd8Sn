using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Property;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    /// <summary>
    ///这是 IPropertyServiceTest 的测试类
    /// </summary>
    [TestFixture()]
    class IPropertyServiceTest : SpringTest
    {

        #region 属性测试

        /// <summary>
        /// 获取属性
        /// </summary>
        [Test]
        public void GetPropertyById()
        {
            Property property = ServiceFactory.PropertyService.GetPropertyById(1);
            Console.WriteLine(property.PropertyName);
        }

        /// <summary>
        /// 通过属性ID,语种ID获取单个属性多语言
        /// </summary>
        [Test]
        public void GetPropertyLanguageById()
        {
            PropertyLanguage desc = ServiceFactory.PropertyService.GetPropertyLanguageById(1, 2);
            Console.WriteLine(desc.PropertyName);

        }

        /// <summary>
        ///GetAllProperties 的测试
        ///</summary>
        [Test]
        public void GetAllProperties()
        {
            var list = ServiceFactory.PropertyService.GetAllProperties();
            foreach (Property property in list)
            {
                Console.WriteLine(property.PropertyName);
            }
            Console.WriteLine(list.Count);
        }

        /// <summary>
        /// 通过属性ID获取所有属性多语言
        /// </summary>
        [Test]
        public void GetPropertyLanguagesById()
        {
            var list = ServiceFactory.PropertyService.GetPropertyLanguagesById(1);
            foreach (PropertyLanguage propertyLanguage in list)
            {
                Console.WriteLine(string.Format("PropertyId:{0},PropertyName:{1}", propertyLanguage.PropertyId, propertyLanguage.PropertyName));
            }
        }

        /// <summary>
        /// 获取所有属性多语言
        /// </summary>
        [Test]
        public void GetAllPropertyLanguages()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyLanguages();
            foreach (PropertyLanguage propertyLanguage in list)
            {
                Console.WriteLine("PropertyId:{0},LanguageId:{1}, PropertyName:{2}", propertyLanguage.PropertyId, propertyLanguage.LanguageId, propertyLanguage.PropertyName);
            }
        }

        /// <summary>
        /// 设置属性是否隐藏
        /// </summary>
        [Test]
        public void SetPropertyDisplay()
        {
            ServiceFactory.PropertyService.SetPropertyDisplay(1, true);
            Property property = ServiceFactory.PropertyService.GetPropertyById(1);
            Console.WriteLine("IsDisplay:" + property.IsDisplay);
        }

        /// <summary>
        /// 修改属性排序
        /// 只允许修改排序字段：DisplayOrder
        /// </summary>
        [Test]
        public void UpdatePropertyOrder()
        {
            List<KeyValuePair<int, int>> propertyList = new List<KeyValuePair<int, int>>();
            KeyValuePair<int, int> keyValue1 = new KeyValuePair<int, int>(1, 11);
            KeyValuePair<int, int> keyValue2 = new KeyValuePair<int, int>(2, 12);
            KeyValuePair<int, int> keyValue3 = new KeyValuePair<int, int>(3, 13);
            propertyList.Add(keyValue1);
            propertyList.Add(keyValue2);
            propertyList.Add(keyValue3);

            ServiceFactory.PropertyService.UpdatePropertyOrder(propertyList);
        }
        #endregion

        #region 属性值组测试

        /// <summary>
        /// 根据属性ID获取属性值组
        /// </summary>
        [Test]
        public void GetAllPropertyValueGroupsOfProperty()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValueGroupsOfProperty(3);
            foreach (var propertyValueGroup in list)
            {
                Console.WriteLine(propertyValueGroup.GroupId + "=>" + propertyValueGroup.PropertyValueGroupName);
            }
        }

        /// <summary>
        /// 获取所有的属性值组
        /// </summary>
        [Test]
        public void GetAllPropertyValueGroups()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValueGroups();
            Console.WriteLine(list.Count());
        }

        /// <summary>
        /// 获取所有的属性值组多语言
        /// </summary>
        [Test]
        public void GetAllPropertyValueGroupLanguages()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValueGroupLanguages();
            foreach (var propertyValueGroupLanguage in list)
            {
                Console.WriteLine("GroupId:{0},LanagueId:{1},Name:{2}", propertyValueGroupLanguage.GroupId, propertyValueGroupLanguage.LanguageId, propertyValueGroupLanguage.PropertyValueGroupName);
            }
        }

        /// <summary>
        /// 通过属性值组Id获取所有的属性值组多语言
        /// </summary>
        [Test]
        public void GetPropertyValueGroupLanguages()
        {
            var list = ServiceFactory.PropertyService.GetPropertyValueGroupLanguages(1);
            foreach (var propertyValueGroupLanguage in list)
            {
                Console.WriteLine("GroupId:{0},LanagueId:{1},Name:{2}", propertyValueGroupLanguage.GroupId, propertyValueGroupLanguage.LanguageId, propertyValueGroupLanguage.PropertyValueGroupName);
            }
        }

        /// <summary>
        /// 获取属性值组多语言
        /// </summary>
        [Test]
        public void GetPropertyValueGroupLanguage()
        {
            PropertyValueGroupLanguage propertyValueGroupLanguage = ServiceFactory.PropertyService.GetPropertyValueGroupLanguage(1, 2);
            Console.WriteLine(propertyValueGroupLanguage.PropertyValueGroupName);
        }

        /// <summary>
        /// 通过属性值组ID获取属性值组
        /// </summary>
        [Test]
        public void GetPropertyValueGroupById()
        {
            PropertyValueGroup propertyValueGroup = ServiceFactory.PropertyService.GetPropertyValueGroupById(1);
            Console.WriteLine(propertyValueGroup.PropertyValueGroupName);
        }
        #endregion

        #region 属性值测试
        /// <summary>
        /// 根据属性ID获取属性值
        /// </summary>
        [Test]
        public void GetAllPropertyValuesOfProperty()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValuesOfProperty(1);
            foreach (var propertyValue in list)
            {
                Console.WriteLine(propertyValue.PropertyValueName);
            }
        }

        /// <summary>
        /// 获取所有属性值
        /// </summary>
        [Test]
        public void GetAllPropertyValues()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValues();
            foreach (var propertyValue in list)
            {
                Console.WriteLine(propertyValue.PropertyValueName);
            }
        }

        /// <summary>
        /// 根据属性值组ID获取属性值
        /// </summary>
        [Test]
        public void GetAllPropertyValuesOfPropertyValueGroup()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValuesOfPropertyValueGroup(1);
            foreach (var propertyValue in list)
            {
                Console.WriteLine(propertyValue.PropertyValueName);
            }
        }

        /// <summary>
        /// 通过属性ID获取属性值
        /// </summary>
        [Test]
        public void GetPropertyValue()
        {
            var propertyValue = ServiceFactory.PropertyService.GetPropertyValue(1);
            Console.WriteLine(propertyValue.PropertyValueName);
        }

        /// <summary>
        /// 获取所有属性值多语种
        /// </summary>
        [Test]
        public void GetAllPropertyValueLanguages()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValueLanguages();
            foreach (var propertyValueLanguage in list)
            {
                Console.WriteLine(propertyValueLanguage.PropertyValueName);
            }

        }

        /// <summary>
        /// 通过属性值ID获取所有属性值多语种
        /// </summary>
        [Test]
        public void GetAllPropertyValueLanguagesByPropertyValueId()
        {
            var list = ServiceFactory.PropertyService.GetAllPropertyValueLanguagesById(2);
            foreach (var propertyValueLanguage in list)
            {
                Console.WriteLine(string.Format("LanguageId:{0},PropertyValueName:{1}", propertyValueLanguage.LanguageId, propertyValueLanguage.PropertyValueName));
            }
        }

        /// <summary>
        /// 属性值多语种实体
        /// </summary>
        [Test]
        public void GetPropertyValueLanguage()
        {
            var propertyValueLanguage = ServiceFactory.PropertyService.GetPropertyValueLanguage(1, 2);
            Console.WriteLine(propertyValueLanguage.PropertyValueName);
        }

        /// <summary>
        /// 修改属性值排序
        /// 只允许修改排序字段：DisplayOrder
        /// </summary>
        [Test]
        public void UpdatePropertyValueOrder()
        {
            List<KeyValuePair<int, int>> propertyValueOrderList = new List<KeyValuePair<int, int>>();
            KeyValuePair<int, int> keyValue1 = new KeyValuePair<int, int>(1, 101);
            KeyValuePair<int, int> keyValue2 = new KeyValuePair<int, int>(2, 102);
            KeyValuePair<int, int> keyValue3 = new KeyValuePair<int, int>(3, 103);
            propertyValueOrderList.Add(keyValue1);
            propertyValueOrderList.Add(keyValue2);
            propertyValueOrderList.Add(keyValue3);
            ServiceFactory.PropertyService.UpdatePropertyValueOrder(propertyValueOrderList);
        }
        #endregion

        /// <summary>
        /// 搜索
        /// </summary>
        [Test]
        public void FindPropertiesForAdminList()
        {
            var properties = ServiceFactory.PropertyService.FindPropertyValuesForAdminList(1, 1, 20, "P");
            Console.WriteLine(properties.Pager.TotalRowCount);
        }
    }
}
