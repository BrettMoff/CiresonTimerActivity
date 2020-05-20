using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cireson.Timer.Activity.WPF
{
    class Constants
    {
        //CiresonTimerActivity.FullProjection 09e828e9-0fc8-b8b6-7184-6519aed32270

        public static readonly Guid guid_Projection_CiresonTimerActivity_FullProjection = new Guid("d8d519be-df31-4fd7-8d5b-155cc5d5fc20"); //Cireson.Timer.Activity.FullProjection  

        public static readonly Guid guidEnum_ActivityStatusEnum_Ready = new Guid("50c667cf-84e5-97f8-f6f8-d8acd99f181c"); //ActivityStatusEnum.Ready

        public static System.Text.RegularExpressions.Regex regex_NumericOnly = new System.Text.RegularExpressions.Regex("^[0-9]*$"); //regex that only allows numbers
    }
}
