﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class MonoBaseWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(MonoBase), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("RegistEventListen", RegistEventListen);
		L.RegFunction("UnRegistEventListen", UnRegistEventListen);
		L.RegFunction("ProcessEvent", ProcessEvent);
		L.RegFunction("SendMsg", SendMsg);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("msgIds", get_msgIds, set_msgIds);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegistEventListen(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			MonoBase obj = (MonoBase)ToLua.CheckObject<MonoBase>(L, 1);
			MonoBase arg0 = (MonoBase)ToLua.CheckObject<MonoBase>(L, 2);
			ushort[] arg1 = ToLua.CheckParamsNumber<ushort>(L, 3, count - 2);
			obj.RegistEventListen(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnRegistEventListen(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			MonoBase obj = (MonoBase)ToLua.CheckObject<MonoBase>(L, 1);
			MonoBase arg0 = (MonoBase)ToLua.CheckObject<MonoBase>(L, 2);
			ushort[] arg1 = ToLua.CheckParamsNumber<ushort>(L, 3, count - 2);
			obj.UnRegistEventListen(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MonoBase obj = (MonoBase)ToLua.CheckObject<MonoBase>(L, 1);
			MsgBase arg0 = (MsgBase)ToLua.CheckObject<MsgBase>(L, 2);
			obj.ProcessEvent(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMsg(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			MonoBase obj = (MonoBase)ToLua.CheckObject<MonoBase>(L, 1);
			MsgBase arg0 = (MsgBase)ToLua.CheckObject<MsgBase>(L, 2);
			obj.SendMsg(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msgIds(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			MonoBase obj = (MonoBase)o;
			ushort[] ret = obj.msgIds;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index msgIds on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msgIds(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			MonoBase obj = (MonoBase)o;
			ushort[] arg0 = ToLua.CheckNumberArray<ushort>(L, 2);
			obj.msgIds = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index msgIds on a nil value");
		}
	}
}
