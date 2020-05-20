using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.UI.SdkDataAccess;
using Microsoft.EnterpriseManagement.ConsoleFramework;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Configuration;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.UI.SdkDataAccess.DataAdapters;
using Microsoft.EnterpriseManagement.UI.DataModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Collections.Concurrent;
using Microsoft.EnterpriseManagement.Configuration.IO;

namespace Cireson.Timer.Activity.WPF
{
    public static class Common
    {
        //private cars
        private static EnterpriseManagementGroup _emg;
        
        public static ConcurrentBag<ManagementPackEnumeration> _lstCachedEnumerations;

        public static EnterpriseManagementGroup GetManagementGroup()
        {
            try
            {
                if (_emg == null)
                {
                    IServiceContainer container = (IServiceContainer)FrameworkServices.GetService(typeof(IServiceContainer));
                    var curSession = (Microsoft.EnterpriseManagement.UI.Core.Connection.IManagementGroupSession)container.GetService(typeof(Microsoft.EnterpriseManagement.UI.Core.Connection.IManagementGroupSession));
                    if (curSession != null)
                    {
                        _emg = curSession.ManagementGroup;
                    }
                    else
                    {
                        //We are probably running in the Authoring Tool or GUI tester, so get server from console install setting
                        string sServer = Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\System Center\\2010\\Service Manager\\Console\\User Settings", "SDKServiceMachine", "").ToString();
                        if (sServer == null)
                            sServer = "localhost";
                        _emg = new EnterpriseManagementGroup(sServer);
                    }
                }
                if (!_emg.IsConnected) _emg.Reconnect();

                return _emg;
            }
            catch
            {
                return null;
            }
        }


        public static EnterpriseManagementObject ConvertIdataitemToEmo(IDataItem iDataItem)
        {
            if (iDataItem == null)
                throw new NullReferenceException("Cannot convert a null iDataItem to an EnterpriseManagementObject");

            EnterpriseManagementObject emo;

            var emg = Common.GetManagementGroup();


            IDataItem iDataItemClass = iDataItem["$Class$"] as IDataItem;
            var classGuid = (Guid)iDataItemClass["Id"]; //probably notification activity class

            ManagementPackClass mpcClass = emg.EntityTypes.GetClass(classGuid);


            if ((bool)iDataItem["$IsNew$"] == true)
            {
                emo = new CreatableEnterpriseManagementObject(emg, mpcClass);
                emo.Id = (Guid)iDataItem["$Id$"];
            }
            else
            {
                emo = emg.EntityObjects.GetObject<EnterpriseManagementObject>(new Guid(iDataItem["$Id$"].ToString()), ObjectQueryOptions.Default);
                //We cannot jsut return this, since if the user was within an SR, and then opened the activity, and hit apply on the activity (but not the SR), then the iDataItem updates with a cached version, while the emo does not update.
                // So if we only return the emo, then all of the original values will be returned.
            }

            //cemo[null, "DisplayName"].Value = iDataItem["DisplayName"]; //the displayname on the idataitem is null
            foreach (var thisProp in mpcClass.GetProperties())
            {
                if (thisProp.Type == ManagementPackEntityPropertyTypes.@enum)
                {
                    if (iDataItem[thisProp.Name] != null && (iDataItem[thisProp.Name] as IDataItem)["DisplayName"] != null && (iDataItem[thisProp.Name] as IDataItem)["Id"] != null)
                        emo[null, thisProp.Name].Value = (iDataItem[thisProp.Name] as IDataItem)["Id"]; //yes, the enumeration is another IDataItem.
                }
                else
                    emo[null, thisProp.Name].Value = iDataItem[thisProp.Name];
            }

            //Also get all properties from base classes too. "Id" might be nice eh?
            var baseClasses = mpcClass.GetBaseTypes();
            foreach (var thisBaseType in baseClasses)
            {
                foreach (var thisProp in thisBaseType.GetProperties())
                {
                    if ((bool)iDataItem["$IsNew$"] == true && thisProp.Name == "Id")
                        continue;

                    try
                    {
                        if (thisProp.Type == ManagementPackEntityPropertyTypes.@enum)
                        {
                            if (iDataItem[thisProp.Name] != null && (iDataItem[thisProp.Name] as IDataItem)["DisplayName"] != null && (iDataItem[thisProp.Name] as IDataItem)["Id"] != null)
                                emo[thisBaseType, thisProp.Name].Value = (iDataItem[thisProp.Name] as IDataItem)["Id"];
                        }
                        else
                            emo[thisBaseType, thisProp.Name].Value = iDataItem[thisProp.Name];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to set emo prop from iDataItem prop - " + ex.Message);
                    }
                }
            }

            return emo;


        }

        public static ManagementPackEnumeration GetEnumerationFromGuid(Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            if (_lstCachedEnumerations == null)
                _lstCachedEnumerations = new ConcurrentBag<ManagementPackEnumeration>();

            ManagementPackEnumeration thisEnum = _lstCachedEnumerations.FirstOrDefault(x => x.Id == guid);
            if (thisEnum != null)
                return thisEnum;

            var emg = Common.GetManagementGroup();

            try
            {
                thisEnum = emg.EntityTypes.GetEnumeration(guid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get enumeration with GUID " + guid.ToString() + ". Exception: " + ex.Message);
            }

            if (thisEnum != null)
            {
                _lstCachedEnumerations.Add(thisEnum);
                return thisEnum;
            }

            return null;
        }

        public static ManagementPackClass GetManagementPackClassByName(string strClassName, EnterpriseManagementGroup emg)
        {
            ManagementPackClassCriteria mpcc = new ManagementPackClassCriteria(String.Format("Name = '{0}'", strClassName));
            IList<ManagementPackClass> mpcResults = emg.EntityTypes.GetClasses(mpcc);

            if (mpcResults.Count == 0)
                throw new InvalidOperationException("Found zero classes with the name '" + strClassName + "'.");

            if (mpcResults.Count > 1)
                throw new InvalidOperationException("Found multiple classes with the name '" + strClassName + "'.");

            return mpcResults[0];
        }


    }
}
