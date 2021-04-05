using System;
using System.Collections.Generic;
using System.Text;
using Application.Dal;

namespace Application.Services.Settings
{
    public class FirstConfigService
    {
        private ConfigDb _config;
        public FirstConfigService(ConfigDb config)
        {
            _config = config;
        }

        public void CreateIndex()
        {
            _config.CalculateIndexes();
        }
    }
}
