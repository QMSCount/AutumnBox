﻿/*

* ==============================================================================
*
* Filename: SnowLake
* Description: 
*
* Version: 1.0
* Created: 2020/5/17 10:09:54
* Compiler: Visual Studio 2019
*
* Author: zsh2401
*
* ==============================================================================
*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutumnBox.Leafx.Container.Support
{
    /// <summary>
    /// 可注册多个同ID组件湖
    /// </summary>
    public class SnowLake : LakeBase, IRegisterableLake
    {
        readonly ConcurrentDictionary<string, List<ComponentFactory>> componentFactories
            = new ConcurrentDictionary<string, List<ComponentFactory>>();

        /// <inheritdoc/>
        public void RegisterComponent(string id, ComponentFactory factory)
        {
            if (!componentFactories.ContainsKey(id))
            {
                componentFactories[id] = new List<ComponentFactory>();
            }
            componentFactories[id].Add(factory);
        }

        /// <inheritdoc/>
        public override object GetComponent(string id)
        {
            if (componentFactories.TryGetValue(id, out List<ComponentFactory> factory))
            {
                return factory.FirstOrDefault()();
            }
            else
            {
                throw new IdNotFoundException(id);
            }
        }

        /// <summary>
        /// 获取同一ID的所有组件
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="IdNotFoundException">进行了检索,但未找到id所对应的组件集合</exception>
        /// <returns></returns>
        public IEnumerable<object> GetComponents(string id)
        {
            if (componentFactories.TryGetValue(id, out List<ComponentFactory> factories))
            {
                return factories.Select(f => f());
            }
            else
            {
                throw new IdNotFoundException(id);
            }
        }
    }
}
