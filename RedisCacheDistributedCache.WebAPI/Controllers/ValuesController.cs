using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisCacheDistributedCache.WebAPI.Models;

namespace RedisCacheDistributedCache.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;

    public ValuesController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    [HttpPost("setdb")]
    public IActionResult SetDb([FromBody] AddDbDto addDbDto)
    {
        DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
        distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(11);
        distributedCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(1);
        _distributedCache.SetString(addDbDto.Key, addDbDto.Value, distributedCacheEntryOptions);
        return Ok("Eklendi.");
    }

    [HttpDelete("removedb")]
    public IActionResult RemoveDb([FromBody] RemoveDbDto removeDbDto)
    {
        _distributedCache.Remove(removeDbDto.Key);
        return Ok("Silindi.");
    }

    [HttpPost("SetDbComplextypeClass")]
    public async Task<IActionResult> SetDbComplextypeClass([FromBody] Category category)
    {
        DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
        distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);
        string jsonproduct = JsonConvert.SerializeObject(category);
        await _distributedCache.SetStringAsync("category:", jsonproduct, distributedCacheEntryOptions);

        var data = await _distributedCache.GetStringAsync("product:");

        return Ok(data);
    }

    [HttpGet("getstring")]
    public async Task<IActionResult> GetString([FromQuery] string key)
    {
        
        var data =await _distributedCache.GetAsync(key);
        var result = Encoding.UTF8.GetString(data);
        return Ok(result);
    }
}