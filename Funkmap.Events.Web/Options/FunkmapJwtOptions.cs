﻿using Microsoft.Extensions.Configuration;

namespace Funkmap.Events.Web.Options
{
    public class FunkmapJwtOptions
    {
        private readonly IConfiguration _configuration;

        public FunkmapJwtOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Issuer => _configuration["Auth:Issuer"];
        public string Audience => _configuration["Auth:Audience"];

        public string Key => _configuration["Auth:Key"];
    }
}