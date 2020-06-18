// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Tests.Web
{
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore.Mvc.Testing;
    using PrimeMultiplication.Web;

    public class TestBase : IDisposable
    {
        private bool disposedValue;

        internal TestBase()
        {
            this.Factory = new WebApplicationFactory<Startup>();
            this.Client = this.Factory.CreateClient();
        }

        protected WebApplicationFactory<Startup> Factory { get; private set; }

        protected HttpClient Client { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.Client.Dispose();
                    this.Factory.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
