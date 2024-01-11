using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication5.ApplicationConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
	private readonly HttpClient _httpClient;

	public CurrencyController()
	{
		_httpClient = new HttpClient();
	}

	[HttpGet("exchange-rates")]
	public async Task<IActionResult> GetExchangeRates(string Base)
	{
		try
		{
			HttpResponseMessage response = await _httpClient.GetAsync(Config.apiUrl + $"/{Base.ToLower()}.json");

			if (response.IsSuccessStatusCode)
			{

				string responseData = await response.Content.ReadAsStringAsync();
				return Ok(responseData);
			}
			else
			{
				string errorResponse = await response.Content.ReadAsStringAsync();
				return StatusCode((int)response.StatusCode, $"API'den hata döndü: {response.ReasonPhrase}. Hata detayları: {errorResponse}");
			}
		}
		catch (HttpRequestException ex)
		{
			return StatusCode(500, $"API'ye istek gönderilirken hata oluştu: {ex.Message}");
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
		}
	}
	[HttpGet("Convert")]
	public async Task<IActionResult> ConvertCurrency([FromQuery] decimal amount, [FromQuery] string baseCurrency, [FromQuery] string targetCurrency)
	{
		try
		{
			HttpResponseMessage response = await _httpClient.GetAsync(Config.apiUrl + $"/{baseCurrency.ToLower()}/{targetCurrency.ToLower()}.json");

			if (response.IsSuccessStatusCode)
			{
				string responseData = await response.Content.ReadAsStringAsync();

				// JSON verisini çıkar
				JObject json = JObject.Parse(responseData);

				// JSON'dan hedeflenen değeri al
				decimal exchangeRate = (decimal)json[targetCurrency.ToLower()];

				// Belirtilen miktarı çevir
				decimal convertedAmount = amount * exchangeRate;

				string output = $"{amount} {baseCurrency.ToUpper()} = {convertedAmount} {targetCurrency.ToUpper()}";

				return Ok(output);
			}
			else
			{
				string errorResponse = await response.Content.ReadAsStringAsync();
				return StatusCode((int)response.StatusCode, $"API'den hata döndü: {response.ReasonPhrase}. Hata detayları: {errorResponse}");
			}
		}
		catch (HttpRequestException ex)
		{
			return StatusCode(500, $"API'ye istek gönderilirken hata oluştu: {ex.Message}");
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
		}
	}
}
