using WebCodeCli.Domain.Common.Extensions;
using AntSK.Domain.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace WebCodeCli.Repositories.Demo
{
    /// <summary>
    /// 聊天消息仓储实现
    /// </summary>
    [ServiceDescription(typeof(ISettingsRepository), ServiceLifetime.Scoped)]
    public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
       
    }
} 