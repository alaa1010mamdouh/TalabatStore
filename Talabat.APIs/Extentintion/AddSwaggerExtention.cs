namespace Talabat.APIs.Extentintion
{
    public static class AddSwaggerExtention
    {
        public static WebApplication UseSwaggerMiddleWare(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
        
    }
}
