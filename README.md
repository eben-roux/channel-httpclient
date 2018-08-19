Singleton with many options requires many singletons
IDisposable on HttpClient not *really* an issue


using (var response = await _httpClient.GetAsync(requestUri))
{
    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<t>(result);
}


https://stackoverflow.com/questions/37928543/httpclient-single-instance-with-different-authentication-headers

