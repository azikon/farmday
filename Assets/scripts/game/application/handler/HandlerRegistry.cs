//*************************************************************************************************
//	<P> Copyright (c) 2016 Restfall Engine. All rights reserved. </P>
//	Author: 
//		Segizbayev Azamat Adilovich
//	History:
//	<TABLE>
//		Author         Date        	Version       Description
//		--------       -----        --------      ------------
//		Segizbayev     01/10/2015   0.1           Created
//		--------       -----       --------      ------------
//	<TABLE>
//	Description:
//		ServiceRegistry.cs - this code 
//*************************************************************************************************
using System;
using System.Collections.Generic;

public class HandlerRegistry : HandlerBase<HandlerRegistry>
{
    public override void LoadData()
    {
    }

    private readonly Dictionary<Type, object> _servicesRegistered = new Dictionary<Type, object>();

    public object Get(Type p_type)
    {
        lock (_servicesRegistered)
        {
            if (_servicesRegistered.ContainsKey(p_type) == true)
            {
                return _servicesRegistered[p_type];
            }
        }
        return null;
    }

    public void Add(Type p_type, object p_provider)
    {
        lock (_servicesRegistered)
        {
            _servicesRegistered.Add(p_type, p_provider);
            Log("service -> add -> " + p_provider.GetType().FullName);
        }
    }

    public void Remove(Type p_type)
    {
        object serviceOld = null;
        lock (_servicesRegistered)
        {
            if (_servicesRegistered.TryGetValue(p_type, out serviceOld) == true)
            {
                _servicesRegistered.Remove(p_type);
            }
        }
    }
}