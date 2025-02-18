using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

public class UnauthorizedExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;

    public UnauthorizedExceptionMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SecurityTokenExpiredException)
        {
            var refreshToken = context.Session.GetString("RefreshToken");
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync($"{context.Request.Scheme}://{context.Request.Host}/api/refresh-token", new StringContent(
                    JsonConvert.SerializeObject(new { refreshToken }),
                    Encoding.UTF8,
                    "application/json"
                ));

                if (response.IsSuccessStatusCode)
                {
                    var responseData = JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());
                    context.Session.SetString("Token", responseData.Token);
                    context.Session.SetString("RefreshToken", responseData.RefreshToken);

                    // Обновляем заголовки
                    context.Request.Headers["Authorization"] = $"Bearer {responseData.Token}";

                    // Повторяем оригинальный запрос
                    await _next(context);
                    return;
                }
            }

            // Если обновление токена не удалось, перенаправляем на страницу входа
            context.Response.Redirect("/login");
            return;
        }
        catch (Exception ex)
        {
            // Обработка других исключений
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}